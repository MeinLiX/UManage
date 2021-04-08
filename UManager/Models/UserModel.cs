using Microsoft.AspNetCore.Identity;


namespace UManager.Models
{
    public class UserModel : IdentityUser
    {
        public bool IsBlocked {get;set;}
        public bool Selected {get;set;}

        public UserModel()
        {
            IsBlocked = false;
            Selected = false;
        }
    }
}
