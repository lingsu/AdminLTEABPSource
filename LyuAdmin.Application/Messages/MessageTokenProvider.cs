using System.Collections.Generic;
using System.Web;
using Abp.Runtime.Session;
using Lyu.Abp.Core;
using Lyu.Abp.Core.Messages;
using LyuAdmin.Users;

namespace LyuAdmin.Messages
{
    public class MessageTokenProvider: IMessageTokenProvider
    {
        private readonly IWebHelper _webHelper;

        public MessageTokenProvider(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

    

        public void AddUserTokens(IList<Token> tokens, User user)
        {
            tokens.Add(new Token("User.Email", user.EmailAddress));
            tokens.Add(new Token("User.Username", user.UserName));
            tokens.Add(new Token("User.Surname", user.Surname));
            tokens.Add(new Token("User.Name", user.Name));

            //note: we do not use SEO friendly URLS because we can get errors caused by having .(dot) in the URL (from the email address)
            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            string passwordRecoveryUrl = string.Format("{0}passwordrecovery/confirm?token={1}&email={2}", _webHelper.GetStoreHost(false), user.PasswordResetCode, HttpUtility.UrlEncode(user.EmailAddress));
            string accountActivationUrl = string.Format("{0}customer/activation?token={1}&email={2}", _webHelper.GetStoreHost(false), user.EmailConfirmationCode, HttpUtility.UrlEncode(user.EmailAddress));
            tokens.Add(new Token("Customer.PasswordRecoveryURL", passwordRecoveryUrl, true));
            tokens.Add(new Token("Customer.AccountActivationURL", accountActivationUrl, true));

        }
    }
}