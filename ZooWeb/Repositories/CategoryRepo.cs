namespace ZooWeb.Repositories;

public class CategoryRepo : Repository<Category>
{
	public CategoryRepo(ZooContext context) : base(context)
	{
	}
	public override async Task<bool> UpdateEntityByIdAsync(int id, Category category)
	{
		var categoryToUpdate = await GetEntityByIdAsync(id);
		if (categoryToUpdate != null)
		{
			categoryToUpdate.Name = category.Name;
			return await Save();
		}
		return false;
	}



}
