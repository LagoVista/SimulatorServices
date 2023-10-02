using LagoVista.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LagoVista.IoT.Simulator.Runtime.Models
{
    public class CSVPlan
    {
        private List<string> _messages;
        private int _currentIndex;
        private int _nextSend;
        private DateTimeOffset _start;

        public static CSVPlan Create(string messageTemplate, string contents)
        {
            var formatParts = messageTemplate.Split(",");


            var plan = new CSVPlan();
            plan._start = DateTimeOffset.UtcNow;
            var lines = contents.Split("\r");
            foreach (var line in lines)
            {
                if (!String.IsNullOrEmpty(line.Trim()))
                {
                    var formatString = String.Empty;
                    var csvParts = line.Split(',');
                    if(csvParts.Length != formatParts.Length)
                    {
                        throw new Exception($"Format string csv line mismatch. {csvParts.Length} {formatParts.Length}");
                    }    

                    for (var idx = 0; idx < formatParts.Length; ++idx)
                    {
                        var part = csvParts[idx];

                        if (formatParts[idx].Contains("epochms"))
                        {
                            var dateStamp = plan._start.AddSeconds(float.Parse(part));
                            csvParts[idx] = dateStamp.ToUnixTimeMilliseconds().ToString();
                        }
                        else if (formatParts[idx].Contains("epoch"))
                        {
                            var dateStamp = plan._start.AddSeconds(float.Parse(part));
                            csvParts[idx] = dateStamp.ToUnixTimeSeconds().ToString();
                        }
                        else if (formatParts[idx].Contains("jsdate"))
                        {
                            var dateStamp = plan._start.AddSeconds(float.Parse(part));
                            csvParts[idx] = dateStamp.DateTime.ToJSONString();
                        }

                        formatString += $"{{{idx}}},";
                    }

                    formatString = formatString.TrimEnd(',');
                    var formattedLine = string.Format(formatString, csvParts);
                    Debug.WriteLine(formattedLine);
                }
            }

            return plan;
        }
    }
}
