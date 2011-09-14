﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Provides a common API for event stores that leverage Linq.
/// </summary>
/// <typeparam name="TAggregateId">The type of identifier used by aggregate roots in the domain.</typeparam>
/// <typeparam name="TBaseEvent">The base type or interface implemented by events in the domain.</typeparam>
/// <typeparam name="TStoredEvent">The type of the stored event.</typeparam>
/// <typeparam name="TStoredAggregate">The type of the stored aggregate.</typeparam>
partial interface IQueryableEventStore<TAggregateId, TBaseEvent, TStoredAggregate, TStoredEvent> : IEventStore<TAggregateId, TBaseEvent>
	where TAggregateId : IComparable
	where TStoredAggregate : class, IStoredAggregate<TAggregateId>, new()
	where TStoredEvent : class, IStoredEvent<TStoredAggregate, TAggregateId>, new()
{
}