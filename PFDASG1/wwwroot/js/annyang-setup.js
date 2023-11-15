var messages = ['🔊 Hey', '🔊 Hi, there!', '🔊 Hello'];
var unrecognizedFlag = false;
var listening = false;

if (annyang) {
    console.log("We have annyang!");

    var commands = {
        'Hello': hello,
        'login page': goToLoginPage,
        'home page': goToHomePage,
        'Go to CARD activation': goToCardActivation,
        'Start listening': startListening,
        'Stop listening': stopListening,
        'Access code *code': enterAccessCode,
        'Pin number *pin': enterPIN,
        'Login': login,
        'Logout': logout,
        'Search': search,
        'Type in *value': typeIn,
        'Go to payment page': goToTransferPage,
        
    };



    function hello() {
        var randomIndex = Math.floor(Math.random() * messages.length);
        var message = messages[randomIndex];
        speakResponse(message);
    }

    function goToLoginPage() {
        window.location.href = '/Home/Login';
        speakResponse("You are at the login page.");
    }

    function goToHomePage() {
        window.location.href = '/Home/Index';
        speakResponse("You are at the home page.");
    }

    function goToCardActivation() {
        console.log("goToCardActivation function called");
        window.location.href = '/VisuallyImpaired/Settings';
        speakResponse("Navigating to card activation.");
    }

    const activated = 0;

    function startListening() {
        annyang.start();
        speakResponse("Listening started");
        console.log("Listening started");
        sessionStorage.setItem("annyangStatus", "true");
    }

    function stopListening() {
        annyang.abort();
        listening = false;
        speakResponse("Listening stopped");
        console.log("Listening stopped");
        sessionStorage.setItem("annyangStatus", "false");
    }

    function enterAccessCode(code) {
        code = code.replace(/\s/g, '');
        document.getElementById('accessCodeInput').value = code;
        speakResponse("Access code entered.");
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

    function search() {
        if (window.location.pathname.toLowerCase() === '/home/index' || window.location.pathname === '/') {
            document.getElementById('searchDemo').focus();
        }
        else {
            window.location.href = '/VisuallyImpaired/Search';
        }
        speakResponse("Search bar, use type in command to enter your input");
    }

    
    function typeIn(value) {
        document.getElementById('search').value = value;
        speakResponse("Input entered, press enter to search.");
    }


    function goToTransferPage() {
        window.location.href = '/VisuallyImpaired/Transfer';
        speakResponse("Navigating to transfer page.");
    }

    

    function speakResponse(message, callback) {
        console.log(`%c ${message}`, 'color: blue; font-weight:bold;');
        var speechSynthesis = window.speechSynthesis;
        var speech = new SpeechSynthesisUtterance(message);
        speech.volume = 1; // Adjust volume
        speech.rate = 1; // Adjust speaking rate

        speech.addEventListener('end', function () {
            if (callback && typeof callback === 'function') {
                callback();
            }
        });

        speechSynthesis.speak(speech);
    }

    annyang.addCommands(commands);

    if (sessionStorage.getItem("annyangStatus")) {
        annyang.start();
    }

    // Handle unrecognized command
    annyang.addCallback('resultNoMatch', function (phrases) {
        console.log("resultNoMatch triggered");
        if (!unrecognizedFlag) {
            console.log("Handling unrecognized command");
            speakResponse("Sorry, I didn't catch that. Please say again.", function () {
                unrecognizedFlag = false;  // Reset the flag after speaking
            });
            unrecognizedFlag = true;
        }
    });
}

document.addEventListener("keydown", function (event) {
    if ((event.ctrlKey || event.metaKey) && event.key === " " && document.activeElement.tagName !== "INPUT") {
        if (annyang && annyang.isListening()) {
            stopListening();
        } else {
            startListening();
        }
    }
});