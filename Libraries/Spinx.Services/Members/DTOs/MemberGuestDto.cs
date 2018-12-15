namespace Spinx.Services.Members.DTOs
{
    public class MemberGuestDto
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IAgree { get; set; }
        public bool IsActive { get; set; }
        public int CreatedSource { get; set; }
    }
}