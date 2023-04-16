// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DollyHotel.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace DollyHotel.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DollyHotel.Server.Services.IEmailSender _sender;
        public IConfiguration Configuration { get; }

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, DollyHotel.Server.Services.IEmailSender sender, IConfiguration configuration)
        {
            _userManager = userManager;
            _sender = sender;
            Configuration = configuration;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool DisplayConfirmAccountLink { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            //DisplayConfirmAccountLink = true;
            //DisplayConfirmAccountLink = false;

            //if (DisplayConfirmAccountLink)
            //{
            //    var userId = await _userManager.GetUserIdAsync(user);
            //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //    EmailConfirmationUrl = Url.Page(
            //        "/Account/ConfirmEmail",
            //        pageHandler: null,
            //        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
            //        protocol: Request.Scheme);
            //}
            //else
            //{
            //    string smtpAddress = Configuration["Email:SmtpAddress"];
            //    string smtpPort = Configuration["Email:SmtpPort"];
            //    string username = Configuration["Email:Username"];
            //    string password = Configuration["Email:Password"];

            //    var userId = await _userManager.GetUserIdAsync(user);

            //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //    EmailConfirmationUrl = Url.Page(
            //        "/Account/ConfirmEmail",
            //        pageHandler: null,
            //        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
            //        protocol: Request.Scheme);

            //    string subject = "SUCCESSFUL REGISTRATION ON DOLLY HOTEL APP";
            //    string body = "Dear " + user.FirstName + " " + user.LastName + ",<br/>" + "This is to inform you that you have successfully registered on our app.Please click the link below to confirm your account.<br/>" + EmailConfirmationUrl;
            //    await _sender.SendHTMLEmailAsync(username, user.Email, subject, body, smtpAddress, smtpPort, username, password);
            //}

            string smtpAddress = Configuration["Email:SmtpAddress"];
            string smtpPort = Configuration["Email:SmtpPort"];
            string username = Configuration["Email:Username"];
            string password = Configuration["Email:Password"];

            var userId = await _userManager.GetUserIdAsync(user);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            EmailConfirmationUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            string subject = "SUCCESSFUL REGISTRATION ON DOLLY HOTEL APP";
            string body = "Dear " + user.FirstName + " " + user.LastName + ",<br/>" + "This is to inform you that you have successfully registered on our app.Your User Details Is As Follows:<br/>Username:"+user.Email+"<br/>Password:"+user.Password+"<br/>Please click the link below to confirm your account.<br/>" + EmailConfirmationUrl;
            await _sender.SendHTMLEmailAsync(username, user.Email, subject, body, smtpAddress, smtpPort, username, password);

            return Page();
        }
    }
}
