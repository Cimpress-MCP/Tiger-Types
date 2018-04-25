// <copyright file="TaskExtensions.cs" company="Cimpress, Inc.">
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
using System.Threading.Tasks;
using JetBrains.Annotations;
using static System.Diagnostics.Contracts.Contract;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="Task{TResult}"/>.</summary>
    public static class TaskExtensions
    {
        /// <summary>Replaces the Result value of this instance with the provided value.</summary>
        /// <typeparam name="TIn">The Result type of <paramref name="taskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type of the replacement value.</typeparam>
        /// <param name="taskValue">The <see cref="Task{TResult}"/> whose result to replace.</param>
        /// <param name="replacement">The value to use as a replacement.</param>
        /// <returns><paramref name="replacement"/>, asynchronously.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="taskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="replacement"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static async Task<TOut> Replace<TIn, TOut>(
            [NotNull, ItemNotNull] this Task<TIn> taskValue,
            [NotNull] TOut replacement)
        {
            if (taskValue is null) { throw new ArgumentNullException(nameof(taskValue)); }
            if (replacement == null) { throw new ArgumentNullException(nameof(replacement)); }

            await taskValue.ConfigureAwait(false);
            return replacement;
        }

        /// <summary>Maps the result of a <see cref="Task{TResult}"/> over a transformation.</summary>
        /// <typeparam name="TIn">The Result type of <paramref name="taskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="taskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="mapper">A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.</param>
        /// <returns>The result of <paramref name="mapper"/>, asynchronously.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="taskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static async Task<TOut> Map<TIn, TOut>(
            [NotNull, ItemNotNull] this Task<TIn> taskValue,
            [NotNull, InstantHandle] Func<TIn, TOut> mapper)
        {
            if (taskValue is null) { throw new ArgumentNullException(nameof(taskValue)); }
            if (mapper is null) { throw new ArgumentNullException(nameof(mapper)); }

            var result = mapper(await taskValue.ConfigureAwait(false));
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Binds the result of a <see cref="Task{TResult}"/> over a transformation.</summary>
        /// <typeparam name="TIn">The Result type of <paramref name="taskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Result type of the return type of <paramref name="binder"/>.</typeparam>
        /// <param name="taskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="binder">An asynchronous transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.</param>
        /// <returns>The result of <paramref name="binder"/>, asynchronously.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="taskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static Task<TOut> Bind<TIn, TOut>(
            [NotNull, ItemNotNull] this Task<TIn> taskValue,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> binder)
        {
            if (taskValue is null) { throw new ArgumentNullException(nameof(taskValue)); }
            if (binder is null) { throw new ArgumentNullException(nameof(binder)); }

            return taskValue.Map(binder).Unwrap();
        }
    }
}
