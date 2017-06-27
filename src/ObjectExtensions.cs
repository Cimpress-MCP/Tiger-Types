﻿// <copyright file="ObjectExtensions.cs" company="Cimpress, Inc.">
//   Copyright 2017 Cimpress, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>

using System;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="object"/>.</summary>
    public static class ObjectExtensions
    {
        /// <summary>Invokes <paramref name="piper"/> with <paramref name="value"/> as its argument.</summary>
        /// <typeparam name="TIn">The type of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TOut">The type to produce.</typeparam>
        /// <param name="value">The value to be piped.</param>
        /// <param name="piper">A function to invoke with <paramref name="value"/> as its argument.</param>
        /// <returns>The value of invoking <paramref name="value"/> to <paramref name="piper"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="piper"/> is <see langword="null"/>.</exception>
        public static TOut Pipe<TIn, TOut>(
            [NotNull] this TIn value,
            [NotNull, InstantHandle] Func<TIn, TOut> piper)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (piper == null) { throw new ArgumentNullException(nameof(piper)); }

            return piper(value);
        }
    }
}
