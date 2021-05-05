using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Encoder = System.Drawing.Imaging.Encoder;

namespace JpgToPdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnHello_Click(object sender, EventArgs e)
        {
            //create a new pdf doucment object to create a pdf using pdfsharp.pdf
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            //add page to the pdf document
            PdfPage page = document.AddPage();

            // XGraphics object needed to draw on the pdf page
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //font style to draw on the pdf
            XFont font = new XFont("Arial",20, XFontStyle.Bold);
            
            //write the string in the page in a rectangle according to the page height and width
            gfx.DrawString("drawing on the pdf file",font, XBrushes.Black, new XRect(0,0,page.Width, page.Height), XStringFormats.Center);

            const string fileName = "D:\\LocalDiskH 2\\JPGTOPDF\\HelloPDf.pdf";
            document.Save(fileName);


        }

        private void btnJpgToPdf_Click(object sender, EventArgs e)
        {
            var quality = 90;
            string[] fileNames = new []{ string.Empty};
            var sourceFilePath = string.Empty;
            var destFilePath = string.Empty;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "D:\\LocalDiskH 2\\JPGTOPDF\\Demo jpgTopdf try 2(multiple files)";
            dlg.Filter = "JPEG files (*.jpg)|*.jpg|All files (*.*)|*.*";
            dlg.Multiselect = true;
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //source path for the file
                sourceFilePath = dlg.InitialDirectory;
                //filename without path
                fileNames = dlg.SafeFileNames;

                //create a pdf document using pdfsharp
                PdfDocument document = new PdfDocument();

                foreach (var fileName in fileNames)
                {
                    destFilePath = "D:\\LocalDiskH 2\\JPGTOPDF\\imageToPdf";
                    destFilePath = CompressImage(sourceFilePath, destFilePath, fileName, quality);
                    //MessageBox.Show(destFilePath);
                    CreatePdf(destFilePath, document);
                }
                //destFilePath = CompressImage(sourceFilePath, destFilePath, fileName, quality);
                //CreatePdf(destFilePath);
                //MessageBox.Show( "source file  path:" + destFilePath + "\\" + fileName);
            }
        }



        private string CompressImage(string sourceFilePath, string destFilePath,string fileName, int quality)
        {
            MessageBox.Show(destFilePath);
            destFilePath = destFilePath + "\\" + fileName;
            sourceFilePath = sourceFilePath + "\\" + fileName;
            //MessageBox.Show(destFilePath);
            using ( Bitmap bmp1 = new Bitmap(sourceFilePath))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                Encoder qualityEncoder = Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(qualityEncoder,quality);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(destFilePath,jpgEncoder,myEncoderParameters);
                return destFilePath;
            }
        }


        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }


        private void CreatePdf(string fileName, PdfDocument document)
        {
            //add page to the document
            PdfPage page = document.AddPage();

            // XGraphics object needed to draw on the pdf page
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawImage(gfx, fileName, 0,0, (int)page.Width, (int)page.Height);
            document.Save(@"D:\\LocalDiskH 2\\JPGTOPDF\\Result.pdf");
        }

        private void DrawImage(XGraphics gfx, string filePath,int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(filePath);
            gfx.DrawImage(image,x,y, width, height);
        }


    }
}
