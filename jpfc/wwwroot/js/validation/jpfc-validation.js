$(function () {
    jQuery.validator.addMethod('requiredif', function (value, element, params) {
        var $form = $(element).closest('form');
        var $element = $('#' + $form.attr('id') + ' #' + params[0]);
        var currentValue = $element.val();
        if ($element.is(':radio')) {
            currentValue = $('#' + $form.attr('id') + ' input[name=' + params[0] + ']:checked').val();
        }

        var valToCompare = params[1];
        var valToAvoid = params[2];
        var valArray, matchFound;


        if (valToAvoid !== '') {
            if (valToAvoid.indexOf(',') > -1) {
                valArray = valToAvoid.split(',');
                matchFound = false;
                for (var i = 0; i < valArray.length; i++) {
                    if (currentValue.toString().toLowerCase().trim() === valArray[i].toString().toLowerCase().trim()) {
                        matchFound = true;
                    }
                }

                if (!matchFound && value === '') {
                    return false;
                } else {
                    return true;
                }
            } else {
                if (currentValue.toString().toLowerCase() !== valToAvoid.toString().toLowerCase() && value === '') {
                    return false;
                } else {
                    return true;
                }
            }
        }

        if (valToCompare !== undefined && valToCompare !== null) {
            if (valToCompare.indexOf(',') > -1) {
                valArray = valToCompare.split(',');
                matchFound = false;
                for (var x = 0; x < valArray.length; x++) {
                    if (currentValue.toString().toLowerCase().trim() === valArray[x].toString().toLowerCase().trim()) {
                        matchFound = true;
                    }
                }

                if (matchFound && value === '') {
                    return false;
                } else {
                    return true;
                }
            } else {
                if (currentValue.toString().toLowerCase() === valToCompare.toString().toLowerCase() && value === '') {
                    return false;
                } else {
                    return true;
                }
            }
        }
        return false;
    });

    jQuery.validator.unobtrusive.adapters.add('requiredif', ['prop', 'hasvalue', 'nothasvalue'], function (options) {
        options.rules['requiredif'] = [options.params['prop'], options.params['hasvalue'], options.params['nothasvalue']];
        options.messages['requiredif'] = options.message;
    });

    // extend range validator method to treat checkboxes differently
    var defaultRangeValidator = jQuery.validator.methods.range;
    jQuery.validator.methods.range = function (value, element, param) {
        if (element.type === 'checkbox') {
            // if it's a checkbox return true if it is checked
            return element.checked;
        } else {
            // otherwise run the default validation function
            return defaultRangeValidator.call(this, value, element, param);
        }
    };
}(jQuery));