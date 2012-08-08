﻿#region BSD License
/* 
Copyright (c) 2011, NETFx
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list 
  of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this 
  list of conditions and the following disclaimer in the documentation and/or other 
  materials provided with the distribution.

* Neither the name of Clarius Consulting nor the names of its contributors may be 
  used to endorse or promote products derived from this software without specific 
  prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
DAMAGE.
*/
#endregion

namespace NetFx.Patterns.Adapter
{
    using System;

    /// <summary>
    /// Static facade for the <see cref="IAdapterService"/>.
    /// </summary>
    ///	<nuget id="netfx-Patterns.Adapter.Interfaces" />
    static partial class Adapt
    {
        static AmbientSingleton<IAdapterService> singleton = new AmbientSingleton<IAdapterService>();

        /// <summary>
        /// Tries to adapt the given <paramref name="instance"/> to the requested type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>The adapted object if an adapter for the source could be found; <see langword="null"/> otherwise.</returns>
        public static T To<T>(object instance) where T : class
        {
            if (singleton.Value == null)
                throw new InvalidOperationException("Initialize the service first by using the AdaptInitializer.");

            return singleton.Value.To<T>(instance);
        }

        internal static void Initialize(IAdapterService service)
        {
            if (singleton.Value != null)
                throw new InvalidOperationException("Already initialized.");

            singleton.Value = service;
        }
    }
}