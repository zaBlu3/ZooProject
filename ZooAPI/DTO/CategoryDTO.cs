namespace ZooAPI.DTO;

public class CategoryDTO
{
    public int CategoryId { get; set; }
    [Required, StringLength(50, MinimumLength = 2)]
    public string? Name { get; set; }
    [NullOnCreation]
    public int? AnimalCount { get; set; }  
}
