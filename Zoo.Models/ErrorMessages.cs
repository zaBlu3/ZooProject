namespace Zoo.Models;

public static class ErrorMessages
{
	public static string NoEntityMessage(this int id, string entity) => $"{entity} with ID {id} doesn`t exists.";
	public static string UniqueNameMessage(this string name, string type) => $"Diffrent{type} with the name {name} already exists";
	public static string ServerFailMessage(string type)=> $"{type}-There was a problem, Please try again later";
	public const string PROVIDE_IMAGE = "You have to provide atleast one image.";
	public const string MAX_IMAGES = "Animal has reached the maximum number of images.";
	public const string DELETE_IMAGE = "Cannot delete the last image of the animal.";
	public const string ID_PROVIDED = "Do not provide an ID when adding.";
	public const string INVALID_ID = "Invalid ID was provided";


}
