// <copyright file="Error.cs" company="Cimpress, Inc.">
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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.Runtime.InteropServices.LayoutKind;
using static System.StringComparer;

namespace Tiger.Types
{
    /// <summary>Represents an unhandlable, non-exceptional error.</summary>
    [PublicAPI]
    [StructLayout(Auto)]
    public readonly struct Error
        : IEquatable<Error>
    {
        const string MessageKey = "Message";

        static readonly ImmutableDictionary<string, object> s_emptyContext =
    ImmutableDictionary.Create<string, object>(Ordinal);

        readonly ImmutableDictionary<string, object> _context;

        /// <summary>Initializes a new instance of the <see cref="Error"/> struct.</summary>
        /// <param name="message">The message to apply.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
        public Error([NotNull] string message)
            : this(message, s_emptyContext)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Error"/> struct.</summary>
        /// <param name="message">The message to apply.</param>
        /// <param name="context">The context under which the operation failed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
        public Error([NotNull] string message, [CanBeNull] object context)
            : this(message, context?.ToDictionary(Ordinal))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Error"/> struct.</summary>
        /// <param name="message">The message to apply.</param>
        /// <param name="context">The context under which the operation failed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
        public Error([NotNull] string message, [CanBeNull] IEnumerable<KeyValuePair<string, object>> context)
        {
            if (message is null) { throw new ArgumentNullException(nameof(message)); }

            var calculatedContext = context?.ToImmutableDictionary() ?? s_emptyContext;
            _context = calculatedContext.SetItem(MessageKey, message);
        }

        /// <summary>Initializes a new instance of the <see cref="Error"/> struct.</summary>
        /// <param name="message">The message to apply.</param>
        /// <param name="context">The context under which the operation failed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
        public Error([NotNull] string message, [CanBeNull] IEnumerable<KeyValuePair<string, string>> context)
        {
            if (message is null) { throw new ArgumentNullException(nameof(message)); }

            var calculatedContext = context?.ToImmutableDictionary(kvp => kvp.Key, kvp => (object)kvp.Value) ?? s_emptyContext;
            _context = calculatedContext.SetItem(MessageKey, message);
        }

        /// <summary>Gets a short description of the error.</summary>
        /// <remarks>This is intended to be human-readable.</remarks>
        [NotNull]
        public string Message => (string)_context?.GetValueOrDefault(MessageKey, string.Empty) ?? string.Empty;

        /// <summary>
        /// Gets the context under which the error occurred.
        /// </summary>
        [NotNull]
        public IImmutableDictionary<string, object> Context => _context?.Remove(MessageKey) ?? s_emptyContext;

        #region Operators

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Error left, Error right) => left.Equals(right);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Error left, Error right) => !left.Equals(right);

        #endregion

        #region Implementations

        /// <inheritdoc/>
        [Pure]
        public bool Equals(Error other) => EqualsCore(other);

        #endregion

        #region Overrides

        #region object

        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => Message; // note(cosborn) ECMAScript prefixes with "Error: ". Eh.

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => _context?.GetHashCode() ?? 0;

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Error error && EqualsCore(error);

        [Pure]
        bool EqualsCore(in Error other)
        {
            if (ReferenceEquals(_context, other._context)) { return true; }
            if (_context is null || other._context is null)
            {
                return false;
            }

            return _context.SequenceEqual(other._context);
        }

        #endregion

        #endregion
    }
}
