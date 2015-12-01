using System.Collections.Generic;
using Lyu.Abp.Core.Messages;
using LyuAdmin.Users;

namespace LyuAdmin.Messages
{
    public interface IMessageTokenProvider
    {
        void AddUserTokens(IList<Token> tokens, User user);
    }
}