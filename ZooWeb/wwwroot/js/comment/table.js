function deleteManyComments(_, table) {
	var selectedRows = table.rows({ selected: true }).data().toArray();
	if (selectedRows.length <= 1) return
	if (confirm("Are you sure you want to delete this comment?")) {
		var commentIds = selectedRows.map(row => `IDs=${row.commentId}`).join('&');
		$.ajax({
			url: API_URL + `DeleteRange?${commentIds}`,
			type: 'DELETE',
		})
			.done((response) => {
				selectedRows.forEach(r => table.row(r).remove());
				table.draw();
				table.rows().deselect();

				toastr.success(response, 'Success', { timeOut: 3000 });
			})
			.fail(() => toastr.error('Error while deleting animal.', 'Error', { timeOut: 3000 }));
	}

}
function deleteComment(row, commentId) {
	if (confirm("Are you sure you want to delete this comment?")) {
		$.ajax({
			url: API_URL + commentId,
			type: 'DELETE',
		})
			.done(function (response) {
				$('#commentTable').DataTable().row(row).remove().draw();
				toastr.success('Comment deleted successfully!', 'Success', { timeOut: 3000 });
			})
			.fail(function (e) {
				toastr.error(e, 'Error', { timeOut: 3000 });
			});
	}
}
$(document).ready(function () {
	const table = $('#commentTable').DataTable({
		ajax: {
			dataSrc: '',
			url: API_URL + "all",
			contentType: 'application/json',
			error: () => alert('Error fetching data'),
		},
		columns: [
			{ data: 'username' },
			{
				data: 'date',
				render: (data) => `${new Date(data).toLocaleString()}`,

			},
			{
				data: 'animalName',
				render: function (data, _, row) {
					return `<a href="/animal/details/${row.animalId}">${data}</a>`;
				},
			},
			{ data: 'text' },
			{
				data: 'commentId',
				render: (data) => `<button class="btn btn-link" onclick="deleteComment($(this).parents('tr'),${data})">
														<i class="fas fa-trash"></i>
														</button>`,
			},
		],
		responsive: true,
		select: true,
		dom: "<'row'<'text-md-end text-center'B>>" +
			"<'row'<'col-sm-12 col-md-6 mt-2'l><'col-sm-12 col-md-6 mt-2'f>>" +
			"<'row'<'col-sm-12'tr>>" +
			"<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
		buttons: [
			{
				text: '<i class="fas fa-trash"></i> Delete Many',
				className: 'btn btn-danger',
				action: deleteManyComments,
				enabled: false,
			},

		],
		columnDefs: [
			{ target: '_all', className: 'text-center' },
		],
	});


	table.on('select deselect', function () {
		var selectedRows = table.rows({ selected: true }).count();
		table.button(0).enable(selectedRows > 1);
	});
});