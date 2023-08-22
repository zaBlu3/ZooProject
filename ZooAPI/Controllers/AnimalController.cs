namespace ZooAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly AnimalsService _animalService;
    private readonly IHubContext<AnimalHub, IAnimalHubBase> _hubAnimalContext;


    public AnimalController(IMapper mapper, AnimalsService animalService, IHubContext<AnimalHub, IAnimalHubBase> hubAnimalContext)
    {
        _mapper = mapper;
        _animalService = animalService;
        _hubAnimalContext = hubAnimalContext;
    }
    [HttpGet("All")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<AnimalDTO>>> GetAllAnimals()
	{
		var aniamls = await _animalService.GetAll();
		var animalsDto = _mapper.Map<IEnumerable<AnimalDTO>>(aniamls);
		return Ok(animalsDto);

	}
	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> DeleteAnimal(int id)
	{
		if (!await _animalService.Exists(id))
			return NotFound(id.NoEntityMessage("Animal"));
		if (!await _animalService.Delete(id))
		{
			ModelState.AddModelError("", ErrorMessages.ServerFailMessage("Animal"));
			return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
		}
		return NoContent();
	}

	[HttpDelete("DeleteRange")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status207MultiStatus)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> DeleteRangeAnimals([FromQuery] List<int> IDs)
	{
		if (IDs == null || IDs.Count == 0)
			return BadRequest("No records to delete.");
		if (IDs.Any(id => id <= 0))
			return BadRequest(ErrorMessages.INVALID_ID);
		int deletedAnimalsCount = await _animalService.Count(a => IDs.Contains(a.AnimalId));
		if (!await _animalService.DeleteRange(IDs))
		{
			return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.ServerFailMessage("Animals"));
		};
        await _hubAnimalContext.Clients.All.AnimalsDeleted(IDs.ToArray());
        string message = $"{deletedAnimalsCount} Animal(s) were deleted.";
		var notFound = IDs.Count - deletedAnimalsCount;
		if (notFound != 0)
		{
			return StatusCode(207,
				new
				{
					Success = message,
					Error = $"{notFound} Animal(s) were not found."
				});
		}
		return Ok(message);
	}
}
