namespace ZooWeb.Controllers;

[Authorize(Roles = Roles.Admin)]
public class CommentController : Controller
{
	private readonly ILogger<CommentController> _logger;
	private readonly string API_URL;

	public CommentController(IConfiguration configuration, ILogger<CommentController> logger)
	{
		_logger = logger;
		API_URL = configuration.GetValue<string>("ApiSettings:BaseUrl")! + "/comment/";
		_logger = logger;
	}

	public IActionResult Table()
	{
		ViewData["Url"] = API_URL;
		return View();
	}
}
