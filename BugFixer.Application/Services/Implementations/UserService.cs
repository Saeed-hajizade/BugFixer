using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.User;
using BugFixer.Domain.Interfaces;
using BugFixer.Domain.Models.User;
using EShop.Application.Generator;
using EShop.Application.Security;
using Microsoft.EntityFrameworkCore;

namespace BugFixer.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task CreateServiceAsync(CreateUserVM createUserVM)
        {
            User newUser = new User()
            {
                Email = createUserVM.Email,
                EmailConfirm = true,
                Mobile = createUserVM.Mobile,
                Password = PasswordHelper.EncodePasswordMd5(createUserVM.Password),
                UserName = createUserVM.UserName,
                ActiveCode = NameGenerator.GenerateUniqCode(),
                Avatar = "Default.png",
                RoleId = createUserVM.RoleId,
                

            };

            await _userRepository.CreateAsync(newUser);
            await _userRepository.SaveChangeAsync();


        }

        public async Task DeleteServiceAsync(int userId)
        {
            User getUser = await _userRepository.GetAsync(userId);

            if (getUser != null)
            {
                _userRepository.Delete(getUser);
                await _userRepository.SaveChangeAsync();
            }
        }

        public async Task<FilterUsersViewModel> FilterUser(FilterUsersViewModel filter)
        {
            IQueryable<User> query = _userRepository.UserListForFilter();

            if (!string.IsNullOrEmpty(filter.UserName)) 
            {
                query = query.Where(u => EF.Functions.Like(u.UserName, $"%{filter.UserName.Trim().ToLower()}%"));
            }

            await filter.Paging(query);

            return filter;
        }

        public async Task<IEnumerable<UserVM>> GetAllServiceAsync()
        {
            IEnumerable<User> userList = await _userRepository.GetAllAsync();
            return userList.Select(user => new UserVM()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirm = user.EmailConfirm,
                Mobile = user.Mobile,
                Avatar = user.Avatar,
                CreateDate = user.CreateDate,
            }).ToList();
        }

        public async Task<UserVM> GetServiceAsync(int userId)
        {
            User getUser = await _userRepository.GetAsync(userId);

            return new UserVM()
            {
                Id = getUser.Id,
                UserName = getUser.UserName,
                Email = getUser.Email,
                EmailConfirm = getUser.EmailConfirm,
                Mobile = getUser.Mobile,
                Avatar = getUser.Avatar,
                CreateDate = getUser.CreateDate,
            };

        }

        public async Task<UpdateUserVM> GetUserInforForUpdate(int userId)
        {
            User getUser = await _userRepository.GetAsync(userId);

            return new UpdateUserVM()
            {
                Id = getUser.Id,
                UserName = getUser.UserName,
                Email = getUser.Email,
                Mobile = getUser.Mobile,
                Password = getUser.Password,
                RoleId = getUser.RoleId,
            };
        }



        public async Task UpdateServiceAsync(UpdateUserVM updateUserVM)
        {
            User getUser = await _userRepository.GetAsync(updateUserVM.Id);

            getUser.UserName = updateUserVM.UserName;
            getUser.Email = updateUserVM.Email;
            getUser.Mobile = updateUserVM.Mobile;
            getUser.Password=updateUserVM.Password;
            getUser.RoleId=updateUserVM.RoleId;

            _userRepository.Update(getUser);
            await _userRepository.SaveChangeAsync();

        }


        #region Profile
        public async Task UpdateVisitProfileServiceAsync(int userId)
        {
            User user = await _userRepository.GetAsync(userId);

            user.ProfileVisit += 1;
            _userRepository.Update(user);
            await _userRepository.SaveChangeAsync();
        }

        public async Task<ProfileVM> ProfileInfoServiceAsync(int userId)
        {
            User user = await _userRepository.ProfileInfoAsync(userId);

            return new ProfileVM()
            {
                User = new UserVM()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Avatar = user.Avatar,
                    CreateDate = user.CreateDate,
                    ProfileVisit = user.ProfileVisit,

                },

                Questions = user.Questions.Select(q => new ViewModels.Questions.QuestionVM()
                {
                    Text = q.Text,
                    CreateDate = q.CreateDate,
                }).ToList(),

                Answers = user.Answers.Select(q => new ViewModels.Questions.AnswerVM()
                {
                    Text = q.Text,
                    CreateDate = q.CreateDate,
                }).ToList(),

                Resume = user.Resume !=null ? new ViewModels.Resume.ResumeVM()
                {
                    Bio = user.Resume.Bio,
                    EducationalDocuments = user.Resume.EducationalDocuments,
                    ProfessionName = user.Resume.ProfessionName,
                    WorkExperienceYears = user.Resume.WorkExperienceYears,
                }:null,
               QuestionRateCount=user.QuestionRates.Count(),

                


            };
        }

        public async Task FollowUserServiceAsync(int userId, int follwingId)
        {
            Following getFollwing = await _userRepository.GetFollowingAsync(userId, follwingId);
            if (getFollwing == null) {
                Following follwing = new Following() { UserId = userId, FollowingId = follwingId };
                await _userRepository.FollowUser(follwing);

            }
            else
            {
                _userRepository.DeleteFollowing(getFollwing);
            }

            await _userRepository.SaveChangeAsync();
        }

        public async Task<IEnumerable<Following>> FollowingsServiceAsync()
        {
            return await _userRepository.Followins();
        }


        #endregion

        #region Users Page
        public async Task<FilterUsersPageVM> FilterUsersPageServiceAsync(FilterUsersPageVM filter)
        {
            IQueryable<User> query = _userRepository.UsersPageQueryable();

            if (!string.IsNullOrEmpty(filter.UserName))
            {
                query = query.Where(u => EF.Functions.Like(u.UserName, $"%{filter.UserName.Trim().ToLower()}%"));
            }

            switch (filter.OrderType)
            {
                case "New":
                    query = query.OrderBy(u => u.CreateDate);
                    break;
                case "MostRatedUsers":
                    query = query.OrderBy(u=> u.Answers.Count());
                    break;
                case "MostScoringQuestioners":
                    query = query.OrderBy(u=> u.Questions.Count());
                    break;



                default:
                    break;
            }

  


            await filter.Paging(query);

            return filter;
        }
        #endregion

        #region Followings
            
        public async Task<UserFollowingFollowersCountVM> GetUserFollowingsFollowersCountServiceAsync(int userId)
        {
            int followersCount = await _userRepository.GetUserFollowersCountAsync(userId);
            int followingsCount = await _userRepository.GetUserFollowingsCountAsync(userId);

            return new UserFollowingFollowersCountVM
            {
                FollowersCount = followersCount == null ? 0 : followersCount,
                FollowingsCount = followingsCount == null ? 0 : followingsCount,
            };
        }

        #endregion
    }
}
