using System;
using System.Linq;
using DBOperation.Data;
using DBOperation.Model;
using Microsoft.AspNetCore.Mvc;

namespace DBOperation.Controllers
{
    [Route("/[Controller]")]
    public class WebDataController : Controller
    {
        WebContext _webDataContext;

        public WebDataController(WebContext masterDataContext)
        {
            _webDataContext = masterDataContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// TotalDevices and DeviceIds may be not provided by the user.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(MediaView view)
        {
            CreateView(view);
            return View();
        }

        /// <summary>
        /// This will save a new view in the database with currect model
        /// </summary>
        /// <param name="mediaView"></param>
        /// <returns></returns>
        [Route("~/CreateView")]
        [HttpPost]
        public int CreateView([FromBody]MediaView mediaView)
        {
            int newId = -1;
            if (ModelState.IsValid)
            {
                try
                {
                    newId = _webDataContext.AddView(ref mediaView);
                    _webDataContext.SaveChanges();
                    MediaView.Message = "Success";
                }
                catch(System.Data.SqlClient.SqlException databaseException)
                {
                    MediaView.Message = "Database Exception";
                    Console.Write(databaseException);
                }
                catch (Exception ex)
                {
                    MediaView.Message = ex.InnerException.ToString();
                }                
            }
            return newId;
        }

        [Route("~/ReadAllViews")]
        [HttpGet]
        public JsonResult ReadAllViews()
        {
            return Json(_webDataContext.MediaView.ToList());
        }

        [Route("~/ReadViewById/{id}")]
        [HttpGet]
        public JsonResult ReadViewById(int id)
        {
            MediaView view = _webDataContext.GetView(id);            
            return Json(view);
        }

        [Route("~/UpdateView")]
        [HttpPut]
        public IActionResult UpdateView([FromBody]MediaView mediView)
        {
            if(ModelState.IsValid)
            {
                _webDataContext.UpdateView(ref mediView);
                _webDataContext.SaveChanges();
            }
            return Json(mediView);
        }

        [Route("~/DeleteView/{id}")]
        [HttpDelete]
        public void DeleteView(int id)
        {
            _webDataContext.DeleteView(id);
            _webDataContext.SaveChanges();
        }
    }
}