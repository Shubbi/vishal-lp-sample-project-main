using System.Collections.Generic;
using BusinessEntities;
using Common;
using Data.Repositories;

namespace Core.Services.Users
{
    //Needed to change the scope to support IUserRepository from UserService    
    [AutoRegister]
    public class UpdateUserService : IUpdateUserService
    {
        //Moved IUserRepository here to support both Create and Update
        //and at the same time preserve existing architecture
        private readonly IUserRepository _userRepository;
        public UpdateUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void Update(User user, string name, string email, UserTypes type, decimal? annualSalary, IEnumerable<string> tags)
        {
            user.SetEmail(email);
            user.SetName(name);
            user.SetType(type);
            //Here 0m was a viable alternative. But since in the design we have kept annulaSalary as nullable, so checking for null and dividing by 12 only when it's not null 
            user.SetMonthlySalary(annualSalary.HasValue ? annualSalary.Value / 12 : annualSalary);
            user.SetTags(tags);
            _userRepository.Save(user);
        }
    }
}