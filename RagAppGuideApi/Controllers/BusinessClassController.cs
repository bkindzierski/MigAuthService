using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//
using NHibernate.Linq;
using Newtonsoft.Json;
using RagAppGuide.DAL;
using RagAppGuide.DAL.Models;
using Microsoft.AspNetCore.Cors;

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace RagAppGuideApi.Controllers
{
    [Produces("application/json")]
    [Route("api/BusinessClass")]
    public class BusinessClassController : Controller
    {
        
        [HttpGet]
        //[Authorize("MIGEAuthorize")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [EnableCors("MyPolicy")]
        public JsonResult Get()
        {
            
            string token = null;
            //get authorization headers
            var authorization = this.HttpContext.Request.Headers["Authorization"];
            foreach (string val in authorization) {
               token = val.Replace("Bearer", "").Trim();
            }

            List<DWXF710> BusinessClassList = new List<DWXF710>();

            //nhibernate connect 
            NHibernateSession nh = new NHibernateSession();
            var sessionFactory = nh.session;

            //read back the new entry
            using (var session = sessionFactory.OpenSession())
            using (var conn = session.BeginTransaction())
            {
                //perform db logic
                var businessClasses = (from DWXF710 in session.Query<DWXF710>()
                                           where DWXF710.CLASX == "013930"
                                           //&& DWXF710T.PRMSTE == "20"
                                           select DWXF710).Take(10).ToArray();

                foreach (var businessclass in businessClasses)
                {
                    //for testing the values
                    //string RCDID = log.RCDID;
                    //string CHG_DESCRIPTION = log.CHG_DESCRIPTION;
                    //string CREATEDBY = log.CREATEDBY;
                    //string APPROVEDBY = log.APPROVEDBY;
                    //string MODIFIEDDATE = String.Format("MM/dd/yyyy", log.MODIFIEDDATE);
                    //string CREATEDATE = String.Format("MM/dd/yyyy", log.CREATEDATE);

                    BusinessClassList.Add(businessclass);
                }
                conn.Commit();
            }
            //
            string json = JsonConvert.SerializeObject(BusinessClassList);
            return Json(BusinessClassList);
        }

        protected virtual bool AuthorizeCore(System.Web.HttpContextBase httpContext) {
            return true;
        }


        // GET: api/BusinessClass/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/BusinessClass
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/BusinessClass/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

    }
}
