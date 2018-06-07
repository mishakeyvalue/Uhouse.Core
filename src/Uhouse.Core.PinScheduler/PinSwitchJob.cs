using System;
using System.Threading.Tasks;
using Quartz;

namespace Uhouse.Core.PinScheduler
{
    internal class PinSwitchJob : IJob
    {
        private readonly IPinSwitcher pinSwitcher;

        public PinSwitchJob(IPinSwitcher pinSwitcher)
        {
            this.pinSwitcher = pinSwitcher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                TimeSpan duration = context.JobDetail.JobDataMap.GetTimeSpanValue("duration");
                Console.WriteLine($"Turning on the pin.");
                pinSwitcher.TurnOn();
                await Task.Delay(duration);
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            finally
            {
                Console.WriteLine($"Turning off the pin.");
                pinSwitcher.TurnOff();
            }
        }
    }
}
