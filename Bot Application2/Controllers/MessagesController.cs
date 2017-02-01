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
using System.Collections.Generic;
using Bot_Application2.Util;
using Bot_Application2.Model;
using Microsoft.Bot.Builder.FormFlow;

namespace Bot_Application
{
    //[BotAuthentication]
    //public class MessagesController : ApiController
    //{
    //    private readonly ITenderBot tenderBot;        

    //    public MessagesController()
    //    {
    //        tenderBot = new TenderBot();
    //    }

    //    /// <summary>
    //    /// POST: api/Messages
    //    /// Receive a message from a user and reply to it
    //    /// </summary>
    //    public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
    //    {           
    //        if (activity.Type == ActivityTypes.Message)
    //        {
    //            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

    //            string result = string.Empty;

    //            var stateClient = new StateClient(new Uri(activity.ServiceUrl));
    //            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);

    //            if (activity.Text.ToLower() == "hi" || activity.Text.ToLower() == "hello")
    //            {
    //                //Activity replyToConversation = activity.CreateReply("Namashkar");
    //                Activity replyToConversation = activity.CreateReply(Constants.ADDED_MSG);
    //                replyToConversation.Recipient = activity.From;
    //                replyToConversation.Type = "message";
    //                replyToConversation.Attachments = new List<Attachment>();

    //                HeroCard plCard = ShowWelcomeButtons();
    //                Attachment plAttachment = plCard.ToAttachment();
    //                replyToConversation.Attachments.Add(plAttachment);

    //                var reply = await connector.Conversations.ReplyToActivityAsync(replyToConversation);
    //                //await connector.Conversations.ReplyToActivityAsync(welcomereply);
    //            }
    //            else if (activity.Text.ToLower() == "pstenderapp")
    //            {
    //                Activity tenderappreply = activityMessage(activity);                    
    //                await connector.Conversations.ReplyToActivityAsync(tenderappreply);
    //            }
    //            else if (activity.Text.ToLower() == "tenderexample")
    //            {                    
    //                Activity examplereply = activity.CreateReply(Constants.EXAMPLE_MSG);
    //                await connector.Conversations.ReplyToActivityAsync(examplereply);
    //            }
    //            else if (activity.Text.ToLower() == "issue")
    //            {
    //                userData.SetProperty<bool>("IssueTicket", true);
    //                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
    //                await Conversation.SendAsync(activity, () => new MainDialog());
    //            }
    //            else if (userData.GetProperty<bool>("IssueTicket"))
    //            {
    //                await Conversation.SendAsync(activity, () => new MainDialog());
    //                BotData conversationData = stateClient.BotState.GetPrivateConversationData(activity.ChannelId, activity.Conversation.Id, activity.From.Id);
    //            }
    //            else
    //            {
    //                TenderLUIS tenderLUIS = await GetEntityFromLUIS(activity.Text);
    //                if (tenderLUIS.intents.Count() > 0)
    //                {
    //                    switch (tenderLUIS.intents[0].intent)
    //                    {
    //                        case "TenderStage":
    //                            if (tenderLUIS.entities.Count() > 0)
    //                            {
    //                                if (tenderLUIS.entities.Any(x => x.entity.Any(char.IsDigit)))
    //                                {
    //                                    string s = tenderLUIS.entities.First(x => x.entity.Any(char.IsDigit)).entity;
    //                                    result = tenderBot.FetchTenderStatus(Convert.ToInt32(s));
    //                                }
    //                            }
    //                            else
    //                                result = Constants.INCORRECT_MSG;
    //                            break;
    //                        case "CurrentOwner":
    //                            if (tenderLUIS.entities.Count() > 0)
    //                            {
    //                                if (tenderLUIS.entities.Any(x => x.entity.Any(char.IsDigit)))
    //                                {
    //                                    string owner = tenderLUIS.entities.First(x => x.entity.Any(char.IsDigit)).entity;
    //                                    result = tenderBot.FetchCurrentOwner(Convert.ToInt32(owner));
    //                                }
    //                            }
    //                            else
    //                                result = Constants.INCORRECT_MSG;
    //                            break;
    //                        case "TenderAnnouncementDate":
    //                            if (tenderLUIS.entities.Count() > 0)
    //                            {
    //                                if (tenderLUIS.entities.Any(x => x.entity.Any(char.IsDigit)))
    //                                {
    //                                    string date = tenderLUIS.entities.First(x => x.entity.Any(char.IsDigit)).entity;
    //                                    result = tenderBot.FetchAnnouncementDate(Convert.ToInt32(date));
    //                                }
    //                            }
    //                            else
    //                                result = Constants.INCORRECT_MSG;
    //                            break;
    //                        case "TenderDetails":
    //                            if (tenderLUIS.entities.Count() > 0)
    //                            {
    //                                if (tenderLUIS.entities.Any(x => x.entity.Any(char.IsDigit)))
    //                                {
    //                                    string details = tenderLUIS.entities.First(x => x.entity.Any(char.IsDigit)).entity;
    //                                    result = tenderBot.FetchTenderDetails(Convert.ToInt32(details));
    //                                }
    //                            }
    //                            else
    //                                result = Constants.INCORRECT_MSG;
    //                            break;
    //                        case "TenderListPrevWeek":
    //                            result = tenderBot.FetchPreviousTenders();
    //                            if (string.IsNullOrEmpty(result))
    //                                result = "No tenders announced last week.";
    //                            break;
    //                        default:
    //                            result = Constants.INVALID_MSG;
    //                            break;
    //                    }
    //                }
    //                else
    //                {
    //                    result = Constants.INVALID_MSG;
    //                }

    //                // return our reply to the user
    //                Activity reply = activity.CreateReply(result);
    //                await connector.Conversations.ReplyToActivityAsync(reply);
    //            }
    //        }
    //        else
    //        {
    //            HandleSystemMessage(activity);
    //        }
    //        var response = Request.CreateResponse(HttpStatusCode.OK);
    //        return response;
    //    }

    //    private Activity HandleSystemMessage(Activity message)
    //    {
    //        if (message.Type == ActivityTypes.DeleteUserData)
    //        {
    //            // Implement user deletion here
    //            // If we handle user deletion, return a real message
    //        }
    //        else if (message.Type == ActivityTypes.ConversationUpdate)
    //        {
    //            // Handle conversation state changes, like members being added and removed
    //            // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
    //            // Not available in all channels

    //            //var response = Request.CreateResponse("Welcome to PS Tender Tool HelpDesk." + Environment.NewLine + "We can help you with the following details" + Environment.NewLine + "1. Get Stage of Tender Id" + Environment.NewLine + "2. Get Current owner of the Tender Id" + Environment.NewLine + "3. Get Current owner of the Tender Id" + Environment.NewLine + "4. Get announcement Date for Tender Id" + Environment.NewLine + "5. Get Details of Tender Id" + Environment.NewLine + "6. Get List of Tenders announced Last week");
    //            //return response;
    //        }
    //        else if (message.Type == ActivityTypes.ContactRelationUpdate)
    //        {
    //            // Handle add/remove from contact lists
    //            // Activity.From + Activity.Action represent what happened
    //        }
    //        else if (message.Type == ActivityTypes.Typing)
    //        {
    //            // Handle knowing tha the user is typing
    //        }
    //        else if (message.Type == ActivityTypes.Ping)
    //        {
    //        }

    //        return null;
    //    }

    //    private static async Task<TenderLUIS> GetEntityFromLUIS(string Query)
    //    {
    //        Query = Uri.EscapeDataString(Query);
    //        TenderLUIS Data = new TenderLUIS();
    //        using (HttpClient client = new HttpClient())
    //        {
    //            string RequestURI = ConfigurationManager.AppSettings["ApiURl"] + Query + "&verbose=true";
    //            HttpResponseMessage msg = await client.GetAsync(RequestURI);

    //            if (msg.IsSuccessStatusCode)
    //            {
    //                var JsonDataResponse = await msg.Content.ReadAsStringAsync();
    //                Data = JsonConvert.DeserializeObject<TenderLUIS>(JsonDataResponse);
    //            }
    //        }
    //        return Data;
    //    }

    //    private bool CheckEntity(string entity)
    //    {
    //        bool isNumeric;
    //        int tenderId;

    //        isNumeric = int.TryParse(entity, out tenderId);

    //        return isNumeric;
    //    }  

    //    private string GetTenderMessage()
    //    {
    //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
    //        sb.Append(String.Format("1. Get Stage of Tender \n\n"));
    //        sb.Append(String.Format("2. Get Current owner of Tender\n\n"));
    //        sb.Append(String.Format("3. Get announcement Date for Tender \n\n"));
    //        sb.Append(String.Format("4. Get Details of Tender \n\n"));
    //        sb.Append(String.Format("5. Get List of Tenders announced Last week \n\n"));

    //        return sb.ToString();
    //    }

    //    private HeroCard ShowWelcomeButtons()
    //    {
    //        List<CardAction> cardButtons = new List<CardAction>();
    //        CardAction supportButton = new CardAction()
    //        {               
    //            Value = "issue",
    //            Type = "postBack",
    //            Title = "Create an Issue Ticket"
    //        };
    //        cardButtons.Add(supportButton);
    //        CardAction detailsButton = new CardAction()
    //        {
    //            Value = "pstenderapp",
    //            Type = "postBack",
    //            Title = "Get PS Tender Details"
    //        };
    //        cardButtons.Add(detailsButton);
    //        HeroCard plCard = new HeroCard()
    //        {
    //            Title = "What would you like to do?",                                       
    //            Buttons = cardButtons
    //        };

    //        return plCard;
    //    }

    //    private HeroCard ShowCardButtons(string Value, string type, string title, string cardTitle)
    //    {
    //        List<CardAction> cardButtons = new List<CardAction>();
    //        CardAction cardActn = new CardAction()
    //        {
    //            Value = Value,
    //            Type = type,
    //            Title = title
    //        };
    //        cardButtons.Add(cardActn);
    //        HeroCard card = new HeroCard()
    //        {
    //            Title = cardTitle,
    //            Buttons = cardButtons
    //        };
    //        return card;
    //    }

    //    private Activity activityMessage(Activity activity)
    //    {
    //        Activity replyActivity = activity.CreateReply(GetTenderMessage());
    //        replyActivity.Attachments = new List<Attachment>();
    //        HeroCard heroCard;

    //        //if (activity.Text.ToLower() == "hi" || activity.Text.ToLower() == "hello")
    //        //{
    //        //    HeroCard supportButton = ShowCardButtons("https://en.wikipedia.org/wiki/Pig_Latin", "openUrl", "Perform Admin Functions", "What would you like to do?");

    //        //}
    //        //else if (activity.Text.ToLower() == "pstenderapp")
    //        //{
    //            heroCard = ShowCardButtons("tenderexample", "postback", "See an example", "What details are you looking for?");
    //        //}
    //        Attachment plAttachment = heroCard.ToAttachment();
    //        replyActivity.Attachments.Add(plAttachment);
    //        return replyActivity;
    //    }
    //}

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

                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetPrivateConversationData(
                    activity.ChannelId, activity.Conversation.Id, activity.From.Id);
                //var stateClient = new StateClient(new Uri(activity.ServiceUrl));
                //BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);

                if (activity.Text.ToLower() == "hi" || activity.Text.ToLower() == "hello")
                {
                    Activity replyToConversation = activity.CreateReply(Constants.ADDED_MSG);
                    replyToConversation.Recipient = activity.From;
                    replyToConversation.Type = "message";
                    replyToConversation.Attachments = new List<Attachment>();

                    HeroCard plCard = ShowWelcomeButtons();
                    Attachment plAttachment = plCard.ToAttachment();
                    replyToConversation.Attachments.Add(plAttachment);

                    var reply = await connector.Conversations.ReplyToActivityAsync(replyToConversation);
                }
                else if (activity.Text.ToLower() == "pstenderapp")
                {
                    Activity tenderappreply = activityMessage(activity);
                    await connector.Conversations.ReplyToActivityAsync(tenderappreply);
                }
                else if (activity.Text.ToLower() == "tenderexample")
                {
                    Activity examplereply = activity.CreateReply(Constants.EXAMPLE_MSG);
                    await connector.Conversations.ReplyToActivityAsync(examplereply);
                }
                else if (activity.Text.ToLower() == "issue" || userData.GetProperty<bool>("IssueTicket"))
                {
                    if (activity.Text.ToLower() == "quit" || activity.Text.ToLower() == "reset" || activity.Text.ToLower() == "help")
                    {
                        userData.SetProperty<bool>("IssueTicket", false);
                        stateClient.BotState.SetPrivateConversationData(
                   activity.ChannelId, activity.Conversation.Id, activity.From.Id, userData);
                    }
                    else
                    {
                        userData.SetProperty<bool>("IssueTicket", true);
                        stateClient.BotState.SetPrivateConversationData(
                        activity.ChannelId, activity.Conversation.Id, activity.From.Id, userData);
                    }

                    if (activity.Text.ToLower() == "quit")
                    {
                        Activity quitreply = quitMessage(activity);
                        await connector.Conversations.ReplyToActivityAsync(quitreply);

                    }
                    //await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                    //await Conversation.SendAsync(activity, () => new MainDialog());
                        await Conversation.SendAsync(activity, MakeRoot);
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
                //RequestURI = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/cc5e3430-bb6b-4ba9-91ec-76cc33fa55f7?subscription-key=997e1b916ae141ceb07a9c38beed3346&q="+ Query + "&verbose=true";
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

        private string GetTenderMessage()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(String.Format("1. Get Stage of Tender \n\n"));
            sb.Append(String.Format("2. Get Current owner of Tender\n\n"));
            sb.Append(String.Format("3. Get announcement Date for Tender \n\n"));
            sb.Append(String.Format("4. Get Details of Tender \n\n"));
            sb.Append(String.Format("5. Get List of Tenders announced Last week \n\n"));
            return sb.ToString();
        }

        private HeroCard ShowWelcomeButtons()
        {
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction supportButton = new CardAction()
            {
                //Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                //Type = "openUrl",
                //Title = "Perform Admin Functions"
                Value = "issue",
                Type = "postBack",
                Title = "Create an Issue Ticket"
            };
            cardButtons.Add(supportButton);
            CardAction detailsButton = new CardAction()
            {
                Value = "pstenderapp",
                Type = "postBack",
                Title = "Get PS Tender Details"
            };
            cardButtons.Add(detailsButton);
            HeroCard plCard = new HeroCard()
            {
                Title = "What would you like to do?",
                //Subtitle = "Pig Latin Wikipedia Page",                        
                Buttons = cardButtons
            };

            return plCard;
        }

        private ThumbnailCard ShowCardButtons(string Value, string type, string title, string cardTitle)
        {
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction cardActn = new CardAction()
            {
                Value = Value,
                Type = type,
                Title = title
            };
            cardButtons.Add(cardActn);
            ThumbnailCard card = new ThumbnailCard()
            {
                Title = cardTitle,
                Buttons = cardButtons
            };
            return card;
        }

        private Activity activityMessage(Activity activity)
        {
            Activity replyActivity = activity.CreateReply(GetTenderMessage());
            replyActivity.Attachments = new List<Attachment>();
            ThumbnailCard plCard = ShowCardButtons("tenderexample", "postBack", "See an example", "What details are you looking for?");
            Attachment plAttachment = plCard.ToAttachment();
            replyActivity.Attachments.Add(plAttachment);
            return replyActivity;
        }

        private Activity quitMessage(Activity activity)
        {
            Activity replyToConversation = activity.CreateReply(Constants.QUIT_MSG);
            replyToConversation.Recipient = activity.From;
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();

            HeroCard plCard = ShowWelcomeButtons();
            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);
            return replyToConversation;            
        }


        internal static IDialog<IssueModel> MakeRoot()
        {
            return Chain.From(() => FormDialog.FromForm(IssueModel.BuildForm));
        }
    }
}