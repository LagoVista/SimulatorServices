using LagoVista.Core.Models;
using LagoVista.IoT.Simulator.Admin.Models;
using System;
using System.Linq;
using LagoVista.Core;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LagoVista.IoT.Simulator.Runtime
{
    public partial class SimulatorRuntime
    {
        private String BuildRequestContent(MessageTemplate messageTemplate)
        {
            var sentContent = new StringBuilder();

            switch (messageTemplate.Transport.Value)
            {

                case TransportTypes.TCP:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port   : {messageTemplate.Port}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;
                case TransportTypes.UDP:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port   : {messageTemplate.Port}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;
                case TransportTypes.AzureIoTHub:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port   : {messageTemplate.Port}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;

                case TransportTypes.AzureEventHub:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;

                case TransportTypes.AzureServiceBus:
                    sentContent.AppendLine($"Host   : {messageTemplate.Name}");
                    //sentContent.AppendLine($"Queue   : {MsgTemplate.Qu}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;
                case TransportTypes.MQTT:
                    sentContent.AppendLine($"Host         : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port         : {messageTemplate.Port}");
                    sentContent.AppendLine($"Topics");
                    sentContent.AppendLine($"Publish      : {ReplaceTokens(messageTemplate, messageTemplate.Topic)}");
                    sentContent.AppendLine($"Subscription : {ReplaceTokens(messageTemplate, _simulator.Subscription)}");

                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;
                case TransportTypes.RestHttps:
                case TransportTypes.RestHttp:
                    {
                        var protocol = messageTemplate.Transport.Value == TransportTypes.RestHttps ? "https" : "http";
                        var uri = $"{protocol}://{_simulator.DefaultEndPoint}:{_simulator.DefaultPort}{ReplaceTokens(messageTemplate, messageTemplate.PathAndQueryString)}";
                        sentContent.AppendLine($"Method       : {messageTemplate.HttpVerb}");
                        sentContent.AppendLine($"Endpoint     : {uri}");
                        var contentType = String.IsNullOrEmpty(messageTemplate.ContentType) ? "text/plain" : messageTemplate.ContentType;
                        sentContent.AppendLine($"Content Type : {contentType}");

                        if (_simulator.BasicAuth)
                        {
                            var authCreds = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(_simulator.UserName + ":" + _simulator.Password));
                            sentContent.AppendLine($"Authorization: Basic {authCreds}");
                        }
                        if (messageTemplate.MessageHeaders.Any())
                        {
                            sentContent.AppendLine($"Custom Headers");
                        }

                        var idx = 1;
                        foreach (var hdr in messageTemplate.MessageHeaders)
                        {
                            sentContent.AppendLine($"\t{idx++}. {hdr.HeaderName}={ReplaceTokens(messageTemplate, hdr.Value)}");
                        }

                        if (messageTemplate.HttpVerb == "POST" || messageTemplate.HttpVerb == "PUT")
                        {
                            sentContent.AppendLine("");
                            sentContent.AppendLine("Post Content");
                            sentContent.AppendLine("=========================");
                            sentContent.AppendLine(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));
                        }
                    }
                    break;
            }

            return sentContent.ToString();
        }

        private byte[] GetMessageBytes(MessageTemplate messageTemplate)
        {
            if (EntityHeader.IsNullOrEmpty(messageTemplate.PayloadType) || messageTemplate.PayloadType.Value == PaylodTypes.Binary)
            {
                return GetBinaryPayload(messageTemplate.BinaryPayload);
            }
            else
            {
                var msgText = ReplaceTokens(messageTemplate, messageTemplate.TextPayload);
                return System.Text.UTF8Encoding.UTF8.GetBytes(msgText);
            }
        }

        private byte[] GetBinaryPayload(string binaryPayload)
        {
            if (String.IsNullOrEmpty(binaryPayload))
            {
                return new byte[0];
            }

            try
            {
                var bytes = new List<Byte>();

                if (binaryPayload.Length % 2 == 0 && !binaryPayload.StartsWith("0x"))
                {
                    for (var idx = 0; idx < binaryPayload.Length; idx += 2)
                    {
                        var byteStr = binaryPayload.Substring(idx, 2);
                        bytes.Add(Byte.Parse(byteStr, System.Globalization.NumberStyles.HexNumber));
                    }
                }
                else
                {
                    var bytesList = binaryPayload.Split(' ');
                    foreach (var byteStr in bytesList)
                    {
                        var lowerByteStr = byteStr.ToLower();
                        if (lowerByteStr.Contains("soh"))
                        {
                            bytes.Add(0x01);
                        }
                        else if (lowerByteStr.Contains("stx"))
                        {
                            bytes.Add(0x02);
                        }
                        else if (lowerByteStr.Contains("etx"))
                        {
                            bytes.Add(0x03);
                        }
                        else if (lowerByteStr.Contains("eot"))
                        {
                            bytes.Add(0x04);
                        }
                        else if (lowerByteStr.Contains("ack"))
                        {
                            bytes.Add(0x06);
                        }
                        else if (lowerByteStr.Contains("cr"))
                        {
                            bytes.Add(0x0d);
                        }
                        else if (lowerByteStr.Contains("lf"))
                        {
                            bytes.Add(0x0a);
                        }
                        else if (lowerByteStr.Contains("nak"))
                        {
                            bytes.Add(0x15);
                        }
                        else if (lowerByteStr.Contains("esc"))
                        {
                            bytes.Add(0x1b);
                        }
                        else if (lowerByteStr.Contains("del"))
                        {
                            bytes.Add(0x1b);
                        }
                        else if ((lowerByteStr.StartsWith("x")))
                        {
                            bytes.Add(Byte.Parse(byteStr.Substring(1), System.Globalization.NumberStyles.HexNumber));
                        }
                        else if (lowerByteStr.StartsWith("0x"))
                        {
                            bytes.Add(Byte.Parse(byteStr.Substring(2), System.Globalization.NumberStyles.HexNumber));
                        }
                        else
                        {
                            bytes.Add(Byte.Parse(byteStr, System.Globalization.NumberStyles.HexNumber));
                        }
                    }
                }

                return bytes.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(Simulator.Runtime.SimulatorRuntimeResources.SendMessage_InvalidBinaryPayload + " " + ex.Message);
            }
        }

        private String ReplaceTokens(MessageTemplate msg, String input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return String.Empty;
            }

            foreach (var attr in msg.DynamicAttributes)
            {
                input = input.Replace($"~{attr.Key}~", attr.DefaultValue);
            }

            input = input.Replace($"~deviceid~", _simulator.DeviceId);
            input = input.Replace($"~datetime~", DateTime.Now.ToString());
            input = input.Replace($"~username~", _simulator.UserName);
            input = input.Replace($"~password~", _simulator.Password);
            input = input.Replace($"~accesskey~", _simulator.AccessKey);
            input = input.Replace($"~password~", _simulator.Password);
            input = input.Replace($"~datetimeiso8601~", DateTime.UtcNow.ToJSONString());

            var floatRegEx = new Regex(@"~random-float,(?'min'[+-]?(([0-9]*[.]?)?[0-9]+)),(?'max'[+-]?([0-9]*[.])?[0-9]+)~");
            var intRegEx = new Regex(@"~random-int,(?'min'[+-]?\d+),(?'max'[+-]?\d+)~");
            var floatMatches = floatRegEx.Matches(input);

            foreach (Match match in floatMatches)
            {
                if (float.TryParse(match.Groups["min"].Value, out float minValue) && float.TryParse(match.Groups["max"].Value, out float maxValue))
                {
                    if (minValue > maxValue)
                    {
                        var tmp = maxValue;
                        maxValue = minValue;
                        minValue = tmp;
                    }

                    var delta = maxValue - minValue;
                    var value = delta * _random.NextDouble() + minValue;
                    input = input.Replace(match.Value, Math.Round(value, 2).ToString());
                }
            }

            var intMatches = intRegEx.Matches(input);

            foreach (Match match in intMatches)
            {
                if (int.TryParse(match.Groups["min"].Value, out int minValue) && int.TryParse(match.Groups["max"].Value, out int maxValue))
                {
                    if (minValue > maxValue)
                    {
                        var tmp = maxValue;
                        maxValue = minValue;
                        minValue = tmp;
                    }
                    var delta = maxValue - minValue;
                    var value = _random.Next(minValue, maxValue);
                    input = input.Replace(match.Value, value.ToString());
                }
            }

            if (msg.AppendCR)
            {
                input += "\r";
            }

            if (msg.AppendLF)
            {
                input += "\n";
            }

            return input;
        }
    }
}
