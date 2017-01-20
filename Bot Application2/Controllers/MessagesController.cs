using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Bot_Application.Model;
using System.Configuration;
using Microsoft.Bot.Builder.Dialogs;
using Bot_Application.Util;
using Microsoft.Lync.Model;

namespace Bot_Application
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private readonly ITenderBot tenderBot;        

        public MessagesController()
        {
            tenderBot = new TenderBot();
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {           
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                string result = string.Empty;

                //Specific to SKYPE
                if (activity.Text.ToLower() == "hi" || activity.Text.ToLower() == "hello")
                {
                    string membersAdded = string.Empty;

                    membersAdded = activity.From.Name.ToString();

                    Activity welcomereply = activity.CreateReply(GetWelcomeMessage(membersAdded));
                    await connector.Conversations.ReplyToActivityAsync(welcomereply);
                }
                else
                {
                    TenderLUIS tenderLUIS = await GetEntityFromLUIS(activity.Text);
                    if (tenderLUIS.intents.Count() > 0)
                    {
                        switch (tenderLUIS.intents[0].intent)
                        {
                            case "TenderStage":
                                if (tenderLUIS.entities.Count() > 0)
                                {
                                    if (tenderLUIS.entities.Any(x => x.entity.Any(char.IsDigit)))
                                    {
                                        string s = tenderLUIS.entities.First(x => x.entity.Any(char.IsDigit)).entity;
                                        result = tenderBot.FetchTenderStatus(Convert.ToInt32(s));
                                    }
                                }
                                else
                                    result = Constants.INCORRECT_MSG;
                                break;
                            case "CurrentOwner":
                                if (tenderLUIS.entities.Count() > 0)
                                {
                                    if (tenderLUIS.entities.Any(x => x.entity.Any(char.IsDigit)))
                                    {
                                        string owner = tenderLUIS.entities.First(x => x.entity.Any(char.IsDigit)).entity;
                                        result = tenderBot.FetchCurrentOwner(Convert.ToInt32(owner));
                                    }
                                }
                                else
                                    result = Constants.INCORRECT_MSG;
                                break;
                            case "TenderAnnouncementDate":
                                if (tenderLUIS.entities.Count() > 0)
                                {
                                    if (tenderLUIS.entities.Any(x => x.entity.Any(char.IsDigit)))
                                    {
                                        string date = tenderLUIS.entities.First(x => x.entity.Any(char.IsDigit)).entity;
                                        result = tenderBot.FetchAnnouncementDate(Convert.ToInt32(date));
                                    }
                                }
                                else
                                    result = Constants.INCORRECT_MSG;
                                break;
                            case "TenderDetails":
                                if (tenderLUIS.entities.Count() > 0)
                                {
                                    if (tenderLUIS.entities.Any(x => x.entity.Any(char.IsDigit)))
                                    {
                                        string details = tenderLUIS.entities.First(x => x.entity.Any(char.IsDigit)).entity;
                                        result = tenderBot.FetchTenderDetails(Convert.ToInt32(details));
                                    }
                                }
                                else
                                    result = Constants.INCORRECT_MSG;
                                break;
                            case "TenderListPrevWeek":
                                result = tenderBot.FetchPreviousTenders();
                                if (string.IsNullOrEmpty(result))
                                    result = "No tenders announced last week.";
                                break;
                            default:
                                result = Constants.INVALID_MSG;
                                break;
                        }
                    }
                    else
                    {
                        result = Constants.INVALID_MSG;
                    }

                    // return our reply to the user
                    Activity reply = activity.CreateReply(result);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                
                //var response = Request.CreateResponse("Welcome to PS Tender Tool HelpDesk." + Environment.NewLine + "We can help you with the following details" + Environment.NewLine + "1. Get Stage of Tender Id" + Environment.NewLine + "2. Get Current owner of the Tender Id" + Environment.NewLine + "3. Get Current owner of the Tender Id" + Environment.NewLine + "4. Get announcement Date for Tender Id" + Environment.NewLine + "5. Get Details of Tender Id" + Environment.NewLine + "6. Get List of Tenders announced Last week");
                //return response;
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        private static async Task<TenderLUIS> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            TenderLUIS Data = new TenderLUIS();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = ConfigurationManager.AppSettings["ApiURl"] + Query + "&verbose=true";
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<TenderLUIS>(JsonDataResponse);
                }
            }
            return Data;
        }

        private bool CheckEntity(string entity)
        {
            bool isNumeric;
            int tenderId;

            isNumeric = int.TryParse(entity, out tenderId);

            return isNumeric;
        }  

        private string GetWelcomeMessage(string members)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("Welcome " + members + " to PS Tender Tool HelpDesk..\n\n We can help you with the following details \n\n");
            sb.Append(String.Format("1. Get Stage of Tender \n\n"));
            sb.Append(String.Format("2. Get Current owner of Tender\n\n"));
            sb.Append(String.Format("3. Get announcement Date for Tender \n\n"));
            sb.Append(String.Format("4. Get Details of Tender \n\n"));
            sb.Append(String.Format("5. Get List of Tenders announced Last week \n\n"));

            return sb.ToString();
        }
    }
}