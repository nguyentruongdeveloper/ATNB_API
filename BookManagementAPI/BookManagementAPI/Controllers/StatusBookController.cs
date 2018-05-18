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
    public class StatusBookController : ApiController
    {
        [HttpGet]
        [Route("api/statusbook/getall")]
        public HttpResponseMessage getAllStatusBook()
        {
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    obj = new
                    {
                        StatusCode = 200,
                        data = _unitOfWork.StatusRepository.GetAll().ToList()




                    };

                };




            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Category>() };
            }
            return Request.CreateResponse(obj);



        }
    }
}
