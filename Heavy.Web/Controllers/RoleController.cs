
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Data;
using Heavy.Web.Models;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heavy.Web.Controllers
{
    //[Authorize(Roles ="administrators")]
    //使用policy策略
    [Authorize(Policy = "仅限管理员")]

    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(
            RoleManager<IdentityRole> _roleManager
            , UserManager<ApplicationUser> _userManager)
        {
            this._roleManager = _roleManager;
            this._userManager = _userManager;
        }


        public IActionResult Index()
        {
            var roleList = _roleManager.Roles.ToList();

            return View(roleList);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]//已使用全局的
        [IgnoreAntiforgeryToken]//不使用验证
        public async Task<IActionResult> AddRole(CreateRoleViewModel createRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createRoleViewModel);
            }
            var role = new IdentityRole
            {
                Name = createRoleViewModel.RoleName
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

                return View(createRoleViewModel);
            }
        }

        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return RedirectToAction("index");
            }

            var vm = new EditRoleViewModel()
            {
                Id = role.Id,
                RoleName = role.Name

            };

            var userList = _userManager.Users.ToList();

            foreach (var user in userList)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    vm.Users.Add(user);
                }
            }


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editRoleViewModel);
            }

            var role = await _roleManager.FindByIdAsync(editRoleViewModel.Id);
            if (role != null)
            {
                role.Name = editRoleViewModel.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }

            }
            ModelState.AddModelError(string.Empty, "角色不存在");
            return View(editRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                {
                    return RedirectToAction("Index");
                }
            }

            return View("Index");
        }


        public async Task<IActionResult> AddUserToRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new AddUserToRoleViewModel
            {
                RoleId = role.Id
            };
            var userList = await _userManager.Users.ToListAsync();
            foreach (var user in userList)
            {
                if (!await _userManager.IsInRoleAsync(user, role.Name))
                {
                    vm.Users.Add(user);

                }
            }
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleViewModel addUserToRoleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(addUserToRoleViewModel.RoleId);
            var user = await _userManager.FindByIdAsync(addUserToRoleViewModel.UserId);

            if (role != null && user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    return RedirectToAction("EditRole", new { id = role.Id });
                }


                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(addUserToRoleViewModel);

            }
            else
            {
                ModelState.AddModelError(string.Empty, "角色或者用户不存在");
            }

            return View(addUserToRoleViewModel);

        }

        public async Task<IActionResult> DeleteUserToRole(string roleId, string userId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var user = await _userManager.FindByIdAsync(userId);
            if (role != null && user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    return RedirectToAction("EditRole", new { id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { id = roleId });

        }

        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> CheckNameExist([Bind("RoleName")] string rolename)
        {
            var role = await _roleManager.FindByNameAsync(rolename);
            if(role!=null)
            {
                return Json(false);
            }
            return Json(true);
        }



    }
}
