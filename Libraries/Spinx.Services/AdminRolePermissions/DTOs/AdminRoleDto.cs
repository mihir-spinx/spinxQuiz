using System.Collections.Generic;

namespace Spinx.Services.AdminRolePermissions.DTOs
{
    public class AdminRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Permissions { get; set; }
    }
}