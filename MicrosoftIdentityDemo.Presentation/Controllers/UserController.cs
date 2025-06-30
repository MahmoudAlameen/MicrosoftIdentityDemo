using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MicrosoftIdentityDemo.Core.Domain.IdentityEntites;
using MicrosoftIdentityDemo.Core.DTOs;
using MicrosoftIdentityDemo.Core.Enums;
using MicrosoftIdentityDemo.Core.Services.Implementation;
using MicrosoftIdentityDemo.Core.Services.Implementation.Interfaces;

namespace MicrosoftIdentityDemo.Presentation.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        UserManager<ApplicationUser> UserManager { get; set; }
        SignInManager<ApplicationUser> SignInManager { get; set; }
        RoleManager<ApplicationRole> RoleManager { get; set; }
        IUserService UsersService { get; set; }
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService usersService, RoleManager<ApplicationRole> roleManager)
        { 
            UserManager = userManager;
            SignInManager = signInManager;
            UsersService = usersService;
            RoleManager = roleManager;
        }

        [HttpGet("register")]
        [AllowAnonymous]
        public IActionResult RegisterView()
        {
            return View("Register");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                var modelerrors = ModelState
                      .Where(ms => ms.Value.Errors.Count > 0)
                      .ToDictionary(
                          kvp => kvp.Key,
                          kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                      );

                return BadRequest(new { errors = modelerrors });
            }
            var result = await UsersService.AddUser(model);
            if(!result.IsValid)
            {
                return BadRequest(new { errors = result.ErrorMessages.ToList() });
            }
            
            return RedirectToAction("LoginView", new LoginDTO { Email = model.Email, Password = model.Password});
        }

        [HttpGet("login-view")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginView(LoginDTO model)
        {
            return View("Login", model);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                var modelerrors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new { errors = modelerrors });
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Login", "invalid username or password");
                return View("login",model);
            }
            return RedirectToAction("users-list", "users");
        }

        [Route("")]
        [HttpGet("users-list")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await UsersService.GetUsers();
            return View("UsersList",users);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            SignInManager.SignOutAsync();
            return RedirectToAction("login-view", "users");
        }

        [HttpGet("user-details")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails(string email)
        {
            var result = await UsersService.GetUserDetails(email);
            if(result == null)
            {
                ModelState.AddModelError("userNotFound", "user not found ");
                return RedirectToAction("users-list", "users");
            }

            return View("UserDetails", result);
        }

        public async Task<IActionResult> EditUser(RegisterDTO model)
        {

            return Ok();
        }

    }


}
