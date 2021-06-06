
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulatedDevice
{
    class Program
    {
        private static DeviceClient s_deviceClient;
        private readonly static string s_myDeviceId = "";
        private readonly static string s_iotHubUri = "";
        private readonly static string s_deviceKey = "";

        private static async Task Main()
        {
            // http://docs.microsoft.com/azure/iot-hub/tutorial-routing

            Console.WriteLine("Routing Tutorial: Simulated device\n");
            s_deviceClient = DeviceClient.Create(s_iotHubUri,
              new DeviceAuthenticationWithRegistrySymmetricKey(s_myDeviceId, s_deviceKey), TransportType.Mqtt);

            using var cts = new CancellationTokenSource();
            var messages = SendDeviceToCloudMessagesAsync(cts.Token);
            Console.WriteLine("Press the Enter key to stop.");
            Console.ReadLine();
            await s_deviceClient.CloseAsync(cts.Token);
            cts.Cancel();
            await messages;

        }

        /// <summary> 
        /// Send message to the Iot hub. This generates the object to be sent to the hub in the message.
        /// </summary>
        private static async Task SendDeviceToCloudMessagesAsync(CancellationToken token)
        {
            double minTemperature = 20;
            double minHumidity = 60;
            Random rand = new Random();

            while (!token.IsCancellationRequested)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                string infoString;
                string levelValue;

                if (rand.NextDouble() > 0.7)
                {
                    if (rand.NextDouble() > 0.5)
                    {
                        levelValue = "critical";
                        infoString = "This is a critical message.";
                    }
                    else
                    {
                        levelValue = "storage";
                        infoString = "This is a storage message.";
                    }
                }
                else
                {
                    levelValue = "normal";
                    infoString = "This is a normal message.";
                }

                var telemetryDataPoint = new
                {
                    deviceId = s_myDeviceId,
                    temperature = currentTemperature,
                    humidity = currentHumidity,
                    pointInfo = infoString
                };
                // serialize the telemetry data and convert it to JSON.
                var telemetryDataString = JsonConvert.SerializeObject(telemetryDataPoint);

                // Encode the serialized object using UTF-8 so it can be parsed by IoT Hub when
                // processing messaging rules.
                using var message = new Message(Encoding.UTF8.GetBytes(telemetryDataString))
                {
                    ContentEncoding = "utf-8",
                    ContentType = "application/json",
                };

                // Add one property to the message.
                message.Properties.Add("level", levelValue);

                // Submit the message to the hub.
                await s_deviceClient.SendEventAsync(message);

                // Print out the message.
                Console.WriteLine("{0} > Sent message: {1}", DateTime.UtcNow, telemetryDataString);

                await Task.Delay(1000);
            }
        }      
    }
}
