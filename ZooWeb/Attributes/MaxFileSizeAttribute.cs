
namespace ZooWeb.Attributes;

public class MaxFileSizeAttribute : ValidationAttribute
{
	private readonly int _maxFileSizeInBytes;

	public MaxFileSizeAttribute(int maxFileSizeInBytes)
	{
		_maxFileSizeInBytes = maxFileSizeInBytes * 1024;
		ErrorMessage = $"Each file should be smaller than {_maxFileSizeInBytes / 1024} KB.";
	}

	protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
	{
		if (value is IEnumerable<IFormFile> files)
		{
			foreach (var file in files)
			{
				if (file.Length > _maxFileSizeInBytes)
				{
					return new ValidationResult(ErrorMessage);
				}
			}
		}

		return ValidationResult.Success!;
	}
}
