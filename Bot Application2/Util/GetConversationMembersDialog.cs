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

                    string str = "to PS Tender Tool HelpDesk." + "\n" + "We can help you with the following details" + Environment.NewLine + "1.Get Stage of Tender Id" + Environment.NewLine + "2.Get Current owner of the Tender Id" + Environment.NewLine + "3.Get Current owner of the Tender Id" + Environment.NewLine + "4.Get announcement Date for Tender Id" + Environment.NewLine + "5.Get Details of Tender Id" + Environment.NewLine + "6.Get List of Tenders announced Last week";

                await context.PostAsync($"Welcome {membersAdded}" + str);
                    
                }
            }
        }


}
}