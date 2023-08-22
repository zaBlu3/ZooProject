namespace Zoo.DataAccess.Repositories;

public class AnimalImageRepo : Repository<Image<Animal>>
{

	public AnimalImageRepo(ZooContext context) : base(context)
	{
	}

	public override Task<bool> UpdateEntityByIdAsync(int id, Image<Animal> Entity)
	{
		return UpdateEntityAsync(Entity);
	}
}
