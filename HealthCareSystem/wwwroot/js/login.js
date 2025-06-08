// Authentication JavaScript for MediCare

document.addEventListener('DOMContentLoaded', function () {
    const activeForm = document.body.getAttribute("data-active-form") || "login";
    showForm(activeForm);
    if (activeForm === "register") {
        setActiveToggle(registerToggle, loginToggle);
    } else {
        setActiveToggle(loginToggle, registerToggle);
    }
    // Form elements
    const loginToggle = document.getElementById('loginToggle');
    const registerToggle = document.getElementById('registerToggle');
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');
    const forgotPasswordForm = document.getElementById('forgotPasswordForm');
    const forgotPasswordLink = document.getElementById('forgotPasswordLink');
    const backToLoginLink = document.getElementById('backToLoginLink');

    // Form elements
    const loginFormElement = document.getElementById('loginFormElement');
    const registerFormElement = document.getElementById('registerFormElement');
    const forgotPasswordFormElement = document.getElementById('forgotPasswordFormElement');

    // Password toggle buttons
    const toggleLoginPassword = document.getElementById('toggleLoginPassword');
    const toggleRegisterPassword = document.getElementById('toggleRegisterPassword');

    // Password strength elements
    const registerPassword = document.getElementById('registerPassword');
    const confirmPassword = document.getElementById('confirmPassword');
    const passwordStrengthBar = document.getElementById('passwordStrengthBar');
    const passwordStrengthText = document.getElementById('passwordStrengthText');

    // Loading overlay
    const loadingOverlay = document.getElementById('loadingOverlay');

    // Success modal
    const successModal = new bootstrap.Modal(document.getElementById('successModal'));
    const successMessage = document.getElementById('successMessage');

    // Add this to the DOMContentLoaded event listener
    const authContainer = document.querySelector('.auth-container');
    authContainer.style.transition = 'all 0.3s ease';

    // Form toggle functionality
    loginToggle.addEventListener('click', function () {
        showForm('login');
        setActiveToggle(loginToggle, registerToggle);
    });

    registerToggle.addEventListener('click', function () {
        showForm('register');
        setActiveToggle(registerToggle, loginToggle);
    });

    forgotPasswordLink.addEventListener('click', function (e) {
        e.preventDefault();
        showForm('forgot');
    });

    backToLoginLink.addEventListener('click', function (e) {
        e.preventDefault();
        showForm('login');
        setActiveToggle(loginToggle, registerToggle);
    });

    // Password toggle functionality
    toggleLoginPassword.addEventListener('click', function () {
        togglePasswordVisibility('loginPassword', this);
    });

    toggleRegisterPassword.addEventListener('click', function () {
        togglePasswordVisibility('registerPassword', this);
    });

    // Password strength checker
    registerPassword.addEventListener('input', function () {
        checkPasswordStrength(this.value);
        validatePasswordMatch();
    });

    confirmPassword.addEventListener('input', function () {
        validatePasswordMatch();
    });

    // Form submissions
    loginFormElement.addEventListener('submit', function (e) {
        e.preventDefault();
        if (validateForm(this)) {
            handleLogin();
        }
    });

    registerFormElement.addEventListener('submit', function (e) {
        e.preventDefault();
        if (validateForm(this) && validatePasswordMatch()) {
            handleRegister();
        }
    });

    forgotPasswordFormElement.addEventListener('submit', function (e) {
        e.preventDefault();
        if (validateForm(this)) {
            handleForgotPassword();
        }
    });

    // Real-time validation
    const inputs = document.querySelectorAll('input[required], select[required]');
    inputs.forEach(input => {
        input.addEventListener('blur', function () {
            validateField(this);
        });

        input.addEventListener('input', function () {
            if (this.classList.contains('is-invalid')) {
                validateField(this);
            }
        });
    });

    // Phone number formatting
    // const phoneInput = document.getElementById('phoneNumber');
    // if (phoneInput) {
    //     phoneInput.addEventListener('input', function() {
    //         formatPhoneNumber(this);
    //     });
    // }

    // Date of birth validation
    const dobInput = document.getElementById('dateOfBirth');
    if (dobInput) {
        const today = new Date();
        const maxDate = new Date(today.getFullYear() - 13, today.getMonth(), today.getDate());
        const minDate = new Date(today.getFullYear() - 120, today.getMonth(), today.getDate());

        dobInput.setAttribute('max', maxDate.toISOString().split('T')[0]);
        dobInput.setAttribute('min', minDate.toISOString().split('T')[0]);
    }

    // Social login handlers
    const socialButtons = document.querySelectorAll('.social-login .btn');
    socialButtons.forEach(button => {
        button.addEventListener('click', function () {
            const provider = this.querySelector('i').classList.contains('fa-google') ? 'Google' :
                this.querySelector('i').classList.contains('fa-facebook') ? 'Facebook' : 'Twitter';
            handleSocialLogin(provider);
        });
    });

    // Functions
    function showForm(formType) {
        const authContainer = document.querySelector('.auth-container');

        // Hide all forms
        loginForm.classList.add('d-none');
        registerForm.classList.add('d-none');
        forgotPasswordForm.classList.add('d-none');

        // Show selected form and adjust container width
        switch (formType) {
            case 'login':
                loginForm.classList.remove('d-none');
                authContainer.classList.remove('register-active');
                break;
            case 'register':
                registerForm.classList.remove('d-none');
                authContainer.classList.add('register-active');
                break;
            case 'forgot':
                forgotPasswordForm.classList.remove('d-none');
                authContainer.classList.remove('register-active');
                break;
        }
    }

    function setActiveToggle(activeBtn, inactiveBtn) {
        activeBtn.classList.add('active');
        inactiveBtn.classList.remove('active');
    }

    function togglePasswordVisibility(inputId, button) {
        const input = document.getElementById(inputId);
        const icon = button.querySelector('i');

        if (input.type === 'password') {
            input.type = 'text';
            icon.classList.remove('fa-eye');
            icon.classList.add('fa-eye-slash');
        } else {
            input.type = 'password';
            icon.classList.remove('fa-eye-slash');
            icon.classList.add('fa-eye');
        }
    }

    function checkPasswordStrength(password) {
        let strength = 0;
        let strengthText = '';
        let strengthClass = '';

        if (password.length >= 8) strength++;
        if (password.match(/[a-z]/)) strength++;
        if (password.match(/[A-Z]/)) strength++;
        if (password.match(/[0-9]/)) strength++;
        if (password.match(/[^a-zA-Z0-9]/)) strength++;

        switch (strength) {
            case 0:
            case 1:
                strengthText = 'Very Weak';
                strengthClass = 'password-weak';
                break;
            case 2:
                strengthText = 'Weak';
                strengthClass = 'password-weak';
                break;
            case 3:
                strengthText = 'Medium';
                strengthClass = 'password-medium';
                break;
            case 4:
                strengthText = 'Strong';
                strengthClass = 'password-strong';
                break;
            case 5:
                strengthText = 'Very Strong';
                strengthClass = 'password-strong';
                break;
        }

        const percentage = (strength / 5) * 100;
        passwordStrengthBar.style.width = percentage + '%';
        passwordStrengthBar.className = `progress-bar ${strengthClass}`;
        passwordStrengthText.textContent = strengthText;
    }

    function validatePasswordMatch() {
        const password = registerPassword.value;
        const confirm = confirmPassword.value;

        if (confirm && password !== confirm) {
            confirmPassword.classList.add('is-invalid');
            confirmPassword.classList.remove('is-valid');
            return false;
        } else if (confirm) {
            confirmPassword.classList.remove('is-invalid');
            confirmPassword.classList.add('is-valid');
            return true;
        }
        return true;
    }

    function validateField(field) {
        const value = field.value.trim();
        let isValid = true;

        // Required field validation
        if (field.hasAttribute('required') && !value) {
            isValid = false;
        }

        // Email validation
        if (field.type === 'email' && value) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) {
                isValid = false;
            }
        }

        // Password validation
        if (field.type === 'password' && field.id === 'registerPassword' && value) {
            if (value.length < 8) {
                isValid = false;
            }
        }

        // Phone validation
        if (field.type === 'tel' && value) {
            const phoneRegex = /^$$\d{3}$$ \d{3}-\d{4}$/;
            if (!phoneRegex.test(value)) {
                isValid = false;
            }
        }

        // Update field appearance
        if (isValid) {
            field.classList.remove('is-invalid');
            field.classList.add('is-valid');
        } else {
            field.classList.remove('is-valid');
            field.classList.add('is-invalid');
        }

        return isValid;
    }

    function validateForm(form) {
        const fields = form.querySelectorAll('input[required], select[required]');
        let isValid = true;

        fields.forEach(field => {
            if (!validateField(field)) {
                isValid = false;
            }
        });

        // Additional validation for register form
        if (form.id === 'registerFormElement') {
            const agreeTerms = document.getElementById('agreeTerms');
            if (!agreeTerms.checked) {
                agreeTerms.classList.add('is-invalid');
                isValid = false;
            } else {
                agreeTerms.classList.remove('is-invalid');
            }
        }

        return isValid;
    }

    function formatPhoneNumber(input) {
        let value = input.value.replace(/\D/g, '');

        if (value.length >= 6) {
            value = value.replace(/(\d{3})(\d{3})(\d{4})/, '($1) $2-$3');
        } else if (value.length >= 3) {
            value = value.replace(/(\d{3})(\d{3})/, '($1) $2');
        }

        input.value = value;
    }

    function showLoading() {
        loadingOverlay.classList.remove('d-none');
    }

    function hideLoading() {
        loadingOverlay.classList.add('d-none');
    }

    function showSuccess(message) {
        successMessage.textContent = message;
        successModal.show();
    }

    function showNotification(message, type = 'info') {
        const notification = document.createElement('div');
        notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
        notification.style.cssText = 'top: 100px; right: 20px; z-index: 9999; min-width: 300px;';

        notification.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        document.body.appendChild(notification);

        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 5000);
    }

    // Form submission handlers
    function handleLogin() {
        showLoading();

        const formData = new FormData(loginFormElement);
        const loginData = {
            email: formData.get('email'),
            password: formData.get('password'),
            remember: document.getElementById('rememberMe').checked
        };

        // Simulate API call
        setTimeout(() => {
            hideLoading();

            // Simulate successful login
            if (loginData.email && loginData.password) {
                showSuccess('Login successful! Redirecting to dashboard...');

                // Store user session (in real app, this would be handled by backend)
                localStorage.setItem('medicare_user', JSON.stringify({
                    email: loginData.email,
                    loginTime: new Date().toISOString()
                }));

                // Redirect after success modal
                setTimeout(() => {
                    window.location.href = 'index.html';
                }, 2000);
            } else {
                showNotification('Invalid email or password. Please try again.', 'danger');
            }
        }, 2000);
    }

    function handleRegister() {
        showLoading();

        const formData = new FormData(registerFormElement);
        const registerData = {
            firstName: formData.get('firstName'),
            lastName: formData.get('lastName'),
            email: formData.get('email'),
            phone: formData.get('phone'),
            dob: formData.get('dob'),
            gender: formData.get('gender'),
            password: formData.get('password'),
            newsletter: document.getElementById('newsletterSubscribe').checked
        };

        // Simulate API call
        setTimeout(() => {
            hideLoading();

            // Simulate successful registration
            showSuccess('Account created successfully! Please check your email for verification.');

            // Store user data (in real app, this would be handled by backend)
            localStorage.setItem('medicare_user', JSON.stringify({
                ...registerData,
                password: undefined, // Don't store password
                registrationTime: new Date().toISOString()
            }));

            // Switch to login form after success
            setTimeout(() => {
                showForm('login');
                setActiveToggle(loginToggle, registerToggle);
                registerFormElement.reset();
            }, 2000);
        }, 2000);
    }

    function handleForgotPassword() {
        showLoading();

        const formData = new FormData(forgotPasswordFormElement);
        const email = formData.get('email');

        // Simulate API call
        setTimeout(() => {
            hideLoading();
            showSuccess('Password reset instructions have been sent to your email.');

            setTimeout(() => {
                showForm('login');
                setActiveToggle(loginToggle, registerToggle);
                forgotPasswordFormElement.reset();
            }, 2000);
        }, 1500);
    }

    function handleSocialLogin(provider) {
        showLoading();

        // Simulate social login
        setTimeout(() => {
            hideLoading();
            showSuccess(`Successfully logged in with ${provider}!`);

            // Store social login data
            localStorage.setItem('medicare_user', JSON.stringify({
                provider: provider,
                loginTime: new Date().toISOString()
            }));

            setTimeout(() => {
                window.location.href = 'index.html';
            }, 2000);
        }, 1500);
    }

    // Auto-fill demo data (for testing purposes)
    function fillDemoData() {
        if (window.location.search.includes('demo=true')) {
            document.getElementById('loginEmail').value = 'demo@medicare.com';
            document.getElementById('loginPassword').value = 'demo123456';
        }
    }

    // Initialize demo data if needed
    fillDemoData();

    // Form auto-save functionality
    const formInputs = document.querySelectorAll('input:not([type="password"]), select');
    formInputs.forEach(input => {
        // Load saved data
        const savedValue = localStorage.getItem(`medicare_auth_${input.id}`);
        if (savedValue && input.type !== 'checkbox') {
            input.value = savedValue;
        } else if (savedValue && input.type === 'checkbox') {
            input.checked = savedValue === 'true';
        }

        // Save data on change
        input.addEventListener('change', function () {
            if (this.id) {
                if (this.type === 'checkbox') {
                    localStorage.setItem(`medicare_auth_${this.id}`, this.checked);
                } else {
                    localStorage.setItem(`medicare_auth_${this.id}`, this.value);
                }
            }
        });
    });

    // Clear auto-saved data on successful submission
    function clearAutoSavedData() {
        const keys = Object.keys(localStorage).filter(key => key.startsWith('medicare_auth_'));
        keys.forEach(key => localStorage.removeItem(key));
    }

    // Keyboard shortcuts
    document.addEventListener('keydown', function (e) {
        // Enter key to submit forms
        if (e.key === 'Enter' && e.target.tagName === 'INPUT') {
            const form = e.target.closest('form');
            if (form && !form.classList.contains('d-none')) {
                e.preventDefault();
                form.querySelector('button[type="submit"]').click();
            }
        }

        // Escape key to close modals
        if (e.key === 'Escape') {
            const openModal = document.querySelector('.modal.show');
            if (openModal) {
                bootstrap.Modal.getInstance(openModal).hide();
            }
        }
    });
});