// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Devices.Client.Samples
{
    public class Program
    {
        static string connString = "";
        public static async Task<int> Main(string[] args)
        {
            using var deviceClient = DeviceClient.CreateFromConnectionString(connString,
                TransportType.Mqtt);

            var sample = new MessageReceiveSample(deviceClient, Timeout.InfiniteTimeSpan);
            await sample.RunSampleAsync();
            await deviceClient.CloseAsync();

            Console.WriteLine("Done.");
            return 0;
        }
    }
}
