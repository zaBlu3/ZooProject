namespace ZooWeb.BL;

public sealed class AnimalsService : ValidationBL<Animal>
{
	private readonly IRepository<Animal> _animalRepo;
	private readonly IRepository<Category> _categoryRepo;
	private readonly IRepository<Image<Animal>> _animalImageRepo;
	private readonly int MAX_IMAGES;

	public AnimalsService(IRepository<Animal> animalRepo, IRepository<Category> categoryRepo, IRepository<Image<Animal>> animalImageRepo, IConfiguration configuration)
	{
		_animalRepo = animalRepo;
		_categoryRepo = categoryRepo;
		_animalImageRepo = animalImageRepo;
		MAX_IMAGES = configuration.GetValue<int>("AppSettings:MaxImages");
	}


	protected override void validateEntity(Animal animal)
	{
		validateNull(animal);
		var context = new ValidationContext(animal);
		var results = new List<ValidationResult>();
		if (!Validator.TryValidateObject(animal, context, results, true))
		{
			throw new FullValidationException("Valdiation failed.", results);
		}
	}

	protected override async Task validateExistence(int id)
	{
		if (!await Exists(id))
			throw new ArgumentException(id.NoEntityMessage("Animal"), nameof(id));

	}
	private async Task validateCategory(int CategoryId)
	{
		if (!await _categoryRepo.CheckExistenceByIdAsync(CategoryId))
			throw new ArgumentException(CategoryId.NoEntityMessage(nameof(Category)), nameof(CategoryId));

	}
	private async Task validateName(string name, int id)
	{
		if (await Exists(exp: a => a.Name == name && a.AnimalId != id))
			throw new ArgumentException(name.UniqueNameMessage("Animal"), nameof(name));

	}



	public (int, IQueryable<Animal>) QueryFillterOnAnimals(Expression<Func<Animal, object>> sortFilter, SortOrder order, string searchString = "", int catigoryId = 0)
	{
		var query = _animalRepo.GetQueryable();
		if (catigoryId != 0)
		{
			query = query.Where(a => a.CategoryId == catigoryId);
		}
		if (!string.IsNullOrEmpty(searchString))
		{
			query = query.Where(
				a => a.Name!.ToLower().Contains(searchString.ToLower()) ||
				a.Description!.ToLower().Contains(searchString.ToLower()) ||
				a.Age.ToString().Contains(searchString)
				);
		}
		int TotalItems = query.Count();
		var sortedQuery = order == SortOrder.Ascending
		? query.OrderBy(sortFilter)
		: query.OrderByDescending(sortFilter);
		return (TotalItems, sortedQuery);
	}

	public async Task<List<Animal>> GetPaginedAnimal(int pageNumber, int pageSize, IQueryable<Animal>? animalsQuery = null)
	{
		if (animalsQuery != null)
			return await animalsQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

		return await _animalRepo.GetQueryable().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
	}

	public async Task<IEnumerable<Animal>> GetAll(Expression<Func<Animal, bool>>? exp = null)
	{
		if (exp != null)
		{
			return await _animalRepo.GetQueryable().Where(exp).ToListAsync();
		}
		return await _animalRepo.GetAllEntitiesAsync();
	}
	public async Task<int> Count(Expression<Func<Animal, bool>>? exp = null)
	{
		if (exp != null)
		{
			return await _animalRepo.GetQueryable().Where(exp).CountAsync();
		}
		return await _animalRepo.GetEntitiesCountAsync();
	}

	public async Task<Animal?> Get(int id = 0, Expression<Func<Animal, bool>>? exp = null)
	{
		if (id != 0)
		{
			return await _animalRepo.GetEntityByIdAsync(id);
		}
		if (exp != null)
		{
			return await _animalRepo.GetEntityAsync(exp);
		}
		throw new ArgumentException();
	}
	public async Task<bool> Create(Animal animal, bool validated = false)
	{
		validateNoId(animal.AnimalId);
		if (!validated) validateEntity(animal);
		await validateName(animal.Name!, animal.AnimalId);
		await validateCategory(animal.CategoryId);
		return await _animalRepo.AddEntityAsync(animal);
	}
	public async Task<bool> Update(int animalId, Animal animal, bool validated = false)
	{
		if (!validated) validateEntity(animal);
		await validateExistence(animalId);
		await validateName(animal.Name!, animalId);
		await validateCategory(animal.CategoryId);
		return await _animalRepo.UpdateEntityByIdAsync(animalId, animal);
	}
	public async Task<bool> Delete(int id = 0, Expression<Func<Animal, bool>>? exp = null)
	{
		if (id != 0)
		{
			await validateExistence(id);
			return await _animalRepo.DeleteEntityByIdAsync(id);
		}
		if (exp != null)
		{
			var animalToDel = await Get(exp: exp);
			return await _animalRepo.DeleteEntityAsync(animalToDel!);
		}
		throw new ArgumentException();
	}

	public async Task<bool> DeleteRange(IEnumerable<int> animalIDs)
	{
		validateNull(animalIDs);
		var animalsToDelete = await GetAll(a => animalIDs.Contains(a.AnimalId));
		return await _animalRepo.DeleteEntitiesAsync(animalsToDelete);
	}

	public async Task<bool> Exists(int id = 0, Expression<Func<Animal, bool>>? exp = null)
	{
		if (id != 0)
		{
			return await _animalRepo.CheckExistenceByIdAsync(id);
		}
		if (exp != null)
		{
			return await _animalRepo.CheckExistenceAsync(exp);
		}
		throw new ArgumentException();
	}
	public async Task<int> GetNextImageId() => await _animalImageRepo.GetQueryable().MaxAsync(im => im.ImageId) + 1;

	public async Task<bool> CreateImage(Image<Animal> animalImage)
	{
		validateNull(animalImage);
		validateNoId(animalImage.ImageId);
		await validateExistence(animalImage.EntityID);
		var animal = await Get(animalImage.EntityID);
		if (animal!.AnimalImages.Count == MAX_IMAGES)
			throw new ArgumentException(ErrorMessages.MAX_IMAGES, nameof(animal.AnimalImages));
		return await _animalImageRepo.AddEntityAsync(animalImage);
	}

	public async Task<bool> DeleteAnimalImage(int imageId)
	{
		var image = await _animalImageRepo.GetEntityByIdAsync(imageId);
		if (image == null)
			throw new ArgumentException(imageId.NoEntityMessage("Image"), nameof(imageId));
		if (image!.Entity!.AnimalImages.Count <= 1)
			throw new ArgumentException(ErrorMessages.DELETE_IMAGE, nameof(image.Entity.AnimalImages));
		return await _animalImageRepo.DeleteEntityAsync(image);
	}
	public async Task<List<Image<Animal>>> GetAnimalImages(int animalId)
	{
		await validateExistence(animalId);
		var aniaml = await Get(animalId);
		return aniaml!.AnimalImages;
	}




}
