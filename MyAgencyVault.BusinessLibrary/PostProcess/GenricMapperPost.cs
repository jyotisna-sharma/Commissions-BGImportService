using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace MyAgencyVault.BusinessLibrary
{
    public class GenricMapperPost
    {
        public static List<PostProcessReturnStatus> GenricMapperPostStart(List<Guid> DeuEntryIdLst, UserRole _UserRole)
        {
            if (DeuEntryIdLst == null || DeuEntryIdLst.Count == 0)
                return null;

            List<PostProcessReturnStatus> PostProcessReturnStatusLst = new List<PostProcessReturnStatus>();
            
            TransactionOptions options = new TransactionOptions();
            options.Timeout = TimeSpan.FromMinutes(2 * DeuEntryIdLst.Count);
            options.IsolationLevel = IsolationLevel.ReadCommitted;
            
            try
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, options))
                {
                    foreach (Guid idx in DeuEntryIdLst)
                    {
                        PostProcessReturnStatusLst.Add(PostUtill.PostStart(PostEntryProcess.FirstPost, idx, Guid.Empty, Guid.Empty, _UserRole, PostEntryProcess.FirstPost,string.Empty,string.Empty));
                    }
                    transaction.Complete();
                }
            }
            catch (Exception)
            {
            }

            return PostProcessReturnStatusLst;
        }
    }
}
