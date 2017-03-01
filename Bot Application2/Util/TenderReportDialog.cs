using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;

namespace Bot_Application2.Util
{
    [LuisModel("d4c0d37f-4eb0-44c7-b868-d898938e9444", "7efa2031-a7e6-4ab6-826b-ba845bdc81fc")]
    [Serializable]
    public class TenderReportDialog: LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task GetTenderReport(IDialogContext context, LuisResult result)
        {
            string returnMessage = "You want the Tender Details Report. Enter the alias name";

            var entities = new List<EntityRecommendation>(result.Entities);

            await context.PostAsync("");
            context.Wait(MessageReceived);
        }

    }
}