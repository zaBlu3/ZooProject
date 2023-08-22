namespace ZooAPI.Helper;
 
public class NullOnCreationAttribute : ValidationAttribute
{
    public NullOnCreationAttribute()
    {
        ErrorMessage = "{0} should not be provided";
    }
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            string propertyName = validationContext.MemberName!;
            string errorMessage = string.Format(ErrorMessage!, propertyName);
            return new ValidationResult(errorMessage);
        }
        return ValidationResult.Success!;

    }

}
