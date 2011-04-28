﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Core.Security;

namespace WebUI.Controllers
{
    public class FormAuthService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie, IEnumerable<string> roles)
        {
            var str = roles.Aggregate(string.Empty, (current, role) => current + (role + ","));

            str.Remove(str.Length - 1, 1);

            var authTicket = new FormsAuthenticationTicket(
                1,
                userName,  //user id
                DateTime.Now,
                DateTime.Now.AddMinutes(20),  // expiry
                createPersistentCookie,
                str,
                "/");

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));

            if (authTicket.IsPersistent)
            {
                cookie.Expires = authTicket.Expiration;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}