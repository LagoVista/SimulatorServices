// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 98fcf45926e36d562ea026dc9bfe1862805291ef50c1c18e14119657b209fd34
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.IoT.Simulator.Admin.Models;
using System;
using System.Linq;
using LagoVista.Core;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace LagoVista.IoT.Simulator.Runtime
{
    public partial class SimulatorRuntime
    {
        private String BuildRequestContent(MessageTransmissionPlan plan)
        {
            var messageTemplate = plan.Message.Value;

            var sentContent = new StringBuilder();

            switch (messageTemplate.Transport.Value)
            {

                case TransportTypes.TCP:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port   : {messageTemplate.Port}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(_instance, plan, messageTemplate.TextPayload));
                    break;

                case TransportTypes.UDP:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port   : {messageTemplate.Port}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(_instance, plan, messageTemplate.TextPayload));
                    break;

                case TransportTypes.AzureIoTHub:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port   : {messageTemplate.Port}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(_instance, plan, messageTemplate.TextPayload));
                    break;

                case TransportTypes.AzureEventHub:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(_instance, plan, messageTemplate.TextPayload));
                    break;

                case TransportTypes.AzureServiceBus:
                    sentContent.AppendLine($"Host   : {messageTemplate.Name}");
                    //sentContent.AppendLine($"Queue   : {MsgTemplate.Qu}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(_instance, plan, messageTemplate.TextPayload));
                    break;

                case TransportTypes.MQTT:
                    sentContent.AppendLine($"Host         : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port         : {messageTemplate.Port}");
                    sentContent.AppendLine($"Topics");
                    sentContent.AppendLine($"Publish      : {ReplaceTokens(_instance, plan, messageTemplate.Topic)}");
                    sentContent.AppendLine($"Subscription : {ReplaceTokens(_instance, plan, _simulator.Subscription)}");

                    sentContent.Append(ReplaceTokens(_instance, plan, messageTemplate.TextPayload));
                    break;

                case TransportTypes.RestHttps:
                case TransportTypes.RestHttp:
                    {
                        var protocol = messageTemplate.Transport.Value == TransportTypes.RestHttps ? "https" : "http";
                        var uri = $"{protocol}://{_simulator.DefaultEndPoint}:{_simulator.DefaultPort}{ReplaceTokens(_instance, plan, messageTemplate.PathAndQueryString)}";
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
                            sentContent.AppendLine($"\t{idx++}. {hdr.HeaderName}={ReplaceTokens(_instance, plan, hdr.Value)}");
                        }

                        if (messageTemplate.HttpVerb == "POST" || messageTemplate.HttpVerb == "PUT")
                        {
                            sentContent.AppendLine("");
                            sentContent.AppendLine("Post Content");
                            sentContent.AppendLine("=========================");
                            sentContent.AppendLine(ReplaceTokens(_instance, plan, messageTemplate.TextPayload));
                        }
                    }
                    break;
            }

            return sentContent.ToString();
        }

        public byte[] GetMessageBytes(MessageTransmissionPlan plan)
        {
            var messageTemplate = plan.Message.Value;

            if (EntityHeader.IsNullOrEmpty(messageTemplate.PayloadType) || messageTemplate.PayloadType.Value == PaylodTypes.Binary)
            {
                return GetBinaryPayload(messageTemplate.BinaryPayload);
            }
            else if(messageTemplate.PayloadType.Value == PaylodTypes.PointArray)
            {
                return GetPointArrayPayload(messageTemplate.BinaryPayload);
            }
            else
            {
                var msgText = ReplaceTokens(_instance, plan, messageTemplate.TextPayload);
                return System.Text.UTF8Encoding.UTF8.GetBytes(msgText);
            }
        }

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private byte[] GetPointArrayPayload(string payload)
        {
            var rnd = new Random();

            var bytes = new List<Byte>();

            var parameters = new Dictionary<string, string>();
            var parts = payload.Split(';');
            foreach(var part in parts)
            {
                if (part.Split('=').Length == 2)
                {
                    var key = part.Split('=')[0];
                    var value = part.Split('=')[1];
                    parameters.Add(key, value);
                }
            }

            var pointCount = parameters.ContainsKey("pointCount") ? int.Parse(parameters["pointCount"]) : 300;
            var sensorIndex = parameters.ContainsKey("sensorIndex") ? double.Parse(parameters["sensorIndex"]) : 1.0;
            var interval = parameters.ContainsKey("interval") ? double.Parse(parameters["interval"]) : 1.0;
            var min = parameters.ContainsKey("min") ? double.Parse(parameters["min"]) : 0.0;
            var max = parameters.ContainsKey("max") ? double.Parse(parameters["max"]) : 100.0;

            var range = max - min;

            var seconds = Convert.ToInt32((DateTime.UtcNow - epoch).TotalSeconds);
            bytes.AddRange(BitConverter.GetBytes(seconds));
            bytes.AddRange(BitConverter.GetBytes((UInt16)sensorIndex));
            bytes.AddRange(BitConverter.GetBytes((UInt16)pointCount));
            bytes.AddRange(BitConverter.GetBytes((UInt16)(interval * 10)));
            for(var idx = 0; idx < pointCount; ++idx)
            {
                var dataPoint = (rnd.NextDouble() * range) + min;
                bytes.AddRange(BitConverter.GetBytes((Int16)(dataPoint * 100)));
            }

            return bytes.ToArray();
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

        private string RandomizeInt(string input, string newValueRegEx, string key = "")
        {
            var floatRegEx = new Regex(@"~random-int,(?'min'[+-]?(([0-9]*[.]?)?[0-9]+)),(?'max'[+-]?([0-9]*[.])?[0-9]+)~");
            var floatMatches = floatRegEx.Matches(newValueRegEx);
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
                    if (String.IsNullOrEmpty(key))
                    {
                        input = input.Replace(match.Value, Math.Round(value).ToString());
                    }
                    else
                    {
                        input = input.Replace($"~{key}~", Math.Round(value, 0).ToString());
                    }
                }
            }

            return input;
        }

        private string RandomizeFloat(string input, string newValueRegEx, string key = "")
        {
            var floatRegEx = new Regex(@"~random-float,(?'min'[+-]?(([0-9]*[.]?)?[0-9]+)),(?'max'[+-]?([0-9]*[.])?[0-9]+)~");
            var floatMatches = floatRegEx.Matches(newValueRegEx);
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
                    if (String.IsNullOrEmpty(key))
                    {
                        input = input.Replace(match.Value, Math.Round(value, 2).ToString());
                    }
                    else
                    {
                        input = input.Replace($"~{key}~", Math.Round(value, 2).ToString());
                    }

                }
            }

            return input;
        }

        private String ReplaceTokens(SimulatorInstance instance, MessageTransmissionPlan plan, String input)
        {
            var msg = plan.Message.Value;
            
            if (String.IsNullOrEmpty(input))
            {
                return String.Empty;
            }

            foreach (var attr in msg.DynamicAttributes)
            {
                var instanceValue = plan.Values.Where(atr => atr.Attribute.Id == attr.Id).FirstOrDefault();
                if (instanceValue == null || String.IsNullOrEmpty(instanceValue.Value))
                {
                    input = input.Replace($"~{attr.Key}~", attr.DefaultValue);
                }
                else
                {
                    if(instanceValue.Value.StartsWith("~random-int"))
                    {
                        input = RandomizeInt(input, instanceValue.Value, attr.Key);
                    }
                    else if (instanceValue.Value.StartsWith("~random-float"))
                    {
                        input = RandomizeFloat(input, instanceValue.Value, attr.Key);
                    }
                    else
                    {
                        input = input.Replace($"~{attr.Key}~", instanceValue.Value);
                    }
                }
            }

            input = input.Replace($"~deviceid~", String.IsNullOrEmpty(instance.DeviceId) ? _simulator.DeviceId : instance.DeviceId);
            input = input.Replace($"~datetime~", DateTime.Now.ToString());
            input = input.Replace($"~username~", _simulator.UserName);
            input = input.Replace($"~password~", _simulator.Password);
            input = input.Replace($"~accesskey~", _simulator.AccessKey);
            input = input.Replace($"~password~", _simulator.Password);
            input = input.Replace($"~datetimeiso8601~", DateTime.UtcNow.ToJSONString());

            input = RandomizeInt(input, input);
            input = RandomizeFloat(input, input);


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
