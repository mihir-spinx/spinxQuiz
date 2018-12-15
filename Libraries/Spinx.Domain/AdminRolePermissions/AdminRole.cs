using Spinx.Core.Domain;
using Spinx.Domain.AdminUsers;
using System;
using System.Collections.Generic;

namespace Spinx.Domain.AdminRolePermissions
{
    public class AdminRole : IModificationHistory
    {
        public AdminRole()
        {
            Users = new List<AdminUser>();
            Permissionses = new List<AdminPermission>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<AdminUser> Users { get; set; }

        public List<AdminPermission> Permissionses { get; set; }
    }
}
