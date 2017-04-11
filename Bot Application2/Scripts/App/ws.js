var websocket = null;
var textDisplay = "";

function startWebSocketForMic() {
    //startDemo();
    /// TODO: Hack to get around restrictions of Akamai on websocket.
    var hostString = "cog-web-wu.azurewebsites.net";
    //var hostString = "localhost";
    if (window.location.port != "80" && window.location.port != "") {
        hostString = hostString.concat(":").concat(window.location.port);
    }
    if (typeof (window.applicationRoot) == 'undefined')
    {
        window.applicationRoot = "/speechapi";
    }
    $('#languageoptions').val()

    //var uri = 'ws://' + hostString + window.applicationRoot + '?language=' + 'en-US'
    //        + '&g_Recaptcha_Response=' + reCaptchaSdk.g_Recaptcha_Response + '&isNeedVerify=' + reCaptchaSdk.isNeedVerify
    var uri = 'wss://' + hostString + window.applicationRoot + '/ws/speechtotextdemo?language=' + 'en-US'
            + '&g_Recaptcha_Response=' + reCaptchaSdk.g_Recaptcha_Response + '&isNeedVerify=' + reCaptchaSdk.isNeedVerify;
    //var uri = 'wss://' + hostString + window.applicationRoot
    websocket = getWebSocket(uri);
    websocket.onopen = function () {
        audioRecorder.sendHeader(websocket);
        audioRecorder.record(websocket);

        $('.mic.demo_btn').addClass("listening");
        $('#microphoneText').text("Please speak. Click on the microphone again to stop listening.");
    };
}

function getWebSocket(uri) {
    websocket = new WebSocket(uri);
    websocket.onerror = function (event) {
        stopRecording();
        websocket.close();
    };

    websocket.onmessage = function (event) {
        var data = event.data.toString();
        if (data == null || data.length <= 0) {
            return;
        }
        else if (data == "Throttled" || data == "Captcha Fail") {
            $('#messages').text(data);
            reCaptchaSdk.ProcessReCaptchaStateCode(data, 'reCaptcha-Speech2Text-demo');
            stopSounds();
            return;
        }
        else {
            reCaptchaSdk.RemoveReCaptcha();
        }
        if (data == null || data.length <= 0) {
            return;
        }

        var ch = data.charAt(0);
        var message = data.substring(1);
        if (ch == 'e') {
            stopRecording();
        }
        else {
            var text = textDisplay + message;
            if (ch == 'f') {
                textDisplay = text + " ";
            }

            $('#messages').text(text);
        }
    };

    websocket.onclose = function (event) {
        stopRecording();
    };

    return websocket;
}