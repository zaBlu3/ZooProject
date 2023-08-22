namespace ZooWeb.BL;

public sealed class CommentService : ValidationBL<Comment>
{
	private readonly IRepository<Comment> _commentRepo;
	private readonly IRepository<Animal> _animalRepo;
	public CommentService(IRepository<Comment> commentRepo, IRepository<Animal> animalRepo)
	{
		_commentRepo = commentRepo;
		_animalRepo = animalRepo;
	}

	protected override void validateEntity(Comment comment)
	{
		validateNull(comment);
		var context = new ValidationContext(comment);
		var results = new List<ValidationResult>();
		if (!Validator.TryValidateObject(comment, context, results, true))
		{
			throw new FullValidationException("Valdiation failed.", results);
		}
	}
	protected override async Task validateExistence(int id)
	{
		if (!await Exists(id))
			throw new ArgumentException(id.NoEntityMessage("Comment"), nameof(id));

	}
	private async Task validateAnimal(int animalId)
	{
		if (!await _animalRepo.CheckExistenceByIdAsync(animalId))
			throw new ArgumentException(animalId.NoEntityMessage(nameof(Animal)), nameof(animalId));
	}



	public async Task<IEnumerable<Comment>> GetAll(Expression<Func<Comment, bool>>? exp = null)
	{
		if (exp != null)
		{
			return await _commentRepo.GetQueryable().Where(exp).ToListAsync();
		}
		return await _commentRepo.GetAllEntitiesAsync();
	}
	public async Task<int> Count(Expression<Func<Comment, bool>>? exp = null)
	{
		if (exp != null)
		{
			return await _commentRepo.GetQueryable().Where(exp).CountAsync();
		}
		return await _commentRepo.GetEntitiesCountAsync();
	}

	public async Task<Comment?> Get(int id = 0, Expression<Func<Comment, bool>>? exp = null)
	{
		if (id != 0)
		{
			return await _commentRepo.GetEntityByIdAsync(id);
		}
		if (exp != null)
		{
			return await _commentRepo.GetEntityAsync(exp);
		}
		throw new ArgumentException();
	}
	public async Task<bool> Create(Comment comment, bool validated = false)
	{
		validateNoId(comment.CommentId);
		if (!validated) validateEntity(comment);
		await validateAnimal(comment.AnimalId);
		return await _commentRepo.AddEntityAsync(comment);
	}
	public async Task<bool> Update(int commentId, Comment comment, bool validated = false)
	{
		if (!validated) validateEntity(comment);
		await validateExistence(commentId);
		await validateAnimal(comment.AnimalId);
		return await _commentRepo.UpdateEntityByIdAsync(commentId, comment);
	}
	public async Task<bool> Delete(int id = 0, Expression<Func<Comment, bool>>? exp = null)
	{
		if (id != 0)
		{
			await validateExistence(id);
			return await _commentRepo.DeleteEntityByIdAsync(id);
		}
		if (exp != null)
		{
			var categoryToDel = await Get(exp: exp);
			return await _commentRepo.DeleteEntityAsync(categoryToDel!);
		}
		throw new ArgumentException();
	}

	public async Task<bool> Exists(int id = 0, Expression<Func<Comment, bool>>? exp = null)
	{
		if (id != 0)
		{
			return await _commentRepo.CheckExistenceByIdAsync(id);
		}
		if (exp != null)
		{
			return await _commentRepo.CheckExistenceAsync(exp);
		}
		throw new ArgumentException();
	}

	public async Task<bool> DeleteRange(IEnumerable<int> commentIds)
	{
		validateNull(commentIds);
		var commentsToDelete = await GetAll(c => commentIds.Contains(c.CommentId));
		return await _commentRepo.DeleteEntitiesAsync(commentsToDelete);
	}
}
