using Microsoft.AspNetCore.Identity;


namespace MicrosoftIdentityDemo.Core.Domain.IdentityEntites
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string FullNameAR { get; set; }
    }
}
