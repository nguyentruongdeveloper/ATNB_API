using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.IO;

namespace BookManagementAPI.Controllers
{
    public class BookController : ApiController
    {
        [HttpGet]
        [Route("api/book/{searchname}/{skip}/{pagesize}")]
        public HttpResponseMessage GetAllBook(string searchname, int skip, int pagesize)
        {
            object obj;
            try
            {

                using (var _unitOfWork = new UnitOfWork())
                {

                    obj = new
                    {
                        StatusCode = 200,
                        data = _unitOfWork._context.Books.Where(x => (x.IsActive == true && x.Title.Contains(searchname)) || (x.IsActive == true && searchname == "0"))


                        .OrderByDescending(x => x.BookID).Skip(skip * pagesize).Take(pagesize).Select(x => new {
                          BookID =  x.BookID,
                          CategoryID = x.CategoryID,
                          CategoryName=x.Category.CategoryName,
                          AuthorID= x.AuthorID,
                          AuthorName = x.Author.AuthorName,
                          PublisherID = x.PublisherID,
                          PublisherName=x.Publisher.PublisherName,
                          Title= x.Title,
                          Summary= x.Summary,
                          Price =x.Price,
                          Quantity = x.Quantity,
                          StatusBookID = x.StatusBookID,
                          StatusBookName = x.StatusBook.StatusBookName,
                          ImgUrl = x.ImgUrl
                        }).ToList(),

                        total = _unitOfWork._context.Books.Count(x => (x.IsActive == true && x.Title.Contains(searchname)) || (x.IsActive == true && searchname == "0"))
                    };

                }



            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Book>() };

            }
            return Request.CreateResponse(obj);



        }

        [HttpGet]
        [Route("api/Book/gettotalrecord")]
        public HttpResponseMessage GetTotalRecord()
        {
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())

                    obj = new { StatusCode = 200, data = _unitOfWork.BookRepository.GetTotalRecord() };


            }
            catch (Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Book>() };
            }
            return Request.CreateResponse(obj);



        }

        [HttpPost]
        [Route("api/Book/uploadfile")]
        public HttpResponseMessage Uploadfile()
        {
            object obj;
            int iUploadedCnt = 0;
            string sPath = "";
            try
            {
                sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/");
              
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                System.Web.HttpPostedFile hpf = hfc[0];
                if (hpf.ContentLength > 0)
                {
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        iUploadedCnt = iUploadedCnt + 1;
                        
                    }
                }
                obj = new { StatusCode = 200, data = "http://localhost:59728/Images/" + hpf.FileName };
              


            }
            catch(Exception ex)
            {
                obj = new { StatusCode = 500, data = new List<Book>() };
            }

            return Request.CreateResponse(obj);


            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.


        }

        [HttpPost]
        public HttpResponseMessage AddBook([FromBody] Book p_Book)
        {
           
            object obj;
            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    p_Book.ModifiedDay = DateTime.Now;
                    p_Book.CreateDay = DateTime.Now;
                    p_Book.ImgUrl = "http://localhost:59728/Images/" + p_Book.ImgUrl;
                    _unitOfWork.BookRepository.Insert(p_Book);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Book };
                }

                  
            }
            catch
            {
                obj = new { StatusCode = 500, data = new Book() };
            }
            return Request.CreateResponse(obj);
        }
        [HttpPut]
        public HttpResponseMessage EditBook([FromBody] Book p_Book)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    Book v_obj = _unitOfWork._context.Books.FirstOrDefault(x => x.BookID == p_Book.BookID);
                    v_obj.Title = p_Book.Title;
                    v_obj.Summary = p_Book.Summary;
                    v_obj.AuthorID = p_Book.AuthorID;
                    v_obj.CategoryID = p_Book.CategoryID;
                    v_obj.PublisherID = p_Book.PublisherID;
                    v_obj.StatusBookID = p_Book.StatusBookID;
                    v_obj.Price = p_Book.Price;
                    v_obj.Quantity = p_Book.Quantity;
                    if(!p_Book.ImgUrl.Contains("http://localhost:59728/Images/"))
                    {
                        v_obj.ImgUrl = "http://localhost:59728/Images/" + p_Book.ImgUrl;
                    }
                    else
                    {
                        v_obj.ImgUrl = p_Book.ImgUrl;
                    }
                  
                    v_obj.ModifiedDay = DateTime.Now;
                    _unitOfWork.BookRepository.Edit(v_obj);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Book };
                }
            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Book() };
            }
            return Request.CreateResponse(obj);
        }
        [HttpDelete]
        public HttpResponseMessage DeleteBook([FromBody] Book p_Book)
        {
            object obj;

            try
            {
                using (var _unitOfWork = new UnitOfWork())
                {
                    p_Book.IsActive = false;

                    _unitOfWork.BookRepository.Edit(p_Book);
                    _unitOfWork.Save();
                    obj = new { StatusCode = 200, data = p_Book };
                }



            }
            catch (Exception e)
            {
                obj = new { StatusCode = 500, data = new Book() };
            }
            return Request.CreateResponse(obj);
        }
    }
}
