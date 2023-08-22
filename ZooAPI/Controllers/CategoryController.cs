
namespace ZooAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : Controller
{
	private readonly CategoryService _categoryService;

	private readonly IMapper _mapper;

	public CategoryController(IMapper mapper, CategoryService categoryService)
	{
		_mapper = mapper;
		_categoryService = categoryService;
	}

	[HttpGet("All")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
	{
		var categories = await _categoryService.GetAll();
		var categoriesDto = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
		return Ok(categoriesDto);
	}
	[HttpGet("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
	{
		var category = await _categoryService.Get(id);
		if (category == null)
		{
			return NotFound(id.NoEntityMessage("Category"));
		}
		var categoryDto = _mapper.Map<CategoryDTO>(category);
		return Ok(categoryDto);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryDTO categoryDto)
	{
		if (categoryDto.CategoryId != 0)
		{
			ModelState.AddModelError(nameof(categoryDto.CategoryId), ErrorMessages.ID_PROVIDED);
			return BadRequest(ModelState);
		}
		if (await _categoryService.Exists(exp: c => c.Name!.ToLower() == categoryDto.Name!.ToLower()))
		{
			ModelState.AddModelError(nameof(categoryDto.Name), categoryDto.Name!.UniqueNameMessage("Category"));
			return Conflict(ModelState);
		}
		var category = _mapper.Map<Category>(categoryDto);
		if (!await _categoryService.Create(category, true))
		{
			ModelState.AddModelError("", ErrorMessages.ServerFailMessage("Category"));
			return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
		}
		categoryDto = _mapper.Map<CategoryDTO>(category);
		return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.CategoryId }, categoryDto);


	}
	[HttpPut("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryDTO categoryDto)
	{
		if (categoryDto.CategoryId != 0 && id != categoryDto.CategoryId)
		{
			ModelState.AddModelError(nameof(categoryDto.CategoryId), "CategoryId does not match the provided id.");
			return BadRequest(ModelState);
		}
		if (!await _categoryService.Exists(id))
			return NotFound(id.NoEntityMessage("Category"));
		if (await _categoryService.Exists(exp: c => c.Name!.ToLower() == categoryDto.Name!.ToLower()))
		{
			ModelState.AddModelError(nameof(categoryDto.Name), categoryDto.Name!.UniqueNameMessage("Category"));
			return Conflict(ModelState);
		}
		var category = _mapper.Map<Category>(categoryDto);
		if (!await _categoryService.Update(id, category, true))
		{
			ModelState.AddModelError("", ErrorMessages.ServerFailMessage("Category"));
			return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
		}
		return NoContent();

	}
	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> DeleteCategory(int id)
	{
		if (!await _categoryService.Exists(id))
			return NotFound(id.NoEntityMessage("Category"));
		if (!await _categoryService.Delete(id))
		{
			ModelState.AddModelError("", ErrorMessages.ServerFailMessage("Category"));
			return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
		}
		return NoContent();
	}
}

