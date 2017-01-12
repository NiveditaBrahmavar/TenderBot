using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Application.Util
{
    public interface ITenderBot
    {
        string FetchTenderStatus(int tenderId);
        string FetchCurrentOwner(int tenderId);
        string FetchAnnouncementDate(int tenderId);
        string FetchTenderDetails(int tenderId);
        string FetchPreviousTenders();
    }
}
