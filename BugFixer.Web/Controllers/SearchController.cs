using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Questions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BugFixer.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IQuestionService _questionService;
        public SearchController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchParameter)
        {
            var result = await _questionService.GetQuestinsBySearchServiceAsync(searchParameter);
         
            return View(result);
        }

        //public IActionResult ResultPage()
        //{

        //    var serializedResult = TempData["SearchResult"] as string;

        //    // Deserialize the result back to its original type
        //    if(serializedResult != null)
        //    {
        //        var result = JsonConvert.DeserializeObject<List<QuestionVM>>(serializedResult);
        //    return View(result);
        //    }
        //    return View(null);
        //}
    }
}
