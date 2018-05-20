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
    public class PublisherController : ApiController
    {
        [HttpGet]
        [Route("api/Publisher/{searchname}/{skip}/{pagesize}")]
        public IHttpActionResult GetPublisher(string searchname, int skip, int pagesize)
        {
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())

                    obj = new
                    {
                        StatusCode = 200,
                        data = _unitOfWork.PublisherRepository.GetAll()
                        .Where(x => (x.IsActive == true && x.PublisherName.Contains(searchname)) || (x.IsActive == true && searchname == "0"))
                        .OrderByDescending(x => x.PublisherID).Skip(skip * pagesize).Take(pagesize)
                        .Select(x => new
                        {
                            PublisherID = x.PublisherID,
                            PublisherName = x.PublisherName,
                            Discription = x.Description
                        })
                        .ToList(),
                        total = _unitOfWork._context.Publishers.Count(x => (x.IsActive == true && x.PublisherName.Contains(searchname)) || (x.IsActive == true && searchname == "0"))
                    };


            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Publisher>() };
            }
            return Ok(obj);



        }
        [HttpGet]
        [Route("api/publisher/getall")]
        public HttpResponseMessage GetAllPublisher()
        {
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    obj = new
                    {
                        StatusCode = 200,
                        data = _unitOfWork.PublisherRepository.GetAll().Where(x => x.IsActive == true).Select(x => new { x.PublisherID, x.PublisherName }).ToList()




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
        public HttpResponseMessage AddPublisher([FromBody] Publisher p_Publisher)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    p_Publisher.IsActive = true;
                    p_Publisher.Created = DateTime.Now;


                    _unitOfWork.PublisherRepository.Insert(p_Publisher);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Publisher };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Publisher() };
            }
            return Request.CreateResponse(obj);
        }
        [HttpPut]
        public HttpResponseMessage EditPublisher([FromBody] Publisher p_Publisher)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    Publisher v_obj = _unitOfWork._context.Publishers.FirstOrDefault(x => x.PublisherID == p_Publisher.PublisherID);
                    v_obj.PublisherName = p_Publisher.PublisherName;
                    v_obj.Description = p_Publisher.Description;
                    _unitOfWork.PublisherRepository.Edit(v_obj);

                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Publisher };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Publisher() };
            }
            return Request.CreateResponse(obj);
        }
        [HttpDelete]
        public HttpResponseMessage DeletePublisher([FromBody] Publisher p_Publisher)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    p_Publisher.IsActive = false;

                    _unitOfWork.PublisherRepository.Edit(p_Publisher);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Publisher };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Publisher() };
            }
            return Request.CreateResponse(obj);
        }

    }
}
