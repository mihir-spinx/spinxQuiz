using System.Linq;

namespace Spinx.Services.AdminRolePermissions.DTOs
{
    public class AdminPermissionDropdownDto
    {
        private string _displayName;
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Left { get; set; }
        public int? Right { get; set; }
        public int Depth { get; set; }

        public string DisplayName
        {
            get { return string.Concat(Enumerable.Repeat("| - ", Depth)) + _displayName + " [" + Name + "]"; }
            set { _displayName = value; }
        }
    }
}