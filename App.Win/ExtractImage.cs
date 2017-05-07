using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text.pdf;

namespace App.Win
{
    public partial class ExtractImage : Form
    {
        public ExtractImage()
        {
            InitializeComponent();
        }

        private void ExtractImage_Load(object sender, EventArgs e)
        {
            txtOutputFolder.Text = @"D:\TestResized - Copy";
        }

        private void ExtractImagesFromPdf(string sourcePdf, string outputPath)
        {
            // NOTE:  This will only get the first image it finds per page.
            var pdf = new PdfReader(sourcePdf);
            //var raf = new RandomAccessFileOrArray(sourcePdf);
            var fileCount = 1;
            for (var pageNumber = 1; pageNumber <= pdf.NumberOfPages; pageNumber++)
            {
                var pg = pdf.GetPageN(pageNumber);
                var resource =
                    (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
                if(resource == null) continue;
                var xobject =
                    (PdfDictionary)PdfReader.GetPdfObject(resource.Get(PdfName.XOBJECT));
                if (xobject == null) continue;
                foreach (var key in xobject.Keys)
                {
                    var pdfObject = xobject.Get(key);
                    if (pdfObject.IsIndirect())
                    {
                        var pdfDictionary = (PdfDictionary)PdfReader.GetPdfObject(pdfObject);
                        var type =
                            (PdfName)PdfReader.GetPdfObject(pdfDictionary.Get(PdfName.SUBTYPE));
                        if (PdfName.IMAGE.Equals(type))
                        {
                            var xrefIndex =
                                Convert.ToInt32(
                                    ((PRIndirectReference)pdfObject).Number.ToString(CultureInfo.InvariantCulture));
                            var pdfObj = pdf.GetPdfObject(xrefIndex);
                            var pdfStrem = (PdfStream) pdfObj;
                            var bytes = PdfReader.GetStreamBytesRaw((PRStream) pdfStrem);
                            if (bytes != null)
                            {
                                using (var memStream = new MemoryStream(bytes))
                                {
                                    memStream.Position = 0;
                                    var img = Image.FromStream(memStream);
                                    if (!Directory.Exists(outputPath))
                                        Directory.CreateDirectory(outputPath);

                                    var path = Path.Combine(outputPath, $@"{fileCount}.jpg");
                                    var parms = new EncoderParameters(1)
                                    {
                                        Param =
                                        {
                                            [0] =
                                                new EncoderParameter(
                                                    Encoder.Compression, 0)
                                        }
                                    };
                                    var jpegEncoder = GetImageEncoder("JPEG");
                                    img.Save(path, jpegEncoder, parms);
                                    fileCount++;
                                }
                            }
                        }
                    }
                }
            }
            pdf.Close();
        }

        public ImageCodecInfo GetImageEncoder(string imageType)
        {
            imageType = imageType.ToUpperInvariant();
            foreach (var info in ImageCodecInfo.GetImageEncoders())
            {
                if (info.FormatDescription == imageType)
                {
                    return info;
                }
            }

            return null;
        }

        private void btnConvertImages_Click(object sender, EventArgs e)
        {
            lbResult.Text = string.Empty;
            var dialogResult = ofdConvertToImage.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var fileName = ofdConvertToImage.FileName;
                ExtractImagesFromPdf(fileName, txtOutputFolder.Text);
            }
        }

        private void btnAddLogo_Click(object sender, EventArgs e)
        {
            var dialogResult = ofdConvertToImage.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var pathFile = ofdConvertToImage.FileName;
                var fileImageToConverts = Directory.GetFiles(txtOutputFolder.Text);
                foreach (var fileImageToConvert in fileImageToConverts)
                {
                    var fileName = fileImageToConvert.Split('\\')[fileImageToConvert.Split('\\').Length - 1];
                    var newFileName = fileName.Replace(fileName.Split('.')[0], $"{fileName.Split('.')[0]}-final");
                    var newPathFile = fileImageToConvert.Replace(fileName, newFileName);

                    using (var image = Image.FromFile(fileImageToConvert))
                    using (var watermarkImage = Image.FromFile(pathFile))
                    using (var imageGraphics = Graphics.FromImage(image))
                    using (var watermarkBrush = new TextureBrush(watermarkImage))
                    {
                        var x = image.Width / 2 - watermarkImage.Width / 2;
                        var y = image.Height / 2 - watermarkImage.Height / 2;
                        watermarkBrush.TranslateTransform(x, y);
                        imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));
                        image.Save(newPathFile);
                    }
                }
            }
        }
    }
}