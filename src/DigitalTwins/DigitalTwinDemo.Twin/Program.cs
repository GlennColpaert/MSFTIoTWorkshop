using System;
using System.Threading.Tasks;

namespace DigitalTwinDemo.Twin
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Twin!");
            Console.WriteLine("Please enter 'SetupDigitalTwin' to create the Digital Twin!");
            Console.WriteLine("Please enter 'SelfDestruct' to remove the Digital Twin!");
            string command = Console.ReadLine().Trim();

            if (command.Equals("SetupDigitalTwin", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Going....");
                DigitalTwin digitalTwin = new DigitalTwin();

                await digitalTwin.CleanupEnvironment();

                await digitalTwin.CreateHouseTwin();

            }
            if (command.Equals("SelfDestruct", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Going....");
                DigitalTwin digitalTwin = new DigitalTwin();

                await digitalTwin.CleanupEnvironment();
            }

            Console.WriteLine("Digital Twin signing off!");
        }
    }
}
