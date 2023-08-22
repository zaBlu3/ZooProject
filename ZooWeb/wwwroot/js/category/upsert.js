const clearCookies = () => document.cookie = "Notification=;expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
$(document).ready(function () {
    const validator = $("#categoryForm").data("validator");
    validator.settings.submitHandler = function (form) {
        const formData = new FormData(form);
        const $submitButton = $(validator.submitButton);
        const $inputs = $(form).find("input");
        $inputs.prop("disabled", true);
        $submitButton.text("Submitting...");
        $submitButton.prop('disabled', true);
        const jsonData = Object.fromEntries(formData);
        $.ajax({
            contentType: "application/json",
            url: API_URL,
            type: MODE,//from rendered view
            data: JSON.stringify(jsonData),
            statusCode: {
                409: ({ responseJSON }) => {
                    validator.showErrors(responseJSON);
                    let inputElement = validator.findByName("Name");
                    let val = inputElement.val().toLowerCase();
                    let customRuleName = `customRule_${val}`;
                    $.validator.addMethod(customRuleName, function (value, element, param) {
                        return value.toLowerCase() !== param;
                    });
                    inputElement.rules("add", {
                        [customRuleName]: val,
                        messages: {
                            [customRuleName]: responseJSON["Name"][0]
                        }
                    });
                }
            },
            success: function () {
                document.cookie = `Notification=SuccessMessage-Category-${MODE}"d"-successfully;path=/;`;
                window.location.href = "/category/table";
            },
            error: function (error) {
                console.error(error);
                document.cookie = "Notification=ErrorMessage-Something-went-wrong;path=/;";
                $submitButton.text("Submit")
                $inputs.prop("disabled", false);
                if (error.status == 0 || error.status == 500) {
                    alert("The site is currently unreachable. Please try again later.");
                    window.location.href = "/category/table";
                }
            }
        });
        return false;
    }
});
