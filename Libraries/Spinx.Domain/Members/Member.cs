﻿using Spinx.Core.Domain;
using System;

namespace Spinx.Domain.Members
{
    public class Member : IModificationHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public bool IsActive { get; set; }
        public string ForgotPasswordToken { get; set; }

        public int CreatedSource { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string Degree { get; set; }
        public string College { get; set; }
        public string LastSemMark { get; set; }
        public string Experience { get; set; }

        public string UploadResume { get; set; }
    }
}