namespace Spinx.Services.AdminAuth.DTOs
{
    public class AdminResetPasswordDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}