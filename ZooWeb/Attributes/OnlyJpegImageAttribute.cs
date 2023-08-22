namespace ZooWeb.Attributes;



public class OnlyJpegImageAttribute : ValidationAttribute
{

	public OnlyJpegImageAttribute()
	{
		ErrorMessage = "Invalid file format. Only JPEG images are allowed.";
	}
	protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
	{
		if (value is IEnumerable<IFormFile> files)
		{
			foreach (var file in files)
			{
				var contentType = file.ContentType;
				if (!contentType.StartsWith("image/jpeg"))
				{
					return new ValidationResult(ErrorMessage);
				}

			}
		}
		


		return ValidationResult.Success!;
	}
}