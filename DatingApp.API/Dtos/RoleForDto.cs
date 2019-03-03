using System;

namespace DatingApp.API.Dtos
{
    public class RoleForDto
    {
        public RoleForDto()
        {
            Created = DateTime.Now;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }

    }
}