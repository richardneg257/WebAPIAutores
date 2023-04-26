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
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book is null) return NotFound();
            return _mapper.Map<GetBookDto>(book);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateBookDto createBookDto)
        {
            var book = _mapper.Map<Book>(createBookDto);
            _context.Add(book);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
