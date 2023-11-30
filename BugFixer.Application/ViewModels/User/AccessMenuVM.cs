using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.ViewModels.User
{
    public class AccessMenuVM
    {
        public int FollowersCount { get; set; }
        public int FollowingsCount { get; set; }
        public int QuestionsCount { get; set; }
        public int AnswersCount { get; set; }
    }
}
