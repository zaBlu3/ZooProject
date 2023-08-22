
namespace ZooWeb.Models;

public class Comment
{
	public int CommentId { get; set; }
	[Required]
	public int AnimalId { get; set; }
	public virtual Animal? Animal { get; set; }

	[Required]
	[StringLength(300)]
	public string? Text { get; set; }
}
