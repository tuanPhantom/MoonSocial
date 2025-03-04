﻿using CQRS.Core.Domain.Entities;

namespace CQRS.Core.Handlers;

/// <summary>
/// This class will provide an interface abstraction through which the command handler will obtain the latest state
/// of the aggregate and through which it will persist the uncommitted changes on the aggregate as events
/// </summary>
/// <typeparam name="T">The actual aggregate's type</typeparam>
public interface IEventSourcingHandler<T>
{
    /// <summary>
    /// Persist an uncommitted changes on the aggregate as event
    /// </summary>
    /// <param name="aggregate">the aggregate to be stored</param>
    /// <returns>A Task represents the storing progress</returns>
    Task SaveAsync(AggregateRoot aggregate);

    /// <summary>
    /// Obtain the latest state of the specified aggregate
    /// </summary>
    /// <param name="id">the identifier of the specified aggregate</param>
    /// <returns>A Task represents the latest state of the specified aggregate</returns>
    Task<T> GetIdAsync(Guid id);
}