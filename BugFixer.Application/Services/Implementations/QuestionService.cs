using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Questions;
using BugFixer.Domain.Interfaces;
using BugFixer.Domain.Models.Questions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace BugFixer.Application.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionRateRepository _questionRateRepository;
        private readonly ITrueAnswerRepository _trueAnswerRepository;
        public QuestionService(IQuestionRepository questionRepository, IQuestionRateRepository questionRateRepository, ITrueAnswerRepository trueAnswerRepository)
        {
            _questionRepository = questionRepository;
            _questionRateRepository = questionRateRepository;
            _trueAnswerRepository = trueAnswerRepository;   
        }



        #region Question Methods
        public async Task CreateQuestionServiceAsync(CreateQuestionVM quesion, int userId)
        {
            Question newQuestion = new Question()
            {
                Title = quesion.Title,
                Text = quesion.Text,
                UserId = userId

            };

            await _questionRepository.CreateQuestionAsync(newQuestion);
            await _questionRepository.SavechangeAsync();

            List<string> tags = quesion.Tag.Split('-', StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (string tag in tags)
            {
                QuestionTag newTag = new QuestionTag()
                {
                    Tag = tag,
                    QuestionId = newQuestion.Id

                };

                await _questionRepository.CreateQuestionTagAsync(newTag);
            }
            await _questionRepository.SavechangeAsync();

        }

        public async Task<QuestionVM> GetQuestionServiceAsync(int id)
        {
            Question question = await _questionRepository.GetQuestionAsync(id);

            return new QuestionVM()
            {
                Id = question.Id,
                Title = question.Title,
                Text = question.Text,
                CreateDate = question.CreateDate,
                Visit = question.Visit,
                TrueAnswer= new TrueAnswerVM { 
                    QuestionId = question.TrueAnswer?.QuestionId != null ? question.TrueAnswer.QuestionId : 0,
                    AnswerId = question.TrueAnswer?.AnswerId !=null? question.TrueAnswer.AnswerId:0
                },
                QuestionTags = question.QuestionTags?.Select(q => new QuestionTagVM()
                {
                    Tag = q.Tag,
                }).ToList(),

                User = question.User,
                Answers = question.Answers,
                QuestionRates = question.QuestionRates?.Select(r => new QuestionRateVM
                {
                    QuestionId = r.QuestionId,
                    UserId=r.UserId
                })
            };
        }

        public async Task<IEnumerable<QuestionVM>> GetQuestionsServiceAsync()
        {
            IEnumerable<Question> questionList = await _questionRepository.GetQuestionsAsync();


            return questionList.Select(q => new QuestionVM()
            {
                Id = q.Id,
                Title = q.Title,
                User = q.User,
                Answers = q.Answers,
                Visit= q.Visit,
                QuestionTags = q.QuestionTags.Select(a => new QuestionTagVM()
                {
                    Tag = a.Tag,
                }),
                Rate=q.QuestionRates.Count()
            }).ToList();
        }
        public async Task<FilterQuestionVM> FilterQuestionsAsync(FilterQuestionVM filterQuestionVM)
        {
            IQueryable<Question> query = _questionRepository.GetQuestionsQueryable();

            switch (filterQuestionVM.OrderType)
            {
                case "New":
                    query = query.OrderBy(q => q.CreateDate);
                    break;
                case "MostControversial":
                    query = query.OrderBy(q => q.Answers.Count());
                    break;
                case "MostOutstanding":
                    query = query.OrderBy(q => q.QuestionRates.Count());
                    break;
                case "WeekAgo":
                    DateTime weeAgoTime = DateTime.Today.AddDays(-7);
                    query = query.OrderBy(q=> q.CreateDate < DateTime.Now && q.CreateDate >=weeAgoTime);
                    break;
                case "MonthAgo":
                    DateTime monthAgoTime = DateTime.Today.AddMonths(-1);
                    query = query.OrderBy(q => q.CreateDate < DateTime.Now && q.CreateDate >= monthAgoTime);

                    break;

                default:
                    break;

            }

     

            await filterQuestionVM.Paging(query);
            return filterQuestionVM;
        }
        public async Task UpdteQuestionVisitService(int questionId)
        {
            Question question = await _questionRepository.GetQuestionAsync(questionId);

            question.Visit += 1;
            _questionRepository.UpdateQuestion(question);
            await _questionRepository.SavechangeAsync();
        }

        public async Task<QACounts> GetQACountsServiceAsync(int userId)
        {
            var qCounts = await _questionRepository.GetUserQuestionsCountAsync(userId);
            var aCounts = await _questionRepository.GetUserAnswersCountAsync(userId);

            return new QACounts
            {
                QuestionsCount = qCounts != null ? qCounts : 0,
                AnswersCount = aCounts != null ? aCounts : 0
            };
        }

        public  async Task<IEnumerable<QuestionVM>> TopRatedQuestionsService()
        {
            IEnumerable<Question> questions =await  _questionRepository.TopRatedQuestions();
            return questions.Select(q => new QuestionVM()
            {
                Rate=q.QuestionRates.Count(),
                Id=q.Id,
                Title=q.Title,
            }).ToList();
        


          
        }


        public  async Task<IEnumerable<QuestionVM>> MostDiscussedQuestionsService()
        {
            IEnumerable<Question> questions = await _questionRepository.MostDiscussedQuestions();
            return questions.Select(q => new QuestionVM()
            {
                NumberOfAnswers=!q.Answers.IsNullOrEmpty()?q.Answers.Count():0,
                Id = q.Id,
                Title = q.Title,
            }).ToList();
        }

        public async Task<IEnumerable<QuestionTagVM>> MostDiscussedQuestionTagsServiceAsync()
        {
            IEnumerable<QuestionTag> questionTags = await _questionRepository.MostDiscussedQuestionTagsAsync();
            return questionTags.Select(qt => new QuestionTagVM()
            {
                Id=qt.Id,
                Tag=qt.Tag,
            }).ToList();
        }

        #endregion


        #region Answer Methods
        public async Task CreateAnswerServiceAsync(string answerText, int questionId, int userId)
        {
            Answer answer = new Answer()
            {
                Text = answerText,
                QuestionId = questionId,
                UserId = userId
            };

            await _questionRepository.CreateAnswerAsync(answer);
            await _questionRepository.SavechangeAsync();
        }
        public async Task<FilterQuestionAswersVM> QuestionAnswersFilter(FilterQuestionAswersVM filter, int questionId)
        {
            IQueryable<Answer> answers = _questionRepository.QuestionAnswersQueryable(questionId);

            IQueryable<ShowQuestionAnswerVM> result = answers.Select(a => new ShowQuestionAnswerVM()
            {
                AnswerId = a.Id,
                Text = a.Text,
                CreateDate = a.CreateDate,
                SenderName = a.User.UserName,
                SenderAvatar = a.User.Avatar,
                NumberOfAnswersSender = a.User.Answers.Count,
                NumberOfQuestionSender = a.User.Questions.Count,
                SenderId = a.UserId
            }).AsQueryable();

            switch (filter.OrderType)
            {
                case "new":

                    result = result.OrderByDescending(a => a.CreateDate);
                    break;

                case "old":

                    result = result.OrderBy(a => a.CreateDate);
                    break;
                default:

                    break;




            }
            await filter.Paging(result);
            return filter;




        }
        public async Task UpdateAnswerService(UpdateAnswerVM updateAnswer)
        {
            Answer answer = await _questionRepository.GetAnswerById(updateAnswer.AnswerId);

            answer.Text = updateAnswer.Text;
            _questionRepository.UpdateAnswer(answer);
            await _questionRepository.SavechangeAsync();
        }

        public async Task<UpdateAnswerVM> GetAnswerForUpdateServiceAsync(int answerId)
        {
            Answer getAnswer = await _questionRepository.GetAnswerById(answerId);

            return new UpdateAnswerVM()
            {
                AnswerId = getAnswer.Id,
                Text = getAnswer.Text,
            };
        }

        #endregion

        #region QuestionRate Methods
     
        public async Task<bool> HandleQuestionRateServiceAsync(int qID, int userID)
        {
            var questionRate = await _questionRateRepository.GetQuestionRateAsync(qID, userID);
            if (questionRate != null)
            {             
                _questionRateRepository.DeleteQuestionRate(questionRate);
                await _questionRepository.SavechangeAsync();
                return false;
            }
            else
            {
                var qr = new QuestionRate
                {
                    QuestionId = qID,
                    UserId = userID,
                };
                await _questionRateRepository.CreateQuestionRateAsync(qr);
                await _questionRepository.SavechangeAsync();
                return true;
            }
        }

        #endregion

        #region TrueAnswer
        public async Task HandleTrueAnswerServiceAsync(int qID, int aID)
        {
            var trueAsnwer = await _trueAnswerRepository.GetTrueAnswerAsync(qID);
            if(trueAsnwer != null)
            {
                if(trueAsnwer.AnswerId != aID)
                {
                    _trueAnswerRepository.DeleteTrueAnswerAsync(trueAsnwer);
                    var ta = new TrueAnswer
                    {
                        AnswerId = aID,
                        QuestionId = qID,
                    };
                    await _trueAnswerRepository.CreateTrueAnswerAsync(ta);
                    await _questionRepository.SavechangeAsync();
                }
            }
            else
            {
                var ta = new TrueAnswer
                {
                    AnswerId = aID,
                    QuestionId = qID,
                };
                await _trueAnswerRepository.CreateTrueAnswerAsync(ta);
                await _questionRepository.SavechangeAsync();
            }
        }
        #endregion

        #region Profile
        public async Task<IEnumerable<QuestionVM>> ProfileSelectedQuestionsServiceAsync(int id)
        {
            IEnumerable<Question> questions = await _questionRepository.ProfileSelectedQuestionsAsync(id);
 
            return questions.Select(q => new QuestionVM()
            {
                Title = q.Title,
                CreateDate = q.CreateDate,
            }).ToList();
        }

        public async Task<IEnumerable<AnswerVM>> ProfileSelectedAswersServiceAsync(int id)
        {
            IEnumerable<Answer> questions = await _questionRepository.ProfileSelectedAswersAsync(id);

            return questions.Select(q => new AnswerVM()
            {
                Text = q.Text,
                CreateDate = q.CreateDate,
            }).ToList();
        }

      






        #endregion
    }
}
