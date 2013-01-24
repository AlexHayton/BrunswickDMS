using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLayer.Models;
using Microsoft.AspNet.Membership.OpenAuth;

namespace BrunswickDMS.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            // Set the auth cookie here.
            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, createPersistentCookie: false);

            // Set the user's data in the database too.
            this.PersistUserData(RegisterUser);

            // Redirect to the next page.
            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (!OpenAuth.IsLocalUrl(continueUrl))
            {
                continueUrl = "~/";
            }
            Response.Redirect(continueUrl);
        }

        /// <summary>
        /// Save the user's data into the database using the Entity Framework
        /// </summary>
        /// <param name="wizard">The wizard that took the user's details</param>
        protected void PersistUserData(CreateUserWizard wizard)
        {
            TextBox FirstNameInput = wizard.CreateUserStep.ContentTemplateContainer.FindControl("FirstName") as TextBox;
            TextBox LastNameInput = wizard.CreateUserStep.ContentTemplateContainer.FindControl("LastName") as TextBox;

            using (DMSContext database = new DMSContext())
            {
                // Check whether this user already exists first...
                // Create a new row if it doesn't exist or update the old one if it does.
                var existingUser = database.Users.SingleOrDefault(
                    u => u.UserName == wizard.UserName);
                if (existingUser == null)
                {
                    User user = new User();
                    user.UserName = wizard.UserName;
                    user.Email = wizard.Email;
                    user.FirstName = FirstNameInput.Text;
                    user.LastName = LastNameInput.Text;

                    database.Users.Add(user);
                    database.SaveChanges();
                }
                else
                {
                    existingUser.UserName = wizard.UserName;
                    existingUser.Email = wizard.Email;
                    existingUser.FirstName = FirstNameInput.Text;
                    existingUser.LastName = LastNameInput.Text;
                    database.SaveChanges();
                }
            }
        }
    }
}