using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Users.Commands;
using ChatApp.Application.Users.Queries;
using ChatApp.Shared.Constants;
using ChatApp.Shared.Models.Commons;
using ChatApp.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserManagementController(IBroker broker) : Controller
    {
        private readonly IBroker _broker = broker;

        [HttpGet("/profile")]
        public async Task<IActionResult> Profile()
        {
            ProfilePageViewModel viewModel = await BuildProfileViewModelAsync();
            return View(viewModel);
        }

        [HttpPost("/profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateCurrentUserProfileCommand command)
        {
            command.CurrentUserId = long.Parse(User.FindFirstValue(IdentityClaims.UserId));
            Result<UpdateCurrentUserProfileCommandResult> result = await _broker.CommandAsync(command);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.IsSuccess
                ? "Profile updated successfully."
                : result.Error;

            return RedirectToAction(nameof(Profile));
        }

        [HttpPost("/profile/password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangeCurrentUserPasswordCommand command)
        {
            command.CurrentUserId = long.Parse(User.FindFirstValue(IdentityClaims.UserId));
            Result<ChangeCurrentUserPasswordCommandResult> result = await _broker.CommandAsync(command);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.IsSuccess
                ? "Password changed successfully."
                : result.Error;

            return RedirectToAction(nameof(Profile));
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpGet("/admin/users")]
        public async Task<IActionResult> AdminUsers(string? searchKeyword, int pageIndex = 1, int pageSize = 10)
        {
            Result<AdminGetUsersQueryResult> result = await _broker.QueryAsync(new AdminGetUsersQuery
            {
                SearchKeyword = searchKeyword,
                PageIndex = pageIndex <= 0 ? 1 : pageIndex,
                PageSize = pageSize <= 0 ? 10 : pageSize
            });

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return View(new AdminUsersPageViewModel());
            }

            return View(new AdminUsersPageViewModel
            {
                SearchKeyword = searchKeyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Users = result.Data.Users
            });
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost("/admin/users/{userId:long}/activation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActivation(long userId, bool isActived, string? searchKeyword, int pageIndex = 1, int pageSize = 10)
        {
            Result<bool> result = await _broker.CommandAsync(new AdminSetUserActivationCommand
            {
                UserId = userId,
                IsActived = isActived
            });
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.IsSuccess
                ? "User status updated."
                : result.Error;

            return RedirectToAction(nameof(AdminUsers), new { searchKeyword, pageIndex, pageSize });
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost("/admin/users/{userId:long}/lockout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetLockout(long userId, bool isLocked, string? searchKeyword, int pageIndex = 1, int pageSize = 10)
        {
            Result<bool> result = await _broker.CommandAsync(new AdminSetUserLockoutCommand
            {
                UserId = userId,
                IsLocked = isLocked
            });
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.IsSuccess
                ? "User lockout updated."
                : result.Error;

            return RedirectToAction(nameof(AdminUsers), new { searchKeyword, pageIndex, pageSize });
        }

        private async Task<ProfilePageViewModel> BuildProfileViewModelAsync()
        {
            Result<GetUserInfoQueryResult> result = await _broker.QueryAsync(
                new GetUserInfoQuery(long.Parse(User.FindFirstValue(IdentityClaims.UserId))));

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return new ProfilePageViewModel();
            }

            return new ProfilePageViewModel
            {
                CurrentUser = result.Data,
                ProfileForm = new UpdateCurrentUserProfileCommand
                {
                    FullName = result.Data.FullName,
                    Alias = result.Data.Alias,
                    Dob = result.Data.Dob
                }
            };
        }
    }
}
