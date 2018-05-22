using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAO;
using DTO;
using System.Web.Routing;

namespace BookManagementAPI.Controllers
{
    public class CategoryController : ApiController
    {
        [HttpGet]
        [Route("api/category/{searchname}/{skip}/{pagesize}")]
        public HttpResponseMessage GetAllCategory(string searchname, int skip, int pagesize)
        {
            object obj;
            try
            {

                using (var _unitOfWork = new UnitOfWork())
                {

                    obj = new
                    {
                        StatusCode = 200,
                        data = _unitOfWork.CategoryRepository.GetAll()
                        .Where(x => (x.IsActive == true && x.CategoryName.Contains(searchname)) || (x.IsActive == true && searchname == "0"))
                        .OrderByDescending(x => x.Created).Skip(skip * pagesize).Take(pagesize)
                        .Select(x => new
                        {
                            CategoryID = x.CategoryID,
                            CategoryName = x.CategoryName,
                            Description = x.Description
                        })
                        .ToList(),
                        total = _unitOfWork._context.Categories.Count(x => (x.IsActive == true && x.CategoryName.Contains(searchname)) || (x.IsActive == true && searchname == "0"))
                    };

                }



            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Category>() };

            }
            return Request.CreateResponse(obj);



        }
        [HttpGet]
        [Route("api/category/searchbyname/{searchname}/{skip}/{pagesize}")]
        public HttpResponseMessage SearchCategoryByName(string searchname, int skip, int pagesize)
        {
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())

                    obj = new
                    {
                        StatusCode = 200,
                        data = _unitOfWork.CategoryRepository.GetAll().Where(x => x.IsActive == true && x.CategoryName.Contains(searchname)).OrderByDescending(x => x.CategoryID).Skip(skip * pagesize).Take(pagesize).ToList(),
                        total = _unitOfWork._context.Categories.Count(x => x.IsActive == true)
                    };


            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Category>() };
            }
            return Request.CreateResponse(obj);



        }
        [HttpGet]
        [Route("api/category/getall")]
        public HttpResponseMessage getAllCategory()
        {
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    obj = new
                    {
                        StatusCode = 200,
                        data = _unitOfWork.CategoryRepository.GetAll().Where(x => x.IsActive == true).Select(x => new { x.CategoryID, x.CategoryName }).ToList()




                    };

                };




            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Category>() };
            }
            return Request.CreateResponse(obj);



        }
        [HttpGet]
        [Route("api/category/gettotalrecord")]
        public HttpResponseMessage GetTotalRecord()
        {
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())

                    obj = new { StatusCode = 200, data = _unitOfWork.CategoryRepository.GetTotalRecord() };


            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Category>() };
            }
            return Request.CreateResponse(obj);



        }
        [HttpPost]
        public HttpResponseMessage AddCategory([FromBody] Category p_Category)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    p_Category.IsActive = true;
                    p_Category.Created = DateTime.Now;


                    _unitOfWork.CategoryRepository.Insert(p_Category);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Category };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Category() };
            }
            return Request.CreateResponse(obj);
        }
        [HttpPut]
        public HttpResponseMessage EditCategory([FromBody] Category p_Category)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    Category v_obj = _unitOfWork._context.Categories.FirstOrDefault(x => x.CategoryID == p_Category.CategoryID);
                    v_obj.CategoryName = p_Category.CategoryName;
                    v_obj.Description = p_Category.Description;
                    _unitOfWork.CategoryRepository.Edit(v_obj);

                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Category };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Category() };
            }
            return Request.CreateResponse(obj);
        }
        [HttpDelete]
        public HttpResponseMessage DeleteCategory([FromBody] Category p_Category)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    p_Category.IsActive = false;

                    _unitOfWork.CategoryRepository.Edit(p_Category);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Category };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Category() };
            }
            return Request.CreateResponse(obj);
        }
    }
}
