namespace ZooWeb.Controllers;

public class AnimalController : Controller
{
	private readonly IWebHostEnvironment _webHostEnvironment;
	private readonly ILogger<AnimalController> _logger;
	private readonly AnimalsService _animalService;
	private readonly CategoryService _categoryService;
	private readonly HttpClient _httpClient;
	private readonly SortModel _sortModel;
	private PaginModel _pageModel;
	private readonly string ANIMAL_IMAGES_FOLDER;



	public AnimalController(ILogger<AnimalController> logger, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IHttpClientFactory httpClientFactory, AnimalsService animalService, CategoryService categoryService)
	{
		_logger = logger;
		_webHostEnvironment = webHostEnvironment;
		_httpClient = httpClientFactory.CreateClient("AnimalsApiClient");
		_sortModel = new SortModel("commentsCount", "name", "age");
		_pageModel = new PaginModel { PageSize = configuration.GetValue<int>("AppSettings:PageSize") };
		ANIMAL_IMAGES_FOLDER = configuration.GetValue<string>("AppSettings:AnimalImageFolder")!;
		_animalService = animalService;
		_categoryService = categoryService;
		//MoveImageToAnimalFolder().GetAwaiter();
	}


	[Route("{controller}/{action}/{pageNumber?}")]
	public async Task<IActionResult> Cards([FromQuery] int selectedCategory, [FromQuery] string sortExpression, string searchString, [FromRoute] int pageNumber = 1)
	{
		var query = ApllyFilter(selectedCategory, sortExpression, searchString, pageNumber);
		if (_pageModel.CurrentPage != pageNumber) // currentpage will be 1 if the page number doesnt exist
		{
			TempData["ErrorMessage"] = "No such page exists.";
			return Redirect("1" + Request.QueryString);
		}
		var paginatedAnimals = await _animalService.GetPaginedAnimal(_pageModel.CurrentPage, _pageModel.PageSize, query);
		ViewBag.CategoryList = await _categoryService.GetAll();
		ViewBag.SelectedCategory = selectedCategory;
		ViewBag.SearchString = searchString;
		ViewBag.sortModel = _sortModel;
		ViewBag.pageModel = _pageModel;
		return View(paginatedAnimals);
	}

	public async Task<IActionResult> Details(int id)
	{
		ViewData["Url"] = _httpClient.BaseAddress?.ToString();
		var animal = await _animalService.Get(id);
		if (animal == null)
		{
			TempData["ErrorMessage"] = id.NoEntityMessage(nameof(animal));
			return RedirectToAction(nameof(Cards));

		}
		return View(animal);
	}

	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> Edit(int id)
	{
		var animal = await _animalService.Get(id);
		if (animal == null)
		{
			TempData["ErrorMessage"] = id.NoEntityMessage(nameof(animal));
			return RedirectToAction(nameof(Cards));
		}
		IEnumerable<SelectListItem> categoryList = _categoryService.GetAll().Result.Select(c => new SelectListItem { Text = c.Name, Value = c.CategoryId.ToString() });
		return View("Upsert", new UpsertAnimalVM { Animal = animal, CategoryList = categoryList });
	}

	[Authorize(Roles = Roles.Admin)]
	public IActionResult Add()
	{
		IEnumerable<SelectListItem> categoryList = _categoryService.GetAll().Result.Select(c => new SelectListItem { Text = c.Name, Value = c.CategoryId.ToString() });
		return View("Upsert", new UpsertAnimalVM { CategoryList = categoryList });
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Add(UpsertAnimalVM animalVM)
	{
		if (animalVM.ImageFiles == null)
		{
			ModelState.AddModelError(nameof(animalVM.ImageFiles), ErrorMessages.PROVIDE_IMAGE);
		}
		if (ModelState.IsValid)
		{
			try
			{
				var animal = animalVM.Animal;
				if (await _animalService.Create(animal, true))
				{
					TempData["SuccessMessage"] = animal.ActionSuccses("create");
					if (await CopyImagesToServer(animalVM.ImageFiles!, animal))
						return RedirectToAction(nameof(Cards));
				}
			}
			catch (Exception e)
			{
				TempData["ErrorMessage"] = e.Message;
			}
		}
		await UpdateAnimalVM(animalVM);
		return View("Upsert", animalVM);
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(UpsertAnimalVM animalVM)
	{
		var animal = animalVM.Animal;
		if (animal.AnimalId <= 0 || !await _animalService.Exists(animal.AnimalId))
		{
			ModelState.AddModelError(nameof(animal.AnimalId), animal.AnimalId.NoEntityMessage(nameof(animal)));
		}
		if (ModelState.IsValid)
		{
			try
			{
				if (await _animalService.Update(animal.AnimalId, animal, true))
					TempData["SuccessMessage"] = animal.ActionSuccses("update");
				if (animalVM.ImageFiles != null)
					await CopyImagesToServer(animalVM.ImageFiles, animal);
				return RedirectToAction(nameof(Cards));

			}
			catch (Exception e)
			{
				TempData["ErrorMessage"] += e.Message;
			}
		}
		await UpdateAnimalVM(animalVM);
		return View("Upsert", animalVM);
	}

	public IActionResult Table()
	{
		ViewData["Url"] = _httpClient.BaseAddress?.ToString();
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteAnimalImage(int imageId, string ImageURI)
	{

		if (imageId > 0 && ImageURI != null)
		{
			try
			{
				if (await _animalService.DeleteAnimalImage(imageId))
				{
					var fullImagePath = _webHostEnvironment.WebRootPath + ImageURI;
					if (System.IO.File.Exists(fullImagePath)) System.IO.File.Delete(fullImagePath);
					TempData["SuccessMessage"] = ImageURI.ActionSuccses("delete", "Image");
				}
				else
					TempData["ErrorMessage"] = ErrorMessages.ServerFailMessage("Image");
			}
			catch (Exception e)
			{
				TempData["ErrorMessage"] = e.Message;
			}

		}
		return Redirect(Request.Headers["Referer"].ToString());
	}




	#region Helper Methods
	[NonAction]
	private IQueryable<Animal> ApllyFilter(int catigId, string sortExp, string search, int pageNum)
	{
		(int TotalItems, IQueryable<Animal> query) = _animalService.QueryFillterOnAnimals(_sortModel.GetFilter(sortExp), _sortModel.SortOrder, search, catigId);
		_pageModel.TotalItems = TotalItems;
		_pageModel.CurrentPage = pageNum;
		return query;
	}

	[NonAction]
	private async Task UpdateAnimalVM(UpsertAnimalVM animalVM)
	{
		animalVM.CategoryList = _categoryService.GetAll().Result.Select(c => new SelectListItem { Text = c.Name, Value = c.CategoryId.ToString() });
		if (animalVM.Animal.AnimalId != 0)
			animalVM.Animal.AnimalImages = await _animalService.GetAnimalImages(animalVM.Animal.AnimalId);
	}
	[NonAction]
	private async Task<bool> CopyImagesToServer(IEnumerable<IFormFile> images, Animal animal)
	{
		string wwwRootPath = _webHostEnvironment.WebRootPath;
		var animalFolderName = string.Format(ANIMAL_IMAGES_FOLDER, animal.AnimalId);
		var animalFolderPath = wwwRootPath + animalFolderName;
		if (!Directory.Exists(animalFolderPath))
		{
			Directory.CreateDirectory(animalFolderPath);
		}
		foreach (var image in images)
		{
			int imageId = await _animalService.GetNextImageId();
			string imageName = animal.Name + imageId + Path.GetExtension(image.FileName);
			var imagePath = Path.Combine(animalFolderPath, imageName);
			Image<Animal> animalImage = new Image<Animal>()
			{
				ImageURI = Path.Combine(animalFolderName, imageName),
				EntityID = animal.AnimalId,
			};
			if (await _animalService.CreateImage(animalImage))
			{
				using (var filestream = new FileStream(imagePath, FileMode.Create))
				{
					image.CopyTo(filestream);
				}
			}
			else
				TempData["ErrorMessage"] = ErrorMessages.ServerFailMessage("Images");
		}
		if (TempData["ErrorMessage"] == null)
		{
			TempData["SuccessMessage"] += "-" + images.ActionSuccses("copy", "Image");
			return true;
		}
		return false;
	}

	private async Task MoveImageToAnimalFolder()
	{
		var animals = await _animalService.GetAll();
		string wwwRootPath = _webHostEnvironment.WebRootPath;
		string AllAnimalsFolder = Path.Combine(wwwRootPath, "Images", "Animals");
		if (!Directory.Exists(AllAnimalsFolder))
		{
			Directory.CreateDirectory(AllAnimalsFolder);
		}
		foreach (var animal in animals)
		{
			var imagePath = Path.Combine(wwwRootPath, "Images", animal.Name + ".jpg");
			var animalFolderName = string.Format(ANIMAL_IMAGES_FOLDER, animal.AnimalId);
			var animalFolderPath = wwwRootPath + animalFolderName;
			if (!Directory.Exists(animalFolderPath))
			{
				Directory.CreateDirectory(animalFolderPath);
			}
			foreach (var image in animal.AnimalImages)
			{
				string fullpath = _webHostEnvironment.WebRootPath + image.ImageURI;
				if (System.IO.File.Exists(fullpath)) continue;

				using (var sourceStream = new FileStream(imagePath, FileMode.Open))
				{
					using (var destinationStream = new FileStream(fullpath, FileMode.Create))
					{
						await sourceStream.CopyToAsync(destinationStream);
					}
				}
			}
		}

	}

	#endregion



	#region API CALLS		
	[Route("animal/api/delete/{id:int}")]
	[HttpDelete]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> DeleteAnimalAndImages(int id)
	{

		var response = await _httpClient.DeleteAsync(id.ToString());
		if (response.IsSuccessStatusCode)
		{
			var animalFolderName = string.Format(ANIMAL_IMAGES_FOLDER, id);
			var animalFolderPath = _webHostEnvironment.WebRootPath + animalFolderName;
			Directory.Delete(animalFolderPath, true);
			TempData["SuccessMessage"] = id.ActionSuccses("delete", "animal");
			return NoContent();
		}
		else
		{
			TempData["ErrorMessage"] = "Delete Failed";
			return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
		}
	}

	#endregion
}

