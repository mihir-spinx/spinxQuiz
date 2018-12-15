using System.Collections.Generic;

namespace Spinx.Services.AdminUsers.DTOs
{
    public class AdminUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public List<int> Roles { get; set; }
    }
}