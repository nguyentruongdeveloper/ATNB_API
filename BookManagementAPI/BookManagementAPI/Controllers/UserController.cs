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
                    string passwordnew = Utility.Encrytion.Encrypt(password);
                    User objUser = _unitOfWork._context.Users.Where(x => x.IsActive == true && x.UserName == username && x.Password == passwordnew)
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
        [HttpGet]
        [Route("api/user/{searchname}/{skip}/{pagesize}")]
        public HttpResponseMessage GetUser(string searchname, int skip, int pagesize)
        {
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())

                    obj = new
                    {
                        StatusCode = 200,
                        data = _unitOfWork.UserRepository.GetAll().Where(x => (x.IsActive == true && x.UserName.Contains(searchname)) || (x.IsActive == true && searchname == "0"))
                        .OrderByDescending(x => x.Created).Skip(skip * pagesize).Take(pagesize)
                         .OrderByDescending(x => x.UserID)
                         
                        .ToList(),
                        total = _unitOfWork._context.Authors.Count(x => (x.IsActive == true && x.AuthorName.Contains(searchname)) || (x.IsActive == true && searchname == "0"))
                    };


            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Author>() };
            }
            return Request.CreateResponse(obj);



        }
        //[HttpGet]
        //[Route("api/user/getall")]
        //public HttpResponseMessage getAllUser()
        //{
        //    object obj;
        //    try
        //    {
        //        using (var _unitOfWork = new UnitOfWork())
        //        {
        //            obj = new
        //            {
        //                StatusCode = 200,
        //                data = _unitOfWork.UserRepository.GetAll().Where(x => x.IsActive == true).Select(x => new { x.AuthorID, x.AuthorName }).ToList()




        //            };

        //        };




        //    }
        //    catch (Exception ex)
        //    {
        //        obj = new { StatusCode = 500, data = new List<Category>() };
        //    }
        //    return Request.CreateResponse(obj);



        //}


        [HttpPost]
        public HttpResponseMessage AddUser([FromBody] User p_User)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    p_User.IsActive = true;
                    p_User.Created = DateTime.Now;
                    p_User.Password = Utility.Encrytion.Encrypt(p_User.Password);


                    _unitOfWork.UserRepository.Insert(p_User);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_User };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new User() };
            }
            return Request.CreateResponse(obj);
        }
        [HttpPut]
        public HttpResponseMessage EditUser([FromBody]  User p_User)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    User v_obj = _unitOfWork._context.Users.FirstOrDefault(x => x.UserID == p_User.UserID);
                    v_obj.UserName = p_User.UserName;
                    v_obj.Password = p_User.Password;
                    v_obj.Email = p_User.Email;
                    _unitOfWork.UserRepository.Edit(v_obj);

                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_User };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new User() };
            }
            return Request.CreateResponse(obj);
        }
        [HttpDelete]
        public HttpResponseMessage DeleteAuthor([FromBody] User p_User)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    p_User.IsActive = false;

                    _unitOfWork.UserRepository.Edit(p_User);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_User };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new User() };
            }
            return Request.CreateResponse(obj);
        }

    }
}
