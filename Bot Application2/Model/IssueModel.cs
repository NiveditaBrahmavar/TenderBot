using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.FormFlow;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot_Application2.Model
{
    [Serializable]
    public class IssueModel
    {
        [Prompt("What is your issue name?")]
        public string IssueName { get; set; }

        [Prompt("What is issue description?")]
        public string IssueDescription { get; set; }

        [Prompt("Whom is it assigned to?")]
        public string AssignedTo { get; set; }


        public static IForm<IssueModel> BuildForm()
        {
            return new FormBuilder<IssueModel>()
                    .Message("OK, so you want to submit an issue? No problem, just need a few details from you.")
                    .Message("Type quit if you do not want to submit an issue and get the main menu.")
                    .Field(nameof(IssueName))
                    .Field(nameof(IssueDescription))
                    .Field(nameof(AssignedTo))
                    .Message("Great. I have the following details and I am ready to submit your message. \r\r Name: {IssueName}\r\rDescription: {IssueDescription}\r\r Ticket is assigned to: {AssignedTo} \r\rIs that all correct \r\r?")
                    .Confirm("Type yes to submit or \r\r quit (to exit) or \r\r reset (issue form from beginning) \r\r How do you want to proceed \r\r?")                    
                    .Message("Thank you, I have submitted your message. Type \"MainMenu\" to start all over again.")
                    //.OnCompletion(ContactMessageSubmitted)
                    .Build();

            
        }

        /// <summary>
        ///  Generated a new FormDialog<T> based on IForm<BasicForm>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IFormDialog<IssueModel> BuildFormDialog(FormOptions options = FormOptions.PromptInStart)
        {
            return FormDialog.FromForm(BuildForm, options);
        }

        private static Task<ValidateResult> ValidateTicketInformation(IssueModel state, object response)
        {
            var result = new ValidateResult();
            string contactInfo = string.Empty;
            if (string.IsNullOrEmpty((string)response))
            {
                result.IsValid = true;
                result.Value = (string)response;
            }
            else
            {
                result.IsValid = false;
                result.Feedback = "You did not enter valid text";
            }
            return Task.FromResult(result);
        }

        private static bool FeedbackEnabled(IssueModel state) =>
        !string.IsNullOrWhiteSpace(state.IssueName);

               
       
    }
}