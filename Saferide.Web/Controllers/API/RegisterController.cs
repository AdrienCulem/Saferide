using System;

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Saferide.Web.Models;
using Saferide.Web.Models.Poco;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Saferide.Web.Controllers.API
{
    public class RegisterController : ApiController
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        //[ResponseType(typeof(Incident))]
        //public async Task Register(RegisterUser userToRegister)
        //{
        //    var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_dbContext));
        //    var signInManage = new ApplicationSignInManager(userManager, );
        //    var user = new ApplicationUser { UserName = userToRegister.Email, Email = userToRegister.Email, Firstname = userToRegister.Firstame, Lastname = userToRegister.Lastname};
        //    var result = await userManager.CreateAsync(user, userToRegister.Password);
        //    if (result.Succeeded)
        //    {
        //        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //        // Send an email with this link
        //        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //        var callbackUrl = Url.Action(
        //            "ConfirmEmail", "Account",
        //            new { userId = user.Id, code = code },
        //            protocol: Request?.Url?.Scheme);

        //        EmailService emailClient = new EmailService();
        //        IdentityMessage mess = new IdentityMessage()
        //        {
        //            Body = callbackUrl,
        //            Destination = model.Email,
        //            Subject = "Saferide! Confirme ton email"
        //        };
        //        await emailClient.SendAsync(mess);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    AddErrors(result);
        //    var client = new SendGridClient(Keys.SendGridKey);
        //    var msg = new SendGridMessage()
        //    {
        //        From = new EmailAddress("cyberhelpmons@gmail.com", "CyberhelpTeam"),
        //        Subject = "Un nouveau dossier a été envoyé",
        //        HtmlContent = $"Bonjour,<br><br>Un nouveau dossier a été envoyé par <strong>{userToRegister.Firstame}</strong>"
        //    };
        //    msg.AddTo(new EmailAddress(userToRegister.Email, userToRegister.Firstame));
        //}
    }
}
