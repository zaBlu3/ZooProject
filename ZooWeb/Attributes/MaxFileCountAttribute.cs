namespace ZooWeb.Attributes;

public class MaxFileCountAttribute : ValidationAttribute
{
	private readonly int _maxFileCount;

	public MaxFileCountAttribute(int maxFileCount)
	{
		_maxFileCount = maxFileCount;
		ErrorMessage = $"You can select a maximum of {_maxFileCount} files.";
	}

	protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
	{
		if (value is IEnumerable<IFormFile> files && files.Count() > _maxFileCount)
		{
			return new ValidationResult(ErrorMessage);
		}

		return ValidationResult.Success!;
	}
}