
namespace Zoo.Models.Zoo;

public class Comment
{
	public int CommentId { get; set; }
	[Required]
	public string? Username { get; set; }

	public DateTime Date { get; }

	[Required]
	[StringLength(300)]
	public string? Text { get; set; }
	[Required]
	public int AnimalId { get; set; }
	[JsonIgnore]

	public virtual Animal? Animal { get; set; }

}
