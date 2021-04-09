using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UManager.Models
{
    public class UserModel : IdentityUser
    {
        [Display(Name = "Ban status")]
        public bool IsBlocked {get;set;}
        public bool Selected {get;set;}

        public UserModel()
        {
            IsBlocked = false;
            Selected = false;
        }
    }
}
