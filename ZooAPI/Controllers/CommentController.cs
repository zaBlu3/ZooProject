
namespace ZooAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase


{
	private readonly CommentService _commentService;
	private readonly IMapper _mapper;
	private readonly IHubContext<CommentHub, ICommentHubBase> _hubCommentContext;

	public CommentController(IMapper mapper, CommentService commentService, IHubContext<CommentHub, ICommentHubBase> hubContext)
	{
		_mapper = mapper;
		_commentService = commentService;
		_hubCommentContext = hubContext;
	}
	[HttpGet("All")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<CommentDTO>>> GetAllComments()
	{
		var comments = await _commentService.GetAll();
		var commentDtos = _mapper.Map<IEnumerable<CommentDTO>>(comments);
		return Ok(commentDtos);

	}
	[HttpGet("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<CommentDTO>> GetCommentById(int id)
	{
		var comment = await _commentService.Get(id);
		if (comment == null)
		{
			return NotFound(id.NoEntityMessage("Comment"));
		}

		var commentDto = _mapper.Map<CommentDTO>(comment);
		return Ok(comment);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<CommentDTO>> CreateComment([FromBody] CommentCreateDto commentCreateDto)
	{
		var comment = _mapper.Map<Comment>(commentCreateDto);
		try
		{
			if (!await _commentService.Create(comment))
			{
				ModelState.AddModelError("", ErrorMessages.ServerFailMessage("Comment"));
				return StatusCode(500, ModelState);
			}
			var commentDto = _mapper.Map<CommentDTO>(comment);
			await _hubCommentContext.Clients.All.CommentAdded(commentDto);
			return CreatedAtAction(nameof(GetCommentById), new { id = commentDto.CommentId }, commentDto);

		}
		catch (ArgumentException e)
		{
			ModelState.AddModelError(e.ParamName!, e.Message);
			return BadRequest(ModelState);
		}

	}



	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> DeleteComment(int id)
	{
		try
		{
			if (!await _commentService.Delete(id))
			{
				ModelState.AddModelError("", "An unexpected error occurred while saving");
				return StatusCode(500, ModelState);
			}
			await _hubCommentContext.Clients.All.CommentDeleted(id);

			return NoContent();

		}
		catch (Exception e)
		{
			return NotFound(e.Message);
		}
	}
	[HttpDelete("DeleteRange")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status207MultiStatus)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> DeleteRangeComments([FromQuery] List<int> IDs)
	{
		if (IDs == null || IDs.Count == 0)
			return BadRequest("No records to delete.");
		if (IDs.Any(id => id <= 0))
			return BadRequest(ErrorMessages.INVALID_ID);
		var deletedCommentsIds = _commentService.GetAll(a => IDs.Contains(a.CommentId)).Result.Select(a=>a.CommentId).ToArray();
		if (deletedCommentsIds.Length == 0) return BadRequest("No records found to delete.");
		if (!await _commentService.DeleteRange(IDs))
		{
			return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.ServerFailMessage("Comments"));
		};
		await _hubCommentContext.Clients.All.CommentsDeleted(deletedCommentsIds);
		string message = $"{deletedCommentsIds.Length} Comment(s) were deleted.";
		var notFound = IDs.Count - deletedCommentsIds.Length;
		if (notFound != 0)
		{
			return StatusCode(207,
				new
				{
					Success = message,
					Error = $"{notFound} Comment(s) were not found."
				});
		}
		return Ok(message);
	}
}

