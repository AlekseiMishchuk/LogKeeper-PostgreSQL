document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.toast').forEach(toastEl => new bootstrap.Toast(toastEl));
});

function showToast(toastId) {
    new bootstrap.Toast(document.getElementById(toastId)).show();
}