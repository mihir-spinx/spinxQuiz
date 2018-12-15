using Spinx.Core.Domain;
using System;
using System.Collections.Generic;

namespace Spinx.Domain.AdminRolePermissions
{
    public class AdminPermission : IModificationHistory
    {
        public AdminPermission()
        {
            AdminRoles = new List<AdminRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public int? Left { get; set; }
        public int? Right { get; set; }

        public int Depth { get; set; }

        public int? ParentId { get; set; }
        public virtual AdminPermission Parent { get; set; }
        public virtual List<AdminPermission> Children { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<AdminRole> AdminRoles { get; set; }
    }
}