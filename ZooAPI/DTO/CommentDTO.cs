namespace ZooAPI.DTO;

public class CommentDTO
{

    public int CommentId { get; set; }
    public int AnimalId { get; set; }
    public string? AnimalName { get; set; }
    public string? Username { get; set; }
    public DateTime Date { get; set; }
    public string? Text { get; set; }
}