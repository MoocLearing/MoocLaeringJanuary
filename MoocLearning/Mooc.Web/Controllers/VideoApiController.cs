using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.Mongo;
using Mooc.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Mooc.Web.Controllers
{
    public class VideoApiController : ApiController
    {

        private readonly DataContext _dataContext;
        public VideoApiController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            try
            {
                string filename = id;
                if (string.IsNullOrEmpty(filename))
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("参数为空") };

                var bytes = MongoDBHelper.down(filename);
                if (bytes == null || bytes.Length == 0)
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("视频不存在") };

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
                ContentDispositionHeaderValue cdh = new ContentDispositionHeaderValue("Inline");
                cdh.FileName = filename;
                result.Content.Headers.ContentDisposition = cdh;
                return result;
            }
            catch (Exception e)
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("异常:" + e.Message) };
            }

        }

        [HttpGet]
        [HttpPost]
        public BaseResult SaveEdit(Course course)
        {
            if (course != null)
            {
                var exist = _dataContext.Courses.Find(course.ID);
                if (exist != null)
                {
                    exist.CourseName = course.CourseName;
                    exist.CourseDetail = course.CourseDetail;
                    exist.TeacherId = course.TeacherId;
                    exist.CategoryId = course.CategoryId;
                    exist.Status = course.Status;
                    exist.CoverPic = course.CoverPic;
                    _dataContext.SaveChanges();
                    return new BaseResult(0, "添加成功");
                }

            }
            return new BaseResult(1, "参数错误");

        }
    }
}
