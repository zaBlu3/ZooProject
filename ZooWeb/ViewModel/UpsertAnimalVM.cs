namespace ZooWeb.ViewModel
{
	public class UpsertAnimalVM
	{
		public Animal Animal { get; set; } = new Animal();

		[DisplayName("Image Files")]
		[DataType(DataType.Upload)]
		[OnlyJpegImage]
		[MaxFileCount(3)]
		[MaxFileSize(1000)]
		public IEnumerable<IFormFile>? ImageFiles { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem>? CategoryList { get; set; }


	}
}
