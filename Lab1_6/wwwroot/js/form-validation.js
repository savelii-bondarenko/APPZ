document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('form');

    const email = document.querySelector('#Email');
    const password = document.querySelector('#Password');
    const confirmPassword = document.querySelector('#ConfirmPassword');
    const firstName = document.querySelector('#FirstName');
    const lastName = document.querySelector('#LastName');

    form.addEventListener('submit', function (e) {
        let isValid = true;

        const emailRegex = /^\S+@\S+\.\S+$/;
        if (!email.value.trim() || !emailRegex.test(email.value)) {
            showError(email, 'Please enter a valid email.');
            isValid = false;
        } else {
            clearError(email);
        }

        if (password.value.length < 6 || password.value.length > 100) {
            showError(password, 'Password must be between 6 and 100 characters.');
            isValid = false;
        } else {
            clearError(password);
        }

        if (password.value !== confirmPassword.value) {
            showError(confirmPassword, 'Passwords do not match.');
            isValid = false;
        } else {
            clearError(confirmPassword);
        }

        const nameRegex = /^[A-Za-zА-Яа-яІіЇїЄєҐґ'-]+$/;
        if (!firstName.value.trim() || !nameRegex.test(firstName.value)) {
            showError(firstName, 'First name can only contain letters.');
            isValid = false;
        } else {
            clearError(firstName);
        }

        if (!lastName.value.trim() || !nameRegex.test(lastName.value)) {
            showError(lastName, 'Last name can only contain letters.');
            isValid = false;
        } else {
            clearError(lastName);
        }

        if (!isValid) {
            e.preventDefault();
        }
    });

    function showError(input, message) {
        let errorSpan = input.nextElementSibling;
        if (errorSpan && errorSpan.classList.contains('text-danger')) {
            errorSpan.textContent = message;
        }
    }

    function clearError(input) {
        let errorSpan = input.nextElementSibling;
        if (errorSpan && errorSpan.classList.contains('text-danger')) {
            errorSpan.textContent = '';
        }
    }
});
