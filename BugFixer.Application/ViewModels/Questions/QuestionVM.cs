﻿using BugFixer.Domain.Models.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugFixer.Application.ViewModels.User;

namespace BugFixer.Application.ViewModels.Questions
{
    public class QuestionVM:BaseVM.BaseVM
    {
        [Display(Name = "عنوان سوال")]
        [MaxLength(400, ErrorMessage = "تعداد کاراکتر های {0}بیش از {1}کاراکتر است")]
        public string? Title { get; set; }
        [Display(Name = "متن سوال")]
        public string? Text { get; set; }
        public int? Visit { get; set; } = 0;


        #region Relations
        public UserVM? User { get; set; }
        public IEnumerable<QuestionTagVM>? QuestionTags { get; set; }
        public IEnumerable<AnswerVM>? Answers { get; set; }
        #endregion
    }
}