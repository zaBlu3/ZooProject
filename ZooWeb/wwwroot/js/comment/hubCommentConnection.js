var connection = new signalR.HubConnectionBuilder()
	.withUrl("http://localhost:5099/hubs/comment")
	.withAutomaticReconnect()
	.build()

const $commentsDiv = $("#commentsDiv")
connection.on("CommentAdded", newComment => {
	let newCommentHtml = $(`<div class="card mb-3" id="comment-${newComment.commentId}">
			${(IS_ADMIN || USERNAME == newComment.username) ? `<div class="text-end">
						<button type="button" class="btn-close bg-danger" aria-label="Delete" onclick="deleteComment(${newComment.commentId})">
						<span aria-hidden="true">&times;</span>
					</button>
					</div>` : ''}
				
					
					
				
			<div class="card-body">
			<p>${newComment.username}</p>
				<p>${newComment.text}</p>
				<p>${new Date(newComment.date).toLocaleString()}</p>
			</div>
		</div>`);
	if ($commentsDiv.find("#noCommentsMessage").length)
		$commentsDiv.html(newCommentHtml);
	else
		$commentsDiv.append(newCommentHtml)
})
connection.on("CommentDeleted", commentId => {
	const commentElement = $("#comment-" + commentId);
	if (commentElement.length) {
		commentElement.remove();
	}
	if (!$commentsDiv.children().length) {
		var noCommentMessage = $('<p id="noCommentMessage">No comments yet.</p>');
		$commentsDiv.append(noCommentMessage)
	}

})

///sss
connection.on("CommentsDeleted", commentIDs => {
	for (const commentId of commentIDs) {
		const commentElement = $("#comment-" + commentId);
		if (commentElement.length) {
			commentElement.remove();
		}
	}
	if (!$commentsDiv.children().length) {
		var noCommentMessage = $('<p id="noCommentMessage">No comments yet.</p>');
		$commentsDiv.append(noCommentMessage)
	}

})

connection.start().catch(e => console.log(e))
