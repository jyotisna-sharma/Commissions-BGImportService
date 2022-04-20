using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Data.Objects;


namespace MyAgencyVault.BusinessLibrary
{
  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public enum Operation
  {
    [EnumMember]
    Add,
    [EnumMember]
    Upadte,
    [EnumMember]
    Delete,
    [EnumMember]
    None
  }

  [DataContract]
  public class OperationSet
  {
      [DataMember]
      public Operation MainOperation { get; set; }
      [DataMember]
      public Operation NickNameOperation { get; set; }
      [DataMember]
      public Guid PreviousCarrierId { get; set; }
      [DataMember]
      public Guid PreviousCoverageId { get; set; }
      [DataMember]
      public string previousCovarageNickName { get; set; }
  }

  [DataContract]
  public class Carrier
  {
    #region "public properties"

    [DataMember]
    public Guid CarrierId { get; set; }
    [DataMember]
    public Guid PayerId { get; set; }
    [DataMember]
    public string CarrierName { get; set; }
    [DataMember]
    public string NickName { get; set; }
    [DataMember]
    public bool IsTrackIncomingPercentage { get; set; }
    [DataMember]
    public bool IsDeleted { get; set; }
    [DataMember]
    public bool IsGlobal { get; set; }
    [DataMember]
    public bool IsTrackMissingMonth { get; set; }
    [DataMember]
    public Guid CreatedBy { get; set; }
    [DataMember]
    public Guid UserID { get; set; }
    [DataMember]
    public Guid? LicenseeId { get; set; }
    [DataMember]
    public List<Coverage> Coverages { get; set; }

    #endregion

    #region IEditable<Carrier> Members

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ReturnStatus AddUpdateDelete(OperationSet operationType)
    {
      using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
      {
        ReturnStatus status = null;
        status = ValidateCarrier(DataModel, operationType);
        if (!status.IsError)
        {
          if (operationType.MainOperation == Operation.Add)
          {
            DLinq.Carrier carrier = new DLinq.Carrier
            {
              CarrierId = this.CarrierId,
              CarrierName = this.CarrierName,
              IsDeleted = this.IsDeleted,
              IsGlobal = this.IsGlobal,
              LicenseeId = (this.LicenseeId == Guid.Empty ? null : this.LicenseeId),
              CreatedBy = this.UserID,
              CreatedOn = DateTime.Now
            };

            DataModel.AddToCarriers(carrier);
            DataModel.SaveChanges();
       
          }

          if (operationType.NickNameOperation == Operation.Add)
          {
            DLinq.CarrierNickName carrierNickName = DataModel.CarrierNickNames.FirstOrDefault(s => s.PayorId == this.PayerId && s.CarrierId == this.CarrierId);

            if (carrierNickName != null)
            {
              carrierNickName.IsDeleted = false;
              carrierNickName.NickName = this.NickName;
              carrierNickName.IsTrackIncomingPercentage = this.IsTrackIncomingPercentage;
              carrierNickName.IsTrackMissingMonth = this.IsTrackMissingMonth;
              carrierNickName.CreatedBy = this.UserID;
              carrierNickName.ModifiedBy = this.UserID;
              carrierNickName.ModifiedOn = DateTime.Now;
            }
            else
            {
              carrierNickName = new DLinq.CarrierNickName
              {
                PayorId = this.PayerId,
                CarrierId = this.CarrierId,
                NickName = this.NickName,
                IsTrackIncomingPercentage = this.IsTrackIncomingPercentage,
                IsTrackMissingMonth = this.IsTrackMissingMonth,
                CreatedBy = this.UserID,
                ModifiedBy = this.UserID,
                IsDeleted = false,
                ModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now
              };
              DataModel.AddToCarrierNickNames(carrierNickName);
                //send mail to benefits 
                  string mailBody = "<html>Hi,<br><br>New Carrier has been added to Commissions Department with following details:<br><br><Table><tr> <td>Payor ID: </td><td> " + this.PayerId + "</td>" +
                "</tr><tr> <td>Payor Name: </td><td> " + Payor.GetPayorByID(this.PayerId).NickName + "</td><tr> <td>Carrier Name: </td><td> " + this.CarrierName + "</td></tr> </table><br><br>Regards,<br>Commissions Department </html>";
                  MailServerDetail.SendMailToBenefits("Carrier", mailBody);
            }
          }
          else if (operationType.NickNameOperation == Operation.Upadte)
          {
            DLinq.CarrierNickName carrierNickName = DataModel.CarrierNickNames.FirstOrDefault(s => s.PayorId == this.PayerId && s.CarrierId == operationType.PreviousCarrierId);

            if (carrierNickName != null)
            {
              DataModel.CarrierNickNames.DeleteObject(carrierNickName);

              carrierNickName = new DLinq.CarrierNickName
              {
                PayorId = this.PayerId,
                CarrierId = this.CarrierId,
                NickName = this.NickName,
                IsTrackIncomingPercentage = this.IsTrackIncomingPercentage,
                IsTrackMissingMonth = this.IsTrackMissingMonth,
                CreatedBy = this.UserID,
                ModifiedBy = this.UserID,
                IsDeleted = false,
                ModifiedOn = DateTime.Now
              };
              DataModel.AddToCarrierNickNames(carrierNickName);
            }
          }
          else if (operationType.NickNameOperation == Operation.Delete)
          {
            DLinq.CarrierNickName carrierNickName = (from c in DataModel.CarrierNickNames
                                                     where (c.CarrierId == this.CarrierId && c.PayorId == this.PayerId && c.IsDeleted == false)
                                                     select c).FirstOrDefault();

            if (carrierNickName != null && carrierNickName.PayorId != null)
              carrierNickName.IsDeleted = true;

            //if last reference of carrier is deleted from the payor then
            //we also delete the carrier row

            //int carrierNickNameExist = (from c in DataModel.CarrierNickNames
            //                            where (c.CarrierId == this.CarrierId && c.IsDeleted == false)
            //                            select c).Count();
            //if (carrierNickNameExist == 0)
            //{
            //    DLinq.Carrier carrier = DataModel.Carriers.FirstOrDefault(s => s.CarrierId == this.CarrierId);
            //    carrier.IsDeleted = true;
            //    status.IsCarrierOrCoverageRemoved = true;
            //}
          }
          DataModel.SaveChanges();
        }
        return status;
      }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns></returns>
    //public ReturnStatus AddUpdateDelete(OperationSet operationType)
    //{
    //    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
    //    {
    //        ReturnStatus status = null;
    //        if (operationType.MainOperation == Operation.Add)
    //        {
    //            status = ValidateCarrier(DataModel, operationType);
    //            if (!status.IsError)
    //            {
    //                DLinq.Carrier carrier = new DLinq.Carrier
    //                    {
    //                        CarrierId = this.CarrierId,
    //                        CarrierName = this.CarrierName,
    //                        IsDeleted = this.IsDeleted,
    //                        IsGlobal = this.IsGlobal,
    //                        LicenseeId = (this.LicenseeId == Guid.Empty ? null : this.LicenseeId),
    //                        CreatedBy = this.UserID
    //                    };

    //                DataModel.AddToCarriers(carrier);

    //                DLinq.CarrierNickName carrierNickName = DataModel.CarrierNickNames.FirstOrDefault(s => s.PayorId == this.PayerId && s.CarrierId == this.CarrierId);

    //                if (carrierNickName != null)
    //                {
    //                    carrierNickName.IsDeleted = false;
    //                    carrierNickName.NickName = this.NickName;
    //                    carrierNickName.IsTrackIncomingPercentage = this.IsTrackIncomingPercentage;
    //                    carrierNickName.IsTrackMissingMonth = this.IsTrackMissingMonth;
    //                    carrierNickName.CreatedBy = this.UserID;
    //                    carrierNickName.ModifiedBy = this.UserID;
    //                    carrierNickName.ModifiedOn = DateTime.Now;
    //                }
    //                else
    //                {
    //                    carrierNickName = new DLinq.CarrierNickName
    //                    {
    //                        PayorId = this.PayerId,
    //                        CarrierId = this.CarrierId,
    //                        NickName = this.NickName,
    //                        IsTrackIncomingPercentage = this.IsTrackIncomingPercentage,
    //                        IsTrackMissingMonth = this.IsTrackMissingMonth,
    //                        CreatedBy = this.UserID,
    //                        ModifiedBy = this.UserID,
    //                        IsDeleted = false,
    //                        ModifiedOn = DateTime.Now
    //                    };
    //                    DataModel.AddToCarrierNickNames(carrierNickName);
    //                }

    //                DataModel.SaveChanges();
    //                int carrierCount = DataModel.CarrierNickNames.Where(s => s.PayorId == this.PayerId && s.IsDeleted == false).ToList().Count;
    //                if (carrierCount > 1)
    //                {
    //                    DLinq.Payor payor = DataModel.Payors.FirstOrDefault(s => s.PayorId == this.PayerId && s.IsDeleted == false);
    //                    payor.PayorTypeId = 1;
    //                }
    //                DataModel.SaveChanges();
    //            }
    //        }
    //        else if (operationType.MainOperation == Operation.Upadte)
    //        {
    //            status = ValidateCarrier(DataModel, operationType);
    //            if (!status.IsError)
    //            {
    //                DLinq.Carrier carrier = DataModel.Carriers.FirstOrDefault(s => s.CarrierId == this.CarrierId);
    //                carrier.CarrierName = this.CarrierName;

    //                DLinq.CarrierNickName carrierNickname = DataModel.CarrierNickNames.FirstOrDefault(s => s.PayorId == this.PayerId && s.CarrierId == this.CarrierId);
    //                if (carrierNickname != null)
    //                {
    //                    carrierNickname.IsDeleted = false;
    //                    carrierNickname.NickName = this.NickName;
    //                    carrierNickname.IsTrackIncomingPercentage = this.IsTrackIncomingPercentage;
    //                    carrierNickname.IsTrackMissingMonth = this.IsTrackMissingMonth;
    //                    carrierNickname.CreatedBy = this.UserID;
    //                    carrierNickname.ModifiedBy = this.UserID;
    //                    carrierNickname.ModifiedOn = DateTime.Now;
    //                }
    //                else
    //                {
    //                    carrierNickname = new DLinq.CarrierNickName
    //                    {
    //                        PayorId = this.PayerId,
    //                        CarrierId = this.CarrierId,
    //                        NickName = this.NickName,
    //                        IsTrackIncomingPercentage = this.IsTrackIncomingPercentage,
    //                        IsTrackMissingMonth = this.IsTrackMissingMonth,
    //                        CreatedBy = this.UserID,
    //                        ModifiedBy = this.UserID,
    //                        IsDeleted = false,
    //                        ModifiedOn = DateTime.Now
    //                    };
    //                    DataModel.AddToCarrierNickNames(carrierNickname);
    //                }
    //                DataModel.SaveChanges();
    //            }
    //        }
    //        else if (operationType.MainOperation == Operation.Delete)
    //        {
    //            status = ValidateCarrier(DataModel, operationType);
    //            if (!status.IsError)
    //            {
    //                DLinq.CarrierNickName carrierNickName = (from c in DataModel.CarrierNickNames
    //                                                         where (c.CarrierId == this.CarrierId && c.PayorId == this.PayerId && c.IsDeleted == false)
    //                                                         select c).FirstOrDefault();

    //                if (carrierNickName != null && carrierNickName.PayorId != null)
    //                {
    //                    carrierNickName.IsDeleted = true;
    //                    DataModel.SaveChanges();
    //                }

    //                int carrierNickNameExist = (from c in DataModel.CarrierNickNames
    //                                            where (c.CarrierId == this.CarrierId && c.IsDeleted == false)
    //                                            select c).Count();
    //                if (carrierNickNameExist == 0)
    //                {
    //                    DLinq.Carrier carrier = DataModel.Carriers.FirstOrDefault(s => s.CarrierId == this.CarrierId);
    //                    carrier.IsDeleted = true;
    //                    status.IsCarrierOrCoverageRemoved = true;
    //                    DataModel.SaveChanges();
    //                }
    //            }
    //        }
    //        else
    //        {
    //            status = new ReturnStatus { ErrorMessage = "Undefine payor operation.", IsError = true };
    //        }

    //        return status;
    //    }
    //}

    #endregion
    /// <summary>
    /// 
    /// </summary>
    /// <param name="?"></param>
    ///GetCarriers(all/all of a licensee/all viewable to a user/all of a given search criteria/)
    public static List<Carrier> GetCarriers(Guid LicenseeId)
    {
      using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
      {
        if (LicenseeId != Guid.Empty)
        {
          return (from c in DataModel.Carriers
                  where (c.IsDeleted == false) && ((c.LicenseeId == LicenseeId) || (c.IsGlobal == true))
                  orderby c.CarrierName
                  select new Carrier
                  {
                    CarrierId = c.CarrierId,
                    CarrierName = c.CarrierName,
                    IsGlobal = c.IsGlobal,
                    LicenseeId = c.LicenseeId ?? Guid.Empty,
                    UserID = c.CreatedBy.Value
                  }).ToList();

        }
        else
        {
          return (from c in DataModel.Carriers
                  where (c.IsDeleted == false) && (c.IsGlobal == true)
                  orderby c.CarrierName
                  select new Carrier
                  {
                    CarrierId = c.CarrierId,
                    CarrierName = c.CarrierName,
                    IsGlobal = c.IsGlobal,
                    LicenseeId = c.LicenseeId ?? Guid.Empty,
                    UserID = c.CreatedBy.Value
                  }).ToList();
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="?"></param>
    ///GetCarriers(all/all of a licensee/all viewable to a user/all of a given search criteria/)
    public static List<Carrier> GetCarriers(Guid LicenseeId, bool isCoveragesRequired)
    {
      using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
      {
        List<Carrier> carriers;
        if (LicenseeId != Guid.Empty)
        {
          carriers = (from c in DataModel.Carriers
                      where (c.IsDeleted == false) && ((c.LicenseeId == LicenseeId) || (c.IsGlobal == true))
                      orderby c.CarrierName
                      select new Carrier
                      {
                        CarrierId = c.CarrierId,
                        CarrierName = c.CarrierName,
                        IsGlobal = c.IsGlobal,
                        LicenseeId = c.LicenseeId ?? Guid.Empty,
                        UserID = c.CreatedBy.Value
                      }).ToList();

        }
        else
        {
          carriers = (from c in DataModel.Carriers
                      where (c.IsDeleted == false) && (c.IsGlobal == true)
                      orderby c.CarrierName
                      select new Carrier
                      {
                        CarrierId = c.CarrierId,
                        CarrierName = c.CarrierName,
                        IsGlobal = c.IsGlobal,
                        LicenseeId = c.LicenseeId ?? Guid.Empty,
                        UserID = c.CreatedBy.Value
                      }).ToList();
        }


        foreach (Carrier carrier in carriers)
        {
          if (isCoveragesRequired)
            carrier.Coverages = Coverage.GetCarrierCoverages(carrier.CarrierId);
          else
            carrier.Coverages = null;
        }

        return carriers;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="?"></param>
    ///GetCarriers(all/all of a licensee/all viewable to a user/all of a given search criteria/)
    public static List<DisplayedCarrier> GetDispalyedCarriers(Guid LicenseeId, bool isCoveragesRequired)
    {
      using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
      {
        List<DisplayedCarrier> carriers;
        if (LicenseeId != Guid.Empty)
        {
          carriers = (from c in DataModel.Carriers
                      where (c.IsDeleted == false) && ((c.LicenseeId == LicenseeId) || (c.IsGlobal == true))
                      orderby c.CarrierName
                      select new DisplayedCarrier
                      {
                        CarrierId = c.CarrierId,
                        CarrierName = c.CarrierName,
                        IsGlobal = c.IsGlobal
                      }).ToList();

        }
        else
        {
          carriers = (from c in DataModel.Carriers
                      where (c.IsDeleted == false) && (c.IsGlobal == true)
                      orderby c.CarrierName
                      select new DisplayedCarrier
                      {
                        CarrierId = c.CarrierId,
                        CarrierName = c.CarrierName,
                        IsGlobal = c.IsGlobal
                      }).ToList();
        }


        foreach (DisplayedCarrier carrier in carriers)
        {
          if (isCoveragesRequired)
            carrier.Coverages = Coverage.GetCarrierCoverages(carrier.CarrierId);
          else
            carrier.Coverages = null;
        }

        return carriers;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PayorId"></param>
    /// <returns></returns>
    public static List<Carrier> GetPayorCarriers(Guid PayorId)
    {
      using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
      {
        List<Carrier> CarrierLst = (from c in DataModel.CarrierNickNames
                                    where (c.IsDeleted == false) && (c.PayorId == PayorId)
                                    orderby c.Carrier.CarrierName
                                    select new Carrier
                                    {
                                      CarrierId = c.CarrierId,
                                      PayerId = c.PayorId,
                                      CarrierName = c.Carrier.CarrierName,
                                      NickName = c.NickName,
                                      IsTrackMissingMonth = c.IsTrackMissingMonth,
                                      IsTrackIncomingPercentage = c.IsTrackIncomingPercentage,
                                      IsDeleted = c.IsDeleted.Value,
                                      IsGlobal = c.Carrier.IsGlobal,
                                      LicenseeId = c.Carrier.LicenseeId ?? Guid.Empty,
                                      UserID = (Guid)c.CreatedBy,
                                      // Coverages = null
                                    }).ToList();

        return CarrierLst;
      }
    }
    public static List<Guid> PayorCarrierGlobal(List<Guid> PayorList)
    {
      using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
      {
        List<Guid> PayorIdList = new List<Guid>();
        var globalPayorList = (from carriernickname in DataModel.CarrierNickNames
                               where carriernickname.IsDeleted == false && PayorList.Contains(carriernickname.PayorId)
                               group carriernickname by new { carriernickname.PayorId } into g
                               where g.Count(p => p.CarrierId != null) > 1
                               select new { g.Key.PayorId }).ToList();
        foreach(var listitem in globalPayorList)
        {
          PayorIdList.Add(listitem.PayorId);
        }
        return PayorIdList;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PayorId"></param>
    /// <returns></returns>
    public static List<Carrier> GetPayorCarriers(Guid PayorId, bool isCoveragesRequired)
    {
      using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
      {
        List<Carrier> carriers = (from c in DataModel.CarrierNickNames
                                  where (c.IsDeleted == false) && (c.PayorId == PayorId)
                                  orderby c.Carrier.CarrierName
                                  select new Carrier
                                  {
                                    CarrierId = c.CarrierId,
                                    PayerId = c.PayorId,
                                    CarrierName = c.Carrier.CarrierName,
                                    NickName = c.NickName,
                                    IsTrackMissingMonth = c.IsTrackMissingMonth,
                                    IsTrackIncomingPercentage = c.IsTrackIncomingPercentage,
                                    IsDeleted = c.IsDeleted.Value,
                                    IsGlobal = c.Carrier.IsGlobal,
                                    LicenseeId = c.Carrier.LicenseeId ?? Guid.Empty,
                                    UserID = (Guid)c.CreatedBy
                                  }).ToList();



        foreach (Carrier car in carriers)
        {
          if (isCoveragesRequired)
            car.Coverages = Coverage.GetCarrierCoverages(PayorId, car.CarrierId);
          else
            car.Coverages = null;
        }

        return carriers;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PayorId"></param>
    /// <param name="CarrierId"></param>
    /// <returns></returns>
    public static Carrier GetPayorCarrier(Guid PayorId, Guid CarrierId)
    {
      if (PayorId == Guid.Empty || CarrierId == Guid.Empty)
        return null;

      using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
      {
        return (from c in DataModel.CarrierNickNames
                where (c.CarrierId == CarrierId) && (c.PayorId == PayorId) && (c.IsDeleted == false)
                select new Carrier
                {
                  CarrierId = c.CarrierId,
                  PayerId = c.PayorId,
                  CarrierName = c.Carrier.CarrierName,
                  NickName = c.NickName,
                  IsDeleted = c.IsDeleted.Value,
                  IsGlobal = c.Carrier.IsGlobal,
                  LicenseeId = c.Carrier.LicenseeId,
                  IsTrackMissingMonth = c.IsTrackMissingMonth,
                  IsTrackIncomingPercentage = c.IsTrackIncomingPercentage,
                  UserID = (Guid)c.CreatedBy
                }).ToList().FirstOrDefault();
      }
    }


    public static bool IsValidCarrier(string carrierNickName, Guid payorId)
    {
      using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
      {
        DLinq.CarrierNickName carrier = DataModel.CarrierNickNames.FirstOrDefault(s => s.NickName.ToUpper() == carrierNickName.ToUpper() && s.PayorId == payorId && s.IsDeleted == false);

        if (carrier != null)
          return true;
        else
          return false;
      }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PayorId"></param>
    /// <param name="CarrierId"></param>
    /// <returns></returns>
    public static string GetCarrierNickName(Guid PayorId, Guid CarrierId)
    {
      if (PayorId == Guid.Empty || CarrierId == Guid.Empty)
        return string.Empty;

      using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
      {
        string nickName = string.Empty;
        DLinq.CarrierNickName payorcarrier = DataModel.CarrierNickNames.FirstOrDefault(s => s.PayorId == PayorId && s.CarrierId == CarrierId && s.IsDeleted == false);
        if (payorcarrier != null)
        {
          nickName = payorcarrier.NickName;
        }
        return nickName;
      }

    }

    public static string GetSingleCarrier(Guid PayorId)
    {
        if (PayorId == Guid.Empty)
            return string.Empty;

        using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        {
            string nickName = string.Empty;
            DLinq.CarrierNickName payorcarrier = DataModel.CarrierNickNames.FirstOrDefault(s => s.PayorId == PayorId && s.IsDeleted == false);
            if (payorcarrier != null)
            {
                
                
            }
            return nickName;
        }

    }

    /// <summary>
    /// This function is used to validate the carrier for system.
    /// </summary>
    /// <returns></returns>
    private ReturnStatus ValidateCarrier(DLinq.CommissionDepartmentEntities DataModel, OperationSet operation)
    {
      ReturnStatus retStatus = new ReturnStatus();

      if (operation.MainOperation == Operation.Add)
      {
        List<DLinq.CarrierNickName> payorCarriers = DataModel.CarrierNickNames.Where(s => s.PayorId == this.PayerId && s.IsDeleted == false).ToList();

        if (payorCarriers == null && payorCarriers.Count == 0)
          return retStatus;
        else
        {
          DLinq.CarrierNickName payorCarrier = payorCarriers.FirstOrDefault(s => s.Carrier.CarrierName == this.CarrierName && s.IsDeleted == false);
          if (payorCarrier != null)
          {
            retStatus.IsError = true;
            retStatus.ErrorMessage = "This carrier is already present. Please select other.";
          }
        }
      }

      if (operation.NickNameOperation == Operation.Add)
      {
        List<DLinq.CarrierNickName> payorCarriers = DataModel.CarrierNickNames.Where(s => s.PayorId == this.PayerId && s.IsDeleted == false).ToList();
        //List<DLinq.CarrierNickName> payorCarriers = DataModel.CarrierNickNames.Where(s=>s.IsDeleted == false).ToList();

        if (payorCarriers == null && payorCarriers.Count == 0)
          return retStatus;
        else
        {
          DLinq.CarrierNickName payorCarrier = payorCarriers.FirstOrDefault(s => s.NickName == this.NickName && s.IsDeleted == false);
          if (payorCarrier != null)
          {
            retStatus.IsError = true;
            retStatus.ErrorMessage = "This carrier nickname is already present. Please select other.";
          }
        }
      }
      else if (operation.NickNameOperation == Operation.Upadte)
      {
        int count = DataModel.CarrierNickNames.Where(s => s.NickName == this.NickName && s.CarrierId != operation.PreviousCarrierId && s.PayorId == this.PayerId && s.IsDeleted == false).ToList().Count;
        //int count = DataModel.CarrierNickNames.Where(s => s.NickName == this.NickName && s.IsDeleted == false).ToList().Count;
        if (count != 0)
        {
          retStatus.IsError = true;
          retStatus.ErrorMessage = "This carrier nickname is already present. Please select other.";
        }
      }
      else if (operation.NickNameOperation == Operation.Delete)
      {
        DLinq.Policy policy = (from po in DataModel.Policies
                               where (po.CarrierId == this.CarrierId
                               && po.PayorId == this.PayerId
                               && po.IsDeleted == false)
                               select po).FirstOrDefault();
        if (policy != null)
        {
          retStatus.IsError = true;
          retStatus.ErrorMessage = "Some policy refer this carrier. You can not delete carrier without deleting all the policies that refer this carrier.";
        }
      }

      return retStatus;
    }


        #region BG Methods 

        public static List<CarrierObject> GetCarriersOnDate(DateTime dtStart, DateTime dtEnd)
        {
            List<CarrierObject> objList = new List<CarrierObject>();
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetCarriersOnDate starts:  ", true);
                if (dtStart == DateTime.MinValue && dtEnd == DateTime.MinValue)
                {
                    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                    {
                        var carr = (from pl in DataModel.CarrierNickNames
                                    where pl.IsDeleted != true
                                    select new { pl.CarrierId, pl.NickName, pl.PayorId }).ToList();

                        foreach (var c in carr)
                        {
                            CarrierObject objCarr = new CarrierObject();
                            objCarr.CarrierID = c.CarrierId;
                            objCarr.PayorID = c.PayorId;
                            objCarr.CarrierName = c.NickName;
                            objList.Add(objCarr);
                        }
                    }
                }
                else
                {
                    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                    {
                        var carr = (from pl in DataModel.CarrierNickNames
                                    where pl.IsDeleted != true && (pl.CreatedOn.HasValue && EntityFunctions.TruncateTime(pl.CreatedOn) > dtStart && EntityFunctions.TruncateTime(pl.CreatedOn) <= dtEnd)
                                    select new { pl.CarrierId, pl.NickName, pl.PayorId }).ToList();

                        foreach (var c in carr)
                        {
                            CarrierObject objCarr = new CarrierObject();
                            objCarr.CarrierID = c.CarrierId;
                            objCarr.PayorID = c.PayorId;
                            objCarr.CarrierName = c.NickName;
                            objList.Add(objCarr);
                        }
                    }
                }
                ActionLogger.Logger.WriteImportPolicyLog("GetCarriersOnDate completed:  records -  " + objList.Count, true);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetCarriersOnDate exception:  " + ex.Message, true);
            }
            return objList;
        }


        public static List<PayorCarrierObject> GetCarriersOnPayor(DateTime dtStart, DateTime dtEnd, List<string> payorIDs, out List<string> invalidPayors)
        {
            //  List<CarrierObject> objList = new List<CarrierObject>();
            List<PayorCarrierObject> list = new List<PayorCarrierObject>();
            invalidPayors = new List<string>();
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetCarriersOnDate starts:  ", true);

                //if (string.IsNullOrEmpty(payorIDs))
                //{
                //    ActionLogger.Logger.WriteImportPolicyLog("Payor IDs received blank, returning...", true);
                //    return null;
                //}

                List<string> payorList = payorIDs; //payorIDs.Split(",").ToList();

                foreach (string payor in payorList)
                {
                    PayorCarrierObject objList = new PayorCarrierObject();
                    objList.Carriers = new List<CarrierIDName>();
                    
                    try
                    {
                        Guid payorID = new Guid(payor);
                        objList.PayorID = payorID;
                        
                        if (dtStart == DateTime.MinValue && dtEnd == DateTime.MinValue)
                        {
                            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                            {
                                var carr = (from carrier in DataModel.CarrierNickNames
                                            join car in DataModel.Carriers  on carrier.CarrierId equals car.CarrierId
                                            where carrier.IsDeleted != true && carrier.PayorId == payorID
                                            select new { carrier.CarrierId, carrier.NickName, carrier.PayorId, car.CarrierName }).ToList();

                                foreach (var c in carr)
                                {
                                    CarrierIDName objCarr = new CarrierIDName();
                                    objCarr.CarrierID = c.CarrierId;
                                    //objCarr.PayorID = c.PayorId;
                                    objCarr.CarrierName = c.CarrierName;
                                    objCarr.CarrierNickName = c.NickName;
                                    objList.Carriers.Add(objCarr);
                                }
                            }
                        }
                        else
                        {
                            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                            {
                                var carr = (from pl in DataModel.CarrierNickNames
                                            join car in DataModel.Carriers on pl.CarrierId equals car.CarrierId
                                            where pl.IsDeleted != true &&
                                             pl.PayorId == payorID &&
                                            (pl.CreatedOn.HasValue && EntityFunctions.TruncateTime(pl.CreatedOn) > dtStart && EntityFunctions.TruncateTime(pl.CreatedOn) <= dtEnd)
                                            select new { pl.CarrierId, pl.NickName, pl.PayorId, car.CarrierName }).ToList();

                                foreach (var c in carr)
                                {
                                    CarrierIDName objCarr = new CarrierIDName();
                                    objCarr.CarrierID = c.CarrierId;
                                    //objCarr.PayorID = c.PayorId;
                                    objCarr.CarrierName = c.CarrierName;
                                    objCarr.CarrierNickName = c.NickName;
                                    objList.Carriers.Add(objCarr);
                                }
                            }
                        }

                        list.Add(objList);
                    }

                    catch (Exception ex)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("GetCarriersOnDate exception for payorID:  " + payor + ", ex: " + ex.Message, true);
                        invalidPayors.Add(payor);
                    }

                    ActionLogger.Logger.WriteImportPolicyLog("GetCarriersOnDate completed:  records -  " + list.Count, true);
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetCarriersOnDate exception:  " + ex.Message, true);
            }
            return list;
        }
        #endregion

    }

  [DataContract]
  public class DisplayedCarrier
  {
    [DataMember]
    public Guid CarrierId { get; set; }
    [DataMember]
    public string CarrierName { get; set; }
    [DataMember]
    public bool IsGlobal { get; set; }
    [DataMember]
    public List<Coverage> Coverages { get; set; }
  }

  [DataContract]
  public class ReturnStatus
  {
    [DataMember]
    public bool IsError { get; set; }
    [DataMember]
    public string ErrorMessage { get; set; }
    [DataMember]
    public bool IsCarrierOrCoverageRemoved { get; set; }
  }
}
