using LagoVista.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LagoVista.IoT.Simulator.Runtime.Models
{
    public class CSVPlan
    {
        private List<CSVPlanMessage> _messages = new List<CSVPlanMessage>();
        private int _currentIndex;
        private int _nextSend;
        private DateTimeOffset _start;
        
        public class CSVPlanMessage
        {
            public string Contents { get; set; }
            public DateTimeOffset TimeStamp { get; set; }
        }

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
                    var message = new CSVPlanMessage();

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
                            message.TimeStamp = plan._start.AddSeconds(float.Parse(part));
                            csvParts[idx] = message.TimeStamp.ToUnixTimeMilliseconds().ToString();
                        }
                        else if (formatParts[idx].Contains("epochseconds"))
                        {
                            message.TimeStamp = plan._start.AddSeconds(float.Parse(part));
                            csvParts[idx] = message.TimeStamp.ToUnixTimeSeconds().ToString();
                        }
                        else if (formatParts[idx].Contains("jsdate"))
                        {
                            message.TimeStamp = plan._start.AddSeconds(float.Parse(part));
                            csvParts[idx] = message.TimeStamp.DateTime.ToJSONString();
                        }

                        formatString += $"{{{idx}}},";
                    }

                    formatString = formatString.TrimEnd(',');
                    message.Contents = string.Format(formatString, csvParts);
                    plan._messages.Add(message);
                }
            }

            return plan;
        }

        public List<CSVPlanMessage> Messages { get => _messages; }
    }
}
