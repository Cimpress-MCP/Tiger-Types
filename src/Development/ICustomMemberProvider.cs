// ReSharper disable CheckNamespace
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace LINQPad
{
    /// <summary>Provides customized information on type members to LINQPad.</summary>
    [ContractClass(typeof(CustomMemberProviderContract))]
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

    [ContractClassFor(typeof(ICustomMemberProvider))]
    abstract class CustomMemberProviderContract
        : ICustomMemberProvider
    {
        CustomMemberProviderContract() { }

        IEnumerable<string> ICustomMemberProvider.GetNames()
        {
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

            throw new NotSupportedException();
        }

        IEnumerable<Type> ICustomMemberProvider.GetTypes()
        {
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);

            throw new NotSupportedException();
        }

        IEnumerable<object> ICustomMemberProvider.GetValues()
        {
            Contract.Ensures(Contract.Result<IEnumerable<object>>() != null);

            throw new NotSupportedException();
        }
    }
}
