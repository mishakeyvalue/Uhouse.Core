using Quartz;
using Quartz.Impl;
using SimpleInjector;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Uhouse.Core.PinScheduler
{
    public static class PinSchedulerFactory
    {
        public static async Task<IPinScheduler> Init(IPinSwitcher pinSwitcher)
        {
            var container = new Container();

            container.RegisterInstance(pinSwitcher);
            var jobFactory = new SimpleInjectorJobFactory(container, Assembly.GetExecutingAssembly());

            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = await schedulerFactory.GetScheduler();
            scheduler.JobFactory = jobFactory;
            await scheduler.Start();
            
            return new PinScheduler(scheduler);
        }
        private class PinScheduler : IPinScheduler
        {
            private readonly IScheduler scheduler;

            public PinScheduler(IScheduler scheduler)
            {
                this.scheduler = scheduler;
            }

            public Guid Schedule(DateTimeOffset startDate, TimeSpan duration)
            {
                var id = Guid.NewGuid();
                var job = JobBuilder.Create<PinSwitchJob>().WithIdentity(id.ToString()).Build();
                var trigger = TriggerBuilder.Create().StartAt(startDate).Build();

                this.scheduler.ScheduleJob(job, trigger);
                return id;
            }
        }
    }
}
