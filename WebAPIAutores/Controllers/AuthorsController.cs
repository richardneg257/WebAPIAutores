using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Dtos;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<GetAuthorDto>> Get()
        {
            var authors = await _context.Authors.ToListAsync();
            return _mapper.Map<List<GetAuthorDto>>(authors);
        }

        [HttpGet("{id:int}", Name = "GetAuthor")]
        public async Task<ActionResult<GetAuthorDtoWithBooks>> Get([FromRoute] int id)
        {
            var author = await _context.Authors.Include(x => x.AuthorsBooks).ThenInclude(x => x.Book).FirstOrDefaultAsync(x => x.Id == id);

            if (author is null) return NotFound();

            return _mapper.Map<GetAuthorDtoWithBooks>(author);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<GetAuthorDto>>> Get([FromRoute] string name)
        {
            var authors = await _context.Authors.Where(x => x.Name.Contains(name)).ToListAsync();
            return _mapper.Map<List<GetAuthorDto>>(authors);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateAuthorDto createAuthorDto)
        {
            var existAuthorWithTheSameName = await _context.Authors.AnyAsync(x => x.Name == createAuthorDto.Name);

            if (existAuthorWithTheSameName) return BadRequest($"There is already an author with the name {createAuthorDto.Name}");

            var author = _mapper.Map<Author>(createAuthorDto);

            _context.Add(author);
            await _context.SaveChangesAsync();

            var authorDto = _mapper.Map<GetAuthorDto>(author);

            return CreatedAtRoute("GetAuthor", new { id = author.Id }, authorDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] CreateAuthorDto authorCreation, [FromRoute] int id)
        {
            var exist = await _context.Authors.AnyAsync(x => x.Id == id);

            if (!exist) return NotFound();

            var author = _mapper.Map<Author>(authorCreation);
            author.Id = id;

            _context.Update(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var exist = await _context.Authors.AnyAsync(x => x.Id == id);

            if (!exist) return NotFound();

            _context.Remove(new Author() { Id = id });
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
