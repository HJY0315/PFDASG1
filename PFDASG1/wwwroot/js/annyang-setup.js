var messages = ['🔊 Hey', '🔊 Hi, there!', '🔊 Hello'];
var unrecognizedFlag = false;

if (annyang) {
    console.log("We have annyang!");

    var commands = {
        'Hello': hello,
        'Go to login page': goToLoginPage,
        'Go to home page': goToHomePage,
        'Start listening': startListening,
        'Stop listening': stopListening,
        'Enter access code *code': enterAccessCode,
        'Enter PIN number *pin': enterPIN,
        'Login': login,
        'Logout': logout,
        'Search': search,
        'Type in': typeIn,
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

    const activated = 0;

    function startListening() {
        annyang.start();
        speakResponse("Listening started");
        console.log("Listening started");
        activated = 1;
    }

    function stopListening() {
        annyang.abort();
        listening = false;
        speakResponse("Listening stopped");
        console.log("Listening stopped");
        activated = 0;
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
        window.location.href = '/VisuallyImpaired/Search';
        speakResponse("Search bar, use type in command to enter your input");
    }

    //this one not working
    function typeIn(value) {
        document.getElementById('search').value = value;
        speakResponse("Input entered, press enter to search.");
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

    annyang.start();

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
        if (annyang) {
            stopListening();
        }
        else {
            startListening();
        }
        event.preventDefault();
    }
});