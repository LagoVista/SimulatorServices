using LagoVista.Client.Core.Models;
using LagoVista.Core.Interfaces;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime.Tests.Simulator
{

    [TestClass]
    public class DataGenreationTests
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [TestMethod]
        public void GeneratePointArray()
        {
            var runtime = new SimulatorRuntime(new Mock<ISimulatorRuntimeServices>().Object, new Mock<INotificationPublisher>().Object, new Mock<IAdminLogger>().Object, new Admin.Models.SimulatorInstance()
            {
                 Simulator = new Core.Models.EntityHeader<Admin.Models.Simulator>()
                 {
                     Value = new Admin.Models.Simulator()
                     {
                         SimulatorStates = new List<Admin.Models.SimulatorState>()
                         {
                              new Admin.Models.SimulatorState() { Key = "default", Name = "Default"}
                         }
                     }
                 }
            });

            var pointsToGenerate = 300;
            var intervalParam = 2.0;
            var sensorIndex = 5;
            var min = 5.5;
            var max = 25.5;

            var messageBytes = runtime.GetMessageBytes(new Admin.Models.MessageTransmissionPlan()
            {
                Message = Core.Models.EntityHeader<Admin.Models.MessageTemplate>.Create(new Admin.Models.MessageTemplate()
                {
                    PayloadType = Core.Models.EntityHeader<Admin.Models.PaylodTypes>.Create(Admin.Models.PaylodTypes.PointArray),
                    BinaryPayload = $"pointCount={pointsToGenerate};sensorIndex={sensorIndex};interval={intervalParam};min={min};max={max}"
                })
            });

            var payloadSize = messageBytes.Length;

            Console.WriteLine("Payload Size:" + payloadSize);

            using (var ms = new MemoryStream(messageBytes))
            using(var rdr = new BinaryReader(ms))
            {
                var epochSeconds = rdr.ReadInt32();
                
                var timeStamp = epoch.AddSeconds(epochSeconds);
                Console.WriteLine($"Time Stamp {timeStamp}");

                var sensorIndexValue = rdr.ReadUInt16();
                Assert.AreEqual(sensorIndex, sensorIndexValue);
                Console.WriteLine($"Sensor Index {sensorIndexValue}");

                var pointCount = rdr.ReadUInt16();
                Assert.AreEqual(pointsToGenerate, pointCount);
                Console.WriteLine($"Point Count {pointCount}");

                var interval = rdr.ReadUInt16() / 10.0;
                Console.WriteLine($"Interval {interval}");
                Assert.AreEqual(intervalParam, interval);
                
                for(var idx = 0; idx < pointCount; ++idx)
                {
                    var pointValue = rdr.ReadInt16() / 100.0;
                    Console.WriteLine($"Point {idx} - {pointValue}");

                    Assert.IsTrue(pointValue >= min);
                    Assert.IsTrue(pointValue <= max);
                }
            }
        }
    }
}
