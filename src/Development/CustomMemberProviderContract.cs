using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LINQPad;

namespace Tiger.Types
{
    [ContractClassFor(typeof(ICustomMemberProvider))]
    abstract class CustomMemberProviderContract
        : ICustomMemberProvider
    {
        CustomMemberProviderContract() { }

        IEnumerable<string> ICustomMemberProvider.GetNames()
        {
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
            
            throw new NotImplementedException();
        }

        IEnumerable<Type> ICustomMemberProvider.GetTypes()
        {
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);

            throw new NotImplementedException();
        }

        IEnumerable<object> ICustomMemberProvider.GetValues()
        {
            Contract.Ensures(Contract.Result<IEnumerable<object>>() != null);

            throw new NotImplementedException();
        }
    }
}