using System;

namespace Uhouse.Core.PinScheduler
{
    public interface IPinScheduler
    {
        Guid Schedule(DateTimeOffset startDate, TimeSpan duration);
    }
}
