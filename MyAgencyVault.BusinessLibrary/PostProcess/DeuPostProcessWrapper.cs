using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Threading;

namespace MyAgencyVault.BusinessLibrary.PostProcess
{
    public class DeuPostProcessWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_PostEntryProcess"></param>
        /// <param name="deuFields"></param>
        /// <param name="deuEntryId">
        /// FirstPost = Guid.Empty
        /// RePort = OldDeuEntryId
        /// DeletePost = DeuEntryId
        /// </param>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public static PostProcessReturnStatus DeuPostStartWrapper(PostEntryProcess _PostEntryProcess, DEUFields deuFields, Guid deuEntryId, Guid userId, UserRole userRole)
        {
            if (deuFields != null)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper  request deuFields: " + deuFields.ToStringDump() + ", user: " + userId, true);
            }
            ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper  request deuEntryId: " + deuEntryId + ", user: " + userId, true);

            //bool lockObtained = false;
            DEUFields tempDeuFields = null;
            BasicInformationForProcess _BasicInformationForProcess = null;

            //var options = new TransactionOptions
            //{
            //    IsolationLevel = IsolationLevel.ReadUncommitted,
            //    Timeout = TimeSpan.FromMinutes(60)
            //};

            PostProcessReturnStatus _PostProcessReturnStatus = null;
            try
            {
                //using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, options))
                //{
                    if (_PostEntryProcess == PostEntryProcess.FirstPost || _PostEntryProcess == PostEntryProcess.RePost)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper FirstPost or RePost  1:", true);
                        ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper FirstPost or RePost  1:deuEntryId: "+ deuEntryId, true);
                        DEU objDeu = new DEU();
                        ModifiyableBatchStatementData batchStatementData = objDeu.AddUpdate(deuFields, deuEntryId);

                        //Check null before assingn the value
                        if (batchStatementData == null)
                            return _PostProcessReturnStatus;
                        //Check null before assingn the value
                        if (batchStatementData.ExposedDeu == null)
                            return _PostProcessReturnStatus;
                        //Check null before assingn the value
                        if (batchStatementData.ExposedDeu.DEUENtryID == null)
                            return _PostProcessReturnStatus;
                        //Assin deuentry ID
                        deuFields.DeuEntryId = batchStatementData.ExposedDeu.DEUENtryID;
                        ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper FirstPost or RePost  1:deuFields.deuEntryId: " + deuFields.DeuEntryId, true);
                       // ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper FirstPost or RePost  2:", true);

                        if (deuFields != null)
                        {
                            if (deuFields.DeuEntryId != null)
                            {
                                tempDeuFields = PostUtill.FillDEUFields(deuFields.DeuEntryId);
                            }
                        }

                        if (deuEntryId != Guid.Empty)
                        {
                            tempDeuFields = PostUtill.FillDEUFields(deuEntryId);
                            _BasicInformationForProcess = PostUtill.GetPolicyToProcess(tempDeuFields, string.Empty);
                        }
                       // ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper FirstPost or RePost 3:", true);
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper  delete request", true);
                        //ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper delete 4:", true);
                        try
                        {
                            tempDeuFields = PostUtill.FillDEUFields(deuEntryId);
                            //_BasicInformationForProcess = PostUtill.GetPolicyToProcess(tempDeuFields, string.Empty);
                        }
                        catch(Exception ex)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper exception: " + ex.Message, true);
                        }
                       // ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper delete 5:", true);
                    }

                    _PostProcessReturnStatus = new PostProcessReturnStatus() { DeuEntryId = Guid.Empty, IsComplete = false, ErrorMessage = null, PostEntryStatus = _PostEntryProcess };

                    if (_PostEntryProcess == PostEntryProcess.FirstPost)
                    {
                        //ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper FirstPost 6:", true);

                        _PostProcessReturnStatus = PostUtill.PostStart(_PostEntryProcess, deuFields.DeuEntryId, deuEntryId, userId, userRole, _PostEntryProcess, string.Empty, string.Empty);

                        _PostProcessReturnStatus.DeuEntryId = deuFields.DeuEntryId;
                        _PostProcessReturnStatus.OldDeuEntryId = Guid.Empty;
                        _PostProcessReturnStatus.ReferenceNo = deuFields.ReferenceNo;
                        //ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper FirstPost 7:", true);

                    }
                    else if (_PostEntryProcess == PostEntryProcess.RePost)
                    {
                        //ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper Repost 8:", true);
                        _PostProcessReturnStatus = PostUtill.PostStart(_PostEntryProcess, deuEntryId, deuFields.DeuEntryId, userId, userRole, _PostEntryProcess, string.Empty, string.Empty);
                        _PostProcessReturnStatus.DeuEntryId = deuFields.DeuEntryId;
                        _PostProcessReturnStatus.OldDeuEntryId = deuEntryId;
                        _PostProcessReturnStatus.ReferenceNo = deuFields.ReferenceNo;
                        //ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper Repost 9:", true);
                    }
                    else
                    {
                        //ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper Delete 10:", true);
                        _PostProcessReturnStatus = PostUtill.PostStart(_PostEntryProcess, deuEntryId, Guid.Empty, userId, userRole, _PostEntryProcess, string.Empty, string.Empty);
                        _PostProcessReturnStatus.DeuEntryId = deuEntryId;
                        _PostProcessReturnStatus.OldDeuEntryId = Guid.Empty;
                        //ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper Delete 11:", true);
                    }
                    if (_PostProcessReturnStatus.IsComplete)
                    {
                        //ts.Complete();
                        //ActionLogger.Logger.WriteImportPolicyLog("Sucess commited:", true);
                    }
                    else
                    {
                        //ActionLogger.Logger.WriteImportPolicyLog("Failed need to rollback:", true);
                        if (deuFields != null)
                        {
                            //ActionLogger.Logger.WriteImportPolicyLog("Rollback id:" + deuFields.DeuEntryId.ToString(), true);
                        }
                    }
               // }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeuPostStartWrapper:" + ex.Message.ToString(), true);
            }
            finally
            {
            }

            return _PostProcessReturnStatus;
        }
    }
}
