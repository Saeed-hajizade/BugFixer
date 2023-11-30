using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Questions;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.ViewComponents
{
    public class TopQuestionsViewComponent:ViewComponent
    {
        private readonly IQuestionService _questionService;

        public TopQuestionsViewComponent(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<QuestionVM> topQuestionList =await _questionService.MostDiscussedQuestionsService();
            return View("/Views/Components/TopQuestionsComponent.cshtml", topQuestionList);
        }
    }
}
