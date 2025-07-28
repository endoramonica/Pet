// === STATE MANAGEMENT ===
let currentStep = 1;
const totalSteps = 4;

// === DOM ELEMENT CACHING ===
const prevBtn = document.getElementById('prevBtn');
const nextBtn = document.getElementById('nextBtn');
const completeBtn = document.getElementById('completeBtn');
const steps = document.querySelectorAll('.step-content');
const progressSteps = document.querySelectorAll('.progress-step');

// === INITIALIZATION ===
document.addEventListener('DOMContentLoaded', () => {
    initializePage();
    setupEventListeners();
});

/**
 * Khởi tạo trang, bao gồm việc lấy dữ liệu đã lưu nếu có.
 */
async function initializePage() {
    try {
        const response = await fetch('/Onboarding/GetData');
        if (response.ok) {
            const data = await response.json();
            prefillForm(data);
        }
    } catch (error) {
        console.error('Không thể tải dữ liệu onboarding đã có:', error);
    }
    updateUI();
}

/**
 * Điền sẵn dữ liệu vào form từ API.
 * @param {object} data - Dữ liệu từ API GetOnboardingDataAsync.
 */
function prefillForm(data) {
    if (!data) return;

    const accountNameInput = document.getElementById('accountName');
    if (accountNameInput && data.accountName) {
        accountNameInput.value = data.accountName;
    }

    if (data.selectedInterests && data.selectedInterests.length > 0) {
        data.selectedInterests.forEach(interest => {
            const card = document.querySelector(`.pet-type-card[data-pet-type="${interest}"]`);
            card?.classList.add('selected');
        });
    }
    // Bạn có thể thêm logic pre-fill cho các trường khác ở đây
}

/**
 * Gán tất cả các sự kiện cho các phần tử tương tác.
 */
function setupEventListeners() {
    document.querySelectorAll('.pet-type-card').forEach(card => card.addEventListener('click', () => {
        card.classList.toggle('selected');
        validateStep(3);
    }));

    document.querySelectorAll('#step-2 input').forEach(input => input.addEventListener('input', () => validateStep(2)));
    document.querySelectorAll('#step-3 input[name="experience"]').forEach(radio => radio.addEventListener('change', () => validateStep(3)));
}

// === CORE LOGIC FUNCTIONS ===

async function nextStep() {
    if (currentStep >= totalSteps) return;
    if (!validateStep(currentStep)) return;

    setButtonLoading(nextBtn, true);
    const success = await saveProgress();
    setButtonLoading(nextBtn, false);

    if (success) {
        currentStep++;
        updateUI();
    } else {
        alert('Đã có lỗi xảy ra khi lưu tiến trình. Vui lòng thử lại.');
    }
}

function previousStep() {
    if (currentStep <= 1) return;
    currentStep--;
    updateUI();
}

async function completeOnboarding() {
    setButtonLoading(completeBtn, true);
    setTimeout(() => {
        window.location.href = '/Home/Index';
    }, 500);
}

async function saveProgress() {
    const requestBody = {
        userId: 0, // Hoặc lấy từ claim nếu cần
        currentStep: currentStep,
        accountName: document.getElementById('accountName')?.value || null,
        selectedInterests: Array.from(document.querySelectorAll('.pet-type-card.selected')).map(card => card.dataset.petType)
    };

    try {
        const response = await fetch('/Onboarding/Update', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(requestBody)
        });
        return response.ok;
    } catch (error) {
        console.error('Lỗi khi gọi API Update:', error);
        return false;
    }
}


// === UI & VALIDATION FUNCTIONS ===

function updateUI() {
    steps.forEach(step => step.classList.remove('active'));
    document.getElementById(`step-${currentStep}`)?.classList.add('active');

    progressSteps.forEach((step, index) => {
        const stepNum = index + 1;
        step.classList.toggle('active', stepNum === currentStep);
        step.classList.toggle('completed', stepNum < currentStep);
    });

    prevBtn.style.display = currentStep > 1 ? 'inline-flex' : 'none';
    nextBtn.style.display = currentStep < totalSteps ? 'inline-flex' : 'none';
    completeBtn.style.display = currentStep === totalSteps ? 'inline-flex' : 'none';

    validateStep(currentStep);
}

function validateStep(stepToValidate) {
    let isValid = true;
    if (stepToValidate === 2) {
        const nameInput = document.getElementById('accountName');
        nameInput.classList.remove('is-invalid');
        if (!nameInput.value.trim()) {
            isValid = false;
            nameInput.classList.add('is-invalid');
        }
    }
    if (stepToValidate === 3) {
        const petError = document.getElementById('pet-type-error');
        const expError = document.getElementById('experience-error');
        const petsSelected = document.querySelectorAll('.pet-type-card.selected').length > 0;
        const expSelected = !!document.querySelector('input[name="experience"]:checked');
        petError.style.display = petsSelected ? 'none' : 'block';
        expError.style.display = expSelected ? 'none' : 'block';
        isValid = petsSelected && expSelected;
    }
    nextBtn.disabled = !isValid;
    return isValid;
}

// === HELPER FUNCTIONS ===

function setButtonLoading(button, isLoading) {
    button.disabled = isLoading;
    const originalText = button.id === 'nextBtn' ? 'Tiếp tục' : 'Bắt đầu khám phá';
    button.innerHTML = isLoading ? '<span class="spinner"></span>' : `<span class="btn-text">${originalText}</span>`;
    button.classList.toggle('loading', isLoading);
}