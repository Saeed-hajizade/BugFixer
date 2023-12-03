using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Questions;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.ViewComponents
{
    public class MostRateQuestionsViewComponent:ViewComponent
    {
        private readonly IQuestionService _questionService;

        public MostRateQuestionsViewComponent(IQuestionService questionService)
        {
                _questionService = questionService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<QuestionVM> mostRateQuestionList =await _questionService.MostDiscussedQuestionsService();
            return View("/Views/Components/MostRateQuestionsComponent.cshtml", mostRateQuestionList);
        }
    }
}
