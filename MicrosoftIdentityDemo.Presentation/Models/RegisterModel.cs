namespace MicrosoftIdentityDemo.Presentation.Models
{
    public class RegisterModel
    {
        public string FullName { get; set; }
        public string FullNameAR { get; set; } = string.Empty;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
