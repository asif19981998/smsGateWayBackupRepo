using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mysms.Models;
using mysms.Models.Auth;
using mysms.Models.ViewModel.Auth;

namespace mysms.Controllers.appControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationIdentityUser> userManager;
        private readonly SignInManager<ApplicationIdentityUser> loginManager;
        private readonly RoleManager<ApplicationIdentityRole> roleManager;

        public AuthController(UserManager<ApplicationIdentityUser> userManager,
           SignInManager<ApplicationIdentityUser> loginManager,
           RoleManager<ApplicationIdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.loginManager = loginManager;
            this.roleManager = roleManager;
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                ApplicationIdentityUser user = new ApplicationIdentityUser();
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FullName = model.FullName;
                

                IdentityResult result = userManager.CreateAsync(user, model.Password).Result;

                if (result.Succeeded)
                {
                    if (!roleManager.RoleExistsAsync("NormalUser").Result)
                    {
                        ApplicationIdentityRole role = new ApplicationIdentityRole();
                        role.Name = "NormalUser";
                        role.Description = "Perform normal operations.";
                        IdentityResult roleResult = roleManager.
                        CreateAsync(role).Result;
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("",
                             "Error while creating role!");
                            return Ok(new ResponseResult { Result = model, IsSuccess = false,Message="Unable To Register" }); 
                        }
                    }

                    userManager.AddToRoleAsync(user,
                                 "NormalUser").Wait();
                    return Ok(new ResponseResult { Result = model, IsSuccess = true, Message = "Successfully Register" });
                }
            }
            return Ok(new ResponseResult { Result = model, IsSuccess = false, Message = "Unable To Register" });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = loginManager.PasswordSignInAsync
                (model.UserName, model.Password,
                  model.RememberMe, false).Result;

                if (result.Succeeded)
                {
                  return  Ok(new ResponseResult { Result = model, IsSuccess = true, Message = "Successfully Login" });

                }

                return Ok(new ResponseResult { Result = model, IsSuccess = false, Message = "Failed To Login" });
            }

            return Ok(new ResponseResult { Result = model, IsSuccess = false, Message = "Failed To Login" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            loginManager.SignOutAsync().Wait();
            return Ok(new ResponseResult { Result = null, IsSuccess = true, Message = "Successfully Logout" });
        }

    }
}
