using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.ViewModels.Questions
{
    public class UserPanelAnswerVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Visits { get; set; }
        public int RatesCount { get; set; }
        public int AnswersCount { get; set; }
        public string LastAnswerUsername { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
