using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Author>> Get()
        {
            return await _context.Authors.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Author>> Get([FromRoute] int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);

            if (author == null) return NotFound();

            return author;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Author>> Get([FromRoute] string name)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(x => x.Name.Contains(name));

            if (author == null) return NotFound();

            return author;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Author author)
        {
            var existAuthorWithTheSameName = await _context.Authors.AnyAsync(x => x.Name == author.Name);

            if (existAuthorWithTheSameName) return BadRequest($"There is already an author with the name {author.Name}");

            _context.Add(author);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Author author, int id)
        {
            if (author.Id != id) return BadRequest("Author ID does not match URL ID");

            var exist = await _context.Authors.AnyAsync(x => x.Id == id);

            if (!exist) return NotFound();

            _context.Update(author);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await _context.Authors.AnyAsync(x => x.Id == id);

            if (!exist) return NotFound();

            _context.Remove(new Author() { Id = id });
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
