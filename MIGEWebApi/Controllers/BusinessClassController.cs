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
using PdfSharp.Drawing;
using MigraDoc;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;


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

                string pdfFilename = "MerchantsAppetiteGuide.pdf";
                
                TableBuilder(pdf, BusinessClassList);

                pdf.Save(pdfFilename);


                //PdfPage pdfPage = pdf.AddPage();
                //XGraphics gfx = XGraphics.FromPdfPage(pdfPage);
                //
                //DrawTitle(pdfPage, gfx, "Merchants Risk Appetite Guide");

                // XFont font = new XFont("Verdana",5, XFontStyle.Regular);
                //yPoint = yPoint + 40;

                //gfx.DrawString("Description", font, XBrushes.Black, new XRect(40, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                //gfx.DrawString("Map Class Code", font, XBrushes.Black, new XRect(280, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                //gfx.DrawString("ISO GL Class Code", font, XBrushes.Black, new XRect(420, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                //yPoint = yPoint + 10;

                //foreach (var bclass in BusinessClassList)
                //{

                //    gfx.DrawString(bclass.DESC, font, XBrushes.Black, new XRect(40, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                //    gfx.DrawString(bclass.MAPCLS, font, XBrushes.Black, new XRect(280, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                //    gfx.DrawString(bclass.CLASX, font, XBrushes.Black, new XRect(420, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                //    yPoint = yPoint + 10;
                //}


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawTitle(PdfPage page, XGraphics gfx, string title)
        {
                //string title = "Merchants Risk Appetite Guide";
                XFont titleFont = new XFont("Verdana", 14, XFontStyle.Bold);
                XRect rect = new XRect(new XPoint(), gfx.PageSize);
                rect.Inflate(-10, -15);
                gfx.DrawString(title, titleFont, XBrushes.MidnightBlue, rect, XStringFormats.TopCenter);               
        }

        static void TableBuilder(PdfDocument document, List<DWXF710> BusinessClassList)
        {
            PdfPage page = document.AddPage();
            //PageSetup ps = new PageSetup();
            //ps.PageWidth = 100;            

            
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // 
            gfx.MUH = PdfFontEncoding.Unicode;

            // You always need a MigraDoc document for rendering.
            Document doc = new Document();
            Section section = doc.AddSection();
            //section.AddTable();
            
            Color TableBorderColor = new Color(0,0,0);
            Color TableHeaderColor = new Color(255, 255, 255);
            Color TableGrey = new Color(217, 217, 217);

            doc.LastSection.AddParagraph("Simple Tables", "Heading2");
                        
            //Table table = new Table();
            Table table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = TableBorderColor;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 10;
            
            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.7cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            Row row = table.AddRow();           

            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 7;
            row.Shading.Color = TableHeaderColor;
            row.Cells[0].AddParagraph("Business Description");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("MAP Class Code");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[2].AddParagraph("ISO GLClass Code");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[3].AddParagraph("Auto Repair Class Code");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[4].AddParagraph("Market Segment");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[5].AddParagraph("MAP Symbol");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[6].AddParagraph("Auto Repair Symbol");
            row.Cells[6].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[7].AddParagraph("Property Symbol");
            row.Cells[7].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[8].AddParagraph("General Liability Symbol");
            row.Cells[8].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[9].AddParagraph("Worker's Comp Symbol");
            row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[10].AddParagraph("Commericial Auto Symbol");
            row.Cells[10].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[11].AddParagraph("Umbrella Symbol");
            row.Cells[11].Format.Alignment = ParagraphAlignment.Left;


            var i = 0;
            foreach (var bclass in BusinessClassList)
            {
                row = table.AddRow();
                row.Format.Font.Bold = false;
                row.Format.Font.Size = 7;

                if (row.Index % 2 != 0)
                {
                    row.Shading.Color = TableGrey;
                }

                row.Cells[0].AddParagraph(bclass.DESC);
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                row.Cells[1].AddParagraph(bclass.MAPCLS);
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[2].AddParagraph(bclass.CLASX);
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[3].AddParagraph((bclass.AUTCLS == null) ? "" : bclass.AUTCLS);
                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[4].AddParagraph((bclass.AUTMS == null) ? "" : bclass.AUTMS);
                row.Cells[4].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[5].AddParagraph((bclass.MAPDSR == null) ? "" : bclass.MAPDSR);
                row.Cells[5].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[6].AddParagraph((bclass.AUTDSR == null) ? "" : bclass.AUTDSR);
                row.Cells[6].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[7].AddParagraph((bclass.PROPDSR == null) ? "" : bclass.PROPDSR);
                row.Cells[7].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[8].AddParagraph((bclass.GLDSR == null) ? "" : bclass.GLDSR);
                row.Cells[8].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[9].AddParagraph((bclass.WCDSR == null) ? "" : bclass.WCDSR);
                row.Cells[9].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[10].AddParagraph((bclass.CAUTDSR == null) ? "" : bclass.CAUTDSR);
                row.Cells[10].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[11].AddParagraph((bclass.CUPDSR == null) ? "" : bclass.CUPDSR);
                row.Cells[11].Format.Alignment = ParagraphAlignment.Left;
            }


            table.SetEdge(0, 0, 12, 1, Edge.Box, BorderStyle.Single, 0.55, Color.Empty);


            // Create a renderer and prepare (=layout) the document
            MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(doc);
            docRenderer.PrepareDocument();

            // Render the paragraph. You can render tables or shapes the same way.
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1), XUnit.FromCentimeter(1), "12cm", table);
            
        }

        static void SamplePage1(PdfDocument document)
        {
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // HACK²
            gfx.MUH = PdfFontEncoding.Unicode;
            //gfx.MFEH = PdfFontEmbedding.Default;

            XFont font = new XFont("Verdana", 13, XFontStyle.Bold);

            gfx.DrawString("The following paragraph was rendered using MigraDoc:", font, XBrushes.Black,
              new XRect(100, 100, page.Width - 200, 300), XStringFormats.Center);

            // You always need a MigraDoc document for rendering.
            Document doc = new Document();
            Section sec = doc.AddSection();
            // Add a single paragraph with some text and format information.
            Paragraph para = sec.AddParagraph();
            para.Format.Alignment = ParagraphAlignment.Justify;
            para.Format.Font.Name = "Times New Roman";
            para.Format.Font.Size = 12;
            para.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.DarkGray;
            para.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.DarkGray;
            para.AddText("Duisism odigna acipsum delesenisl ");
            para.AddFormattedText("ullum in velenit", TextFormat.Bold);
            para.AddText(" ipit iurero dolum zzriliquisis nit wis dolore vel et nonsequipit, velendigna " +
              "auguercilit lor se dipisl duismod tatem zzrit at laore magna feummod oloborting ea con vel " +
              "essit augiati onsequat luptat nos diatum vel ullum illummy nonsent nit ipis et nonsequis " +
              "niation utpat. Odolobor augait et non etueril landre min ut ulla feugiam commodo lortie ex " +
              "essent augait el ing eumsan hendre feugait prat augiatem amconul laoreet. ≤≥≈≠");
            para.Format.Borders.Distance = "5pt";
            para.Format.Borders.Color = Colors.Gold;

            // Create a renderer and prepare (=layout) the document
            MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(doc);
            docRenderer.PrepareDocument();

            // Render the paragraph. You can render tables or shapes the same way.
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(5), XUnit.FromCentimeter(10), "12cm", para);
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