namespace Zoo.Models;

public static class SuccssesMessages
{
	public static string ActionSuccses<T>(this T _, string action, string? name = null) => $"{name ?? typeof(T).Name} {action} successesfully";
}
