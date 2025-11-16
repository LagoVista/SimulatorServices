// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 37af070697c40f9f2170f920a9fef883c9eb23b38ffa33b356d76fefd1175777
// IndexVersion: 2
// --- END CODE INDEX META ---
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Runtime
{
    public class ReceivedMessage
    {
        public ReceivedMessage(byte[] payload)
        {
            TextPayload = System.Text.UTF8Encoding.UTF8.GetString(payload);

            foreach (var ch in payload)
            {
                BinaryPayload += $"{ch:x2} ";
            }

            ReceivedTimeStamp = DateTime.Now;
        }

        public string MessageId { get; set; }

        public DateTime ReceivedTimeStamp { get; set; }
        public String Topic { get; set; }

        public String TextPayload { get; set; }

        public string BinaryPayload { get; set; }
    }
}
