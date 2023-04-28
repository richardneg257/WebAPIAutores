using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Dtos;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers;
[Route("api/Books/{bookId:int}/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CommentsController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<GetCommentDto>>> Get([FromRoute] int bookId)
    {
        var existBook = await _context.Books.AnyAsync(x => x.Id == bookId);
        if (!existBook) return NotFound();

        var comments = await _context.Comments.Where(x => x.BookId == bookId).ToListAsync();
        return _mapper.Map<List<GetCommentDto>>(comments);
    }

    [HttpGet("{id:int}", Name = "GetComment")]
    public async Task<ActionResult<GetCommentDto>> GetById([FromRoute] int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
        if (comment is null) return NotFound();

        return _mapper.Map<GetCommentDto>(comment);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromRoute] int bookId, CreateCommentDto createCommentDto)
    {
        var existBook = await _context.Books.AnyAsync(x => x.Id == bookId);
        if (!existBook) return NotFound();

        var comment = _mapper.Map<Comment>(createCommentDto);
        comment.BookId = bookId;

        _context.Add(comment);
        await _context.SaveChangesAsync();

        var commentDto = _mapper.Map<GetCommentDto>(comment);

        return CreatedAtRoute("GetComment", new { id = comment.Id, bookId = bookId }, commentDto);
    }
}
