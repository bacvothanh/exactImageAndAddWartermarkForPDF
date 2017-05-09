using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            txtOutputFolder.Text = @"C:\Users\Admin\Desktop\Oc\output";
            txtInput.Text = @"C:\Users\Admin\Desktop\Oc\input";
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

                                    var path = Path.Combine(outputPath, $@"{fileCount}.png");
                                    var parms = new EncoderParameters(1)
                                    {
                                        Param =
                                        {
                                            [0] =
                                                new EncoderParameter(
                                                    Encoder.Compression, 0)
                                        }
                                    };
                                    var jpegEncoder = GetImageEncoder("PNG");
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
                var fileImageToConverts = Directory.GetFiles(txtInput.Text);
                foreach (var fileImageToConvert in fileImageToConverts)
                {
                    var fileName = fileImageToConvert.Split('\\')[fileImageToConvert.Split('\\').Length - 1];
                    var newPathFile = $"{txtOutputFolder.Text}\\{fileName}";

                    using (var image = Image.FromFile(fileImageToConvert))
                    using (var watermarkImage = Image.FromFile(pathFile))
                    using (var imageGraphics = Graphics.FromImage(image))
                    {
                        if (image.Width <= image.Height)
                        {
                            var newWidth = image.Width*90/100;
                            double percent = (double) newWidth/(double) watermarkImage.Width;
                            var newHeight = watermarkImage.Height*percent;

                            var watermarkImageResized = ResizeImage(watermarkImage, newWidth, (int) newHeight);
                            using (var watermarkBrush = new TextureBrush(watermarkImageResized))
                            {
                                var x = image.Width/2 - watermarkImageResized.Width/2;
                                var y = image.Height/2 - watermarkImageResized.Height/2;
                                watermarkBrush.TranslateTransform(x, y);
                                imageGraphics.FillRectangle(watermarkBrush,
                                    new Rectangle(new Point(x, y),
                                        new Size(watermarkImageResized.Width + 1, watermarkImageResized.Height)));
                                image.Save(newPathFile);
                            }
                        }
                        else
                        {
                            var newHeight = image.Height*90/100;
                            double percent = (double)newHeight / (double)watermarkImage.Height;
                            var newWidth = watermarkImage.Width * percent;
                            var watermarkImageResized = ResizeImage(watermarkImage, (int)newWidth, newHeight);
                            using (var watermarkBrush = new TextureBrush(watermarkImageResized))
                            {
                                var x = image.Width / 2 - watermarkImageResized.Width / 2;
                                var y = image.Height / 2 - watermarkImageResized.Height / 2;
                                watermarkBrush.TranslateTransform(x, y);
                                imageGraphics.FillRectangle(watermarkBrush,
                                    new Rectangle(new Point(x, y),
                                        new Size(watermarkImageResized.Width + 1, watermarkImageResized.Height)));
                                image.Save(newPathFile);
                            }
                        }
                        
                    }
                }
            }
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void btnAddWhiteBackground_Click(object sender, EventArgs e)
        {
            var backgroundImagePath = @"C:\Users\Admin\Desktop\Oc\background.jpg";
            var fileImageToConverts = Directory.GetFiles(txtInput.Text);
            foreach (var fileImageToConvert in fileImageToConverts)
            {
                var fileName = fileImageToConvert.Split('\\')[fileImageToConvert.Split('\\').Length - 1];
                var newPathFile = $"{txtOutputFolder.Text}\\{fileName}";

                using (var backgroundImage = Image.FromFile(backgroundImagePath))
                using (var orinalImage = Image.FromFile(fileImageToConvert))
                {
                    if (orinalImage.Width >= orinalImage.Height)
                    {
                        var newWidth = orinalImage.Width * 140 / 100;
                        double percent = (double)newWidth / (double)orinalImage.Width;
                        var newHeight = orinalImage.Height * percent;
                        var newBackgroundImage = ResizeImage(backgroundImage, newWidth, (int)newHeight);
                        using (var imageGraphics = Graphics.FromImage(newBackgroundImage))
                        {
                            using (var watermarkBrush = new TextureBrush(orinalImage))
                            {
                                var x = newBackgroundImage.Width / 2 - orinalImage.Width / 2;
                                var y = newBackgroundImage.Height / 2 - orinalImage.Height / 2;
                                watermarkBrush.TranslateTransform(x, y);
                                imageGraphics.FillRectangle(watermarkBrush,
                                    new Rectangle(new Point(x, y),
                                        new Size(orinalImage.Width + 1, orinalImage.Height)));
                                newBackgroundImage.Save(newPathFile);
                            }
                        }
                    }
                    else
                    {
                        var newHeight = orinalImage.Height * 140 / 100;
                        double percent = (double)newHeight / (double)orinalImage.Height;
                        var newWidth = orinalImage.Width * percent;
                        var newBackgroundImage = ResizeImage(backgroundImage, (int)newWidth, newHeight);
                        using (var imageGraphics = Graphics.FromImage(newBackgroundImage)) 
                        using (var watermarkBrush = new TextureBrush(orinalImage))
                        {
                            var x = newBackgroundImage.Width / 2 - orinalImage.Width / 2;
                            var y = newBackgroundImage.Height / 2 - orinalImage.Height / 2;
                            watermarkBrush.TranslateTransform(x, y);
                            imageGraphics.FillRectangle(watermarkBrush,
                                new Rectangle(new Point(x, y),
                                    new Size(orinalImage.Width + 1, orinalImage.Height)));
                            newBackgroundImage.Save(newPathFile);
                        }
                    }
                }
                
            }
        }
    }
}