using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        //Renamed Found() to Success() seems more meaningful here
        // As Found represents HttpStatus Code of 302 (previously known as "MovedTemporarily")  and we are return Status code 200
        public HttpResponseMessage Success(object obj)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        //Renamed Found() to Success() seems more meaningful here
        // As Found represents HttpStatus Code of 302 (previously known as "MovedTemporarily")  and we are return Status code 200      
        public HttpResponseMessage Success()
        {            
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage DoesNotExist()
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.NotFound);
        }

        //I could have named it as AlreadyExists but it may have defeated the person by making this method to be too restrictive.  
        //Conflict or 409 can happen in other situations as well, not just when we try to create something which already exists
        public HttpResponseMessage ConflictError(object obj)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.Conflict, obj);
        }

        public HttpResponseMessage BadRequestError(object obj)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, obj);
        }
    }
}