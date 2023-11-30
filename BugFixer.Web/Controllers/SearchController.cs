using BugFixer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IQuestionService _questionService;
        public SearchController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        public async Task<IActionResult>  Index(string searchParameter)
        {
            var result = await _questionService.GetQuestinsBySearchServiceAsync(searchParameter);
            return View(result);
        }
    }
}
