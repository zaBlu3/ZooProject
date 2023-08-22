const setting = {
	onfocusout: false,
	errorClass: "is-invalid",
	validClass: "is-valid",

};
$.validator.setDefaults(setting);
$.validator.unobtrusive.options = setting;
$(function () {
	const $form = $(".validateForm");
	const $submitBtn = $form.find(':submit');

	$form.bind("invalid-form.validate", function () {
		$submitBtn.prop('disabled', true);
	});
	$form.on('input', function () {

		if ($form.valid()) {
			$submitBtn.prop('disabled', false);
		} else {
			$submitBtn.prop('disabled', true);
		}
	});



});