
namespace ZooWeb.Models;

public class Animal
{
	public int AnimalId { get; set; }
	[Required, StringLength(20, MinimumLength = 2)]
	public string? Name { get; set; }
	[Required, Range(1, 100)]
	public int Age { get; set; }
	[Required, StringLength(50),DisplayName("Image Name")]
	public string? ImageName { get; set; }
	[StringLength(2000)]
	public string? Description { get; set; }
	[Required]
	public int CategoryId { get; set; }
	public virtual Category? Category { get; set; }
	public virtual ICollection<Comment>? Comments { get; set; }

}