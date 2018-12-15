using Spinx.Services.Infrastructure;

namespace Spinx.Services.AdminRolePermissions.DTOs
{
    public class AdminPermissionFilterDto : BaseFilterDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}