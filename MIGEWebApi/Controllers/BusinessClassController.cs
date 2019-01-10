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
using PdfSharp.Pdf;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Serialization;
using PdfSharp.Drawing;

namespace MIGEWebApi.Controllers
{
   //my gitrepo tester
    public class BusinessClassController : Controller
    {

        [HttpGet]
        [Route("api/BusinessClass")]
        [Produces("application/json")]
        //[Authorize("MIGEAuthorize")] // <--custom policy to implement
        //[Authorize(Roles = "Admin")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[EnableCors("MyPolicy")]
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
                                         //where DWXF710.CLASX == "013930"
                                     where DWXF710.PRMSTE == "20"
                                     select new DWXF710 {
                                            PRMSTE = DWXF710.PRMSTE,
                                            DESC = DWXF710.DESC,
                                            CLASX = DWXF710.CLASX,
                                            MAPCLS = DWXF710.MAPCLS,
                                            MAPMS = DWXF710.MAPMS,
                                            MAPDSR = DWXF710.MAPDSR,
                                            AUTDSR = DWXF710.AUTDSR,
                                            PROPDSR = DWXF710.PROPDSR,
                                            GLDSR = DWXF710.GLDSR,
                                            WCDSR = DWXF710.WCDSR,
                                            CAUTDSR = DWXF710.CAUTDSR,
                                            COVDSR = DWXF710.COVDSR,
                                            CUPDSR = DWXF710.CUPDSR
                                       }).ToList();
                conn.Commit();
            }

            CreatePDF(BusinessClassList);
            // --?? does not work for angular --??
            //return json = JsonConvert.SerializeObject(BusinessClassList);
            //return Json(BusinessClassList);
            
            //
            JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            return Json(BusinessClassList, settings);

        }



        public void CreatePDF(List<DWXF710> BusinessClassList)
        {
            try
            {
                int yPoint = 0;

                PdfDocument pdf = new PdfDocument();
                pdf.Info.Title = "Merchants Appetite Guide";

                PdfPage pdfPage = pdf.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(pdfPage);


                //
                string title = "Merchants Risk Appetite Guide";
                XFont titleFont = new XFont("Verdana", 14, XFontStyle.Bold);
                XRect rect = new XRect(new XPoint(), gfx.PageSize);
                rect.Inflate(-10, -15);
                gfx.DrawString(title, titleFont, XBrushes.MidnightBlue, rect, XStringFormats.TopCenter);

                //rect.Offset(0, 5);
                //XStringFormat format = new XStringFormat();
                //format.Alignment = XStringAlignment.Near;
                //format.LineAlignment = XLineAlignment.Far;
                //gfx.DrawString("Created with " + PdfSharp.ProductVersionInfo.Producer, font, XBrushes.DarkOrchid, rect, format);

                //format.Alignment = XStringAlignment.Center;
                //gfx.DrawString(pdf.PageCount.ToString(), titleFont, XBrushes.DarkOrchid, rect, format);
                //pdf.Outlines.Add(title, pdfPage, true);




                XFont font = new XFont("Verdana",5, XFontStyle.Regular);
                yPoint = yPoint + 40;

                gfx.DrawString("Description", font, XBrushes.Black, new XRect(40, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                gfx.DrawString("Map Class Code", font, XBrushes.Black, new XRect(280, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                gfx.DrawString("ISO GL Class Code", font, XBrushes.Black, new XRect(420, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                yPoint = yPoint + 10;

                foreach (var bclass in BusinessClassList)
                {

                    gfx.DrawString(bclass.DESC, font, XBrushes.Black, new XRect(40, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    gfx.DrawString(bclass.MAPCLS, font, XBrushes.Black, new XRect(280, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    gfx.DrawString(bclass.CLASX, font, XBrushes.Black, new XRect(420, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    yPoint = yPoint + 10;
                }
                string pdfFilename = "MerchantsAppetiteGuide.pdf";
                pdf.Save(pdfFilename);

                //Process.Start(pdfFilename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void DrawTitle(PdfPage page, XGraphics gfx, string title)
        //{
        //    XRect rect = new XRect(new XPoint(), gfx.PageSize);
        //    rect.Inflate(-10, -15);
        //    XFont font = new XFont("Verdana", 14, XFontStyle.Bold);
        //    gfx.DrawString(title, font, XBrushes.MidnightBlue, rect, XStringFormats.TopCenter);

        //    rect.Offset(0, 5);
        //    font = new XFont("Verdana", 8, XFontStyle.Italic);
        //    XStringFormat format = new XStringFormat();
        //    format.Alignment = XStringAlignment.Near;
        //    format.LineAlignment = XLineAlignment.Far;
        //    gfx.DrawString("Created with " + PdfSharp.ProductVersionInfo.Producer, font, XBrushes.DarkOrchid, rect, format);

        //    font = new XFont("Verdana", 8);
        //    format.Alignment = XStringAlignment.Center;
        //    gfx.DrawString(Program.s_document.PageCount.ToString(), font, XBrushes.DarkOrchid, rect, format);

        //    Program.s_document.Outlines.Add(title, page, true);
        //}




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
            settings.ContractResolver = new UppercaseContractResolver(); // <-- needed for response to be in upper case ?? new thing or something
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
                                     //where DWXF710.RCDID == 25
                                     select DWXF710).SingleOrDefault();
                conn.Commit();
            }

            if (BusinessClass != null){

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