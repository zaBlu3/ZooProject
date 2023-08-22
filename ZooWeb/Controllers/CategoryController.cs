namespace ZooWeb.Controllers;



[Authorize(Roles = Roles.Admin)]
public class CategoryController : Controller
{
	private readonly IHttpClientFactory _httpClientFactory;

	public CategoryController(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	public IActionResult Table()
	{
		HttpClient httpClient = _httpClientFactory.CreateClient("CategoriesApiClient");
		ViewData["Url"] = httpClient.BaseAddress!.ToString();
		var notification = Request.Cookies["Notification"];
		if (notification != null)
		{
			Response.Cookies.Delete("Notification");
			var notifInfo = notification.Split("-");
			var (notifType, notifMessage) = (notifInfo[0], notifInfo[1..]);
			TempData[notifType] = string.Join(" ", notifMessage);

		}
		return View();
	}
	public async Task<IActionResult> Upsert(int id)
	{
		Category category = new Category();
		HttpClient httpClient = _httpClientFactory.CreateClient("CategoriesApiClient");
		ViewData["Url"] = httpClient.BaseAddress!.ToString();
		if (id != 0)
		{
			var response = await httpClient.GetAsync(id.ToString());
			if (response.IsSuccessStatusCode)
			{
				category = response.Content.ReadFromJsonAsync<Category>().Result!;
			}
			else
			{
				return Content(await response.Content.ReadAsStringAsync());

			}
		}
		return View(category);
	}
}