using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Threading;
using Microsoft.CognitiveServices.SpeechRecognition;
using System.Text;

namespace Bot_Application2.Controllers
{

    public class SpeechController : Controller
    {

        private MicrophoneRecognitionClient micClient;

        public bool IsDataClientDictation { get; set; }

        public bool IsMicrophoneClientDictation { get; set; }

        private SpeechRecognitionMode Mode
        {
            get
            {
                if (this.IsMicrophoneClientDictation ||
                    this.IsDataClientDictation)
                {
                    return SpeechRecognitionMode.LongDictation;
                }

                return SpeechRecognitionMode.ShortPhrase;
            }
        }

        private string DefaultLocale
        {
            get { return "en-US"; }
        }

        public string SubscriptionKey
        {
            get
            {
                return ConfigurationManager.AppSettings["SubscriptionKey"]; ;
            }
        }

        private string AuthenticationUri
        {
            get
            {
                return ConfigurationManager.AppSettings["AuthenticationUri"];
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StartSpeechRecording()
        {
            this.CreateMicrophoneRecoClient();

            return View("Index");
        }



        /// <summary>
        /// Creates a new microphone reco client without LUIS intent support.
        /// </summary>
        private void CreateMicrophoneRecoClient()
        {
            this.micClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(
                this.Mode,
                this.DefaultLocale,
                this.SubscriptionKey);
            // this.micClient = this.AuthenticationUri;
            this.micClient.AuthenticationUri = this.AuthenticationUri;
            //this.micClient.OnIntent += this.OnIntentHandler;

            //this.OnMicrophoneStatus();
            //// Event handlers for speech recognition results
            this.micClient.OnMicrophoneStatus += this.OnMicrophoneStatus;
            this.micClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;


            this.micClient.OnResponseReceived += this.OnMicShortPhraseResponseReceivedHandler;


            this.micClient.StartMicAndRecognition();
            //this.micClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;
            //if (this.Mode == SpeechRecognitionMode.ShortPhrase)
            //{
            //    this.micClient.OnResponseReceived += this.OnMicShortPhraseResponseReceivedHandler;
            //}
            //else if (this.Mode == SpeechRecognitionMode.LongDictation)
            //{
            //    this.micClient.OnResponseReceived += this.OnMicDictationResponseReceivedHandler;
            //}

            //this.micClient.OnConversationError += this.OnConversationErrorHandler;
        }

        private void OnMicrophoneStatus(object sender, MicrophoneEventArgs e)
        {
            if (e.Recording)
            {
                ViewBag.StartSpeaking = "Please start speaking.";
            }
        }


        private void OnMicShortPhraseResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            // we got the final result, so it we can end the mic reco.  No need to do this
            // for dataReco, since we already called endAudio() on it as soon as we were done
            // sending all the data.
            this.micClient.EndMicAndRecognition();

            this.WriteResponseResult(e);         
        }


        private void WriteResponseResult(SpeechResponseEventArgs e)
        {
            if (e.PhraseResponse.Results.Length == 0)
            {
                //this.WriteLine("No phrase response is available.");
                ViewBag.SpeechData = "No phrase response is available.";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("********* Final n-BEST Results *********");
                for (int i = 0; i < e.PhraseResponse.Results.Length; i++)
                {
                    sb.Append(e.PhraseResponse.Results[i].Confidence);
                    sb.Append("<br/>");
                    sb.Append(e.PhraseResponse.Results[i].DisplayText);
                }
                ViewBag.SpeechData = sb.ToString();


            }
        }

        private void OnPartialResponseReceivedHandler(object sender, PartialSpeechResponseEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("--- Partial result received by OnPartialResponseReceivedHandler() ---");
            sb.Append( e.PartialResult);
            ViewBag.PartialData = sb.ToString();
        }
    }

}
