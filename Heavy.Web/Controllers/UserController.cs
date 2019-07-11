using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Data;
using Heavy.Web.Models;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Heavy.Web.Controllers
{
    [Authorize(Roles = "administrators")]
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



        public IActionResult Index()
        {


            var userList = _UserManager.Users.ToList();
            return View(userList);
        }


        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateViewModel userCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userCreateViewModel.UserName,
                    Email = userCreateViewModel.Email,
                    Birthday = userCreateViewModel.Birthday,
                    IdCardNo = userCreateViewModel.IdCardNo
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


        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);

            var claims = await _UserManager.GetClaimsAsync(user);
            UserCreateViewModel userCreateViewModel = new UserCreateViewModel
            {
                id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Birthday = user.Birthday,
                IdCardNo = user.IdCardNo,
                ClaimNames = claims.Select(x => x.Value).ToList()
            };


            return View(userCreateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserCreateViewModel userCreateViewModel)
        {
            if (ModelState.IsValid)
            {


                var userModel = await _UserManager.FindByIdAsync(userCreateViewModel.id);


                if (userModel != null)
                {
                    userModel.UserName = userCreateViewModel.UserName;
                    userModel.Email = userCreateViewModel.Email;
                    userModel.IdCardNo = userCreateViewModel.IdCardNo;
                    userModel.Birthday = userCreateViewModel.Birthday;
                    var result = await _UserManager.UpdateAsync(userModel);
                    if (result.Succeeded)
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

        public async Task<IActionResult> ManagerClaims(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("index");
            }
            var vm = new CreateClaimsViewModel
            {
                UserId = id,

            };

            var claims = await _UserManager.GetClaimsAsync(user);

            var claimsList = claims.Select(x => x.Value).ToList();

            var ss = new List<string>();


            //foreach (var item1 in ClaimsType.ClaimType)
            //{
            //    if (claimsList.IndexOf(item1)==-1)
            //    {
            //        ss.Add(item1);
            //    }
            //}

            foreach (var item1 in ClaimsType.ClaimType)
            {
                var val_st = false;

                foreach (var item in claimsList)
                {
                    if (item == item1)
                    {
                        val_st = true;
                    }
                }
                if (!val_st)
                {
                    ss.Add(item1);

                }

            }


            //13
            //1234

            vm.AllClaims = ClaimsType.ClaimType.Except(claimsList).ToList();




            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> ManagerClaims(CreateClaimsViewModel createClaimsViewModel)
        {


            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByIdAsync(createClaimsViewModel.UserId);
                if (createClaimsViewModel.ClaimId == "-1")
                {

                    var claims = await _UserManager.GetClaimsAsync(user);

                    var claimsList = claims.Select(x => x.Value).ToList();



                    //13
                    //1234

                    createClaimsViewModel.AllClaims = ClaimsType.ClaimType.Except(claimsList).ToList();

                    ModelState.AddModelError(string.Empty, "请选择正确的Claim");
                    return View(createClaimsViewModel);
                }


                if (user != null)
                {
                    var claim = new IdentityUserClaim<string>
                    {
                        ClaimType = createClaimsViewModel.ClaimId,
                        ClaimValue = createClaimsViewModel.ClaimId
                    };

                    user.Claims.Add(claim);
                    var result = await _UserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("EditUser", new { id = createClaimsViewModel.UserId });
                    }

                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }

                }

            }

            return View(createClaimsViewModel);


        }

    }
}