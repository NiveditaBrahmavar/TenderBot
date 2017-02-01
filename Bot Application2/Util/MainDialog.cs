using Bot_Application2.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application2.Util
{
    public class MainDialog : IDialog<IssueModel>
    {
        public MainDialog()
        {
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.PostAsync("That's great. You will need to provide few details about yourself before giving feedback.");
            //context.Call(IssueModel.BuildFormDialog(FormOptions.PromptInStart), FormComplete);            
            var feedbackForm = new FormDialog<IssueModel>(new IssueModel(), IssueModel.BuildForm, FormOptions.PromptInStart);
            context.Call(feedbackForm, FormComplete);
        }
        private async Task FormComplete(IDialogContext context, IAwaitable<IssueModel> result)
        {
            try
            {
                var form = await result;
                if (form != null)
                {
                    await context.PostAsync("Thanks for completing the form! Just type anything to restart it.");
                }
                else
                {
                    await context.PostAsync("Form returned empty response! Type anything to restart it.");
                }
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync("You canceled the form! Type anything to restart it.");
            }

            context.Wait(MessageReceivedAsync);
        }
    }

}