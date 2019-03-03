namespace DatingApp.API.Common.Paging
{
    public class UserParams : Params
    {
        public int UserId { get; set; }
        public string MessageContainer { get; set; } = "Unread";
    }
}