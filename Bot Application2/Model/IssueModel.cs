using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.FormFlow;
using System.Linq;
using System.Web;

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
                    .Field(nameof(IssueName))
                    .Field(nameof(IssueDescription))
                    .Field(nameof(AssignedTo))
                    .Confirm("Great. I have the following details and I am ready to submit your message. \r\r Name: {IssueName}\r\rDescription: {IssueDescription}\r\r Ticket is assigned to: {AssignedTo} \r\rIs that all correct ?")
                    //.OnCompletion(ContactMessageSubmitted)
                    .Message("Thank you, I have submitted your message.")
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
    }
}