using BugFixer.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.ViewModels.Questions
{
    public class QuestionTagVM:BaseVM.BaseVM
    {
        [Display(Name = "تگ")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0}بیش از {1}کاراکتر است")]
        [Required(ErrorMessage = "{0}الزامی می باشد")]
        public string? Tag { get; set; }
        public int? QuestionId { get; set; }


        #region Relations
        public QuestionVM? Question { get; set; }
        #endregion
    }
}
