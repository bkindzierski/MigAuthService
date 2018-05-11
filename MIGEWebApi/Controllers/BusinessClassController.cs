using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//
using NHibernate.Linq;
using Newtonsoft.Json;
using RagAppGuide.DAL;
using RagAppGuide.DAL.Models;

//
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Serialization;

namespace MIGEWebApi.Controllers
{
   //my gitrepo tester
    public class BusinessClassController : Controller
    {

        [HttpGet]
        [Route("api/BusinessClass")]
        [Produces("application/json")]
        //[Authorize("MIGEAuthorize")] // <--custom policy to implement
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [EnableCors("MyPolicy")]
        public JsonResult Get()
        {

            //not needed here, really just a tester
            //var identity = User.Identity as ClaimsIdentity;
            //string token = null;
            ////get authorization headers
            //var authorization = this.HttpContext.Request.Headers["Authorization"];
            //foreach (string val in authorization)
            //{
            //    token = val.Replace("Bearer", "").Trim();
            //}

            List<DWXF710> BusinessClassList = new List<DWXF710>();

            //nhibernate connect 
            NHibernateSession nh = new NHibernateSession();
            var sessionFactory = nh.session;

            //read back the new entry
            using (var session = sessionFactory.OpenSession())
            using (var conn = session.BeginTransaction())
            {
                //perform db logic
                BusinessClassList = (from DWXF710 in session.Query<DWXF710>()
                                       where DWXF710.CLASX == "013930"
                                       //&& DWXF710T.PRMSTE == "20"
                                       select DWXF710).Take(10).ToList();
                conn.Commit();
            }
            
            // --?? does not work for angular --??
            //return json = JsonConvert.SerializeObject(BusinessClassList);
            //return Json(BusinessClassList);
            
            //
            JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            return Json(BusinessClassList, settings);

        }

        [HttpPost]
        [Route("api/ProxyPostCall")]
        [Produces("application/json")]
        //[Authorize("MIGEAuthorize")] // <--custom policy to implement
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [EnableCors("MyPolicy")]
        //public IActionResult postNewBusinessClass([FromBody] DWXF710 postdata)
        public JsonResult postNewBusinessClass([FromBody] DWXF710 postdata)
        {

            JsonSerializerSettings settings = new JsonSerializerSettings(); //{ Formatting = Formatting.Indented };
            settings.ContractResolver = new UppercaseContractResolver();
            string json = JsonConvert.SerializeObject(postdata, Formatting.Indented, settings);

            //nhibernate connect 
            NHibernateSession nh = new NHibernateSession();
            var sessionFactory = nh.session;

            DWXF710 BusinessClass = new DWXF710();
            DWXF710 postBusinessClass = new DWXF710();
            postBusinessClass = postdata;

            //would insert new record here and return the row
            using (var session = sessionFactory.OpenSession())
            using (var conn = session.BeginTransaction())
            {
                //perform db logic
                BusinessClass = (from DWXF710 in session.Query<DWXF710>()
                                     where DWXF710.RCDID == 25
                                     select DWXF710).SingleOrDefault();
                conn.Commit();
            }

            if (BusinessClass != null){
                //return Ok(200);
                return Json(BusinessClass, settings);
                //return new OkObjectResult(BusinessClass);
            }
            else {
                return null;
            }


        }

        public class UppercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToUpper();
            }
        }

    }
}