using WebAPIAutores.Validations;

namespace WebAPIAutores.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [FirstCapitalLetter]
        public string Title { get; set; }
    }
}
