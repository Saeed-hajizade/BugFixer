using BugFixer.Application.ViewModels.Questions;
using BugFixer.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Interfaces
{
    public interface IQuestionService
    {
        #region Question Methods
        Task<IEnumerable<QuestionVM>> GetQuestionsServiceAsync();
        Task<FilterQuestionVM> FilterQuestionsAsync(FilterQuestionVM filterQuestionVM);
        Task<QuestionVM> GetQuestionServiceAsync(int id);
        Task CreateQuestionServiceAsync(CreateQuestionVM quesion, int userId);
        Task UpdteQuestionVisitService(int questionId);
        Task<QACounts> GetQACountsServiceAsync(int userId);
        Task<List<QuestionVM>> GetQuestinsBySearchServiceAsync(string search);
        Task<IEnumerable<QuestionVM>> TopRatedQuestionsService();
        Task<IEnumerable<QuestionVM>> MostDiscussedQuestionsService();
        Task<IEnumerable<QuestionTagVM>> MostDiscussedQuestionTagsServiceAsync();
        #endregion


        #region Answer Methods
        Task CreateAnswerServiceAsync(string answerText, int questionId, int userId);
        Task<FilterQuestionAswersVM> QuestionAnswersFilter(FilterQuestionAswersVM filter, int questionId);
        Task UpdateAnswerService(UpdateAnswerVM updateAnswer);
        Task<List<UserPanelAnswerVM>> GetUserAnswerServiceAsync(int userId);
        Task<UpdateAnswerVM> GetAnswerForUpdateServiceAsync(int answerId);
        Task<int> GetQuestionsCountServiceAsync();    


        #endregion

        #region QuestionRate Methods

        //Task CreateQuestionRateServiceAsync(int qID, int userID);
        //Task DeleteQuestionRateServiceAsync(int qID, int userID);

        Task<bool> HandleQuestionRateServiceAsync(int qID, int userID);

        #endregion
        #region QuestionRate Methods

        //Task CreateQuestionRateServiceAsync(int qID, int userID);
        //Task DeleteQuestionRateServiceAsync(int qID, int userID);

        Task HandleTrueAnswerServiceAsync(int qID, int aID, int userID);

        #endregion


        #region Profile
        Task<IEnumerable<QuestionVM>> ProfileSelectedQuestionsServiceAsync(int id);
        Task<IEnumerable<AnswerVM>> ProfileSelectedAswersServiceAsync(int id);

        #endregion

        #region UserPanel

        Task<List<UserPanelQuestionsVM>> GetUserQuestionsSeviceAsync(int userId);

        #endregion
    }
}
