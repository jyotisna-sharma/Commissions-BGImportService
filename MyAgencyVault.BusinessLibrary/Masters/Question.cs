using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class Question
    {
        #region "public properties"
        [DataMember]
        public int QuestionId { get; set; }
        [DataMember]
        public string QuestionText { get; set; }        
        #endregion 
        public static List<Question> GetQuestions()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from q in DataModel.MasterPasswordHintQuestions
                        select new Question
                        {
                            QuestionText = q.Qestion,
                            QuestionId = q.PHQuestionId
                        }).ToList();
            }
        }
    }
}
