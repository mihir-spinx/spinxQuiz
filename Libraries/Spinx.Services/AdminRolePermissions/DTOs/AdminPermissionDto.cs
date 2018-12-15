namespace Spinx.Services.AdminRolePermissions.DTOs
{
    public class AdminPermissionDto
    {
        public int Id { get; set; }
        public bool IsParentSelected { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? Left { get; set; }
        public int? Right { get; set; }
    }
}