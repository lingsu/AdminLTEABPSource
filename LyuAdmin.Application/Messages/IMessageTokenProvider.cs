using System.Collections.Generic;
using Abp.Dependency;
using Lyu.Abp.Core.Messages;
using LyuAdmin.Users;

namespace LyuAdmin.Messages
{
    public interface IMessageTokenProvider: ITransientDependency
    {
        void AddUserTokens(IList<Token> tokens, User user);
    }
}