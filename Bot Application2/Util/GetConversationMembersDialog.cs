using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application.Util
{
    [Serializable]
    public class GetConversationMembersDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IActivity> result)
        {
            var activity = (Activity)await result;


            if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (activity.MembersAdded != null && activity.MembersAdded.Any())
                {
                    string membersAdded = string.Join(
                        ", ",
                        activity.MembersAdded.Select(
                            newMember => (newMember.Id != activity.Recipient.Id) ? $"{newMember.Name} (Id: {newMember.Id})"
                                            : $"{activity.Recipient.Name} (Id: {activity.Recipient.Id})"));

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("Welcome to PS Tender Tool HelpDesk..\n\n We can help you with the following details \n\n");                    
                    sb.Append(String.Format("1. Get Stage of Tender Id \n\n"));
                    sb.Append(String.Format("2. Get Current owner of the Tender Id \n\n"));                    
                    sb.Append(String.Format("3. Get announcement Date for Tender Id \n\n"));
                    sb.Append(String.Format("4. Get Details of Tender Id \n\n"));
                    sb.Append(String.Format("5. Get List of Tenders announced Last week \n\n"));                    

                    await context.PostAsync(sb.ToString());
                    
                }
            }
        }


}
}