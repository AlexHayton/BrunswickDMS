using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Membership.OpenAuth;

namespace BrunswickDMS
{
    internal static class AuthConfig
    {
        public static void RegisterOpenAuth()
        {
            // See http://go.microsoft.com/fwlink/?LinkId=252803 for details on setting up this ASP.NET
            // application to support logging in via external services.

            //OpenAuth.AuthenticationClients.AddTwitter(
            //    consumerKey: "your Twitter consumer key",
            //    consumerSecret: "your Twitter consumer secret");

            //OpenAuth.AuthenticationClients.AddFacebook(
            //    appId: "your Facebook app id",
            //    appSecret: "your Facebook app secret");

            //OpenAuth.AuthenticationClients.AddMicrosoft(
            //    clientId: "00000000480E88B3",
            //    clientSecret: "ADb8NaYry0XLklPfjYIbGssIYEa4QkZG");

            //OpenAuth.AuthenticationClients.AddGoogle();
        }
    }
}