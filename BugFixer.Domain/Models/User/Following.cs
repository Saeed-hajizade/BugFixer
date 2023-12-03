using BugFixer.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Domain.Models.User
{
    public class Following: BaseEntity
    {
        public int UserId { get; set; }
        public int FollowingId { get; set; }
    }
}
