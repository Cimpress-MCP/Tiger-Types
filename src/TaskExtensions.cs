﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="Task{TResult}"/>.</summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class TaskExtensions
    {
        /// <summary>Applies a <see cref="Task"/> over a function.</summary>
        /// <typeparam name="TOut">The return type of <paramref name="applier"/>.</typeparam>
        /// <param name="task">The <see cref="Task"/> to be applied.</param>
        /// <param name="applier">A function producing a <typeparamref name="TOut"/>.</param>
        /// <returns>The return value of <paramref name="applier"/>, asynchronously.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="applier"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static async Task<TOut> Apply<TOut>(
            [NotNull] this Task task,
            [NotNull, InstantHandle] Func<TOut> applier)
        {
            if (task == null) { throw new ArgumentNullException(nameof(task)); }
            if (applier == null) { throw new ArgumentNullException(nameof(applier)); }

            await task.ConfigureAwait(false);
            return applier();
        }

        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> asynchronously over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Result type of <paramref name="taskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="taskValue">The <see cref="Task{TResult}"/> to be mapped.</param>
        /// <param name="mapper">A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.</param>
        /// <returns>The result of <paramref name="mapper"/>, asynchronously.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="taskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static async Task<TOut> Map<TIn, TOut>(
            [NotNull, ItemNotNull] this Task<TIn> taskValue,
            [NotNull, InstantHandle] Func<TIn, TOut> mapper)
        {
            if (taskValue == null) { throw new ArgumentNullException(nameof(taskValue)); }
            if (mapper == null) { throw new ArgumentNullException(nameof(mapper)); }

            return mapper(await taskValue.ConfigureAwait(false));
        }

        /// <summary>
        /// Performs actions in a sequence.
        /// </summary>
        /// <typeparam name="TOut">The return type of <paramref name="binder"/>.</typeparam>
        /// <param name="task">The <see cref="Task"/> to perform first.</param>
        /// <param name="binder">A function producing a <typeparamref name="TOut"/>, asynchronously.</param>
        /// <returns>The return value of <paramref name="binder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static async Task<TOut> Then<TOut>(
            [NotNull] this Task task,
            [NotNull, InstantHandle] Func<Task<TOut>> binder)
        {
            if (task == null) { throw new ArgumentNullException(nameof(task)); }
            if (binder == null) { throw new ArgumentNullException(nameof(binder)); }

            await task.ConfigureAwait(false);
            return await binder().ConfigureAwait(false);
        }

        /// <summary>
        /// Binds the result of a <see cref="Task{TResult}"/> asynchronously over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Result type of <paramref name="taskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Result type of the return type of <paramref name="binder"/>.</typeparam>
        /// <param name="taskValue">The <see cref="Task{TResult}"/> to be bound.</param>
        /// <param name="binder">An asynchronous transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.</param>
        /// <returns>The result of <paramref name="binder"/>, asynchronously.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="taskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static async Task<TOut> Bind<TIn, TOut>(
            [NotNull, ItemNotNull] this Task<TIn> taskValue,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> binder)
        {
            if (taskValue == null) { throw new ArgumentNullException(nameof(taskValue)); }
            if (binder == null) { throw new ArgumentNullException(nameof(binder)); }

            return await binder(await taskValue.ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
