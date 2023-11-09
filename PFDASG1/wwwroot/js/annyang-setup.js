var messages = ['🔊 Hey', '🔊 Hi, there!', '🔊 Hello'];

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
        'Logout': logout
    }

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

    function startListening() {
        annyang.start();
        speakResponse("Listening started");
        console.log("Listening started");
    }

    function stopListening() {
        annyang.abort();
        listening = false;
        speakResponse("Listening stopped");
        console.log("Listening stopped");
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

    function speakResponse(message) {
        console.log(`%c ${message}`, 'color: blue; font-weight:bold;');

        // Use the Web Speech API to respond with spoken text
        var speechSynthesis = window.speechSynthesis;
        var speech = new SpeechSynthesisUtterance(message);
        speech.volume = 1; // Adjust volume
        speech.rate = 1; // Adjust speaking rate

        speechSynthesis.speak(speech);
    }

    annyang.addCommands(commands);

    annyang.start();
}

document.addEventListener("keydown", function (event) {
    if ((event.ctrlKey || event.metaKey) && event.key === " ") {
        startListening();
        event.preventDefault();
    }
});
