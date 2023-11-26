using BugFixer.Application.ViewModels.User;
using BugFixer.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Interfaces
{
    public interface IUserService
    {
        #region Admin
        Task<IEnumerable<UserVM>> GetAllServiceAsync();
        Task<UserVM> GetServiceAsync(int userId);
        Task CreateServiceAsync(CreateUserVM user);
        Task UpdateServiceAsync(UpdateUserVM user);
        Task<UpdateUserVM> GetUserInforForUpdate(int userId);
        Task DeleteServiceAsync(int userId);


        #endregion


        Task<FilterUsersViewModel> FilterUser(FilterUsersViewModel filter);


        #region Profile
        Task<ProfileVM> ProfileInfoServiceAsync(int userId);
        Task UpdateVisitProfileServiceAsync(int userId);
        Task FollowUserServiceAsync(int userId, int follwingId);
        Task<IEnumerable<Following>> FollowingsServiceAsync();
        #endregion


        #region Users Page
        Task<FilterUsersPageVM> FilterUsersPageServiceAsync(FilterUsersPageVM filter);
        #endregion




    }
}
