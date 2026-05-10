using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.ViewModel
{
    public class PostVM
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please Enter Title")]
        public string Title { get; set; }

        [Required(ErrorMessage ="Please provide subtitle")]
        public string SubTitle { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public string Slug { get; set; }


        [DisplayName("Cover image for blog post")]
        public IFormFile? Image { get; set; }
        
    }
}
