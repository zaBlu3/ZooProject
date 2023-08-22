function deleteAnimal(_, table) {
	var selectedRows = table.rows({ selected: true }).data().toArray();
	if (selectedRows.length <= 0) return
	if (confirm("Are you sure you want to delete this animal?")) {
		var animalIds = selectedRows.map(row => `IDs=${row.animalId}`).join('&');
		$.ajax({
			url: API_URL + `DeleteRange?${animalIds}`,
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
function editAnimal(_, table) {
	var selectedRows = table.rows({ selected: true }).data().toArray();
	if (selectedRows.length !== 1) return;
	var animalId = selectedRows[0].animalId;
	window.location.href = "/animal/edit/" + animalId;

}
function detailsAnimal(_, table) {
	var selectedRows = table.rows({ selected: true }).data().toArray();
	if (selectedRows.length !== 1) return;
	var animalId = selectedRows[0].animalId;
	window.location.href = "/animal/details/" + animalId;
}



const addButton = {
	text: '<i class="fas fa-plus-circle"></i> Add Animal',
	className: 'btn btn-primary',
	action: () => window.location.href = "/animal/add", 
};

const editButton = {
	text: '<i class="fas fa-edit"></i> Edit Animal',
	className: 'btn btn-outline-primary',
	action: editAnimal,
	enabled: false,
};

const deleteButton = {
	text: '<i class="fas fa-trash"></i> Delete Animal',
	className: 'btn btn-danger',
	action: deleteAnimal,
	enabled: false,
};

const detailsButton = {
	text: '<i class="fas fa-info-circle"></i> Details',
	className: 'btn btn-info',
	action: detailsAnimal,
	enabled: false,
};



$(document).ready(function () {
	const table = $('#animalTable').DataTable({
		ajax: {
			url: API_URL + "all",
			dataSrc: '',
			contentType: 'application/json',
			error: () => alert('Error fetching data'),
		},
		columns: [
			{ data: 'name' },
			{ data: 'age' },
			{
				data: 'categoryName',
				render: (data) => `<a href="/category/table?cn=${data}" onclick="event.stopPropagation();">${data}</a>`,

			},
			{
				data: 'commentsCount',
				render: (data, _, row) =>
					`<a href="/animal/details/${row.animalId}" onclick="event.stopPropagation();">${data}</a>`,

			},
			{ data: 'description' },


		],
		lengthMenu: [1, 3, 6, 12, 18],
		pageLength: 6,
		select: true,
		responsive: true,
		dom: "<'row'<'text-md-end text-center'B>>" +
			"<'row'<'col-sm-12 col-md-6 mt-2'l><'col-sm-12 col-md-6 mt-2'f>>" +
			"<'row'<'col-sm-12'tr>>" +
			"<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
		buttons: [
			addButton,
			editButton,
			deleteButton,
			detailsButton
		],
		columnDefs: [
			{ targets: [0, 4], className: 'text-start' },
			{ targets: [1, 2, 3], className: 'text-center' },
		],
	});

	table.on('select deselect', function () {
		var selectedRows = table.rows({ selected: true }).count();
		table.button(1).enable(selectedRows === 1);
		table.button(2).enable(selectedRows > 0);
		table.button(3).enable(selectedRows === 1);

	});

});