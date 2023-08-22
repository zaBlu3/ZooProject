$(document).ready(function () {
    $(".validateForm").submit(function (e) {
        e.preventDefault();
        if (!$(this).valid()) return;
        const formData = new FormData(this);
        const $submitButton = $(this).find("[type='submit']")
        const submitVal = $submitButton.text();
        const $inputs = $(this).find("input");
        $inputs.prop("disabled", true);
        $submitButton.text("Submitting...");
        $submitButton.prop('disabled', true);
        const jsonData = Object.fromEntries(formData);
        $.ajax({
            contentType: "application/json",
            url: "http://localhost:5099/api/comment",
            type: "POST",
            data: JSON.stringify(jsonData)
        })
            .done(() => {
                this.reset();
                toastr.success('Comment Added successfully!', 'Success', {
                    timeOut: 3000
                })
            }  
            )
            .fail(
                ({ message }) => toastr.error(message, 'Error', { timeOut: 3000 })
            )
            .always(() => {
            $submitButton.text(submitVal)
            $submitButton.prop('disabled', false);
            $inputs.prop("disabled", false);
        })
              
    });

});
