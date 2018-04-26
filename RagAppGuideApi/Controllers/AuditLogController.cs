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


namespace RagAppGuideApi.Controllers
{
    [Produces("application/json")]
    [Route("api/AuditLog")]
    public class AuditLogController : Controller
    {
        // GET: api/AuditLog
        [HttpGet]
        public JsonResult Get()
        {
            List<DWXF712> AuditList = new List<DWXF712>();

            //nhibernate connect 
            NHibernateSession nh = new NHibernateSession();            
            var sessionFactory = nh.session;

            //SAVE new entry
            using (var session = sessionFactory.OpenSession())
            using (var conn = session.BeginTransaction())
            {
                //grab last row
                var auditLogRow = (from DWXF712 in session.Query<DWXF712>()
                                   orderby DWXF712.ID descending
                                   select DWXF712).Take(1);

                var alRow = auditLogRow.FirstOrDefault();
                int newRowID = alRow.ID + 1;

                var newLog = new DWXF712
                {
                    //ID = 5, <-- specified in mapping file
                    RCDID = 7401,
                    CHNGDESC = "New Change - " + newRowID,
                    CREATEBY = "kindzieb",
                    APPROVBY = "kindzieb",
                    MDFYDATE = 20171010,
                    CREATDTE = 20171010
                };

                //session.Save(newLog);
                conn.Commit();
            }


            //read back the new entry
            using (var session = sessionFactory.OpenSession())
            using (var conn = session.BeginTransaction())
            {
                //perform db logic
                var auditlogs = from Audit_Log in session.Query<DWXF712>()
                                select Audit_Log;

                foreach (var log in auditlogs)
                {
                    //for testing the values
                    //string RCDID = log.RCDID;
                    //string CHG_DESCRIPTION = log.CHG_DESCRIPTION;
                    //string CREATEDBY = log.CREATEDBY;
                    //string APPROVEDBY = log.APPROVEDBY;
                    //string MODIFIEDDATE = String.Format("MM/dd/yyyy", log.MODIFIEDDATE);
                    //string CREATEDATE = String.Format("MM/dd/yyyy", log.CREATEDATE);

                    AuditList.Add(log);
                }
                conn.Commit();
            }

            //
            string json = JsonConvert.SerializeObject(AuditList);
            return Json(AuditList);
        }


        // GET: api/AuditLog/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}


        //// POST: api/AuditLog
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/AuditLog/5
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
