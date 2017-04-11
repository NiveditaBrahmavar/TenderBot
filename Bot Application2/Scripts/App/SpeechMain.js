var speechForm = (function () {

    var controls = {
        btnStartRecording: '#btnStart',
        pSpeakNow: '#info_speak_now',
        pPageDisplayMsg : '#initiate_speech'
        
    };

    var initialForm;

    //Private members
    var init = function () {
        
        $('#microphoneText').show();

        window.AudioContext = window.AudioContext || window.webkitAudioContext;

        var audioContext = null;
        if (AudioContext) {
            audioContext = new AudioContext();
        }

        var audioRecorder = null;
        var isRecording = false;
        var audioSource = null;

        $('#btnStart').click(function () {
            micOnClick();
        });

        function gotAudioStream(stream) {
            this.audioSource = stream;
            var inputPoint = audioContext.createGain();
            audioContext.createMediaStreamSource(this.audioSource).connect(inputPoint);
            audioRecorder = new Recorder(inputPoint);
            //this.audioContext.createMediaStreamSource(audioSource).connect(inputPoint);
            //this.audioRecorder = new Recorder(inputPoint);
            startWebSocketForMic();
            this.isRecording = true;
        }

        function startRecording() {
            if (!navigator.getUserMedia) {
                navigator.getUserMedia = navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;
            }

            navigator.getUserMedia({
                "audio": true,
            }, gotAudioStream.bind(this), function (e) {
                window.alert('Microphone access was rejected.');
            });
        }


        function stopRecording() {
            if (this.isRecording) {
                this.isRecording = false;
                if (audioSource.stop) {
                    audioSource.stop();
                }
                audioRecorder.stop();
                stopWebSocket();                
                start_img.src = '//google.com/intl/en/chrome/assets/common/images/content/mic.gif"';
                $('#microphoneText').text("Click on the microphone to start speaking.");
            }
        }

        function stopWebSocket() {
            if (websocket) {
                websocket.onmessage = function () { };
                websocket.onerror = function () { };
                websocket.onclose = function () { };
                websocket.close();
            }
        }


        function showInfo(s) {
            if (s) {
                for (var child = info.firstChild; child; child = child.nextSibling) {
                    if (child.style) {
                        child.style.display = child.id == s ? 'inline' : 'none';
                    }
                }
                info.style.visibility = 'visible';
            } else {
                info.style.visibility = 'hidden';
            }
        }


        function micOnClick() {
            if (this.isRecording) {
                this.stopRecording();
            }
            else {
                $('#microphoneText').text("Speak Now!");
                start_img.src = '//google.com/intl/en/chrome/assets/common/images/content/mic-animate.gif';
                startRecording();
            }
        }

    };

    return {
        init: init
    };

})();