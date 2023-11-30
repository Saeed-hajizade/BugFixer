using BugFixer.Application.ViewModels.User;
using BugFixer.Domain.Models.Questions;
using BugFixer.Domain.Models.User;
using System.ComponentModel.DataAnnotations;

namespace BugFixer.Application.ViewModels.Questions
{
    public class QuestionVM : BaseVM.BaseVM
    {
        [Display(Name = "عنوان سوال")]
        [MaxLength(400, ErrorMessage = "تعداد کاراکتر های {0}بیش از {1}کاراکتر است")]
        public string? Title { get; set; }
        [Display(Name = "متن سوال")]
        public string? Text { get; set; }
        public int? Visit { get; set; } = 0;
        public int Rate { get; set; }
        public int NumberOfAnswers { get; set; }
        public TrueAnswerVM? TrueAnswer { get; set; }
        #region Relations
        public Domain.Models.User.User? User { get; set; }
        public IEnumerable<QuestionTagVM>? QuestionTags { get; set; }
        public IEnumerable<Answer>? Answers { get; set; }
        public IEnumerable<QuestionRateVM>? QuestionRates  { get; set; }
        #endregion
    }
}
