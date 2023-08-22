namespace ZooWeb.Components;

public class CommentsFormViewComponent : ViewComponent
{
	public IViewComponentResult Invoke(Comment c)
	{
		if (User.Identity != null && User.Identity!.IsAuthenticated)
			return View("_CommentsForm", c);
		else
			return Content("Login To Add Comment");
	}
}
