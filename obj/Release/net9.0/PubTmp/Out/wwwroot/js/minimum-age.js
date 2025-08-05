// Custom method for validating age range
jQuery.validator.addMethod("agerange", function (value, element, params) {
    if (!value) return false;

    var birthDate = new Date(value);
    if (isNaN(birthDate.getTime())) return false;

    var today = new Date();
    var age = today.getFullYear() - birthDate.getFullYear();

    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }

    return age >= params.min && age <= params.max;
}, function (params, element) {
    return $(element).attr("data-val-agerange") || "Invalid age range.";
});

// Unobtrusive adapter for the agerange attribute
jQuery.validator.unobtrusive.adapters.add("agerange", ["min", "max"], function (options) {
    options.rules["agerange"] = {
        min: parseInt(options.params.min),
        max: parseInt(options.params.max)
    };
    options.messages["agerange"] = options.message;
});
