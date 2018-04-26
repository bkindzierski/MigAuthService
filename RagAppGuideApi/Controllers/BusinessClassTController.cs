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

using System.Net.Http;

namespace RagAppGuideApi.Controllers
{
    //[Produces("application/json")]
   
    public class BusinessClassTController : Controller
    {
        //nhibernate connect
        NHibernateSession nh = new NHibernateSession();
       
        // GET: api/BusinessClassT
        [HttpGet]
        [Route("api/BusinessClassT")]
        public JsonResult Get()
        {
            List<DWXF710T> BusinessClassListT = new List<DWXF710T>();
            var sessionFactory = nh.session;
            
            //read back the new entry
            using (var session = sessionFactory.OpenSession())
            using (var conn = session.BeginTransaction())
            {
                //perform db logic
                var businessClassesT = (from DWXF710T in session.Query<DWXF710T>()
                                       where DWXF710T.RCDID == 9409
                                        //where DWXF710T.CLASX == "068706"
                                        //&& DWXF710T.PRMSTE == "20"
                                        select DWXF710T).ToArray();

                foreach (var businessclass in businessClassesT)
                {
                    //for testing the values
                    //string RCDID = log.RCDID;
                    //string CHG_DESCRIPTION = log.CHG_DESCRIPTION;
                    //string CREATEDBY = log.CREATEDBY;
                    //string APPROVEDBY = log.APPROVEDBY;
                    //string MODIFIEDDATE = String.Format("MM/dd/yyyy", log.MODIFIEDDATE);
                    //string CREATEDATE = String.Format("MM/dd/yyyy", log.CREATEDATE);

                    BusinessClassListT.Add(businessclass);
                }
                conn.Commit();
            }
            //
            string json = JsonConvert.SerializeObject(BusinessClassListT);
            return Json(BusinessClassListT);
        }

        // GET: api/BusinessClassT/5
        //HttpGet("{id}", Name = "Get")]
        [HttpGet]
        [Route("api/BusinessClassT/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        [Route("api/BusinessClassT/GetHazardGrades")]
        public JsonResult GetHazardGrades()
        {
            
            List<DWXF711> HazardGradeList = new List<DWXF711>();
            var sessionFactory = nh.session;

            //read back the new entry
            using (var session = sessionFactory.OpenSession())
            using (var conn = session.BeginTransaction())
            {
                //perform db logic
                var hazardGrades = (from DWXF711 in session.Query<DWXF711>()
                                        where DWXF711.RCDID >= 9000
                                        orderby DWXF711.RCDID
                                        select DWXF711).ToArray();


                foreach (var hgrade in hazardGrades)
                {                   
                    HazardGradeList.Add(hgrade);
                }
                conn.Commit();
            }
            //
            string jsonString = JsonConvert.SerializeObject(HazardGradeList);
            return Json(HazardGradeList);
        }

        // POST: api/BusinessClassT
        [HttpPost]
        [Route("api/BusinessClassT")]        
        public HttpResponseMessage Post([FromBody] DWXF710T obj)
        {
            
            var sessionFactory = nh.session;

            // ** SAVE new entry
            using (var session = sessionFactory.OpenSession())
            using (var conn = session.BeginTransaction())
            {
                //grab last row
                var auditLogRow = (from DWXF710T in session.Query<DWXF710T>()
                                   orderby DWXF710T.RCDID descending
                                   select DWXF710T).Take(1);

                var alRow = auditLogRow.FirstOrDefault();
                int NextRecordID = alRow.RCDID + 1;


                obj.DESC = obj.DESC + " - kindzieb " + NextRecordID;
                //
                session.Save(obj);
                conn.Commit();

                //var newBusinessClass = new DWXF710T
                //{

                //    PRMSTE = "44",
                //    DESC = "Storage/Retail Building - kindzieb " + NextRecordID,
                //    CLASX = "068706",
                //    CLSSEQ = "09",
                //    EFFDTE = 20171010,
                //    ENDDTE = 20171101,
                //    RNWEFF = 20171101,
                //    RNWEXP = 0,
                //    WEBCLS = "",REFDSC = "",REFCLS = "",REFSEQ = "",
                //    MAPCLS = "",AUTCLS = "",COVCLS = "",SICCDE = "",MAPMS = "",AUTMS = "",
                //    COVMS = "",MAPDSR = "",AUTDSR = "",PROPDSR = "",GLDSR = "",WCDSR = "",
                //    CAUTDSR = "",COVDSR = "",CUPDSR = "",MAPCMT = "",AUTCMT = "",PROPCMT = "",
                //    GLCMT = "",WCCMT = "",CAUTCMT = "",COVCMT = "",CUPCMT = "",SPCTYP = "",
                //    CLSDED = 0,
                //    SUBMITBY = "kindzieb",
                //    APPROVBY = "",
                //    ENVSTUS = "UA",
                //    TRANTYPE = "New"
                //    //RCDID = NextRecordID, //<--no good yo.-?

                //};

                //session.Save(newBusinessClass);
                //conn.Commit();
            }

            HttpResponseMessage msg = new HttpResponseMessage();
            return msg;
        }

        // PUT: api/BusinessClassT/5
        [HttpPut("{id}")]
        [Route("api/BusinessClassT/{rcdid}")]
        public HttpResponseMessage Put(int rcdid, [FromBody] DWXF710T obj)
        {
            var sessionFactory = nh.session;

            // ** UPDATE business class entry
            using (var session = sessionFactory.OpenSession())
            using (var conn = session.BeginTransaction())
            {

                var bClass = from DWXF710T in session.Query<DWXF710T>()
                             where DWXF710T.RCDID == rcdid
                             select DWXF710T;
                var bc = bClass.FirstOrDefault();

                bc.EFFDTE = obj.EFFDTE;
                bc.ENDDTE = obj.ENDDTE;
                bc.RNWEFF = obj.RNWEFF;
                bc.RNWEXP = obj.RNWEXP;
                session.Update(bc);
                //
                conn.Commit();
            }

            HttpResponseMessage msg = new HttpResponseMessage();
            return msg;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


    }   
}
