using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application.Util
{
    public class TenderBot: ITenderBot
    {
        public string FetchTenderStatus(int tenderId)
        {
            TenderBot _tenderbot = new TenderBot();
            var returnValue = _tenderbot.GetData(Constants.STAGE_QUERY + tenderId);

            if (returnValue != null)
            {
                string Stage = ((Enums.TenderStatusLookup)returnValue).ToString();
                return string.Format("Status of Tender {0} is {1}", tenderId, Stage);
            }
            else
            {
                return string.Format("This \"{0}\" is not an valid tender", tenderId);
            }
        }

        public string FetchCurrentOwner(int tenderId)
        {
            TenderBot _tenderbot = new TenderBot();
            var returnValue = _tenderbot.GetData(Constants.CURRENTOWNER_QUERY + tenderId);
            if (returnValue != null)
            {
                return string.Format("Cuurent Owner of Tender {0} is {1}", tenderId, returnValue.ToString());
            }
            else
            {
                return string.Format("This \"{0}\" is not an valid tender", tenderId);
            }
        }
        public  string FetchAnnouncementDate(int tenderId)
        {
            TenderBot _tenderbot = new TenderBot();
            var returnValue = _tenderbot.GetData(Constants.ANNOUNCEMENTDATE_QUERY + tenderId);
            if (returnValue != null)
            {
                return string.Format("Announcement Date for Tender {0} is {1}", tenderId, Convert.ToDateTime(returnValue).ToShortDateString());
            }
            else
            {
                return string.Format("This \"{0}\" is not an valid tender", tenderId);
            }
        }

        public string FetchTenderDetails(int tenderId)
        {
            TenderBot _tenderbot = new TenderBot();
            string connectionString = ConfigurationManager.ConnectionStrings["PSTenderDev"].ConnectionString;
            StringBuilder sb = new StringBuilder();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var queryString = Constants.TENDERDETAILS_QUERY;
                    using (var command = new SqlCommand(queryString, connection))
                    {   
                        command.Parameters.Add(new SqlParameter("@tenderID", tenderId));
                        command.CommandType = CommandType.StoredProcedure;

                        using (IDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                
                                var index = dr.GetOrdinal("TenderId");
                                if (!dr.IsDBNull(index))
                                    sb.Append("TenderId: " + dr.GetValue(index) + "\n");
                                    sb.AppendLine();

                                index = dr.GetOrdinal("Title");
                                if (!dr.IsDBNull(index))
                                    sb.Append("Title: " + dr.GetValue(index) + "\n");
                                    sb.AppendLine();

                                index = dr.GetOrdinal("Tendernumber");
                                if (!dr.IsDBNull(index))
                                    sb.Append("Tender Number: " + dr.GetString(index) + "\n");
                                    sb.AppendLine();

                                index = dr.GetOrdinal("CurrentOwner");
                                if (!dr.IsDBNull(index))
                                    sb.Append("Current Owner: " + dr.GetString(index) + "\n");
                                    sb.AppendLine();

                                index = dr.GetOrdinal("LastModified");
                                if (!dr.IsDBNull(index))
                                    sb.Append("Last Modified: " + Convert.ToString(dr.GetDateTime(index)) + "\n");
                                    sb.AppendLine();

                                index = dr.GetOrdinal("Subsidiary");
                                if (!dr.IsDBNull(index))
                                    sb.Append("Subsidiary: " + dr.GetString(index) + "\n");
                                    sb.AppendLine();

                                index = dr.GetOrdinal("STAGE");
                                if (!dr.IsDBNull(index))
                                    sb.Append("Stage: " + dr.GetString(index) + "\n");

                                                       
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return sb.ToString();
        }

        public string FetchPreviousTenders()
        {
            TenderBot _tenderbot = new TenderBot();
            List<string> result = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["PSTenderDev"].ConnectionString;
            StringBuilder sb = new StringBuilder();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var queryString = Constants.TENDERLIST_QUERY;
                    using (var command = new SqlCommand(queryString, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (IDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (!Convert.IsDBNull(dr[0]))
                                    result.Add(dr[0].ToString());
                            }
                            return String.Join(",", result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result.ToString();
        }

        public object GetData(string query)
        {
            object returnValue = null;
            string connectionString = ConfigurationManager.ConnectionStrings["PSTenderDev"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        returnValue = command.ExecuteScalar();
                        return returnValue;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return returnValue;
        }
    }
}