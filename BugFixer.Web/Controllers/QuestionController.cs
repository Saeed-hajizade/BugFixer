using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Questions;
using BugFixer.Domain.Models.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugFixer.Web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        #region Constructor
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        #endregion
        #region Actions
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("submit-question")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost("submit-question")]
        [Authorize]

        public async Task<IActionResult> Create(CreateQuestionVM create)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ModelState.IsValid)
            {
                return View(create);
            }
            await _questionService.CreateQuestionServiceAsync(create, userId);
            return Redirect("/");
        }






        [HttpGet("show-question/{id}")]
        public async Task<IActionResult> ShowQuestion(int id, int pageId = 1, string orderType = "")
        {
            QuestionVM question = await _questionService.GetQuestionServiceAsync(id);
            await _questionService.UpdteQuestionVisitService(id);

            FilterQuestionAswersVM filter = new FilterQuestionAswersVM()
            {
                Page = pageId,
                OrderType = orderType,


            };


            ViewBag.Answers = await _questionService.QuestionAnswersFilter(filter, id);

            return View(question);
        }


        [HttpPost("show-question/{id}")]
        [Authorize]
        public async Task<IActionResult> ShowQuestion(string AnswerText, int QuestionId, int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            await _questionService.CreateAnswerServiceAsync(AnswerText, QuestionId, userId);
            return Redirect($"/show-question/{id}");
        }




        #region Edit Answer
        [HttpGet("edit-answer/{id}/{questionId}")]
        public async Task<IActionResult> EditAnswer(int id, int questionId)
        {
            UpdateAnswerVM answer = await _questionService.GetAnswerForUpdateServiceAsync(id);
            return View(answer);
        }

        [HttpPost("edit-answer/{id}/{questionId}")]
        public async Task<IActionResult> EditAnswer(int id, UpdateAnswerVM updateAnswer, int questionId)
        {
            if (!ModelState.IsValid)
            {
                return View(updateAnswer);
            }

            await _questionService.UpdateAnswerService(updateAnswer);
            return Redirect($"/show-question/{questionId}");

        }
        #endregion



        #region QuestionRate

        [HttpPost("/add-rate/{questionId}")]
        [Authorize]
        public async Task<IActionResult> AddRate(int questionId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var rateStatus = await _questionService.HandleQuestionRateServiceAsync(questionId, userId);
            return Json(new { rateStatus = rateStatus });
        }

        #endregion

        #region TrueAnswer
        [HttpPost("/true-answer/{questionID}/{answerID}")]
        [Authorize]
        public async Task<IActionResult> AddTrueAnswer(int questionID, int answerID)
        {
            await _questionService.HandleTrueAnswerServiceAsync(questionID, answerID);
            return Json(new { taStatus = true });
        }

        #endregion

        #endregion
    }
}
