namespace DatingApp.API.Dtos
{
    public class UserForUpdateDto
    {
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int RoleId {get;set;}

        public RoleForDto Roles {get;set;}
    }
}