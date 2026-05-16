using System.ComponentModel.DataAnnotations;

namespace BlogSite.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Please Enter Username")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
    }
}
