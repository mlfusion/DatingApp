using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{
    public class Photo : IEntity
    {
        public Photo()
        {
            Created = DateTime.Now;
        }

        public int Id { get; set; }
        public string Url { get; set; } 
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified {get;set;}
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        
        //public ICollection<Comment> Comments { get; set; }
    }
}