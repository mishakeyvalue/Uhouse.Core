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
            int duration = 2000;
            try
            {
                pinSwitcher.TurnOn();
                await Task.Delay(duration);
            }
            finally
            {
                pinSwitcher.TurnOff();
            }
        }
    }
}
