// For enlarging the whole page
function toggleEnlargedPage() {
    document.body.classList.toggle('enlarged-page');

    // Store the user's preference in localStorage
    const isEnlarged = document.body.classList.contains('enlarged-page');
    sessionStorage.setItem('enlargedPage', isEnlarged);
}

function applyUserPagePreference() {
    const isEnlargedPage = sessionStorage.getItem('enlargedPage') === 'true';

    // Toggle the class based on the stored preference
    if (isEnlargedPage) {
        document.body.classList.add('enlarged-page');
    } else {
        document.body.classList.remove('enlarged-page');
    }
}

// For enlarging all content text
function toggleEnlargedText() {
    document.body.classList.toggle('enlarged-text');

    // Store the user's preference in localStorage
    const isEnlarged = document.body.classList.contains('enlarged-text');
    sessionStorage.setItem('enlargedText', isEnlarged);
}

function applyUserTextPreference() {
    const isEnlargedText = sessionStorage.getItem('enlargedText') === 'true';

    // Toggle the class based on the stored preference
    if (isEnlargedText) {
        document.body.classList.add('enlarged-text');
    } else {
        document.body.classList.remove('enlarged-text');
    }
}

document.addEventListener('DOMContentLoaded', () => {
    applyUserPagePreference();
    applyUserTextPreference();
});