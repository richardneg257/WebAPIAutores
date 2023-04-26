using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.Entities
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(maximumLength: 120, ErrorMessage = "The field {0} must not have more than {1} characters")]
        [FirstCapitalLetter]
        public string Name { get; set; }
        public List<AuthorBook> AuthorsBooks { get; set; }
    }
}
