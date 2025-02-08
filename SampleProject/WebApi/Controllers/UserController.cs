using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Users;
using WebApi.Models.Users;


namespace WebApi.Controllers
{
    //ToDo - Implement Authorization to ensure that only Admin can access this
    [RoutePrefix("users")]
    public class UserController : BaseApiController
    {
        private readonly ICreateUserService _createUserService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IGetUserService _getUserService;
        private readonly IUpdateUserService _updateUserService;

        public UserController(ICreateUserService createUserService, IDeleteUserService deleteUserService, IGetUserService getUserService, IUpdateUserService updateUserService)
        {
            _createUserService = createUserService;
            _deleteUserService = deleteUserService;
            _getUserService = getUserService;
            _updateUserService = updateUserService;
        }

        [Route("{userId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateUser(Guid userId, [FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestError(ModelState);
            }   

            var user = _getUserService.GetUser(userId);
            if (user != null)
            {
                return ConflictError($"User {userId} already exists");
            }

            //Checking to ensure the user if it is type of Customer
            //If same email already exists we should not be able to create it
            //For Admin and Employees, I'm not sure,
            //As sometimes it happens that some employees have a previleged user account and 
            //an ordinary account for admin and non admin operations
            //Ideally this should be checked in the Business layer
            if(model.Type == UserTypes.Customer)
            {
                user = _getUserService.GetUsers(UserTypes.Customer, string.Empty, model.Email).FirstOrDefault();
                if (user != null)
                {
                    return ConflictError($"User with email {model.Email} already exists");
                }
            }            

            user = _createUserService.Create(userId, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
            return Success(new UserData(user));
        }

        [Route("{userId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateUser(Guid userId, [FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestError(ModelState);
            }

            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                return DoesNotExist();
            }
                        
            //Not sure but we need to think about
            //whether a customer's email can be updated or not
            //if yes - we have to ensure
            //that the email is not being used for some other customer
            //may be a future enhancement

            _updateUserService.Update(user, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
            return Success(new UserData(user));
        }

        [Route("{userId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                return DoesNotExist();
            }
            _deleteUserService.Delete(user);
            return Success();
        }

        [Route("{userId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);

            if (user == null)
            {
                return DoesNotExist();
            }

            return Success(new UserData(user));
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetUsers(int skip, int take, UserTypes? type = null, string name = null, string email = null)
        {
            var users = _getUserService.GetUsers(type, name, email)
                                       .Skip(skip).Take(take)
                                       .Select(q => new UserData(q))
                                       .ToList();
            return Success(users);
        }

        [Route("clear")]
        [HttpDelete]
        public HttpResponseMessage DeleteAllUsers()
        {
            _deleteUserService.DeleteAll();
            return Success();
        }

        [Route("list/tag")]
        [HttpGet]
        public HttpResponseMessage GetUsersByTag(string tag)
        {
            var users = _getUserService.GetUsers(tag);

            return Success(users);
        }
    }
}