using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tiger.Types.Properties;
using static System.Diagnostics.Contracts.Contract;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="Task{TResult}"/>.</summary>
    [SuppressMessage("ReSharper", "ExceptionNotThrown", Justification = "R# doesn't understand Code Contracts.")]
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
            Requires<ArgumentNullException>(task != null);
            Requires<ArgumentNullException>(applier != null);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            await task.ConfigureAwait(false);
            var result = applier();
            Assume(result != null, Resources.ResultIsNull);
            return result;
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
            Requires<ArgumentNullException>(taskValue != null);
            Requires<ArgumentNullException>(mapper != null);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            var result = mapper(await taskValue.ConfigureAwait(false));
            Assume(result != null, Resources.ResultIsNull);
            return result;
        }

        /// <summary>
        /// Performs actions in a sequence.
        /// </summary>
        /// <typeparam name="TOut">The return type of <paramref name="thenner"/>.</typeparam>
        /// <param name="task">The <see cref="Task"/> to perform first.</param>
        /// <param name="thenner">A function producing a <typeparamref name="TOut"/>, asynchronously.</param>
        /// <returns>The return value of <paramref name="thenner"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="thenner"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static async Task<TOut> Then<TOut>(
            [NotNull] this Task task,
            [NotNull, InstantHandle] Func<Task<TOut>> thenner)
        {
            Requires<ArgumentNullException>(task != null);
            Requires<ArgumentNullException>(thenner != null);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            await task.ConfigureAwait(false);
            var result = await thenner().ConfigureAwait(false);
            Assume(result != null, Resources.ResultIsNull);
            return result;
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
            Requires<ArgumentNullException>(taskValue != null);
            Requires<ArgumentNullException>(binder != null);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            var result = await binder(await taskValue.ConfigureAwait(false)).ConfigureAwait(false);
            Assume(result != null, Resources.ResultIsNull);
            return result;
        }
    }
}
