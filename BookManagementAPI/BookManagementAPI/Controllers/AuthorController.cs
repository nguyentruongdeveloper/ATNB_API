using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAO;
using DTO;

namespace BookManagementAPI.Controllers
{
    public class AuthorController : ApiController
    {
        [HttpGet]
        [Route("api/Author/{searchname}/{skip}/{pagesize}")]
        public HttpResponseMessage GetAuthor(string searchname, int skip, int pagesize)
        {
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())

                    obj = new
                    {
                        StatusCode = 200,
                        data = _unitOfWork.AuthorRepository.GetAll().Where(x => (x.IsActive == true && x.AuthorName.Contains(searchname)) || (x.IsActive == true && searchname == "0")).OrderByDescending(x => x.AuthorID).Skip(skip * pagesize).Take(pagesize).ToList(),
                        total = _unitOfWork._context.Authors.Count(x => (x.IsActive == true && x.AuthorName.Contains(searchname)) || (x.IsActive == true && searchname == "0"))
                    };


            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Author>() };
            }
            return Request.CreateResponse(obj);



        }
        [HttpGet]
        [Route("api/Author/getall")]
        public HttpResponseMessage getAllAuthor()
        {
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    obj = new
                    {
                        StatusCode = 200,
                        data = _unitOfWork.AuthorRepository.GetAll().Where(x => x.IsActive == true).Select(x => new { x.AuthorID, x.AuthorName}).ToList()




                    };

                };




            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Category>() };
            }
            return Request.CreateResponse(obj);



        }


        [HttpPost]
        public HttpResponseMessage AddAuthor([FromBody] Author p_Author)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    p_Author.IsActive = true;
                    p_Author.Created = DateTime.Now;


                    _unitOfWork.AuthorRepository.Insert(p_Author);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Author };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Author() };
            }
            return Request.CreateResponse(obj);
        }
        [HttpPut]
        public HttpResponseMessage EditAuthor([FromBody] Author p_Author)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    Author v_obj = _unitOfWork._context.Authors.FirstOrDefault(x => x.AuthorID == p_Author.AuthorID);
                    v_obj.AuthorName = p_Author.AuthorName;
                    v_obj.History = p_Author.History;
                    _unitOfWork.AuthorRepository.Edit(v_obj);

                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Author };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Author() };
            }
            return Request.CreateResponse(obj);
        }
        [HttpDelete]
        public HttpResponseMessage DeleteAuthor([FromBody] Author p_Author)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    p_Author.IsActive = false;

                    _unitOfWork.AuthorRepository.Edit(p_Author);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Author };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Author() };
            }
            return Request.CreateResponse(obj);
        }
    }
}
