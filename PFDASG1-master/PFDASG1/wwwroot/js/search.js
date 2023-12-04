document.addEventListener('DOMContentLoaded', function () {
    var searchInput = document.getElementById('search');

    // Handle click event on the search input
    searchInput.addEventListener('click', function () {
        window.location.href = '/VisuallyImpaired/Search';
    });

    // Handle focus event on the search input
    searchInput.addEventListener('focus', function () {
        // Add an event listener for the Enter key press
        document.addEventListener('keydown', handleKeyPress);

        // Remove the event listener after the user presses Enter or blurs the input
        searchInput.addEventListener('blur', function () {
            document.removeEventListener('keydown', handleKeyPress);
        });
    });

    function handleKeyPress(event) {
        if (event.key === 'Enter') {
            window.location.href = '/VisuallyImpaired/Search'; // Change '/Search' to the actual URL of your Search page
        }
    }

});