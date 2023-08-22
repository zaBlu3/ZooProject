using System.Text.Json.Serialization;

namespace Zoo.Models.Zoo;

public class Category
{
    public int CategoryId { get; set; }
    [Required, StringLength(50, MinimumLength = 2)]
    [RegularExpression(@"^[a-zA-Z,.`-]+(?:\s+[a-zA-Z`?!-]+)*$", ErrorMessage = "The field {0} should contain only letters or  .,`?!- charachters and no double spaces.")]
    public string? Name { get; set; }
	[JsonIgnore]

	public virtual ICollection<Animal>? Animals { get; set; }

}