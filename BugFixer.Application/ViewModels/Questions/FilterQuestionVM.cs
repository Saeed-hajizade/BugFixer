using BugFixer.Application.ViewModels.Common;
using BugFixer.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.ViewModels.Questions
{
    public class FilterQuestionVM:BasePaging<Question>
    {

        public string OrderType { get; set; }


    }
}
