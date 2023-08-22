const initialCategoryName = new URLSearchParams(window.location.search).get("cn");
const deleteCategory = (row, categoryId) => {
	if (confirm("Are you sure you want to delete this category?")) {
		$.ajax({
			url: `${API_URL}${categoryId}`,
			type: 'DELETE',
		})
			.done(function (response) {
				$('#categoryTable').DataTable().row(row).remove().draw();
				toastr.success('Category deleted successfully!', 'Success', { timeOut: 3000 });
			})
			.fail(function () {
				toastr.error('Error while deleting category.', 'Error', { timeOut: 3000 });
			});
	}
}


$(document).ready(function () {
	$('#categoryTable').DataTable({
		ajax: {
			url: API_URL + "all",
			dataSrc: '',
			contentType: 'application/json',
			error: () => alert('Error fetching data'),
		},
		columns: [
			{ data: 'name' },
			{
				data: 'animalCount',
				render: (data, _, row) => `<a href="/animal/cards/1?selectedCategory=${row.categoryId}" ">${data}</a>`,
			},
			{
				data: 'categoryId',
				render: function (data) {
					return `<div>
														<a href="/category/upsert/${data}" title="Edit Category"><i class="fas fa-edit"></i></a>
													 <button class="btn btn-link" onclick="deleteCategory($(this).parents('tr'),${data})" title="Delete Category">
														<i class="fas fa-trash"></i>
														</button>
														</div>`;
				},
				orderable: false
			}

		],
		responsive: true,
		lengthMenu: [1, 3, 5, 10],
		pageLength: 3,
		dom: "<'row'<'text-md-end text-center'B>>" +
			"<'row'<'col-sm-12 col-md-6 mt-2'l><'col-sm-12 col-md-6 mt-2'f>>" +
			"<'row'<'col-sm-12'tr>>" +
			"<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
		buttons: [
			{
				text: '<i class="fas fa-plus-circle"></i>Add Category',
				className: 'btn btn-primary',
				action: () => window.location.href = "/category/upsert"
			},
			{
				text: '<i class="fas fa-sync-alt"></i>Refresh Table',
				className: 'btn btn-primary',
				action: () => window.location.href = window.location.href.split("?")[0]
			}
		],
		columnDefs: [
			{ targets: [0], className: 'text-start' },
			{ targets: [1, 2], className: 'text-center' },
		],
		initComplete: function () {
			if (initialCategoryName) {
				this.api().search(initialCategoryName).draw();
			}
		},

	});

});