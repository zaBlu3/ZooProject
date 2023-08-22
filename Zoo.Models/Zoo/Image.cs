
namespace Zoo.Models.Zoo;

public class Image<TEntity>
{
    public int ImageId { get; set; }
    [Required]
    public string? ImageURI { get; set; }
    [Required]
    public int EntityID { get; set; }
    [JsonIgnore]
    public virtual TEntity? Entity { get; set; }

}
