namespace Spinx.Data.Migrations.SeedData
{
    public class AdminUsersSeed
    {
        private readonly SqlContext _context;

        public AdminUsersSeed(SqlContext context)
        {
            _context = context;
        }

        //static IEnumerable<AdminUser> AdminUsers()
        //{
        //    return new AdminUser[]
        //    {
        //        new AdminUser() { Name = "Keyur Ajmera", Email = "keyur.ajmera@spinxdigital.com", Password = "ajmera", Salt = "", IsActive = true, CreatedAt = DateTime.Now}
        //    };
        //}
    }
}
