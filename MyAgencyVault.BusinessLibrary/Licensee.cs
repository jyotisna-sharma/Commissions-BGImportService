using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Collections;
using System.Data;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Data.SqlClient;
using System.Data.EntityClient;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public enum LicenseeStatusEnum
    {
        [EnumMember]
        Active = 0,
        [EnumMember]
        InActive = 1,
        [EnumMember]
        Pending = 2,
        [EnumMember]
        ActiveAndPending = 3,
        [EnumMember]
        All = 4
    }

    [DataContract]
    public class LicenseeBalance
    {
        [DataMember]
        public Guid LicenseeId { get; set; }
        [DataMember]
        public decimal? DueBalance { get; set; }
    }

    [DataContract]
    public class Licensee
    {
        #region IEditable<Licensee> Members

        public static void AddUpdate(LicenseeDisplayData Licensee)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                DLinq.Licensee licensee = (from e in DataModel.Licensees
                                           where e.LicenseeId == Licensee.LicenseeId
                                           select e).FirstOrDefault();

                if (licensee == null)
                {
                    licensee = new DLinq.Licensee
                    {
                        LicenseeId = Licensee.LicenseeId,
                        Company = Licensee.Company,
                        ContactFirst = Licensee.ContactFirst,
                        ContactLast = Licensee.ContactLast,
                        Address1 = Licensee.Address1,
                        Address2 = Licensee.Address2,
                        City = Licensee.City,
                        State = Licensee.State,
                        ZipCode = Licensee.ZipCode.CustomParseToLong(),
                        Phone = Licensee.Phone,
                        Fax = Licensee.Fax,
                        Email = Licensee.Email,
                        AccountCode = Licensee.AccountCode,
                        LicenseStatusId = (int)Licensee.LicenseeStatus,
                        LicensePaymentModeId = Licensee.LicensePaymentModeId,
                        LicenseeSource = Licensee.LicenseeSource,
                        Commissionable = Licensee.Commissionable,
                        TrackDateDefault = Licensee.TrackDateDefault,
                        TaxRate = Licensee.TaxRate,
                        CutOffDay1 = Licensee.CutOffDay1,
                        CutOffDay2 = Licensee.CutOffDay2,
                        LastLogin = Licensee.LastLogin,
                        LastUpload = Licensee.LastUpload,
                        UserName = Licensee.UserName,
                        Password = Licensee.Password,
                        DueBalance = 0,
                        IsDeleted = false,
                        IsClientEnable=Licensee.IsClientEnable
                    };

                    DataModel.AddToLicensees(licensee);
                }
                else
                {
                    licensee.Company = Licensee.Company;
                    licensee.ContactFirst = Licensee.ContactFirst;
                    licensee.ContactLast = Licensee.ContactLast;
                    licensee.Address1 = Licensee.Address1;
                    licensee.Address2 = Licensee.Address2;
                    licensee.City = Licensee.City;
                    licensee.State = Licensee.State;
                    licensee.ZipCode = Licensee.ZipCode.CustomParseToLong();
                    licensee.Phone = Licensee.Phone;
                    licensee.Fax = Licensee.Fax;
                    licensee.Email = Licensee.Email;
                    licensee.LicensePaymentModeId = Licensee.LicensePaymentModeId;
                    licensee.AccountCode = Licensee.AccountCode;
                    licensee.LicenseeSource = Licensee.LicenseeSource;
                    licensee.Commissionable = Licensee.Commissionable;
                    licensee.TrackDateDefault = Licensee.TrackDateDefault;
                    licensee.TaxRate = Licensee.TaxRate;
                    licensee.CutOffDay1 = Licensee.CutOffDay1;
                    licensee.CutOffDay2 = Licensee.CutOffDay2;
                    licensee.LastLogin = Licensee.LastLogin;
                    licensee.LastUpload = Licensee.LastUpload;
                    licensee.IsDeleted = Licensee.IsDeleted;
                    licensee.LicenseStatusId = (int)Licensee.LicenseeStatus;
                    licensee.IsClientEnable = Licensee.IsClientEnable;
                }

                DataModel.SaveChanges();
            }
            SaveAllNotes(Licensee);
        }

        public static void Delete(LicenseeDisplayData Licensee)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.Licensee _License = (from n in DataModel.Licensees
                                           where (n.LicenseeId == Licensee.LicenseeId)
                                           select n).FirstOrDefault();
                var _userCrediantial = (from n in DataModel.UserCredentials
                                        where (n.Licensee.LicenseeId == Licensee.LicenseeId)
                                        select n).ToList();
                _userCrediantial.ForEach(n => n.IsDeleted = true);
                if (_License != null)
                    _License.IsDeleted = true;
                DataModel.SaveChanges();
            }
        }

        #endregion

        #region "methods"
        /// <summary>
        /// call to save all the notes in this licensee.
        /// </summary>
        public static void SaveAllNotes(LicenseeDisplayData Licensee)
        {
            foreach (LicenseeNote n in Licensee.Notes)
            {
                n.AddUpdate();
            }
        }

        /// <summary>
        /// need to look forward the requirement of this function in this class.
        /// </summary>
        /// <returns></returns>
        public static List<LicenseeDisplayData> GetLicenseeList(LicenseeStatusEnum Status, Guid LicenseeId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<LicenseeDisplayData> pp = null;
                if (Status == LicenseeStatusEnum.ActiveAndPending)
                {
                    pp = (from s in DataModel.Licensees
                          where (s.IsDeleted == false && (s.LicenseStatusId == 0 || s.LicenseStatusId == 2) && ((LicenseeId == Guid.Empty) || (LicenseeId == s.LicenseeId)))
                          orderby s.Company, s.ContactLast, s.ContactFirst
                          select new LicenseeDisplayData
                          {
                              LicenseeStatus = (LicenseeStatusEnum)s.LicenseStatusId,
                              LicenseeId = s.LicenseeId,
                              Company = s.Company,
                              ContactFirst = s.ContactFirst,
                              ContactLast = s.ContactLast,
                              Address1 = s.Address1,
                              Address2 = s.Address2,
                              City = s.City,
                              State = s.State,
                              Phone = s.Phone,
                              Fax = s.Fax,
                              Email = s.Email,
                              LicensePaymentModeId = s.LicensePaymentModeId.Value,
                              AccountCode = s.AccountCode,
                              LicenseeSource = s.LicenseeSource,
                              Commissionable = s.Commissionable,
                              TrackDateDefault = s.TrackDateDefault,
                              TaxRate = s.TaxRate,
                              CutOffDay1 = s.CutOffDay1,
                              CutOffDay2 = s.CutOffDay2,
                              LastLogin = s.LastLogin,
                              LastUpload = s.LastUpload,
                              DueBalance = s.DueBalance,
                              IsClientEnable = s.IsClientEnable,
                          }
                     ).ToList();
                }
                else if (Status == LicenseeStatusEnum.All)
                {
                    pp = (from s in DataModel.Licensees
                          where (s.IsDeleted == false && ((LicenseeId == Guid.Empty) || (LicenseeId == s.LicenseeId)))
                          orderby s.Company, s.ContactLast, s.ContactFirst
                          select new LicenseeDisplayData
                          {
                              LicenseeStatus = (LicenseeStatusEnum)s.LicenseStatusId,
                              LicenseeId = s.LicenseeId,
                              Company = s.Company,
                              ContactFirst = s.ContactFirst,
                              ContactLast = s.ContactLast,
                              Address1 = s.Address1,
                              Address2 = s.Address2,
                              City = s.City,
                              State = s.State,
                              Phone = s.Phone,
                              Fax = s.Fax,
                              Email = s.Email,
                              LicensePaymentModeId = s.LicensePaymentModeId.Value,
                              AccountCode = s.AccountCode,
                              LicenseeSource = s.LicenseeSource,
                              Commissionable = s.Commissionable,
                              TrackDateDefault = s.TrackDateDefault,
                              TaxRate = s.TaxRate,
                              CutOffDay1 = s.CutOffDay1,
                              CutOffDay2 = s.CutOffDay2,
                              LastLogin = s.LastLogin,
                              IsDeleted = s.IsDeleted,
                              LastUpload = s.LastUpload,
                              DueBalance = s.DueBalance,
                              IsClientEnable = s.IsClientEnable,
                          }
                     ).ToList();
                }
                else
                {
                    pp = (from s in DataModel.Licensees
                          where (s.IsDeleted == false && (s.LicenseStatusId == (int)Status) && ((LicenseeId == Guid.Empty) || (LicenseeId == s.LicenseeId)))
                          orderby s.Company, s.ContactLast, s.ContactFirst
                          select new LicenseeDisplayData
                          {
                              LicenseeStatus = (LicenseeStatusEnum)s.LicenseStatusId,
                              LicenseeId = s.LicenseeId,
                              Company = s.Company,
                              ContactFirst = s.ContactFirst,
                              ContactLast = s.ContactLast,
                              Address1 = s.Address1,
                              Address2 = s.Address2,
                              City = s.City,
                              State = s.State,
                              Phone = s.Phone,
                              Fax = s.Fax,
                              Email = s.Email,
                              LicensePaymentModeId = s.LicensePaymentModeId.Value,
                              AccountCode = s.AccountCode,
                              LicenseeSource = s.LicenseeSource,
                              Commissionable = s.Commissionable,
                              TrackDateDefault = s.TrackDateDefault,
                              TaxRate = s.TaxRate,
                              CutOffDay1 = s.CutOffDay1,
                              CutOffDay2 = s.CutOffDay2,
                              LastLogin = s.LastLogin,
                              LastUpload = s.LastUpload,
                              IsDeleted = s.IsDeleted,
                              DueBalance = s.DueBalance,
                              IsClientEnable = s.IsClientEnable,
                          }
                     ).ToList();
                }

                foreach (var p in pp)
                {
                    DLinq.Licensee licensee = DataModel.Licensees.FirstOrDefault(s => s.LicenseeId == p.LicenseeId);
                    p.UserName = DataModel.UserCredentials.Where(s => s.MasterRole.RoleId == 2 && s.Licensee.LicenseeId == p.LicenseeId).Select(s => s.UserName).FirstOrDefault();
                    p.Password = DataModel.UserCredentials.Where(s => s.MasterRole.RoleId == 2 && s.Licensee.LicenseeId == p.LicenseeId).Select(s => s.Password).FirstOrDefault();
                    if (licensee.ZipCode != null)
                        p.ZipCode = licensee.ZipCode.Value.ToString("D5");
                    else
                        p.ZipCode = null;
                }

                return pp;
            }
        }

        public static LicenseeDisplayData GetLicenseeByID(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                var lic = (from s in DataModel.Licensees
                           from Us in DataModel.UserCredentials
                           where (s.LicenseeId == id) && (s.IsDeleted == false || s.IsDeleted == null)
                           select new LicenseeDisplayData
                       {
                           LicenseeStatus = (LicenseeStatusEnum)s.LicenseStatusId,
                           LicenseeId = s.LicenseeId,
                           Company = s.Company,
                           ContactFirst = s.ContactFirst,
                           ContactLast = s.ContactLast,
                           Address1 = s.Address1,
                           Address2 = s.Address2,
                           City = s.City,
                           State = s.State,
                           Phone = s.Phone,
                           Fax = s.Fax,
                           Email = s.Email,
                           LicensePaymentModeId = s.MasterLicensePaymentMode.LicensePaymentModeId,
                           AccountCode = s.AccountCode,
                           LicenseeSource = s.LicenseeSource,
                           Commissionable = s.Commissionable,
                           TrackDateDefault = s.TrackDateDefault,
                           TaxRate = s.TaxRate,
                           DueBalance = s.DueBalance,
                           CutOffDay1 = s.CutOffDay1,
                           CutOffDay2 = s.CutOffDay2,
                           LastLogin = s.LastLogin,
                           LastUpload = s.LastUpload,
                           UserName = s.UserName,
                           Password = s.Password,
                           IsClientEnable=s.IsClientEnable

                       }
             ).FirstOrDefault();

                DLinq.Licensee licensee = DataModel.Licensees.First(s => s.LicenseeId == lic.LicenseeId);

                if (licensee.ZipCode != null)
                    lic.ZipCode = licensee.ZipCode.Value.ToString("D5");
                else
                    lic.ZipCode = null;

                return lic;
            }
        }

        /// <summary>
        /// compile and create Licensee wise batch files (all for Card payee) to be sent to Licensees given in the parameter, 
        /// for the given From and To Date.
        /// <param name="From Date"/>
        /// <param name="To Date"/>        
        /// <param name="LicenseeList"/>        
        /// </summary>
        /// <returns></returns>
        public static bool ExportCardPayees()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// compile and create Licensee wise batch files (all for Check payee) to be sent to Licensees given in the parameter, 
        /// for the given From and To Date.
        /// <param name="From Date"/>
        /// <param name="To Date"/>        
        /// <param name="LicenseeList"/>        
        /// </summary>
        /// <returns></returns>
        public static bool ExportCheckPayees()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool ImportDataFromFile()
        {
            throw new NotImplementedException();
        }

        public static List<string> getPaymentTypes()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from m in DataModel.MasterLicensePaymentModes select m.Name).ToList();
            }
        }

        public static void SetLastUploadTime(Guid LicenseeId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.Licensee licensee = DataModel.Licensees.FirstOrDefault(s => s.LicenseeId == LicenseeId);

                if (licensee != null)
                    licensee.LastUpload = DateTime.Now;

                DataModel.SaveChanges();
            }
        }

        public static void SetLastLoginTime(Guid LicenseeId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.Licensee licensee = DataModel.Licensees.FirstOrDefault(s => s.LicenseeId == LicenseeId);

                if (licensee != null)
                    licensee.LastLogin = DateTime.Now;

                DataModel.SaveChanges();
            }
        }

        public static List<LicenseeBalance> getLicenseesBalance()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<LicenseeBalance> licenseesBalance = DataModel.Licensees.Where(s => s.IsDeleted == false).Select(s => new LicenseeBalance { LicenseeId = s.LicenseeId, DueBalance = s.DueBalance }).ToList();
                return licenseesBalance;
            }
        }

        #endregion

    }

    [DataContract]
    public class LicenseeDisplayData
    {
        #region "Data members aka - Public Properties"
        [DataMember]
        public Guid LicenseeId { get; set; }
        [DataMember]
        public LicenseeStatusEnum LicenseeStatus { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string ContactFirst { get; set; }
        [DataMember]
        public string ContactLast { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int LicensePaymentModeId { get; set; }
        [DataMember]
        public string AccountCode { get; set; }
        [DataMember]
        public string LicenseeSource { get; set; }
        [DataMember]
        public Boolean? Commissionable { get; set; }
        [DataMember]
        public DateTime? TrackDateDefault { get; set; }
        [DataMember]
        public double? TaxRate { get; set; }
        [DataMember]
        public int? CutOffDay1 { get; set; }
        [DataMember]
        public int? CutOffDay2 { get; set; }
        [DataMember]
        public DateTime? LastLogin { get; set; }
        [DataMember]
        public DateTime? LastUpload { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public Boolean? IsDeleted { get; set; }

        [DataMember]
        public Boolean? IsClientEnable { get; set; }

        [DataMember]
        public decimal? DueBalance { get; set; }
        [DataMember]
        public List<Payor> LocalPayors { get; set; }
        [DataMember]
        public List<LicenseeNote> Notes
        {
            get
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    return (from u in DataModel.LicenseeNotes
                            where (this.LicenseeId == u.Licensee.LicenseeId)
                            select new LicenseeNote
                            {
                                NoteID = u.LicenseeNoteId,
                                Content = u.Note,
                                CreatedDate = u.CreatedDate,
                                LastModifiedDate = u.LastModifiedDate,
                                LicenseeId = u.LicenseeNoteId
                            }).ToList();
                }
            }
            set
            {

            }
        }
        /// <summary>
        /// holds all Journals under this licensee.
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public List<LicenseeInvoice> LicenseeJournals { get; set; }
        /// <summary>
        /// holds all Invoices under this licensee till date.
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public List<Invoice> LicenseeInvoices { get; set; }
        /// <summary>
        /// holds all Exported Imported files for all licensees.
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public List<IFile> ExportedImportedFiles { get; set; }
        #endregion

        public static List<LicenseeDisplayData> GetDisplayedLicenseeList(Guid licenseeId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                if (licenseeId == Guid.Empty)
                {
                    return (from s in DataModel.Licensees
                            where (s.IsDeleted == false && s.LicenseStatusId != 1)
                            orderby s.Company
                            select new LicenseeDisplayData
                            {
                                LicenseeStatus = (LicenseeStatusEnum)s.LicenseStatusId,
                                LicenseeId = s.LicenseeId,
                                Company = s.Company,
                                ContactFirst = s.ContactFirst,
                                ContactLast = s.ContactLast,
                                Address1 = s.Address1,
                                Address2 = s.Address2,
                                City = s.City,
                                State = s.State,
                                Phone = s.Phone,
                                Fax = s.Fax,
                                Email = s.Email,
                                LicensePaymentModeId = s.MasterLicensePaymentMode.LicensePaymentModeId,
                                AccountCode = s.AccountCode,
                                LicenseeSource = s.LicenseeSource,
                                Commissionable = s.Commissionable,
                                TrackDateDefault = s.TrackDateDefault,
                                TaxRate = s.TaxRate,
                                DueBalance = s.DueBalance,
                                CutOffDay1 = s.CutOffDay1,
                                CutOffDay2 = s.CutOffDay2,
                                LastLogin = s.LastLogin,
                                LastUpload = s.LastUpload,
                                UserName = s.UserName,
                                IsDeleted = s.IsDeleted,
                                Password = s.Password
                            }
                 ).ToList();
                }
                else
                {
                    return (from s in DataModel.Licensees
                            where s.LicenseeId == licenseeId
                            orderby s.Company
                            select new LicenseeDisplayData
                            {
                                LicenseeStatus = (LicenseeStatusEnum)s.LicenseStatusId,
                                LicenseeId = s.LicenseeId,
                                Company = s.Company,
                                ContactFirst = s.ContactFirst,
                                ContactLast = s.ContactLast,
                                Address1 = s.Address1,
                                Address2 = s.Address2,
                                City = s.City,
                                State = s.State,
                                Phone = s.Phone,
                                Fax = s.Fax,
                                Email = s.Email,
                                LicensePaymentModeId = s.MasterLicensePaymentMode.LicensePaymentModeId,
                                AccountCode = s.AccountCode,
                                LicenseeSource = s.LicenseeSource,
                                Commissionable = s.Commissionable,
                                TrackDateDefault = s.TrackDateDefault,
                                TaxRate = s.TaxRate,
                                DueBalance = s.DueBalance,
                                CutOffDay1 = s.CutOffDay1,
                                CutOffDay2 = s.CutOffDay2,
                                LastLogin = s.LastLogin,
                                LastUpload = s.LastUpload,
                                UserName = s.UserName,
                                IsDeleted = s.IsDeleted,
                                Password = s.Password
                            }).ToList();
                }
            }
        }


        public static List<LicenseeDisplayData> GetDisplayedLicenseeListPolicyManger(Guid licenseeId)
        {
            List<LicenseeDisplayData> lstLicenseeDisplayData = new List<LicenseeDisplayData>();

            SqlConnection con = null;
            try
            {

                using (con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetLicenseeName", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LicenseeId", licenseeId);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        // Call Read before accessing data. 
                        while (reader.Read())
                        {
                            try
                            {
                                LicenseeDisplayData objLicenseeDisplayData = new LicenseeDisplayData();

                                try
                                {

                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["LicenseeId"])))
                                    {
                                        objLicenseeDisplayData.LicenseeId = (Guid)reader["LicenseeId"];
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["IsClientEnable"])))
                                    {
                                        objLicenseeDisplayData.IsClientEnable = Convert.ToBoolean(reader["IsClientEnable"]);
                                    }
                                    else
                                    {
                                        objLicenseeDisplayData.IsClientEnable = false;
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["Company"])))
                                    {

                                        objLicenseeDisplayData.Company = Convert.ToString(reader["Company"]);
                                    }
                                }
                                catch
                                {
                                }

                                lstLicenseeDisplayData.Add(objLicenseeDisplayData);

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["ContactFirst"])))
                                //{
                                //    objLicenseeDisplayData.ContactFirst = Convert.ToString(reader["ContactFirst"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["ContactLast"])))
                                //{
                                //    objLicenseeDisplayData.ContactLast = Convert.ToString(reader["ContactLast"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["Address1"])))
                                //{
                                //    objLicenseeDisplayData.Address1 = Convert.ToString(reader["Address1"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["Address2"])))
                                //{
                                //    objLicenseeDisplayData.Address2 = Convert.ToString(reader["Address2"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["City"])))
                                //{
                                //    objLicenseeDisplayData.City = Convert.ToString(reader["City"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["State"])))
                                //{
                                //    objLicenseeDisplayData.State = Convert.ToString(reader["State"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["Phone"])))
                                //{
                                //    objLicenseeDisplayData.Phone = Convert.ToString(reader["Phone"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["Fax"])))
                                //{
                                //    objLicenseeDisplayData.Fax = Convert.ToString(reader["Fax"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["Email"])))
                                //{
                                //    objLicenseeDisplayData.Email = Convert.ToString(reader["Email"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["LicensePaymentModeId"])))
                                //{
                                //    objLicenseeDisplayData.LicensePaymentModeId = Convert.ToInt32(reader["LicensePaymentModeId"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["LicensePaymentModeId"])))
                                //{
                                //    objLicenseeDisplayData.LicensePaymentModeId = Convert.ToInt32(reader["LicensePaymentModeId"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["AccountCode"])))
                                //{
                                //    objLicenseeDisplayData.AccountCode = Convert.ToString(reader["AccountCode"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["LicenseeSource"])))
                                //{
                                //    objLicenseeDisplayData.LicenseeSource = Convert.ToString(reader["LicenseeSource"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["Commissionable"])))
                                //{
                                //    objLicenseeDisplayData.Commissionable = Convert.ToBoolean(reader["Commissionable"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["TrackDateDefault"])))
                                //{
                                //    objLicenseeDisplayData.TrackDateDefault = Convert.ToDateTime(reader["TrackDateDefault"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["TrackDateDefault"])))
                                //{
                                //    objLicenseeDisplayData.TrackDateDefault = Convert.ToDateTime(reader["TrackDateDefault"]);
                                //}

                                //if (!string.IsNullOrEmpty(Convert.ToString(reader["IsDeleted"])))
                                //{
                                //    objLicenseeDisplayData.IsDeleted = (bool)reader["IsDeleted"];
                                //}

                                //TaxRate = s.TaxRate,
                                //DueBalance = s.DueBalance,
                                //CutOffDay1 = s.CutOffDay1,
                                //CutOffDay2 = s.CutOffDay2,
                                //LastLogin = s.LastLogin,
                                //LastUpload = s.LastUpload,
                                //UserName = s.UserName,
                                //IsDeleted = s.IsDeleted,
                                //Password = s.Password
                            }
                            catch
                            {

                            }

                        }

                        // Call Close when done reading.
                        reader.Close();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (con != null)
                {
                    con.Dispose();
                    con.Close();
                }
            }

            return lstLicenseeDisplayData;

        }

        public static List<LicenseeDisplayData> GetLicenseeName(string strCompanyName)
        {
            List<LicenseeDisplayData> objList = new List<LicenseeDisplayData>();
           
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                if (strCompanyName != string.Empty)
                {
                    var varValue = (from s in DataModel.Licensees
                                    where (s.Company.ToUpper() == strCompanyName.ToUpper() && s.IsDeleted==false)
                                    select s.LicenseeId);


                    if (varValue != null)
                    {
                        foreach (var item in varValue)
                        {
                            objList = GetDisplayedLicenseeList(item);
                            if (objList != null && objList.Count > 0)
                            {
                                break;
                            }
                        }

                    }

                }
            }
            return objList;
        }

        public static List<LicenseeDisplayData> GetLicenseeByID(Guid? licID)
        {
            List<LicenseeDisplayData> objList = new List<LicenseeDisplayData>();

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                if (licID != null)
                {
                    var varValue = (from s in DataModel.Licensees
                                    where (s.LicenseeId == licID && s.IsDeleted == false)
                                    select s.LicenseeId);


                    if (varValue != null)
                    {
                        foreach (var item in varValue)
                        {
                            objList = GetDisplayedLicenseeList(item);
                            if (objList != null && objList.Count > 0)
                            {
                                break;
                            }
                        }

                    }

                }
            }
            return objList;
        }
    }
}
  
        
  

  
      
