// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggleEnlargedText() {
    // Assuming 'enlarged-text' is the class for enlarged text
    document.body.classList.toggle('enlarged-text');

    // Store the user's preference in localStorage
    const isEnlarged = document.body.classList.contains('enlarged-text');
    sessionStorage.setItem('enlargedText', isEnlarged);
}

function applyUserPreference() {
    const isEnlarged = sessionStorage.getItem('enlargedText') === 'true';

    // Toggle the class based on the stored preference
    if (isEnlarged) {
        document.body.classList.add('enlarged-text');
    } else {
        document.body.classList.remove('enlarged-text');
    }
}

function toggleEnlargedPage() {
    // Assuming 'enlarged-page' is the class for enlarged page
    document.body.classList.toggle('enlarged-page');

    // Store the user's preference in localStorage
    const isEnlarged = document.body.classList.contains('enlarged-page');
    sessionStorage.setItem('enlargedPage', isEnlarged);
}

function applyUserPagePreference() {
    const isEnlarged = sessionStorage.getItem('enlargedPage') === 'true';

    // Toggle the class based on the stored preference
    if (isEnlarged) {
        document.body.classList.add('enlarged-page');
    } else {
        document.body.classList.remove('enlarged-page');
    }
}

document.addEventListener('DOMContentLoaded', applyUserPreference);
document.addEventListener('DOMContentLoaded', applyUserPagePreference);