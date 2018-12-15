using Spinx.Core.Domain;
using System;

namespace Spinx.Domain.EmailTemplates
{
    public class EmailTemplate : IModificationHistory
    {
        public int Id { get; set; }      

        public string Name { get; set; }
        public string Slug { get; set; }

        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}