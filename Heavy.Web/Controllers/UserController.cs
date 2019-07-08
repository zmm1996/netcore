using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Data;
using Heavy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Heavy.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        public readonly SignInManager<ApplicationUser> _SignInManager;
        public UserController(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager
            )
        {
            _UserManager = _userManager;
            _SignInManager = _signInManager;
        }

       
        [Authorize]
        public IActionResult Index()
        {
            var userList = _UserManager.Users.ToList();
            return View(userList);
        }

        [Authorize]
        public IActionResult AddUser()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateViewModel userCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userCreateViewModel.UserName,
                    Email = userCreateViewModel.Email,
                     Birthday= userCreateViewModel.Birthday,
                      IdCardNo= userCreateViewModel.IdCardNo
                };

                var result = await _UserManager.CreateAsync(user, userCreateViewModel.PassWord);

                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }

                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(userCreateViewModel);

            }
            else
            {
                return View(userCreateViewModel);
            }
        }


        public async Task<IActionResult>  EditUser(string id)
        {
          var user= await _UserManager.FindByIdAsync(id);

            UserCreateViewModel userCreateViewModel = new UserCreateViewModel
            {
                id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                  Birthday=user.Birthday,
                   IdCardNo=user.IdCardNo
            };

            return View(userCreateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserCreateViewModel userCreateViewModel)
        {
            if (ModelState.IsValid)
            {
               

                var userModel = await _UserManager.FindByIdAsync(userCreateViewModel.id);
                

                if (userModel!=null)
                {
                    userModel.UserName = userCreateViewModel.UserName;
                    userModel.Email = userCreateViewModel.Email;
                    userModel.IdCardNo = userCreateViewModel.IdCardNo;
                    userModel.Birthday = userCreateViewModel.Birthday;
                    var result= await _UserManager.UpdateAsync(userModel);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("index");
                    }

                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "用户不存在");
                }

                
                return View(userCreateViewModel);

            }
            else
            {
                return View(userCreateViewModel);
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {

            var user = await _UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "删除用户是发生错误");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "用户为找到");

            }
            return RedirectToAction("index");

        }

    }
}