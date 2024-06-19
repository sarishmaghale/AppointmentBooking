
$(document).ready(function () {
    $('#toggleInsidCheckbox').on('click', function () {
        if ($(this).is(':checked')) {
            $('#insid').removeAttr('readonly');
        } else {
            $('#insid').attr('readonly', 'readonly');
        }

    });
});

 function calculateBirthDate(e) {
    var age = $(".Age").val();
    var ageType = $(".AgeType").val();
    var today = new Date();
    var dob = new Date();
    if (ageType == "years") {
        dob.setFullYear(today.getFullYear() - age);
    } else if (ageType == "months") {
        let totalMonths = (today.getFullYear() * 12 + today.getMonth()) - age;
        dob.setFullYear(Math.floor(totalMonths / 12));
        dob.setMonth(totalMonths % 12);
    } else if (ageType == "days") {
        dob.setDate(today.getDate() - age);
    }
    // Parse the input date string into a Date object
    var inputDate = new Date(dob);
   // Extract year, month, and day from the Date object
    var year = inputDate.getFullYear();
    var month = (inputDate.getMonth() + 1).toString().padStart(2, '0'); // Months are zero-based
    var day = inputDate.getDate().toString().padStart(2, '0');
    // Return the formatted date string
    var data = `${year}-${month}-${day}`;
    $(".DOB").val(data);
}


function ShowAge(e) {
    var data = $(".DOB").val();
    var today = new Date();
    var dob = new Date(data);
    var ageInYears = today.getFullYear() - dob.getFullYear();
    var monthDiff = today.getMonth() - dob.getMonth();
    var dayDiff = today.getDate() - dob.getDate();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < dob.getDate())) {
        ageInYears--;
        monthDiff += 12; // Correct month difference
    }
    // Adjust for the case where current day is before birth day in the same month
    if (dayDiff < 0) {
        monthDiff--;
        let daysInLastMonth = new Date(today.getFullYear(), today.getMonth(), 0).getDate();
        dayDiff += daysInLastMonth;
    }
    var ageInMonths = ageInYears * 12 + monthDiff;
    // Calculate age in days if age is less than a month
    if (ageInMonths === 0) {
        var ageInDays = Math.floor((today - dob) / (1000 * 60 * 60 * 24));
        $(".Age").val(ageInDays);
        $(".AgeType").val("days");
        return ageInDays;
    } else if (ageInYears < 1) {
        $(".Age").val(ageInMonths);
        $(".AgeType").val("months");
        return ageInMonths;
    } else {
        $(".Age").val(ageInYears);
        $(".AgeType").val("years");
        return ageInYears;
    }

}

function ShowMessage(string? message) {
    if (message !== '') {
        // Display SweetAlert
        Swal.fire({
            title: message,
            showCancelButton: true,
            confirmButtonText: 'OK',
            CancelButtonText: 'Cancel',
        });
    }
}