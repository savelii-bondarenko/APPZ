document.addEventListener('DOMContentLoaded', function () {
    console.log('DOMContentLoaded');
    const form = document.getElementById('reservation-form');

    if (!form) {
        console.error('Форма не найдена!');
        return;
    }

    form.addEventListener('submit', function (event) {
        console.log("Срабатывает submit!");

        let startInput = document.getElementById("StartDate").value;
        let endInput = document.getElementById("EndDate").value;

        let startDate = new Date(startInput);
        let endDate = new Date(endInput);
        let now = new Date();
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
    });

    function showError(message, event) {
        alert(message);
        event.preventDefault();
    }
});
