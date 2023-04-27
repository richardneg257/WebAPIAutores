using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Dtos;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetBookDto>> Get([FromRoute] int id)
        {
            var book = await _context.Books.Include(x => x.AuthorsBooks).ThenInclude(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
            if (book is null) return NotFound();

            book.AuthorsBooks = book.AuthorsBooks.OrderBy(x => x.Orden).ToList();
            return _mapper.Map<GetBookDto>(book);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateBookDto createBookDto)
        {
            if (createBookDto.AuthorsIds is null) return BadRequest("No se puede crear un libro sin autores");

            var authorsIds = await _context.Authors.Where(x => createBookDto.AuthorsIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();
            if (createBookDto.AuthorsIds.Count != authorsIds.Count) return BadRequest("No existe uno de los autores enviados");

            var book = _mapper.Map<Book>(createBookDto);

            for(int i = 0; i < book.AuthorsBooks.Count; i++)
            {
                book.AuthorsBooks[i].Orden = i;
            }

            _context.Add(book);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
