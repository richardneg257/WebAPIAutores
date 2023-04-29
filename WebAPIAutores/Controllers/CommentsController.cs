using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<IdentityUser> _userManager;

    public CommentsController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Post([FromRoute] int bookId, CreateCommentDto createCommentDto)
    {
        var emailClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email");
        var email = emailClaim.Value;

        var user = await _userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var existBook = await _context.Books.AnyAsync(x => x.Id == bookId);
        if (!existBook) return NotFound();

        var comment = _mapper.Map<Comment>(createCommentDto);
        comment.BookId = bookId;
        comment.UserId = userId;

        _context.Add(comment);
        await _context.SaveChangesAsync();

        var commentDto = _mapper.Map<GetCommentDto>(comment);

        return CreatedAtRoute("GetComment", new { id = comment.Id, bookId = bookId }, commentDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put([FromRoute] int bookId, [FromRoute] int id, CreateCommentDto createCommentDto)
    {
        var existBook = await _context.Books.AnyAsync(x => x.Id == bookId);
        if (!existBook) return NotFound();

        var existComment = await _context.Comments.AnyAsync(x => x.Id == bookId);
        if (!existComment) return NotFound();

        var comment = _mapper.Map<Comment>(createCommentDto);
        comment.Id = id;
        comment.BookId = bookId;

        _context.Update(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
