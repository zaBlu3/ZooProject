namespace ZooAPI.Hubs;

public class CommentHub : Hub<ICommentHubBase>
{
	public async Task CommentAdded(CommentDTO c)
	{
		await Clients.All.CommentAdded(c);
	}
	public async Task CommentDeleted(int id)
	{
		await Clients.All.CommentDeleted(id);
	}
	public async Task CommentsDeleted(params int[]ids)
	{
		await Clients.All.CommentsDeleted(ids);
	}
}
	
