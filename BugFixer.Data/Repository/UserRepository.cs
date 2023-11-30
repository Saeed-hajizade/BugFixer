using BugFixer.Data.Context;
using BugFixer.Domain.Interfaces;
using BugFixer.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
namespace BugFixer.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BugFixerDBContext _ctx;
        public UserRepository(BugFixerDBContext ctx)
        {
            _ctx = ctx;
        }

        public async Task SaveChangeAsync()
        {
            await _ctx.SaveChangesAsync();
        }


        #region Admin
        public async Task CreateAsync(User user)
        {
            await _ctx.AddAsync(user);

        }

        public void Delete(User user)
        {
            _ctx.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _ctx.Users.ToListAsync();
        }

        public async Task<User> GetAsync(int userId)
        {
            return await _ctx.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public void Update(User user)
        {
            _ctx.Update(user);
        }


        public  IQueryable<User> UserListForFilter()
        {
            return  _ctx.Users.AsQueryable();
        }
        #endregion




        #region Account
        public async Task<User> RegisterAsync(User user)
        {
            await _ctx.AddAsync(user);
            return user;
        }

        public async Task<User> LoginUserAsync(string email, string password)
        {
            var user = await _ctx.Users
                         .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            return user;
        }

        public Task<bool> IsUserNameExistAsync(string userName)
        {
            return _ctx.Users.AnyAsync(u => u.UserName == userName);
        }

        public Task<bool> IsEmailExistAsync(string email)
        {
            return _ctx.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByActiveCodeAsync(string activeCode)
        {
            var user = await _ctx.Users
                .FirstOrDefaultAsync(u => u.ActiveCode == activeCode);
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _ctx.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

      public async Task<User> GetUserForEditByIdAsync(int id)
        {
            var user = await _ctx.Users.FindAsync(id);
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _ctx.Users.FindAsync(id);
        }


        #endregion


        #region Profile
        public async Task<User> ProfileInfoAsync(int id)
        {
            return await _ctx.Users.Include(u => u.Questions).Include(u => u.QuestionRates)
                .Include(u => u.Answers).Include(u => u.Resume)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task FollowUser(Following following)
        {
            await _ctx.Followings.AddAsync(following);
        }

        public async Task<IEnumerable<Following>> Followins()
        {
           return await _ctx.Followings.ToListAsync();
        }

        public async Task<Following> GetFollowingAsync(int userId, int followingId)
        {
            return await _ctx.Followings.FirstOrDefaultAsync(f => f.UserId == userId && f.FollowingId == followingId);
        }

        public void DeleteFollowing(Following following)
        {
            _ctx.Followings.Remove(following);
        }


        #endregion

        #region Users Page
        public  IQueryable<User> UsersPageQueryable()
        {
            return  _ctx.Users.Include(u=> u.Answers)
                .ThenInclude(a=> a.TrueAnswer)
                .Include(u=> u.Questions)
                .Include(u=> u.Resume)
                .ThenInclude(r=> r.ResumeSkills)
                .AsQueryable();
        }
        #endregion
    }
}
