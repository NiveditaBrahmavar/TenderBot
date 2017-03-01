using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Bot_Application2.Model;

namespace Bot_Application.Util
{
    public interface ITenderBot
    {
        string FetchTenderStatus(int tenderId);
        string FetchCurrentOwner(int tenderId);
        string FetchAnnouncementDate(int tenderId);
        string FetchTenderDetails(int tenderId);
        string FetchPreviousTenders();

        IList<TenderModel> GetTenderDetails(string alias);

        bool ExportReportExcel(string alias, string subsidiaryName, string startDate, string endDate, string mailRecipient);
    }
}
