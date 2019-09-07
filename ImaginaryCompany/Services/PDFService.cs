using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ImaginaryCompany.Data;
using ImaginaryCompany.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ImaginaryCompany.Services
{
    public class PDFService : I_PDFService
    {

        private readonly CompanyContext _db;                      /// Database context 
        private readonly IHostingEnvironment _hostingEnvironment; /// Generating File on Server


        public PDFService (
                            CompanyContext db,
                            IHostingEnvironment hostingEnvironment
                           )
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }// end constructor

        /// <summary>
        /// Ref: How to make a table in PDF => https://www.youtube.com/watch?v=JBSt2L_faD8
        /// https://www.youtube.com/watch?v=MurNTcbsWkI
        /// Column:  https://www.mikesdotnetting.com/article/89/itextsharp-page-layout-with-columns
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="filenameShort"></param>
        /// <param name="donationId"></param>
        public void GenerateProductTemplate (string filePath, List<Product> productList)
        {
            try
            {
                /// Creating a A4 size doc
                var pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);



                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    var writer = PdfWriter.GetInstance(pdfDoc, fs);

                    var labelFont_9 = FontFactory.GetFont(FontFactory.HELVETICA, 9, new BaseColor(76, 76, 76));
                    var labelFont_9_Bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, new BaseColor(76, 76, 76));


                    pdfDoc.Open();

                    /// Adding Image
                    var p = new Paragraph(); 
                    var imgPath = $"{_hostingEnvironment.WebRootPath}\\images\\Gimp.png";
                    var img = Image.GetInstance(imgPath);
                    img.ScaleToFit(100, 100);
                    p.Add(new Chunk(img, 20, 0, true)); 
                    pdfDoc.Add(p);

                    /// Adding the Paragraph
                    p = new Paragraph(new Chunk("Imaginary Company", labelFont_9_Bold));
                    p.IndentationLeft = 40f;
                    p.SpacingBefore = -35f; 
                    pdfDoc.Add(p);

                    p = new Paragraph(new Chunk("PDF is created with iTextSharp", labelFont_9));
                    p.IndentationLeft = 40f;
                    pdfDoc.Add(p);


                    /// Creating a table
                    var productTable = new PdfPTable(4);
                    productTable.SpacingBefore = 20f; 

                    productTable.SetTotalWidth(new float[] { 100f, 200f, 100f, 100f });
                    // productTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    //productTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    //productTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;


                    /// Creating headers 
                    productTable.AddCell(CustomAlignMethod_BorderBottom("Id", labelFont_9_Bold, Element.ALIGN_LEFT));
                    productTable.AddCell(CustomAlignMethod_BorderBottom("Product Name", labelFont_9_Bold, Element.ALIGN_LEFT));
                    productTable.AddCell(CustomAlignMethod_BorderBottom("Quantity", labelFont_9_Bold, Element.ALIGN_LEFT));
                    productTable.AddCell(CustomAlignMethod_BorderBottom("Price", labelFont_9_Bold, Element.ALIGN_LEFT));


                    /// Creating Body
                    foreach (var product in productList)
                    {
                        productTable.AddCell(CustomAlignMethod_BorderBottom($"{product.ProductId}", labelFont_9, Element.ALIGN_LEFT));
                        productTable.AddCell(CustomAlignMethod_BorderBottom($"{product.ProductName}", labelFont_9, Element.ALIGN_LEFT));
                        productTable.AddCell(CustomAlignMethod_BorderBottom($"{product.Quantity}", labelFont_9, Element.ALIGN_LEFT));
                        productTable.AddCell(CustomAlignMethod_BorderBottom($"{product.Price}", labelFont_9, Element.ALIGN_LEFT));

                    }// end foreach

                    pdfDoc.Add(productTable);

                    

                    /// Closing the file and pdf instances 
                    pdfDoc.Close();
                    writer.Close();
                    fs.Close();


                }// end using{}




            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error]: {e.Message}");
            }
        }

        /// <summary>
        /// Test Ref: https://github.com/VahidN/iTextSharp.LGPLv2.Core/blob/master/src/iTextSharp.LGPLv2.Core.FunctionalTests/HtmlWorkerTests.cs
        /// </summary>
        /// <param name="filePath"></param>
        public void Test (string filePath)
        {
            var pdfDoc = new Document(PageSize.A4);

            var fileStream = new FileStream(filePath, FileMode.Create);
            PdfWriter.GetInstance(pdfDoc, fileStream);

            pdfDoc.Open();



            var html = "<img alt='' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAodJREFUeNpsk0tME1EUhv87UwlCREhRFpi4cGMMRrTE4MaoxBhAsDyMssFHfCQu3BlXGuNKNy5NmqALoqEEMJWCgEUjYojllSpofIUNBNqmIKU6OnQennunUxvgJF86957z/+d27hkGigMlDJfOAmV7AcYsKGqIZljRSvhNE+CMTwEtXmBy2gQb7mCQJUBKkTIQYtfJYCNMAxO9hzq5CYmFiWFY6ISE9VFLRedc1SONeqwf+uJLuKreNPI9nltbLG0orhpqUCM90DRVoEbJ5MSLho1MMg1O0bHOuyoD9crCcxL+xa0HqwL+rEQHsb/CW89reO1aAyEuq+yp+zXvg66rgng8LrDXSmwYpUc8dZkmDsJNL+NCeVVXbWK+O32cpJ7E6OgkwuEwrl8phaHrVsfYD+x03XTPjN3nzZnD0HGxvPppTSLcLwo0I4lldRFK8jdCoZBlJquAbBnr0BD9GUTRvubahclW5qDukqkpIqlodGQ1At3UxZXaIUvauqsyjBV+jZJEJ3s83HO5j+UWI7E6C4mp2EQCTixyV2CvbbKzNmN2zNfHtbzPM3p4FOy/M5CXtwsOKZmmsOi2IHMvyyFhJhgY4BqutQ/aRRstocEngZzswnQnO+x1lqTjy8hIgNdyDc+x5nomxrKJhpcSp2lSrx48WlZhGArynG5hsLLoE7/jQ59f0aR7ZBkdbf7U6Ge+mKYaBvdx8wwZXjtWvfswfTrp3Over29J8NAXYO1t/v/7csZA5U5/Q35nH+aKt8OMR2POPSUFOyRmorvje3BiCt4b9zBANTmwGvP/aMoZRluJbURB8APmnPlQliNLzk8flxbeh9Du8eId5bYQ2SnxH36b/wQYABNFRsIaESsTAAAAAElFTkSuQmCC' />";

            var styles = new StyleSheet();
            PdfPCell pdfCell = new PdfPCell
            {
                Border = 0,
                RunDirection = PdfWriter.RUN_DIRECTION_LTR
            };

            using (var reader = new StringReader(html))
            {
                var parsedHtmlElements = HtmlWorker.ParseToList(reader, styles);

                foreach (IElement htmlElement in parsedHtmlElements)
                {
                    pdfCell.AddElement(htmlElement);
                }
            }

            var table1 = new PdfPTable(1);
            table1.AddCell(pdfCell);
            pdfDoc.Add(table1);


            pdfDoc.Close();
            fileStream.Dispose();

        }

        /// <summary>
        /// This CustomAlignMethod will align the contents to its alignment specified, you have to apply 
        /// the specific parameters. 
        /// </summary>
        /// <param name="textText">A text to display</param>
        /// <param name="fontStyle">Your style font</param>
        /// <param name="hroizontalAlignment">Left, Center or Right</param>
        /// <returns></returns>
        private PdfPCell CustomAlignMethod (string textText, Font fontStyle, int hroizontalAlignment)
        {
            var cell = new PdfPCell(new Phrase(textText, fontStyle));
            cell.UseVariableBorders = true;

            cell.BorderColorLeft = BaseColor.White;
            cell.BorderColorRight = BaseColor.White;
            cell.BorderColorTop = BaseColor.White;
            cell.BorderColorBottom = BaseColor.White;
            cell.HorizontalAlignment = hroizontalAlignment;

            return cell;

        }// end RightAlignMethod()


        private PdfPCell CustomAlignMethod_BorderBottom (string textText, Font fontStyle, int hroizontalAlignment)
        {
            var cell = new PdfPCell(new Phrase(textText, fontStyle));
            cell.UseVariableBorders = true;

            cell.BorderColorLeft = BaseColor.White;
            cell.BorderColorRight = BaseColor.White;
            cell.BorderColorTop = BaseColor.White;
            cell.BorderColorBottom = BaseColor.LightGray;
            cell.HorizontalAlignment = hroizontalAlignment;

            return cell;

        }// end RightAlignMethod()

        public byte[] GetFile (string fileName)
        {
            var result = File.ReadAllBytes(WebUtility.UrlDecode(fileName));
            return result;
        }


    }// end class 
}
