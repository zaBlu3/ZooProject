using System.Text.Json.Serialization;

namespace Zoo.Models.Zoo;

public class Animal
{
    public int AnimalId { get; set; }
    [Required, DisplayName("Animal Name")]
    [RegularExpression(@"^[a-zA-Z,.`-]+(?:\s+[a-zA-Z`?!-]+)*$", ErrorMessage = "The field {0} should contain only letters or  .,`?!- charachters and no double spaces.")]
    [StringLength(20, MinimumLength = 2)]
    public string? Name { get; set; }
    [Required, DisplayName("Animal Age")]
    [Range(1, 100)]
    public int Age { get; set; }
   
    public string? Description { get; set; }
    [Required, DisplayName("Category")]
    public int CategoryId { get; set; }
	[JsonIgnore]

	public virtual Category? Category { get; set; }
	[JsonIgnore]

	public virtual ICollection<Comment>? Comments { get; set; }
	[JsonIgnore]

	public virtual List<Image<Animal>> AnimalImages { get; set; } = new List<Image<Animal>>();

}


