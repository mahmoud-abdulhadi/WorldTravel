using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorldTravel.Models;
using WorldTravel.ViewModels;

namespace WorldTravel.Controllers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthController : Controller
    {
        private SignInManager<WorldUser> _signInManager;

        private UserManager<WorldUser> _userManager; 

       

        public AuthController(SignInManager<WorldUser> signInManager,UserManager<WorldUser> userManager)
        {
            _signInManager = signInManager;

            _userManager = userManager; 
        }

        public IActionResult Login()
        {

            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "App"); 
            }

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel vm,string ReturnUrl)
        {

            if(ModelState.IsValid)
            {
                var signInResult = await  _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);

                if(signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(ReturnUrl))
                    {
                        return RedirectToAction("Trips", "App");
                    }
                    else
                    {

                        return Redirect(ReturnUrl); 
                    }
                }
                else
                {

                    ModelState.AddModelError("", "Username or Password Incorrect"); 
                }

            }

            return View();
        }

        public IActionResult Register()
        {

            if (User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Trips", "App");
            }

            return View();

        }
        [HttpPost]
        public async  Task<IActionResult> Register(RegisterViewModel vm,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new WorldUser{ UserName = vm.Username ,Email = vm.Email };
                var result = await _userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Trips", "App");



                }else
                {

                    foreach(var error in result.Errors)
                    {


                        ModelState.AddModelError("", error.Description);
                    }
                }

                

            }
            else
            {
                ModelState.AddModelError("","Incorrect Information Supplied!");
            }

            return View();
        }

        public async Task<ActionResult> Logout()
        {

            if(User.Identity.IsAuthenticated)
            {

                await _signInManager.SignOutAsync(); 

            }

            return RedirectToAction("Index", "App"); 
        }
    }
}
