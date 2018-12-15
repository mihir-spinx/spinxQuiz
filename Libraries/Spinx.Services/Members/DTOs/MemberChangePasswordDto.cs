namespace Spinx.Services.Members.DTOs
{
    public class MemberChangePasswordDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}