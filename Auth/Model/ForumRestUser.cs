using Microsoft.AspNetCore.Identity;

namespace KartuliAPI1.Auth.Model
{
    public class ForumRestUser : IdentityUser
    {
        [PersonalData]
        public bool AdditionalInfo { get; set; }
    }
}