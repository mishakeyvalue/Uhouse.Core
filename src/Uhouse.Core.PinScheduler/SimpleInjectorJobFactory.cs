using Quartz;
using Quartz.Spi;
using SimpleInjector;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Uhouse.Core.PinScheduler
{
    public class SimpleInjectorJobFactory : IJobFactory
    {
        private readonly Dictionary<Type, InstanceProducer> jobProducers;

        public SimpleInjectorJobFactory(Container container, Assembly assembly)
        {
            var types = container.GetTypesToRegister(typeof(IJob), assembly);

            var lifestyle = Lifestyle.Transient;

            // By creating producers here by the IJob service type, jobs can be decorated.
            this.jobProducers = (
                from type in types
                let producer = lifestyle.CreateProducer(typeof(IJob), type, container)
                select new { type, producer })
                .ToDictionary(t => t.type, t => t.producer);
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)this.jobProducers[bundle.JobDetail.JobType].GetInstance();
        }

        public void ReturnJob(IJob job)
        {
            throw new System.NotImplementedException();
        }
    }
}
