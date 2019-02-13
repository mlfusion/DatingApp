using System;

namespace DatingApp.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}