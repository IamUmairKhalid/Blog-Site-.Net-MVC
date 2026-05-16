using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.ViewModel
{
    public class ProfileVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "FatherName is Required")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "Bio is Required")]
        public string Bio { get; set; }
        public IFormFile? Image { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password not match")]
        public string? ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string username { get; set; }

    }
}
