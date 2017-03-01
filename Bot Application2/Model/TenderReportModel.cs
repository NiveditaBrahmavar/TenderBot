using Bot_Application.Util;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application2.Model
{
    [Serializable]
    public class TenderReportModel
    {
        [Prompt("Please enter the Alias Name")]
        public string AliasName { get; set; }

        [Prompt("Which Subsidiary do you want the Report for?")]
        public string SubsidiaryName { get; set; }

        [Prompt("What is the Start Date you want?")]        
        public string StartDate { get; set; }

        [Prompt("What is the End Date you want?")]        
        public string EndDate { get; set; }

        [Prompt("Please enter the Email address to whom the mail needs to be sent")]
        public string SendMail { get; set; }


        public static IForm<TenderReportModel> BuildForm()
        {
            return new FormBuilder<TenderReportModel>()
                    .Message("OK, so you want to get the tender report to ur mail Id? No problem, just need a few details from you.")
                    .Message("Type quit if you do not want to submit an issue and get the main menu.")
                    .Field(nameof(AliasName))
                    .Message("**Note: if you dont want to mention subsidiary then type NO **")
                    .Field(nameof(SubsidiaryName))
                    .Message("**Note: if you dont want to mention date then type NO **")
                    .Field(nameof(StartDate))
                    .Field(nameof(EndDate))
                    //.Field(nameof(StartDate), validate: ValidateStartDateFormat)
                    //.Field(nameof(EndDate), validate: ValidateEndDateFormat)
                    .Field(nameof(SendMail))
                    .Message("Great. I have the following details and I am ready to submit your message.\r\r Name: {AliasName}\r\r Subsidiary: {SubsidiaryName}\r\r Start Date: {StartDate} \r\r End Date: {EndDate} \r\r Alias Name for mail: {SendMail}\r\rIs that all correct \r\r?")
                    .Confirm("Type yes to submit or \r\r quit (to exit) or \r\r reset (issue form from beginning) \r\r How do you want to proceed \r\r?")
                    .Message("We are currently processing your Request. We will message you the status.")
                    .OnCompletion(async (context, TenderReportModel) =>
                    {
                        ITenderBot tenderBot = new TenderBot();
                        bool issuccess = tenderBot.ExportReportExcel(TenderReportModel.AliasName, TenderReportModel.SubsidiaryName, TenderReportModel.StartDate, TenderReportModel.EndDate, TenderReportModel.SendMail);
                        IMessageActivity reply = context.MakeMessage();
                        if (issuccess)
                            reply.Text = "Mail sent";
                        else
                            reply.Text = "Mail not sent";

                        //     StateClient stateClient = activity.GetStateClient();
                        //     BotData userData = stateClient.BotState.GetPrivateConversationData(
                        //         activity.ChannelId, activity.Conversation.Id, activity.From.Id);
                        //     userData.SetProperty<bool>("ReportDetails", false);
                        //     stateClient.BotState.SetPrivateConversationData(
                        //activity.ChannelId, activity.Conversation.Id, activity.From.Id, userData);
                        string value;
                        context.PrivateConversationData.TryGetValue("ReportDetails", out value);
                        if(value.ToLower() == "true")
                        {
                            context.PrivateConversationData.SetValue("ReportDetails", false);
                        }
                        await context.PostAsync(reply);                        

                    })
                    .Message("Type \"MainMenu\" to start all over again.")
                    .Build();


        }

        private static Task<ValidateResult> ValidateStartDateFormat(TenderReportModel state, object response)
        {
            var result = new ValidateResult();            
            DateTime dateTime;
            //if (state.StartDate.ToLower() == "no")
            //{
            //    result.IsValid = true;
            //}
            //else
            //{
                if (DateTime.TryParse(state.StartDate, out dateTime))
                {
                    result.IsValid = true;
                }
                else
                {
                    result.IsValid = false;
                    result.Feedback = "You did not enter valid Start Date. Make sure it's in MM/dd/yyyy format.";
                }
            //}
            return Task.FromResult(result);
        }

        private static Task<ValidateResult> ValidateEndDateFormat(TenderReportModel state, object response)
        {
            var result = new ValidateResult();
            DateTime dateTime;
            if (state.EndDate.ToLower() == "no")
            {
                result.IsValid = true;
            }
            else
            {
                if (DateTime.TryParse(state.EndDate, out dateTime))
                {
                    result.IsValid = true;
                }
                else
                {
                    result.IsValid = false;
                    result.Feedback = "You did not enter valid End Date. Make sure it's in MM/dd/yyyy format.";
                }
            }
            return Task.FromResult(result);
        }
    }
}