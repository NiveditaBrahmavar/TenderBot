using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application2.Model
{
    public class TenderModel
    {

        //public TenderModel()
        //{
        //    TenderBasicModel = new TenderBasicModel();
        //    TenderAcctExecModel = new TenderAcctExecModel();
        //}
        //public TenderBasicModel TenderBasicModel { get; set; }

        //public TenderAcctExecModel TenderAcctExecModel { get; set; }

        //public TenderEvidenceModel TenderEvidenceModel { get; set; }

        public long TenderId { get; set; }
        public string Title { get; set; }

        public string TenderNumber { get; set; }

        public string CurrentOwner { get; set; }
        public DateTime LastModified { get; set; }

        public string Subsidiary { get; set; }

        public string Stage { get; set; }
    }
}