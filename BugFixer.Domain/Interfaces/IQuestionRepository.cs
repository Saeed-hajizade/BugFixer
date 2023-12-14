using BugFixer.Domain.Models.Questions;

namespace BugFixer.Domain.Interfaces
{
    public interface IQuestionRepository
    {
        Task SavechangeAsync();


        #region Question Methods
        Task<IEnumerable<Question>> GetQuestionsAsync();
        IQueryable<Question> GetQuestionsQueryable();
        Task<Question> GetQuestionAsync(int id);
        Task CreateQuestionAsync(Question quesion);

        Task CreateQuestionTagAsync(QuestionTag questionTag);
        void UpdateQuestion(Question question);
        Task<int> GetUserQuestionsCountAsync(int userId);
        Task<List<Question>> GetQuestinsBySearchAsync(string search);
        Task<IEnumerable<Question>> TopRatedQuestions();
        Task<IEnumerable<Question>> MostDiscussedQuestions();
        Task<IEnumerable<QuestionTag>> MostDiscussedQuestionTagsAsync();
        Task<int> GetQuestionsCountAsync();

        #endregion

        #region Answer Methods
        Task CreateAnswerAsync(Answer answer);
        IQueryable<Answer> QuestionAnswersQueryable(int id);
        void UpdateAnswer(Answer answer);
        Task<Answer> GetAnswerById(int id);
        Task<int> GetUserAnswersCountAsync(int userId);
        Task<List<Answer>> GetUserAnswersAsync(int userId);

        #endregion

        #region Profile
        Task<IEnumerable<Question>> ProfileSelectedQuestionsAsync(int id);

        Task<IEnumerable<Answer>> ProfileSelectedAswersAsync(int id);

        #endregion

        #region UserPanel      
        Task<List<Question>> GetUserQuestionsAsync(int userId);
        #endregion

    }
}
