using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApplication.Data;
using StoreApplication.Repositories;
using StoreApplication.ViewModels;

namespace StoreApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private ApplicationDbContext _context;
        private IServiceProvider _serviceProvider;

        public UserRoleController(ApplicationDbContext context,
                                    IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public ActionResult Index()
        {
            UserRepo userRepo = new UserRepo(_context);
            var users = userRepo.All();
            return View(users);
        }

        public async Task<IActionResult> Detail(string userName)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider);
            var roles = await userRoleRepo.GetUserRoles(userName);
            ViewBag.UserName = userName;
            return View(roles);
        }

        public ActionResult Assign(string userName)
        {
            ViewBag.SelectedUser = userName;

            RoleRepo roleRepo = new RoleRepo(_context);
            var roles = roleRepo.GetAllRoles().ToList();

            var preRoleList = roles.Select(r =>
                new SelectListItem { Value = r.RoleName, Text = r.RoleName })
                   .ToList();

            var roleList = new SelectList(preRoleList, "Value", "Text");

            ViewBag.RoleSelectList = roleList;

            var userList = _context.Users.ToList();

            var preUserList = userList.Select(u => new SelectListItem
            { Value = u.Email, Text = u.Email }).ToList();
            SelectList userSelectList = new SelectList(preUserList, "Value", "Text");

            ViewBag.UserSelectList = userSelectList;
            return View();
        }

        // Assigns role to user.
        [HttpPost]
        public async Task<IActionResult> Assign(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider);

            if (ModelState.IsValid)
            {
                var addUR = await userRoleRepo.AddUserRole(userRoleVM.Email,
                                                            userRoleVM.Role);
            }
            try
            {
                return RedirectToAction("Detail", "UserRole",
                       new { userName = userRoleVM.Email });
            }
            catch
            {
                return View();
            }
        }

        [HttpGet, ActionName("DeleteUserRole")]
        public async Task<IActionResult> DeleteUserRole(string email, string roleName)
        {
            if (ModelState.IsValid)
            {
                UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider);
                var success = await userRoleRepo.RemoveUserRole(email, roleName);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Error = "An error occurred while deleting this role. Please try again.";
            return View();
        }

        [HttpGet, ActionName("DeleteUser")]
        public IActionResult DeleteUser(string email)
        {
            if (ModelState.IsValid)
            {
                UserRepo userRepo = new UserRepo(_context);
                var success = userRepo.DeleteUser(email);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Error = "An error occurred while deleting this user. Please try again.";
            return View();
        }

    }

}