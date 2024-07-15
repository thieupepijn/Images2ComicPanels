using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Images2ComicPanels
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: Collage [ImageFilesLocation]");
                return;
            }

            string imageFilesDirectory = args[0];

            if (!Directory.Exists(imageFilesDirectory))
            {
                Console.WriteLine(string.Format("Directory \"{0}\" does not exist.", imageFilesDirectory));
                return;
            }

            List<string> filePaths = Util.GetImageFiles(imageFilesDirectory);

            if (filePaths.Count == 0)
            {
                Console.WriteLine("No image-files found");
                return;
            }
            else if (!Util.SquareRootIsInteger(filePaths.Count))
            {
                Console.WriteLine("number of image-files found not correct, square root of number of image-files should be an integer");
                return;
            }

            List<Bitmap> bitmaps;
            try
            {
                bitmaps = Util.ImageFiles2Bitmaps(filePaths);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
            int averageWidth = (int)bitmaps.Average(b => b.Width);
            int averageHeight = (int)bitmaps.Average(b => b.Height);

            Util.ResizeBitmaps(bitmaps, averageWidth, averageHeight);

            Bitmap[,] bitmapsArray2D = Util.BitmapsList2Bitmaps2DimensionalArray(bitmaps);
            Bitmap stitchedBitmap = Util.StitchBitmaps(bitmapsArray2D);

            string outputFilePath = Path.Combine(imageFilesDirectory, "Collage.jpg");
            stitchedBitmap.Save(outputFilePath, ImageFormat.Jpeg);
            string message = string.Format("Images in {0} turned into collage {1}", imageFilesDirectory, outputFilePath);
            Console.WriteLine(message);



        }
    }
}
