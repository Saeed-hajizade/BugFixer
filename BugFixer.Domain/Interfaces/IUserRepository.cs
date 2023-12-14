using BugFixer.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task SaveChangeAsync();
        #region Admin
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(int userId);
        Task CreateAsync(User user);
        void Update(User user);
        void Delete(User user);
        IQueryable<User> UserListForFilter();

        #endregion

        #region Account
        Task<User> RegisterAsync(User user);
        Task<User> LoginUserAsync(string email, string password);
        Task<bool> IsUserNameExistAsync(string userName);
        Task<bool> IsEmailExistAsync(string email);
        Task<User> GetUserByActiveCodeAsync(string activeCode);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserForEditByIdAsync(int id);
        Task<User> GetUserByIdAsync(int id);

        #endregion

        #region Profile
        Task<User> ProfileInfoAsync(int id);
        Task FollowUser(Following following);
        Task<IEnumerable<Following>> Followins();
        Task<Following> GetFollowingAsync(int userId, int followingId);
        void DeleteFollowing(Following following);
        #endregion

        #region Users Page
        IQueryable<User> UsersPageQueryable();
        #endregion

        #region Following
        Task<int> GetUserFollowingsCountAsync(int userId);
        Task<int> GetUserFollowersCountAsync(int userId);
        //Task<List<Following>> GetUserFollowersAsync(int userId);
        //Task<List<Following>> GetUserFollowingsAsync(int userId);
        #endregion
    }
}
