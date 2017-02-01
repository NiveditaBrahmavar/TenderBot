using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application.Util
{
    public class Constants
    {
        public const string CURRENTOWNER_QUERY = "select CurrentOwner from pst.Tender where TenderId=";
        public const string STAGE_QUERY = "select TenderStatus_ValueId from pst.Tender where TenderId=";
        public const string ANNOUNCEMENTDATE_QUERY = "select WinnerResultdate from pst.TenderDetails where TenderId=";
        public const string TENDERDETAILS_QUERY = "[PST].[usp_FetchTenderDetails]";
        public const string TENDERLIST_QUERY = "SELECT TenderId FROM pst.tenderdetails Where WinnerResultDate >= DATEADD(WEEK,-1, GETDATE())";
        public const string INCORRECT_MSG = "U haven't entered tender Number or incorrect input";
        public const string INVALID_MSG = "Sorry, I am not getting you...";

        public const string ADDED_MSG = "Hi!.. Welcome to **PS Tender Tool HelpDesk**. We can help you with the following details.";

        public const string EXAMPLE_MSG = "For instance, you can say: Get Stage of 999 or Get Details of Tender 999. Then you get results like those.";

        public const string QUIT_MSG = "We can help you with the following options.";


    }

    public static class Enums
    {
        public enum TenderStatusLookup
        {
            NewTender = 101,
            Questionaire = 102,
            PendingAward = 103,
            Evidence = 104,
            ComplianceCheck = 105,
            InProgress = 106,
            Approved = 107,
            Closed = 108
        }
    }
}