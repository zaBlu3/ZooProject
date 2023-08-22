namespace ZooAPI.Helper;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDTO>()
            .ForMember(dest => dest.AnimalCount, opt => opt.MapFrom(src => src.Animals != null ? src.Animals.Count : 0));
        CreateMap<CategoryDTO, Category>();
    }
}
public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentDTO>()
            .ForMember(dest => dest.AnimalName, opt => opt.MapFrom(src => src.Animal!.Name));
        CreateMap<CommentCreateDto, Comment>();
    }
}
public class AnimalMappingProfile : Profile
{
	public AnimalMappingProfile()
    {
		CreateMap<Animal, AnimalDTO>()
			.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
			.ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments!.Count));
	}
}