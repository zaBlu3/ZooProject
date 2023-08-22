tinymce.init({
	selector: 'textarea',
	plugins: 'anchor autolink charmap emoticons lists  table visualblocks checklist  tableofcontents powerpaste tinymcespellchecker autocorrect ',
	toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | align lineheight | checklist numlist bullist indent outdent',
});
function confirmSubmit() {
	if (confirm("Are you sure you want to delete image?\n*This action will RESET the from - All changes will be LOST*")) {
		return true;
	} else {
		return false;
	}
}