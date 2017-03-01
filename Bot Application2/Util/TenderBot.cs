using Bot_Application2.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bot_Application.Util
{
    public class TenderBot : ITenderBot
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
        public string FetchAnnouncementDate(int tenderId)
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


        public IList<TenderModel> GetTenderDetails(string alias)
        {
            IList<TenderModel> tendersList = new List<TenderModel>();
            string connectionString = ConfigurationManager.ConnectionStrings["PSTenderDev"].ConnectionString;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var queryString = Constants.TENDERDETAILS_REPORT;
                using (var command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.Add(new SqlParameter("@v_Alias", alias));
                    command.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            TenderModel tenderModel = new TenderModel();

                            var index = dr.GetOrdinal("TenderId");
                            if (!dr.IsDBNull(index))
                                tenderModel.TenderId = Convert.ToInt64(dr.GetValue(index));


                            index = dr.GetOrdinal("Title");
                            if (!dr.IsDBNull(index))
                                tenderModel.Title = Convert.ToString(dr.GetValue(index));

                            index = dr.GetOrdinal("Tendernumber");
                            if (!dr.IsDBNull(index))
                                tenderModel.TenderNumber = Convert.ToString(dr.GetValue(index));

                            index = dr.GetOrdinal("CurrentOwner");
                            if (!dr.IsDBNull(index))
                                tenderModel.CurrentOwner = Convert.ToString(dr.GetValue(index));

                            index = dr.GetOrdinal("LastModified");
                            if (!dr.IsDBNull(index))
                                tenderModel.LastModified = Convert.ToDateTime(dr.GetValue(index));

                            index = dr.GetOrdinal("Subsidiary");
                            if (!dr.IsDBNull(index))
                                tenderModel.Subsidiary = Convert.ToString(dr.GetValue(index));


                            index = dr.GetOrdinal("STAGE");
                            if (!dr.IsDBNull(index))
                                tenderModel.Stage = Convert.ToString(dr.GetValue(index));

                            tendersList.Add(tenderModel);
                        }

                    }
                }
            }
            return tendersList;
        }


        public DataTable GetTenderReportDetails(string alias)
        {
            IList<TenderModel> tendersList = new List<TenderModel>();
            string connectionString = ConfigurationManager.ConnectionStrings["PSTenderDev"].ConnectionString;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var queryString = Constants.TENDERDETAILS_REPORT;
                using (var command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.Add(new SqlParameter("@v_Alias", alias));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {

                        sda.SelectCommand = command;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            return dt;
                        }
                    }

                }
            }
        }

        private string DataMapper(DataTable dt)
        {

            string rw = "";
            StringBuilder builder = new StringBuilder();

            foreach (DataColumn dc in dt.Columns)
            {
                rw = dc.ToString();
                if (rw.Contains(",")) rw = "\"" + rw + "\"";
                builder.Append(rw + ",");
            }
            builder.Append(Environment.NewLine);

            foreach (DataRow dr in dt.Rows)
            {

                builder.AppendLine(string.Join(",", dr.ItemArray));
            }

            return builder.ToString();
        }



        private byte[] ConvertDataSetToByteArray(DataTable dataSet)
        {
            byte[] binaryDataResult = null;
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter brFormatter = new BinaryFormatter();
                dataSet.RemotingFormat = SerializationFormat.Binary;
                brFormatter.Serialize(memStream, dataSet);
                binaryDataResult = memStream.ToArray();
            }
            return binaryDataResult;
        }

        public bool ExportReportExcel(string alias, string subsidiaryName, string startDate, string endDate, string mailRecipient)
        {

            bool ismailSent = false;

            DataTable dt = GetTenderReportDetails(alias);
            if (dt != null && dt.Rows.Count > 0)
            {

                byte[] bytes = Encoding.UTF8.GetBytes(DataMapper(dt));

                DateTime startDateTime;
                DateTime endDateTime;
                if (subsidiaryName.ToLower() != "no" && startDate.ToLower() != "no" && endDate.ToLower() != "no")
                {
                    if (DateTime.TryParse(startDate, out startDateTime) && DateTime.TryParse(endDate, out endDateTime))
                    {
                        DataTable tblFiltered = dt.AsEnumerable()
                       .Where(row => row.Field<String>("Subsidiary").ToLower() == subsidiaryName.Trim().ToLower()
                       && row.Field<DateTime>("LastModified") >= startDateTime
                       && row.Field<DateTime>("LastModified") <= endDateTime)
                       .CopyToDataTable();
                        bytes = Encoding.UTF8.GetBytes(DataMapper(tblFiltered));
                    }
                }
                else if (startDate.ToLower() != "no")
                {
                    if (DateTime.TryParse(startDate, out startDateTime))
                    {
                        DataTable tblFiltered = dt.AsEnumerable()
                       .Where(row => row.Field<DateTime>("LastModified").ToShortDateString() == startDateTime.ToShortDateString())
                       .CopyToDataTable();
                        bytes = Encoding.UTF8.GetBytes(DataMapper(tblFiltered));
                    }
                }
                else if (endDate.ToLower() != "no")
                {
                    if (DateTime.TryParse(endDate, out endDateTime))
                    {
                        DataTable tblFiltered = dt.AsEnumerable()
                       .Where(row => row.Field<DateTime>("LastModified") <= endDateTime)
                       .CopyToDataTable();
                        bytes = Encoding.UTF8.GetBytes(DataMapper(tblFiltered));
                    }
                }
                else
                {
                    DataTable tblFiltered = dt.AsEnumerable()
                   .Where(row => row.Field<String>("Subsidiary").ToLower() == subsidiaryName.Trim().ToLower())
                   .CopyToDataTable();
                    bytes = Encoding.UTF8.GetBytes(DataMapper(tblFiltered));
                }
                

                try
                {
                    //Send Email with Excel attachment.
                    using (MailMessage mm = new MailMessage("bottender@gmail.com", mailRecipient))
                    {
                        mm.Subject = "Exported Excel";
                        mm.Body = "Hello!, Please find attached tender details.";

                        //Add Byte array as Attachment.
                        mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "TenderDetailsReport.xls"));
                        mm.IsBodyHtml = true;

                        string senderID = "bottender@gmail.com";
                        const string senderPassword = "P@$$w)rd";

                        SmtpClient smtp = new SmtpClient()
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                            Timeout = 30000,

                        };

                        smtp.Send(mm);
                        ismailSent = true;
                    }
                }
                catch (Exception ex)
                {
                    return ismailSent;
                }
                return ismailSent;
            }
            else
            {
                return ismailSent;
            }
        }


    }
}