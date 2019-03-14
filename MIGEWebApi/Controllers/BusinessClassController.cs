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
using MigraDoc;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;

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
                                            PRMSTE  = DWXF710.PRMSTE,
                                            DESC    = DWXF710.DESC,
                                            CLASX   = DWXF710.CLASX,
                                            MAPCLS  = DWXF710.MAPCLS,
                                            AUTCLS = DWXF710.AUTCLS,
                                            MAPMS   = DWXF710.MAPMS,
                                            MAPDSR  = DWXF710.MAPDSR,
                                            AUTDSR  = DWXF710.AUTDSR,
                                            PROPDSR = DWXF710.PROPDSR,
                                            GLDSR   = DWXF710.GLDSR,
                                            WCDSR   = DWXF710.WCDSR,
                                            CAUTDSR = DWXF710.CAUTDSR,
                                            COVDSR  = DWXF710.COVDSR,
                                            CUPDSR  = DWXF710.CUPDSR,
                                            RCDID   = DWXF710.RCDID
                                       }).Take(100).ToList();
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

                XFont font = new XFont("Verdana", 10, XFontStyle.Bold);
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Center;
                format.LineAlignment = XLineAlignment.Far;
                XGraphics gfx;
                XRect box;
                PdfDocument inputDocument1 = PdfReader.Open("pdf/RAG-Quick_Reference_All.pdf", PdfDocumentOpenMode.Import);

                PdfPage page1 = inputDocument1.Pages[0];
                PdfPage page2 = inputDocument1.Pages[1];

                //output document
                page1 = pdf.AddPage(page1);
                page2 = pdf.AddPage(page2);

                // Write document file name and page number on each page
                gfx = XGraphics.FromPdfPage(page1);
                //box = page1.MediaBox.ToXRect();
                //box.Inflate(0, -10);
                //gfx.DrawString(String.Format("{0} • {1}", pdfFilename, 0),font, XBrushes.Red, box, format);

                gfx = XGraphics.FromPdfPage(page2);
                //box = page2.MediaBox.ToXRect();
                //box.Inflate(0, -10);
                //gfx.DrawString(String.Format("{0} • {1}", pdfFilename, 1),font, XBrushes.Red, box, format);

                TableBuilder(pdf, BusinessClassList);
                

                pdf.Save(pdfFilename);              


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        static void TableBuilder(PdfDocument document, List<DWXF710> BusinessClassList)
        {

            PdfPage page;// = document.AddPage();
            //PageSetup ps = new PageSetup();
            //ps.PageWidth = 100;            
            XGraphics gfx;// = XGraphics.FromPdfPage(page);
                          //gfx.MUH = PdfFontEncoding.Unicode;
            
            // You always need a MigraDoc document for rendering.
            Document doc = new Document();
            Section section = doc.AddSection();
            
            Color TableBorderColor = new Color(0,0,0);
            Color TableHeaderColor = new Color(255, 255, 255);
            Color TableGrey = new Color(217, 217, 217);

            //center the table on the page
            //doc.LastSection.AddParagraph("Simple Tables", "Heading2");
            //TextFrame addressFrame;
            //addressFrame = section.AddTextFrame();
            //addressFrame.LineFormat.Width = 0.5; //Only for visual purposes 
            //addressFrame.Height = "15.0cm";//any number 
            //addressFrame.Width = "10.0cm";//sum of col widths 
            //addressFrame.Left = ShapePosition.Center;
            //addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;//irrelevant 
            //addressFrame.Top = "10.0cm";//irrelevant 
            //addressFrame.RelativeVertical = RelativeVertical.Page;//irrelevant 
            //Table table = addressFrame.AddTable();
            

            //
            var datachunks = SplitList(BusinessClassList);
            var totpages = datachunks.Count();
            foreach (var chunk in datachunks.Select((value, index) => new { Value = value, Index = index }))
            //foreach (var chunk in datachunks)
            {
                //Table table = new Table();
                Table table = section.AddTable();
                table.Style = "Table";
                table.Borders.Color = TableBorderColor;
                table.Borders.Width = 0.25;
                table.Borders.Left.Width = 0.5;
                table.Borders.Right.Width = 0.5;
                table.Rows.LeftIndent = "10cm";

                //section.PageSetup = doc.DefaultPageSetup.Clone();
                //var tableWidth = Unit.FromCentimeter(1);
                //table.AddColumn(tableWidth);
                //var leftIndentToCenterTable = (section.PageSetup.PageWidth.Centimeter -
                //                               section.PageSetup.LeftMargin.Centimeter -
                //                               section.PageSetup.RightMargin.Centimeter -
                //                               tableWidth.Centimeter) / 2;
                //table.Rows.LeftIndent = leftIndentToCenterTable;

                // Before you can add a row, you must define the columns
                Column column = table.AddColumn("7cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("1.7cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("2.1cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                // Create the header of the table
                Row row = table.AddRow();

                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Format.Font.Size = 8;
                row.TopPadding = 12;
                row.BottomPadding = 1;
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
                row.Cells[11].Format.Alignment = ParagraphAlignment.Center;
                

                foreach (var bc in chunk.Value)
                {
                    Row datarow = table.AddRow();
                    datarow.Format.Font.Bold = false;
                    datarow.Format.Font.Size = 6;
                    datarow.TopPadding = 3;
                    datarow.BottomPadding = 3;

                    if (datarow.Index % 2 != 0)
                    {
                        datarow.Shading.Color = TableGrey;
                    }

                    datarow.Cells[0].AddParagraph(bc.DESC);
                    datarow.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                    datarow.Cells[1].AddParagraph(bc.MAPCLS);
                    datarow.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                    datarow.Cells[2].AddParagraph(bc.CLASX);
                    datarow.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                    datarow.Cells[3].AddParagraph((bc.AUTCLS == null) ? "" : bc.AUTCLS);
                    datarow.Cells[3].Format.Alignment = ParagraphAlignment.Center;

                    datarow.Cells[4].AddParagraph((bc.MAPMS == null) ? "" : bc.MAPMS);
                    datarow.Cells[4].Format.Alignment = ParagraphAlignment.Center;

                    //datarow.Cells[5].AddParagraph((bc.MAPDSR == null) ? "" : bc.MAPDSR);
                    datarow.Cells[5].AddImage("images/green-sm.gif");
                    datarow.Cells[5].Format.Alignment = ParagraphAlignment.Center;

                    datarow.Cells[6].AddParagraph((bc.AUTDSR == null) ? "" : bc.AUTDSR);
                    datarow.Cells[6].Format.Alignment = ParagraphAlignment.Center;

                    datarow.Cells[7].AddParagraph((bc.PROPDSR == null) ? "" : bc.PROPDSR);
                    datarow.Cells[7].Format.Alignment = ParagraphAlignment.Center;

                    datarow.Cells[8].AddParagraph((bc.GLDSR == null) ? "" : bc.GLDSR);
                    datarow.Cells[8].Format.Alignment = ParagraphAlignment.Center;

                    datarow.Cells[9].AddParagraph((bc.WCDSR == null) ? "" : bc.WCDSR);
                    row.Cells[9].Format.Alignment = ParagraphAlignment.Center;

                    datarow.Cells[10].AddParagraph((bc.CAUTDSR == null) ? "" : bc.CAUTDSR);
                    datarow.Cells[10].Format.Alignment = ParagraphAlignment.Center;

                    datarow.Cells[11].AddParagraph((bc.CUPDSR == null) ? "" : bc.CUPDSR);
                    datarow.Cells[11].Format.Alignment = ParagraphAlignment.Center;
                   
                }
                
                //
                page = document.AddPage();
                page.Orientation = PageOrientation.Landscape;
                gfx = XGraphics.FromPdfPage(page);
                gfx.MUH = PdfFontEncoding.Unicode;

               

                DrawTitle(page, gfx, chunk.Index, totpages);

                // Create a renderer and prepare (=layout) the document
                MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(doc);
                docRenderer.PrepareDocument();                

                // Render the paragraph. You can render tables or shapes the same way.
                docRenderer.RenderObject(gfx, XUnit.FromCentimeter(2), XUnit.FromCentimeter(2), "0cm", table);

                table.SetEdge(0, 0, 12, 1, Edge.Box, BorderStyle.Single, 0.55, Color.Empty);

            }            

        }

        //public void DrawFooter(PdfPage page, XGraphics gfx) {


        //}

        // ** chunk size here determines the number of pages ** 
        public static IEnumerable<List<T>>SplitList<T>(List<T> businessclasses, int nSize = 29)
        {
            for (int i = 0; i < businessclasses.Count; i += nSize)
            {
                yield return businessclasses.GetRange(i, Math.Min(nSize, businessclasses.Count - i));
            }
        }


        public static void DrawTitle(PdfPage page, XGraphics gfx, int pageindex, int totalPages)
        {
            
            const string facename    = "Times New Roman";
            const string lftnavtitle = "Risk Appetite Guide - Quick Reference (All States)";
            const string rtnavtitle1 = "Merchants Mutual Insurance Company";
            const string rtnavtitle2 = "Merchants Preferred Insurance Company";
            //XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.WinAnsi, PdfFontEmbedding.Default);        

            XFont rtnavtitleFont = new XFont(facename, 14, XFontStyle.Italic);
            XFont lftnavtitleFont = new XFont("Arial", 15, XFontStyle.Regular);

            XRect rect = new XRect(new XPoint(-15,0), gfx.PageSize);
            rect.Inflate(-10, -15);
            
            XRect rect2 = new XRect(new XPoint(-15,15), gfx.PageSize);
            rect2.Inflate(-10, -15);

            XRect rect3 = new XRect(new XPoint(35,20), gfx.PageSize);

            gfx.DrawString(rtnavtitle1, rtnavtitleFont, XBrushes.DarkSlateGray, rect, XStringFormats.TopRight);
            gfx.DrawString(rtnavtitle2, rtnavtitleFont, XBrushes.DarkSlateGray, rect2, XStringFormats.TopRight);
            gfx.DrawString(lftnavtitle, lftnavtitleFont, XBrushes.DarkSlateGray, rect3, XStringFormats.TopLeft);


            //DRAW THE FOOTER
            rect.Offset(40, -20);
            XFont ftrfont = new XFont("Times New Roman", 6, XFontStyle.Regular);
            XStringFormat format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Far;

            gfx.DrawString("Risk Appetite Guide - Quick Reference (All States)", ftrfont, XBrushes.DarkSlateGray, rect, format);

            XRect rect4 = new XRect(new XPoint(), gfx.PageSize);
            rect4.Offset(35, -27);
            gfx.DrawString("062017 - Proprietary & Confidential", ftrfont, XBrushes.DarkSlateGray, rect4, format);

            //
            XStringFormat format2 = new XStringFormat();
            XRect rect5 = new XRect(new XPoint(), gfx.PageSize);
            rect5.Offset(-45, -33);
            format2.Alignment = XStringAlignment.Far;
            format2.LineAlignment = XLineAlignment.Far;
            gfx.DrawString("page " + (pageindex + 1) + " of " + totalPages  + " pages", ftrfont, XBrushes.DarkSlateGray, rect5, format2);

        }

        public class LayoutHelper
        {
            private readonly PdfDocument _document;
            private readonly XUnit _topPosition;
            private readonly XUnit _bottomMargin;
            private XUnit _currentPosition;

            public LayoutHelper(PdfDocument document, XUnit topPosition, XUnit bottomMargin)
            {
                _document = document;
                _topPosition = topPosition;
                _bottomMargin = bottomMargin;
                // Set a value outside the page - a new page will be created on the first request.
                _currentPosition = bottomMargin + 10000;
            }

            public XUnit GetLinePosition(XUnit requestedHeight)
            {
                return GetLinePosition(requestedHeight, -1f);
            }

            public XUnit GetLinePosition(XUnit requestedHeight, XUnit requiredHeight)
            {
                XUnit required = requiredHeight == -1f ? requestedHeight : requiredHeight;
                if (_currentPosition + required > _bottomMargin)
                    CreatePage();
                XUnit result = _currentPosition;
                _currentPosition += requestedHeight;
                return result;
            }

            public XGraphics Gfx { get; private set; }
            public PdfPage Page { get; private set; }

            void CreatePage()
            {
                Page = _document.AddPage();
                Page.Size = PageSize.A4;
                Gfx = XGraphics.FromPdfPage(Page);
                _currentPosition = _topPosition;
            }
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

            if (BusinessClass != null)
            {

                return Json(BusinessClass, settings);
                //return new OkObjectResult(BusinessClass);
            }
            else
            {
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