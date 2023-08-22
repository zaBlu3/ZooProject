namespace ZooAPI.DTO;

public class CommentCreateDto
{
    [Required]
    [StringLength(300)]
    public string? Text { get; set; }

    [Required]
    public int AnimalId { get; set; }
    [Required]
	public string? Username { get; set; }

}
