﻿var unrecognizedFlag = false;
var listening = false;
var isShiftKeyPressed = false;
var annyangListening = false;



var commandKeywords = {
    'Go to login page': ['login page', 'bring me to login page', 'take me to login'],
    'Go to home page': ['home page', 'bring me to home page', 'take me to home'],
    'Go to cut activation': ['card activation', 'activate card', 'activate my card', 'go to card activation'],
    'Go to payment page': ['payment page', 'go to payment', 'navigate to payment'],

};

if (annyang) {
    console.log("We have annyang!");

    var commands = {
        'Go to login page': goToLoginPage,
        'Go to home page': goToHomePage,
        'Go to cut activation': goToCardActivation,
        'Start listening': startListening,
        'Stop listening': stopListening,
        'Access code *code': enterAccessCode,
        'Pin *pin': enterPIN,
        'Login': login,
        'Logout': logout,
        'Search': search,
        'Type in *value': typeIn,
        'Go to transfer page': goToTransferPage,
        'Enter': searchEnter,
        'balance': getBalance,
        'Next': clickNextButton,
        'Skip': clickSkipButton,
        'Submit': clickSubmitButton,
        'Increase font size': increaseFontSize,
        'Decrease font size': decreaseFontSize,
        'Toggle dark mode': toggledarkmode,
        'Help': helpCommand,
        'Voice command': voiceCommand,
        'Keyboard': keyboardCommand,
        'Increase font size *fontsize': increaseFontSizeByValue,
        'Decrease font size *fsize': decreaseFontSizeByValue,
        'Yes': closeTutorial,
    };

    function goToLoginPage() {
        window.location.href = '/Home/Login';
        speakResponse("You are at the login page")
    }

    function goToHomePage() {
        window.location.href = '/Home/Index';
        speakResponse("You are at the home page")
    }

    function goToCardActivation() {
        console.log("goToCardActivation function called");
        window.location.href = '/VisuallyImpaired/CardActivation';
        speakResponse("Navigating to card activation.");
    }

    const activated = 0;

    function startListening() {
        if (!annyangListening) {
            annyang.start();
            speakResponse("Listening started");
            console.log("Listening started");
            sessionStorage.setItem("annyangStatus", "true");
            annyangListening = true; // Set the flag to true
        }
    }

    function stopListening() {
        annyang.abort();
        annyangListening = false;
        speakResponse("Listening stopped");
        console.log("Listening stopped");
        sessionStorage.setItem("annyangStatus", "false");
    }

    function enterAccessCode(code) {
        code = code.replace(/-/g, '');
        code = code.replace(/\s/g, '');
        document.getElementById('accessCodeInput').value = code;
        speakResponse("Access code entered.");
    }

    function closeTutorial() {
        var cancelButton = document.getElementById('cancelButton');
        if (cancelButton) {
            cancelButton.click();
        }
    }

    function clickNextButton() {
        var nextButton = document.getElementById('nextButton');
        if (nextButton) {
            nextButton.click();
            activateCount++;
        }
    }

    function clickSkipButton() {
        var skipButton = document.getElementById('skipButton');
        if (skipButton) {
            skipButton.click();
        }
    }

    function clickSubmitButton() {
        var submitButton = document.getElementById('SubmitButton');
        if (submitButton) {
            submitButton.click();
        }
    }

    function enterPIN(pin) {
        pin = pin.replace(/\s/g, '');
        document.getElementById('pinInput').value = pin;
        speakResponse("PIN entered.");
    }

    function login() {
        document.getElementById('login login_btn').click();
        speakResponse("Logging in.");
    }

    function logout() {
        window.location.href = '/Home/Index';
        speakResponse("Logging out.");
    }

    function suggestCommand(input, availableCommands) {
        var minDistance = Number.MAX_VALUE;
        var suggestedCommand = null;

        availableCommands.forEach(function (command) {
            var distance = levenshteinDistance(input, command);
            var keywords = commandKeywords[command] || [];

            // Check if any keyword is present in the input
            if (keywords.some(keyword => input.toLowerCase().includes(keyword.toLowerCase())) ||
                (distance < minDistance && distance < command.length / 2)) {
                minDistance = distance;
                suggestedCommand = command;
            }
        });

        return suggestedCommand;
    }

    function levenshteinDistance(a, b) {
        if (a.length === 0) return b.length;
        if (b.length === 0) return a.length;

        var matrix = [];

        for (var i = 0; i <= b.length; i++) {
            matrix[i] = [i];
        }

        for (var j = 0; j <= a.length; j++) {
            matrix[0][j] = j;
        }

        for (var i = 1; i <= b.length; i++) {
            for (var j = 1; j <= a.length; j++) {
                if (b.charAt(i - 1) === a.charAt(j - 1)) {
                    matrix[i][j] = matrix[i - 1][j - 1];
                } else {
                    matrix[i][j] = Math.min(
                        matrix[i - 1][j - 1],
                        matrix[i][j - 1],
                        matrix[i - 1][j]
                    ) + 1;
                }
            }
        }

        return matrix[b.length][a.length];
    }



    annyang.addCallback('resultNoMatch', function (phrases) {
        console.log("resultNoMatch triggered");
        if (!unrecognizedFlag) {
            console.log("Handling unrecognized command");

            // Check for suggested commands
            var suggestedCommand = suggestCommand(phrases[0], Object.keys(commands));

            if (suggestedCommand) {
                speakResponse(`Did you mean "${suggestedCommand}"?`, function () {
                    unrecognizedFlag = false;  // Reset the flag after speaking
                });
            } else {
                speakResponse("Sorry, I didn't catch that. Please say again.", function () {
                    unrecognizedFlag = false;  // Reset the flag after speaking
                });
            }

            unrecognizedFlag = true;
        }
    });



    function search() {
        if (window.location.pathname.toLowerCase() === '/home/index' || window.location.pathname === '/') {
            document.getElementById('searchDemo').focus();
        } else {
            window.location.href = '/VisuallyImpaired/Search';
        }
        speakResponse("Search bar, use type in command to enter your input");
    }

    function typeIn(value) {
        const focusedInput = document.activeElement;

        if (window.location.pathname.toLowerCase() === '/visuallyimpaired/cardactivation') {
            if (focusedInput.tagName === 'INPUT') {
                // Remove spaces from the input value
                value = value.replace(/\s/g, '');
                focusedInput.value = value;
                speakResponse("Input entered.");
            } else {
                speakResponse("No input field is focused.");
            }
        } else if (window.location.pathname.toLowerCase() === '/home/index' || window.location.pathname === '/') {
            // Remove spaces from the input value
            value = value.replace(/\s/g, '');
            document.getElementById('searchDemo').value = value;
            speakResponse("Input entered, You can go to the next step now.");
        } else if (window.location.pathname.toLowerCase() === '/visuallyimpaired/transfer') {
            if (focusedInput.tagName === 'INPUT') {
                // Remove spaces from the input value
                value = value.replace(/\s/g, '');
                focusedInput.value = value;
                speakResponse("Input entered.");
            } else {
                speakResponse("No input field is focused.");
            }
        } else if (window.location.pathname.toLowerCase() === '/visuallyimpaired/sqsetup' || window.location.pathname === '/') {
            const answerInput = document.querySelector('.security-answer');
            if (answerInput) {
                // Remove spaces from the input value
                value = value.replace(/\s/g, '');
                answerInput.value = value;
                speakResponse("Answer entered.");
            } else {
                speakResponse("No input field is focused.");
            }
        } else {
            // Remove spaces from the input value
            value = value.replace(/\s/g, '');
            document.getElementById('search').value = value;
            const inputEvent = new Event('input', { bubbles: true });
            document.getElementById('search').dispatchEvent(inputEvent);
        }
    }



    function getBalance() {
        var totalBalanceElement = document.querySelector('.total-balance h1');
        if (totalBalanceElement) {
            var totalBalanceText = totalBalanceElement.textContent.trim();
            speakResponse("Your  " + totalBalanceText);
        } else {
            speakResponse("Unable to retrieve your balance at the moment.");
        }
    }

    function searchEnter() {
        const enterEvent = new KeyboardEvent('keypress', { 'key': 'Enter' });
        document.getElementById('search').dispatchEvent(enterEvent);
    }

    function goToTransferPage() {
        window.location.href = '/VisuallyImpaired/Transfer';
        speakResponse("Navigating to payment page.");
    }

    function helpCommand() {
        document.body.classList.toggle("show-chatbot");
    }

    function voiceCommand() {
        handleButtonClick("voice-command");
    }

    function keyboardCommand() {
        handleButtonClick("keyboard-shortcut");
    }


    function speakResponse(message, callback) {
        console.log(`%c ${message}`, 'color: blue; font-weight:bold;');
        var speechSynthesis = window.speechSynthesis;
        var speech = new SpeechSynthesisUtterance(message);
        speech.volume = 1;
        speech.rate = 1;

        speech.addEventListener('end', function () {
            if (callback && typeof callback === 'function') {
                callback();
            }
        });

        speechSynthesis.speak(speech);
    }

    annyang.addCommands(commands);



    annyang.addCallback('resultNoMatch', function (phrases) {
        console.log("resultNoMatch triggered");
        if (!unrecognizedFlag) {
            console.log("Handling unrecognized command");
            speakResponse("Sorry, I didn't catch that. Please say again.", function () {
                unrecognizedFlag = false;
            });
            unrecognizedFlag = true;
        }
    });
}

document.addEventListener("keydown", function (event) {
    if (event.key === "Control") {
        event.preventDefault();
        isShiftKeyPressed = true;
        togglePushToTalk();
    }
});

document.addEventListener("keyup", function (event) {
    if (event.key === "Control") {
        event.preventDefault();
        isShiftKeyPressed = false;
        togglePushToTalk();
    }
});

function togglePushToTalk() {
    if (isShiftKeyPressed) {
        startListening();
    } else {
        if (annyangListening) {
            stopListening();
        }
    }
}

function increaseFontSize() {
    adjustFontSize(2);
}

function decreaseFontSize() {
    adjustFontSize(-2); // Call the existing adjustFontSize function with a change of -1
}

function increaseFontSizeByValue(fontsize) {
    // Parse the spoken value to an integer
    var fontSizeChange = parseInt(fontsize);

    // Check if the parsed value is a valid number
    if (!isNaN(fontSizeChange)) {
        adjustFontSize(fontSizeChange);
        speakResponse(`Font size increase by ${fontSizeChange} units.`);
    } else {
        speakResponse('Invalid font size adjustment. Please specify a number.');
    }
}

function decreaseFontSizeByValue(fsize) {
    // Parse the spoken value to an integer
    var fontSizeChange = parseInt(fsize);

    // Check if the parsed value is a valid number
    if (!isNaN(fontSizeChange)) {
        // Invert the value for decreasing font size
        fontSizeChange = -fontSizeChange;
        adjustFontSize(fontSizeChange);
        speakResponse(`Font size decreased by ${-fontSizeChange} units.`);
    } else {
        speakResponse('Invalid font size adjustment. Please specify a number.');
    }
}

function toggledarkmode() {
    toggleDarkMode();
}