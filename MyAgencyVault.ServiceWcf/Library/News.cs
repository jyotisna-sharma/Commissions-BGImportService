using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface INews
    {
        #region IEditable<News> Members
        [OperationContract]
        void AddUpdateNews(News Nws);
        [OperationContract]
        void DeleteNews(News Nws);
        [OperationContract]
        News GetNews();
        [OperationContract]
        bool IsValidNews(News Nws);
        #endregion

        [OperationContract]
       List<News> GetNewsList();
    }
    public partial class MavService : INews
    {
        public void AddUpdateNews(News Nws)
        {
            Nws.AddUpdate();
        }

        public void DeleteNews(News Nws)
        {
            Nws.Delete() ;
        }

        public News GetNews()
        {
            throw new NotImplementedException();
        }

        public bool IsValidNews(News Nws)
        {
            return Nws.IsValid() ;
        }

        public List<News> GetNewsList()
        {
            return News.GetNewsList();
        }
    }
}
