using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [FirstCapitalLetter]
        [StringLength(250)]
        public string Title { get; set; }
    }
}
