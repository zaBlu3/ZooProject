$(document).ready(function () {
    const MAX_SIZE_FILES = 1000;
    $.validator.addMethod("fileSize", function (_, element, param)
    {
        if (element.files && element.files.length > 0)
        {
            for (var i = 0; i < element.files.length; i++)
            {
                if (element.files[i].size > param * 1024)
                {
                    return false;
                }
            }
        }
        return true;
    });
    $.validator.addMethod("minFiles", function (_, element, param) {
        if (!param) return true;
        if (element.files && element.files.length >= param) {
            return true;
        }
        return false;
    });

    $.validator.addMethod("maxFiles", function (_, element, param) {
        if (element.files && element.files.length > param) {
            return false;
        }
        return true;

    }, (param) => {
        if (param < 3)
            return `You can select a maximum of ${param} Images.
                You can delete current images to be able to add more (if there is one image it can't be deleted without adding new one). 
*******IMPORTANT : Deleteing image will reset form! *******`;
        else
            return `Every Animal can have a maximum of ${param} Images.`;
    });
    $.validator.addMethod("accept", function (_, element, param) {
        if (element.files && element.files.length > 0) {
            for (var i = 0; i < element.files.length; i++) {
                if (element.files[i].type !== param) {
                    return false;
                }
            }
        }
        return true;
    });

    $('#ImageFiles').rules('add', {
        fileSize: MAX_SIZE_FILES,
        maxFiles: MAX_NUMBER_FILES,
        minFiles: MIN_NUMBER_FILES,
        messages: {
            fileSize: "File size exceeds the allowed {0} KB limit.",
            minFiles:"You have to select atleast {0} Image(s)",
            accept: "Please select a valid image file of type: {0}."
        }

    });
});