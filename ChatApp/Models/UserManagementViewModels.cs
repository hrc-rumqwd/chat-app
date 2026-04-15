using ChatApp.Application.Users.Commands;
using ChatApp.Application.Users.Queries;

namespace ChatApp.Web.Models
{
    public class ProfilePageViewModel
    {
        public GetUserInfoQueryResult? CurrentUser { get; set; }
        public UpdateCurrentUserProfileCommand ProfileForm { get; set; } = new();
        public ChangeCurrentUserPasswordCommand PasswordForm { get; set; } = new();
    }

    public class AdminUsersPageViewModel
    {
        public string? SearchKeyword { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public ICollection<AdminUserListItemDto> Users { get; set; } = [];
    }
}
