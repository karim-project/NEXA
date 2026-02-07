using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using movieSystem.ViewModel;
using NEXA.Models;
using NEXA.Repositories.IRepository;
using NEXA.ViewModels;
using System.Threading.Tasks;

namespace NEXA.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<applicationUserOTP> _applicationUserOTPRepository;
        
        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager, IEmailSender emailSender , IRepository<applicationUserOTP> applicationUserOTPRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _applicationUserOTPRepository = applicationUserOTPRepository;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if(!ModelState.IsValid) 
                return View(registerVM);

            var user = new ApplicationUser()
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.Email
            };

            var result = await _userManager.CreateAsync(user , registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

                return View(registerVM);
            }
            //send confirmation mail
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", token, userId = user.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(registerVM.Email, "NEXA - confirm your email !",
             $"<h1>Confirm your email by clicking <a href='{link}' target=\"_blank\">Here</a></h1>");

            return RedirectToAction("Login");
        }


        //***********************************
        //ForgetPassword
        //***********************************

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(forgetPasswordVM);


            var user = await _userManager.FindByNameAsync(forgetPasswordVM.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Username / Email");
                return View(forgetPasswordVM);
            }


            var UserOtps = await _applicationUserOTPRepository.GetAsync(e => e.ApplicationUserId == user.Id);

            var totalOTPs = UserOtps.Count(e => (DateTime.UtcNow - e.CreateAt).TotalHours < 24);

            if (totalOTPs > 3)
            {
                ModelState.AddModelError(string.Empty, "Too Many Atemps");
                return View(forgetPasswordVM);
            }

            var OTP = new Random().Next(1000, 9999).ToString();

            await _applicationUserOTPRepository.AddAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                CreateAt = DateTime.UtcNow,
                Isvalid = true,
                OTP = OTP,
                ValidTo = DateTime.UtcNow.AddDays(1),
            });
            await _applicationUserOTPRepository.CommitAsync();


            await _emailSender.SendEmailAsync(user.Email!, "NEXA - Reset your password !",
           $"<h1>Use this OTP: {OTP} to Reset Your Account , Dont share it</h1>");


            return RedirectToAction("ValidateOTP", new { UserId = user.Id });
        }


        //***********************************
        //ValidateOTP
        //***********************************
        [HttpGet]
        public IActionResult ValidateOTP(string UserId)
        {
            return View(new ValidateOTPVM
            {
                ApplicationUserId = UserId
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOTP(ValidateOTPVM validateOTPVM)
        {
            var OTP = await _applicationUserOTPRepository.GetOneAsync(e => e.ApplicationUserId == validateOTPVM.ApplicationUserId
            && e.OTP == validateOTPVM.OTP && e.Isvalid);

            if (OTP is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid OTP");
                return RedirectToAction(nameof(validateOTPVM), new { UserId = validateOTPVM.ApplicationUserId });
            }
            return RedirectToAction("NewPassword", new { UserId = OTP.ApplicationUserId });
        }


            //***********************************
            //NewPassword
            //***********************************
            [HttpGet]
        public IActionResult NewPassword(string UserId)
        {
            return View(new NewPasswordVM
            {
                ApplicationUserId = UserId
            });
        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordVM newPasswordVM)
        {

            var user = await _userManager.FindByIdAsync(newPasswordVM.ApplicationUserId);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email");
                return View(newPasswordVM);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, newPasswordVM.Password);


            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Code);
                }

                return View(newPasswordVM);
            }

            return RedirectToAction("Login");
        }

        //***********************************
        //ResendEmailConfirmation
        //***********************************

        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM resendemailconfirmation)
        {
            if (!ModelState.IsValid)
                return View(resendemailconfirmation);


            var user = await _userManager.FindByNameAsync(resendemailconfirmation.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email");
                return View(resendemailconfirmation);
            }

            if (user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Already Confirmed!!");
                return View(resendemailconfirmation);
            }


            //send confirmation mail
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", token, userId = user.Id }, Request.Scheme);


            await _emailSender.SendEmailAsync(user.Email!, "NEXA - confirm your email !",
                $"<h1>Confirm your email by clicking <a href='{link}' target=\"_blank\">Here</a></h1>");


            return RedirectToAction("Login");
        }



        //***********************************
        //ConfirmEmail
        //***********************************

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                TempData["error-notification"] = "Invalid User.";
                return RedirectToAction("Login");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);


            if (!result.Succeeded)
                TempData["error-notification"] = "Invalid OR Expired Token";
            else
                TempData["success-notification"] = "Confirm Email Successfully";

            return RedirectToAction("Login");
        }



        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) 
                return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.Email);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Username / Email Or Password");
                return View(loginVM);
            }

           var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, lockoutOnFailure : true);

            if(!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Too many Atemp , Please try again after 5 minutes");
                }
                else if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Please, confirm your email first");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username / Email Or Password");
                }

                return View(loginVM);
            }

            return RedirectToAction("Index" , "Home" , new {area = "Employee"});
        }
    }
}
