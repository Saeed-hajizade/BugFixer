using BugFixer.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.ViewModels.User
{
    public class FilterUsersPageVM:BasePaging<Domain.Models.User.User>
    {
        public string? UserName { get; set; }
        public string? OrderType { get; set; }
        public bool Reverse { get; set; } = false;

    }
}
