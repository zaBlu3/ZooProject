namespace Zoo.DataAccess.Repositories;

public class CommentRepo : Repository<Comment>
{
	public CommentRepo(ZooContext context) : base(context)
	{
	}
	public override async Task<bool> UpdateEntityByIdAsync(int id, Comment comment)
	{
		var commentToUpdate = await GetEntityByIdAsync(id);
		if (commentToUpdate != null)
		{
			commentToUpdate.Text = comment.Text;
			return await SaveAsync();
		}
		return false;
	}
}
