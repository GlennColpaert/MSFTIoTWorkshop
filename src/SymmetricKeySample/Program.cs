// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;
using Microsoft.Azure.Devices.Provisioning.Client.Samples;
using System;
using System.Threading.Tasks;

namespace SymmetricKeySample
{
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var sample = new ProvisioningDeviceClientSample();
            await sample.RunSampleAsync();

            Console.WriteLine("Enter any key to exit.");
            Console.ReadKey();

            return 0;
        }
    }
}
