﻿namespace Spinx.Services.EmailTemplates.DTOs
{
    public class EmailTemplateEditAdminDto
    {
        public int Id { get; set; }      

        public string Name { get; set; }
        public string Slug { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
