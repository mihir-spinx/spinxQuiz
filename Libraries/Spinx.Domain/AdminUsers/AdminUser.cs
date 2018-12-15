using Spinx.Core.Domain;
using Spinx.Domain.AdminRolePermissions;
using System;
using System.Collections.Generic;

namespace Spinx.Domain.AdminUsers
{
    public class AdminUser : IModificationHistory
    {
        public AdminUser()
        {
            Roles = new List<AdminRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsActive { get; set; }
        public string ForgotPasswordToken { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<AdminRole> Roles { get; set; }
    }
}