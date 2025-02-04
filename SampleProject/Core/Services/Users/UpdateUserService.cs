using System.Collections.Generic;
using BusinessEntities;
using Common;

namespace Core.Services.Users
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateUserService : IUpdateUserService
    {
        public void Update(User user, string name, string email, UserTypes type, decimal? annualSalary, IEnumerable<string> tags)
        {
            user.SetEmail(email);
            user.SetName(name);
            user.SetType(type);

            //Here 0m was a viable alternative. But since in the design we have kept annulaSalary as nullable, so checking for null and dividing by 12 only when it's not null 
            user.SetMonthlySalary(annualSalary.HasValue ? annualSalary.Value / 12 : annualSalary);
            user.SetTags(tags);
        }
    }
}