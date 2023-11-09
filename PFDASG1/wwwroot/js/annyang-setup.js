﻿
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
        var messages = ['Hey', 'Hi, there!', 'Hello'];
        var randomIndex = Math.floor(Math.random() * messages.length);
        var message = messages[randomIndex];

        console.log(`%c ${message}`, 'color: green; font-weight:bold;');

        // Use the Web Speech API to respond with spoken text
        var speechSynthesis = window.speechSynthesis;
        var speech = new SpeechSynthesisUtterance(message);
        speech.volume = 1; // Adjust volume
        speech.rate = 1; // Adjust speaking rate

        speechSynthesis.speak(speech);
    }



    function goToLoginPage() {
        window.location.href = '/Home/Login';
    }

    function goToHomePage() {
        window.location.href = '/Home/Index';
    }

    function startListening() {
        annyang.start();
        speechMessage.text = "Listening started";
        speechSynthesis.speak(speechMessage);
        console.log("Listening started");
    }

    function stopListening() {
        annyang.abort();
        listening = false;
        speechMessage.text = "Listening stopped";
        speechSynthesis.speak(speechMessage);
        console.log("Listening stopped");
    }

    function enterAccessCode(code) {
        code = code.replace(/\s/g, '');
        document.getElementById('accessCodeInput').value = code;
    }

    function enterPIN(pin) {
        pin = pin.replace(/\s/g, '');
        document.getElementById('pinInput').value = pin;
    }

    function login() {
        document.getElementById('login login_btn').click();
    }

    function logout() {
        window.location.href = '/Home/Index';
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