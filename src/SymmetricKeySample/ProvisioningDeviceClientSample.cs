// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Devices.Provisioning.Client.Samples
{
    internal class ProvisioningDeviceClientSample
    {
        public ProvisioningDeviceClientSample() {}

        public async Task RunSampleAsync()
        {
            Console.WriteLine($"Initializing the device provisioning client...");

             using var security = new SecurityProviderSymmetricKey("", 
                 "", null);

            ProvisioningDeviceClient provClient = ProvisioningDeviceClient.Create(
                "global.azure-devices-provisioning.net",
                "",
                security,
                new ProvisioningTransportHandlerMqtt());

            Console.WriteLine($"Initialized for registration Id {security.GetRegistrationID()}.");

            Console.WriteLine("Registering with the device provisioning service...");
            DeviceRegistrationResult result = await provClient.RegisterAsync();

            Console.WriteLine($"Registration status: {result.Status}.");
            if (result.Status != ProvisioningRegistrationStatusType.Assigned)
            {
                Console.WriteLine($"Registration status did not assign a hub, so exiting this sample.");
                return;
            }

            Console.WriteLine($"Device {result.DeviceId} registered to {result.AssignedHub}.");

            Console.WriteLine("Creating symmetric key authentication for IoT Hub...");
            IAuthenticationMethod auth = new DeviceAuthenticationWithRegistrySymmetricKey(
                result.DeviceId,
                security.GetPrimaryKey());

            Console.WriteLine($"Testing the provisioned device with IoT Hub...");
            using DeviceClient iotClient = DeviceClient.Create(result.AssignedHub, auth, TransportType.Mqtt);

            Console.WriteLine("Sending a telemetry message...");
            using var message = new Message(Encoding.UTF8.GetBytes("TestMessage"));

            while (true)
            {
                await iotClient.SendEventAsync(message);
            }
            

            Console.WriteLine("Finished.");
        }
    }
}
