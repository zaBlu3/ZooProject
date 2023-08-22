namespace Logic.Validation.CustomExceptions;

public class FullValidationException : Exception
{
	public List<ValidationResult>? Errors { get; }

	public FullValidationException() { }
	public FullValidationException(List<ValidationResult> errors)
	{
		Errors = errors;
	}

	public FullValidationException(string message, List<ValidationResult> errors) : base(message)
	{
		Errors = errors;

	}

}
