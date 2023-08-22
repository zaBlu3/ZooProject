
namespace ZooAPI.Hubs.Infra;

public interface ICommentHubBase
{
	Task CommentAdded(CommentDTO c);
	Task CommentDeleted(int id);
	Task CommentsDeleted(params int[] ids);

}