namespace ZooWeb;

public class PaginModel
{
	private int _currentPage;
	public bool AtLastPage => _currentPage == TotalPages;
	public bool AtFirstPage => _currentPage == 1;

	public int TotalItems { get; set; }
	public int PageSize { get; set; }
	public int CurrentPage
	{
		get => _currentPage;
		set
		{
			if (value > TotalPages || value < 1)
				_currentPage = 1;
			else
				_currentPage = value;
			updatePages();

		}
	}

	private void updatePages()
	{
		StartPage = _currentPage - 2;
		LastPage = _currentPage + 2;
		if (StartPage <= 0)
		{
			LastPage -= StartPage - 1;
			StartPage = 1;
		}
		if (LastPage > TotalPages)
		{
			if (StartPage != 1)
				StartPage -= LastPage - TotalPages;
			LastPage = TotalPages;
		}
	}

	public int StartPage { get; set; } = 1;
	public int LastPage { get; set; }

	public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);


}