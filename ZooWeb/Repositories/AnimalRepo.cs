namespace ZooWeb.Repositories;

public class AnimalRepo : Repository<Animal>
{

	public AnimalRepo(ZooContext context) : base(context)
	{
	}


	public override async Task<bool> UpdateEntityByIdAsync(int id, Animal animal)
	{
		var animalToUpdate = await GetEntityByIdAsync(id);
		if (animalToUpdate != null)
		{
			animalToUpdate.Name = animal.Name;
			animalToUpdate.Age = animal.Age;
			animalToUpdate.ImageName = animal.ImageName;
			animalToUpdate.Description = animal.Description;
			animalToUpdate.CategoryId = animal.CategoryId;
			return await Save();
		}
		return false;
	}



}
