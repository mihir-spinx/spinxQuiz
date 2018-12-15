using Spinx.Core.Domain;
using System;

namespace Spinx.Domain.Members
{
    public class MemberResult : IModificationHistory
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int AttempedQues { get; set; }
        public int Score { get; set; }
        public decimal Percentage { get; set; }

        public string TimeDuration { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
