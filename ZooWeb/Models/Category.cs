
namespace ZooWeb.Models;

public class Category
{
	public int CategoryId { get; set; }
	[Required, StringLength(50, MinimumLength = 2)]
	public string? Name { get; set; }
	public virtual ICollection<Animal>? Animals { get; set; }

}