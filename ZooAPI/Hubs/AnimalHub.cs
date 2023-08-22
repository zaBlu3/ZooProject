namespace ZooAPI.Hubs;

public class AnimalHub : Hub<IAnimalHubBase>
{
	public async Task AnimalDeleted(int id)
	{
		await Clients.All.AnimalDeleted(id);
	}
	public async Task AnimalsDeleted(params int[]ids)
	{
		await Clients.All.AnimalsDeleted(ids);
	}
}
	
