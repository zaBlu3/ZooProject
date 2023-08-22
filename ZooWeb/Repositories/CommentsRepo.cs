namespace ZooWeb.Repositories;

public class CommentsRepo : Repository<Comment>
{
	public CommentsRepo(ZooContext context) : base(context)
	{
	}
	public override async Task<bool> UpdateEntityByIdAsync(int id, Comment comment)
	{
		var commentToUpdate = await GetEntityByIdAsync(id);
		if (commentToUpdate != null)
		{
			commentToUpdate.Text = comment.Text;
			return await Save();
		}
		return false;
	}
}
