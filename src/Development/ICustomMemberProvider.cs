// ReSharper disable CheckNamespace
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace LINQPad
{
    /// <summary>Provides customized information on type members to LINQPad.</summary>
    interface ICustomMemberProvider
    {
        /// <summary>Gets a collection of member names.</summary>
        /// <returns>A collection of member names.</returns>
        [NotNull, ItemNotNull, Pure]
        IEnumerable<string> GetNames();

        /// <summary>Gets a collection of member types.</summary>
        /// <returns>A collection of member types.</returns>
        [NotNull, ItemNotNull, Pure]
        IEnumerable<Type> GetTypes();

        /// <summary>Gets a collection of member values.</summary>
        /// <returns>A collection of member values.</returns>
        [NotNull, ItemCanBeNull, Pure]
        IEnumerable<object> GetValues();
    }
}
