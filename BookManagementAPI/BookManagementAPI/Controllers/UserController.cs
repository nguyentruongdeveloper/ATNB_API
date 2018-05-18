using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookManagementAPI.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("api/user/{username}/{password}")]
        public HttpResponseMessage Login(string username, string password)
        {
            object obj;
            Boolean IsLoginSuss = false;
            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    User objUser = _unitOfWork._context.Users.Where(x => x.IsActive == true && x.UserName == username && x.Password == password)
                        .FirstOrDefault();
                     
                    if (objUser != null)
                    {
                        IsLoginSuss = true;
                    }
                  
                       

                    obj = new
                    {
                        StatusCode = 200,
                        data = objUser,

                        IsLoginSuss = IsLoginSuss





                    };

                };




            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new User(), IsLoginSuss = IsLoginSuss };
            }
            return Request.CreateResponse(obj);



        }

    }
}
