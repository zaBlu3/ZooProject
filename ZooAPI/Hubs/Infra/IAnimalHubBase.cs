namespace ZooAPI.Hubs.Infra
{
    public interface IAnimalHubBase
    {
        Task AnimalDeleted(int id);
        Task AnimalsDeleted(params int[] ids);
    }
}