using System;

namespace PluralsightDdd.SharedKernel.Interfaces
{
    public interface IDomainEvent
    {
        DateTime DateTimeEventOccurred { get; }
    }
}
