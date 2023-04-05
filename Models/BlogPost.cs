using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebApp.Models
{
    public class BlogPost
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime UploadDateTime { get; set; }  
    }
}
