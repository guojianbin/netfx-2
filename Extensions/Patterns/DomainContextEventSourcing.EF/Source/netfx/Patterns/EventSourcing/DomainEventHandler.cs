﻿#region BSD License
/* 
Copyright (c) 2010, NETFx
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

* Neither the name of Clarius Consulting nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
using System;

/// <summary>
/// Base class part of the infrastructure. Concrete 
/// handlers should inherit <see cref="DomainEventHandler{TAggregateId, TEventArgs}"/> instead.
/// </summary>
/// <nuget id="netfx-Patterns.EventSourcing.Core" />
abstract partial class DomainEventHandler
{
	/// <summary>
	/// Invocation style hint that the <see cref="IDomainEventBus{TId}"/> implementation
	/// can use to invoke a handler asynchronously with regards to the event publisher.
	/// </summary>
	public virtual bool IsAsync { get { return false; } }

	/// <summary>
	/// Gets the type of the event argument this handler can process.
	/// </summary>
	public abstract Type EventType { get; }
}

/// <summary>
/// Base class for domain event handlers that handle a specific type of event.
/// </summary>
/// <typeparam name="TAggregateId">Type of identifier used by the aggregate roots.</typeparam>
/// <typeparam name="TEventArgs">Type of event argument this handler can process.</typeparam>
/// <nuget id="netfx-Patterns.EventSourcing.Core" />
abstract partial class DomainEventHandler<TAggregateId, TEventArgs> : DomainEventHandler
	where TEventArgs : TimestampedEventArgs
{
	/// <summary>
	/// Handles the specified event.
	/// </summary>
	public abstract void Handle(TAggregateId aggregateId, TEventArgs @event);

	/// <summary>
	/// Gets the type of the event this handler can process, which equals 
	/// the generic type parameter of <see cref="DomainEventHandler{TAggregateId, TEventArgs}"/>.
	/// </summary>
	public override Type EventType { get { return typeof(TEventArgs); } }
}