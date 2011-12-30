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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Reflection;
using System.Diagnostics;

namespace NetFx.Patterns.EventSourcing.Tests
{
	public class Sample
	{
		[Fact(Skip = "Demo code")]
		public void WhenEventPersisted_ThenCanObserveIt()
		{
			var id1 = Guid.NewGuid();
			var id2 = Guid.NewGuid();

			var store = new MemoryEventStore<Guid, DomainEvent>();
			var product = new Product(id1, "DevStore");
			product.Publish(1);
			product.Publish(2);
			product.Publish(3);

			store.SaveChanges(product);

			product = new Product(id2, "WoVS");
			product.Publish(1);
			product.Publish(2);

			store.SaveChanges(product);

			var product2 = new Product(store.Query().For<Product>(id2).Execute());

			Assert.Equal(product.Id, product2.Id);
			Assert.Equal(product.Version, product2.Version);

			var events = store.Query().For<Product>(id1).OfType<Product.PublishedEvent>().Execute();
			Assert.Equal(3, events.Count());

			Console.WriteLine("For first product, of type published:");
			foreach (var e in events)
			{
				Console.WriteLine("\t" + e);
			}

			events = store.Query().For<Product>().OfType<Product.PublishedEvent>().Execute();
			Assert.Equal(5, events.Count());

			Console.WriteLine();
			Console.WriteLine("For all products, of type published:");
			foreach (var e in events)
			{
				Console.WriteLine("\t" + e);
			}

			events = store.Query().For<Product>().Execute();
			Assert.Equal(7, events.Count());
			
			Console.WriteLine();
			Console.WriteLine("For all products, all event types:");
			foreach (var e in events)
			{
				Console.WriteLine("\t" + e);
			}

			events = store.Query().OfType<Product.CreatedEvent>().Execute();
			Assert.Equal(2, events.Count());

			Console.WriteLine();
			Console.WriteLine("Products created events:");
			foreach (var e in events)
			{
				Console.WriteLine("\t" + e);
			}
		}
	}
}