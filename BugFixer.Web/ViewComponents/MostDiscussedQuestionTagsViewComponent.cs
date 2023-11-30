using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Questions;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.ViewComponents
{
    public class MostDiscussedQuestionTagsViewComponent:ViewComponent
    {
        private readonly IQuestionService _questionService;
        public MostDiscussedQuestionTagsViewComponent(IQuestionService questionService)
        {
            _questionService= questionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<QuestionTagVM> model = await _questionService.MostDiscussedQuestionTagsServiceAsync();
            return View("/Views/Components/MostDiscussedQuestionTagsComponent.cshtml", model);
        }
    }
}
