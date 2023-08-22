namespace ZooWeb.BL;

public sealed class CategoryService : ValidationBL<Category>
{
	private readonly IRepository<Category> _categoryRepo;
	public CategoryService(IRepository<Category> categoryRepo)
	{
		_categoryRepo = categoryRepo;
	}

	protected override void validateEntity(Category category)
	{
		validateNull(category);
		var context = new ValidationContext(category);
		var results = new List<ValidationResult>();
		if (!Validator.TryValidateObject(category, context, results, true))
		{
			throw new FullValidationException("Valdiation failed.", results);
		}
	}
	protected override async Task validateExistence(int id)
	{
		if (!await Exists(id))
			throw new ArgumentException(id.NoEntityMessage("Category"), nameof(id));

	}
	private async Task validateName(string name, int id)
	{
		if (await Exists(exp: c => c.Name == name && c.CategoryId != id))
			throw new ArgumentException(name.UniqueNameMessage("Category"), nameof(name));

	}



	public async Task<IEnumerable<Category>> GetAll(Expression<Func<Category, bool>>? exp = null)
	{
		if (exp != null)
		{
			return await _categoryRepo.GetQueryable().Where(exp).ToListAsync();
		}
		return await _categoryRepo.GetAllEntitiesAsync();
	}
	public async Task<int> Count(Expression<Func<Category, bool>>? exp = null)
	{
		if (exp != null)
		{
			return await _categoryRepo.GetQueryable().Where(exp).CountAsync();
		}
		return await _categoryRepo.GetEntitiesCountAsync();
	}

	public async Task<Category?> Get(int id = 0, Expression<Func<Category, bool>>? exp = null)
	{
		if (id != 0)
		{
			return await _categoryRepo.GetEntityByIdAsync(id);
		}
		if (exp != null)
		{
			return await _categoryRepo.GetEntityAsync(exp);
		}
		throw new ArgumentException();
	}
	public async Task<bool> Create(Category category, bool validated = false)
	{
		validateNoId(category.CategoryId);
		if (!validated) validateEntity(category);
		await validateName(category.Name!, category.CategoryId);
		return await _categoryRepo.AddEntityAsync(category);
	}
	public async Task<bool> Update(int categoryId, Category category, bool validated = false)
	{
		if (!validated) validateEntity(category);
		await validateExistence(categoryId);
		await validateName(category.Name!, categoryId);
		return await _categoryRepo.UpdateEntityByIdAsync(categoryId, category);
	}
	public async Task<bool> Delete(int id = 0, Expression<Func<Category, bool>>? exp = null)
	{
		if (id != 0)
		{
			await validateExistence(id);
			return await _categoryRepo.DeleteEntityByIdAsync(id);
		}
		if (exp != null)
		{
			var categoryToDel = await Get(exp: exp);
			return await _categoryRepo.DeleteEntityAsync(categoryToDel!);
		}
		throw new ArgumentException();
	}

	public async Task<bool> Exists(int id = 0, Expression<Func<Category, bool>>? exp = null)
	{
		if (id != 0)
		{
			return await _categoryRepo.CheckExistenceByIdAsync(id);
		}
		if (exp != null)
		{
			return await _categoryRepo.CheckExistenceAsync(exp);
		}
		throw new ArgumentException();
	}

}

