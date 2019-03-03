using System;

namespace DatingApp.API.Dtos
{
    public class UserForReturnRegisterDto
    {
        public int Id { get; set; } 
        public string Username { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastAcitve { get; set; }
        public string PhotoUrl { get; set; }
    }
}