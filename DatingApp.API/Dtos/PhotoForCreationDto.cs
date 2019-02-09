using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Dtos
{
    public class PhotoForCreationDto
    {
        public PhotoForCreationDto()
        {
            Created = DateTime.Now;
        }
        public string Url { get; set; }

        [Required]
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string PublicId { get; set; }
        public int UserId { get; internal set; }
        public int Id { get; internal set; }
    }
}