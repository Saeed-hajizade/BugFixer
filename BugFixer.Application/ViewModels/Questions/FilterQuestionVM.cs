using BugFixer.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.ViewModels.Questions
{
    public class FilterQuestionVM: BasePaging<QuestionVM>
    {
        public string OrderByType { get; set; }
    }
}
