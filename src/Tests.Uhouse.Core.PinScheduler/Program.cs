using System;
using System.Threading.Tasks;
using Uhouse.Core.PinScheduler;

namespace Tests.Uhouse.Core.PinScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Go();
            Console.ReadKey();
        }

        static async Task Go()
        {
            var scheduler = await PinSchedulerFactory.Init(new MockSwitcher());
            scheduler.Schedule(DateTimeOffset.Now.AddSeconds(2), TimeSpan.FromSeconds(2));
        }

        class MockSwitcher : IPinSwitcher
        {
            public void TurnOff()
            {
                Console.WriteLine("Pin is turned off!");
            }

            public void TurnOn()
            {
                Console.WriteLine("Pin is turned on!");
            }
        }
    }
}
