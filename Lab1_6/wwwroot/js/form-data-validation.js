document.addEventListener('DOMContentLoaded', function () {
    console.log('DOMContentLoaded');
    const form = document.getElementById('reservation-form');

    if (!form) {
        console.error('Form error');
        return;
    }

    form.addEventListener('submit', function (event) {
        let startInput = document.getElementById("StartDate").value;
        let endInput = document.getElementById("EndDate").value;

        let startDate = new Date(startInput);
        let endDate = new Date(endInput);
        let now = new Date();

        let allBookDate = endDate.getTime() - startDate.getTime();  
        now.setSeconds(0, 0);

        if (!startDate.getTime() || !endDate.getTime()) {
            showError("Please enter a valid date.", event);
            return false;
        }

        if (startDate < now) {
            showError("Start date cannot be in the past.", event);
            return false;
        }

        if (endDate < now) {
            showError("End date cannot be in the past.", event);
            return false;
        }

        if (startDate >= endDate) {
            showError("Start date must be earlier than end date.", event);
            return false;
        }

        if (allBookDate < 3600000) {
            showError("Reservation must be at least 1 hour.", event);
            return false;
        }
    });

    function showError(message, event) {
        alert(message);
        event.preventDefault();
    }
});
