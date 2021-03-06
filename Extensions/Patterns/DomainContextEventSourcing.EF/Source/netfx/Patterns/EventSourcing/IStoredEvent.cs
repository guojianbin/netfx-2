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
/// Represents a persisted event in an event store.
/// </summary>
/// <remarks>
/// The core interface does not expose the event payload, 
/// as its representation as well as specific storage type
/// and serialization/deserialization can vary wildly 
/// across store implementation. Imposing a particular 
/// representation via this interface would be unnecessarily 
/// restrictive and is not needed for the rest of the APIs.
/// </remarks>
/// <typeparam name="TId">The type of identifiers used by the aggregate roots.</typeparam>
/// <nuget id="netfx-Patterns.EventSourcing.Core"/>
partial interface IStoredEvent<TId>
	where TId : IComparable
{
	/// <summary>
	/// Gets the aggregate id that the event applies to.
	/// </summary>
	TId AggregateId { get; }

	/// <summary>
	/// Gets the type of the aggregate root that this event applies to.
	/// </summary>
	string AggregateType { get; }

	/// <summary>
	/// Gets the type of the event.
	/// </summary>
	string EventType { get; }

	/// <summary>
	/// Gets the UTC timestamp of the event.
	/// </summary>
	DateTime Timestamp { get; }
}
