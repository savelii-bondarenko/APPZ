function OnClick(event) {
    // Get the values from the input fields
    let startDateValue = document.getElementById('StartDate').value;
    let endDateValue = document.getElementById('EndDate').value;

    // Get the current date and time (removing time part for today comparison)
    let today = new Date();
    today.setHours(0, 0, 0, 0); // Remove the time part for today comparison

    // Check if both start date and end date are selected
    if (!startDateValue || !endDateValue) {
        alert("Please select both start and end dates.");
        event.preventDefault();  // Prevent form submission
        return false;
    }

    // Convert string date-time to Date objects
    let startDate = new Date(startDateValue);
    let endDate = new Date(endDateValue);

    // Check if the start date is in the past
    if (startDate < today) {
        alert("Start date cannot be in the past.");
        event.preventDefault();  // Prevent form submission
        return false;
    }

    // Check if start date is after end date
    if (startDate > endDate) {
        alert("Start date cannot be after end date.");
        event.preventDefault();  // Prevent form submission
        return false;
    }

    // If everything is valid, allow form submission
}
