using MyAgencyVault.BusinessLibrary.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class Settings : IEditable<Settings>
    {

        public static void AddUpdate(Guid licenseeId, Guid reportID, string fields)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Settings update request for report save - " + licenseeId + ", reportID: " + reportID + ", fields: " + fields, true);
            try
            {
                SqlConnection con = null;
                using (con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveLicenseeReportFields", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LicenseeId", licenseeId);
                        cmd.Parameters.AddWithValue("@ReportID", reportID);
                        cmd.Parameters.AddWithValue("@Fields", fields);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Settings update exception in report save - " + ex.Message, true);
                throw ex;
            }
        }

        public void AddUpdate()
        {
            throw new NotImplementedException();
        }
        public Settings GetOfID()
        {
            throw new NotImplementedException();
        }
        #region
        public static void SaveNamedSchedule(PayorIncomingSchedule schedule)
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("SaveSchedule schedule" + schedule.ToStringDump(), true);
                if(schedule.Mode== Mode.Custom)
                {
                    schedule.FirstYearPercentage = 0;
                    schedule.RenewalPercentage = 0;

                }
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_saveNamedSchedule", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ScheduleID", schedule.IncomingScheduleID);
                        cmd.Parameters.AddWithValue("@LicenseeID", schedule.LicenseeID);
                        cmd.Parameters.AddWithValue("@FirstYear", schedule.FirstYearPercentage);
                        cmd.Parameters.AddWithValue("@Renewal", schedule.RenewalPercentage);
                        cmd.Parameters.AddWithValue("@ScheduleTypeID", schedule.ScheduleTypeId);
                        cmd.Parameters.AddWithValue("@IncomingPaymentTypeId", schedule.IncomingPaymentTypeID);
                        cmd.Parameters.AddWithValue("@SplitPercentage", schedule.SplitPercentage);
                        cmd.Parameters.AddWithValue("@Advance", schedule.Advance);
                        cmd.Parameters.AddWithValue("@CreatedBy", schedule.CreatedBy);
                        cmd.Parameters.AddWithValue("@ModifiedBy", schedule.ModifiedBy);
                        cmd.Parameters.AddWithValue("@Title", schedule.Title);
                        cmd.Parameters.AddWithValue("@Mode", (int)schedule.Mode);
                        cmd.Parameters.AddWithValue("@CustomType", (int)schedule.CustomType);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                ActionLogger.Logger.WriteImportPolicyLog("SaveSchedule main scheudle saved ", true);
                if (schedule.Mode == Mode.Custom)
                {
                    if (schedule.CustomType == CustomMode.Graded)
                    {
                       PayorIncomingSchedule.SaveGradedSchedule(schedule);
                    }
                    else
                    {
                        PayorIncomingSchedule.SaveNonGradedSchedule(schedule);
                    }
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("SaveSchedule exception: " + ex.Message, true);
                throw ex;
            }
        }
        public static bool CheckNamedscheduleExist(Guid licenseeId)
        {
            bool isExist = false;
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_CheckNamedScheduleExist", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@licenseeId", licenseeId);
                        cmd.Parameters.Add("@isExist", SqlDbType.Bit);
                        cmd.Parameters["@isExist"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteScalar();
                        isExist = (bool)cmd.Parameters["@isExist"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isExist;
        }
        public static PayorIncomingSchedule IsNamedScheduleExist(string scheduleName, Guid? incomingScheduleId,Guid licenseeId, out bool isExist)
        {
            PayorIncomingSchedule scheduleDetails = new PayorIncomingSchedule();
             isExist = true;
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("IsNamedScheduleExist:" + scheduleName, true);
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_IsNamedScheduleExist", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Title", scheduleName);
                        cmd.Parameters.AddWithValue("@IncomingScheduleId", incomingScheduleId);
                        cmd.Parameters.AddWithValue("@licenseeId", licenseeId);
                        cmd.Parameters.Add("@IsExist", SqlDbType.Bit);
                        cmd.Parameters["@IsExist"].Direction = ParameterDirection.Output;
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                            scheduleDetails.Title = Convert.ToString(reader["Title"]);
                            scheduleDetails.IncomingScheduleID = (Guid)reader["IncomingScheduleId"];
                            scheduleDetails.LicenseeID = (Guid)reader["LicenseeId"];
                            
                        }
                        con.Close();
                        isExist = (bool)cmd.Parameters["@IsExist"].Value;
                    }

                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("SaveSchedule exception: " + ex.Message, true);
                throw ex;
            }
            return scheduleDetails;
        }

        public static List<PayorIncomingSchedule> GetNamedScheduleList(Guid LicenseeId)
        {
            List<PayorIncomingSchedule> lstSchedules = new List<PayorIncomingSchedule>();
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetAllSchedules LicenseeID" + LicenseeId, true);
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getAllNamedScheduledList", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LicenseeID", LicenseeId);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            PayorIncomingSchedule schedule = new PayorIncomingSchedule();
                            schedule.IncomingScheduleID = (Guid)dr["IncomingScheduleID"];
                            schedule.ScheduleTypeId = (int)(dr["ScheduleTypeId"]);
                            schedule.StringFirstYearPercentage = Convert.ToString(dr["FirstYearPercentage"]);
                            schedule.StringRenewalPercentage = Convert.ToString(dr["RenewalPercentage"]);
                            var splitper = Convert.ToString(dr["SplitPercentage"]);
                            if (!string.IsNullOrEmpty(splitper))
                            {
                                schedule.StringSplitPercentage = splitper;
                            }
                            string frstYear = Convert.ToString(dr["FirstYearPercentage"]);
                            double fy = 0;
                            double.TryParse(frstYear, out fy);
                            schedule.FirstYearPercentage = fy;

                            string renewYear = Convert.ToString(dr["RenewalPercentage"]);
                            double ry = 0;
                            double.TryParse(renewYear, out ry);
                            schedule.RenewalPercentage = ry;

                            string split = Convert.ToString(dr["SplitPercentage"]);
                            double splitPer = 0;
                            double.TryParse(split, out splitPer);
                            schedule.SplitPercentage = splitPer;

                            string advanc = Convert.ToString(dr["Advance"]);
                            int adv = 0;
                            int.TryParse(advanc, out adv);
                            schedule.Advance = adv;

                            schedule.Title = Convert.ToString(dr["Title"]);
                            schedule.IsNamedSchedule= Convert.ToBoolean(dr["IsNamedSchedule"]);
                            schedule.ScheduleType = Convert.ToString(dr["ScheduleType"]);

                            string mod = Convert.ToString(dr["Mode"]);
                            int scheduleMode = 0;
                            int.TryParse(mod, out scheduleMode);
                            schedule.Mode = (Mode)scheduleMode;

                            string strType = Convert.ToString(dr["CustomType"]);
                            int intType = 0;
                            int.TryParse(strType, out intType);
                              intType = (intType == 0) ? 1 : intType;
                            schedule.CustomType = (CustomMode)intType;
                          

                            if (schedule.Mode == Mode.Custom)
                            {
                                if (schedule.CustomType == CustomMode.Graded)
                                {
                                    schedule.GradedSchedule = PayorIncomingSchedule.GradedScheduleList(schedule.IncomingScheduleID);

                                }
                                else
                                {
                                    schedule.NonGradedSchedule = PayorIncomingSchedule.NonGradedScheduleList(schedule.IncomingScheduleID);
                                }
                            }

                            lstSchedules.Add(schedule);
                        }
                        dr.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetAllSchedules exception: " + ex.Message, true);
                throw ex;
            }
            return lstSchedules;
        }

        #endregion

        public static List<ReportCustomFieldSettings> getData(Guid licenseeId, Guid reportID)
        {
            SqlConnection con = null;
            List<ReportCustomFieldSettings> lstSettings = new List<ReportCustomFieldSettings>();
            try
            {
                using (con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getLicenseeReportFields", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LicenseeId", licenseeId);
                        cmd.Parameters.AddWithValue("@ReportID", reportID);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        // Call Read before accessing data. 
                        while (reader.Read())
                        {
                            ReportCustomFieldSettings obj = new ReportCustomFieldSettings();
                            obj.FieldName = Convert.ToString(reader["field"]);

                            string strIsSelected = Convert.ToString(reader["IsSelected"]);
                            bool isSelected = false;
                            bool.TryParse(strIsSelected, out isSelected);
                            obj.IsSelected = isSelected;

                            bool isModifiable = false;
                            string strIsModifiable = Convert.ToString(reader["IsModifiable"]);
                            bool.TryParse(strIsModifiable, out isModifiable);
                            obj.IsModifiable = isModifiable;
                            obj.IsReadOnly = !isModifiable;

                            lstSettings.Add(obj);
                        }
                        reader.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception while getData in custom field settings:" + ex.Message, true);
            }
            return lstSettings;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }


    }
    [DataContract]
    public class ReportCustomFieldSettings
    {
        [DataMember]
        public string FieldName { get; set; }
        [DataMember]
        public bool IsModifiable { get; set; }
        [DataMember]
        public bool IsSelected { get; set; }
        [DataMember]
        public bool IsReadOnly { get; set; } //kept to bind grid property
    }
}
