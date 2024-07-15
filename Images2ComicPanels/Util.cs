using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;


namespace Images2ComicPanels
{
    public class Util
    {

        public static List<string> GetImageFiles(string directory)
        {

            return Directory.GetFiles(directory, "*.*").Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") ||
                              f.EndsWith(".png") || f.EndsWith(".gif") || f.EndsWith(".bmp")).ToList();

        }

        public static bool SquareRootIsInteger(int number)
        {
            int squareroot = (int)Math.Sqrt(number);

            if (squareroot * squareroot == number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<Bitmap> ImageFiles2Bitmaps(List<string> imageFiles)
        {
            List<Bitmap> bitmaps = new List<Bitmap>();
            foreach (string imageFile in imageFiles)
            {
                Bitmap bitmap;
                try
                {
                    bitmap = new Bitmap(imageFile);
                }
                catch
                {
                    string message = string.Format("{0} does not seem to be an image-file", imageFile);
                    throw new Exception(message);
                }
                bitmaps.Add(bitmap);
            }
            return bitmaps;
        }

        public static void ResizeBitmaps(List<Bitmap> bitmaps, int resizedWidth, int resizedHeight)
        {
            for (int counter = 0; counter < bitmaps.Count; counter++)
            {
                Bitmap bitmap = bitmaps[counter];
                Bitmap resizedBitmap = new Bitmap(bitmap, new Size(resizedWidth, resizedHeight));
                bitmaps[counter] = resizedBitmap;
            }
        }

        public static Bitmap[,] BitmapsList2Bitmaps2DimensionalArray(List<Bitmap> bitmaps)
        {
            int numberOfColumns = (int)Math.Sqrt(bitmaps.Count);
            int numberOfRows = (int)Math.Sqrt(bitmaps.Count);
            Bitmap[,] bitmapArray2D = new Bitmap[numberOfColumns, numberOfRows];

            int column = 0;
            int row = 0;
            foreach (Bitmap bitmap in bitmaps)
            {
                bitmapArray2D[row, column] = bitmap;
                column++;
                if (column > numberOfColumns - 1)
                {
                    column = 0;
                    row++;
                }
            }
            return bitmapArray2D;
        }

        public static Bitmap StitchBitmaps(Bitmap[,] bitmaps)
        {
            //all bitmaps should have te same witdh and height
            int columns = bitmaps.GetLength(0);
            int rows = bitmaps.GetLength(1);
            int bitmapWidth = bitmaps[0, 0].Width;
            int bitmapHeight = bitmaps[0, 0].Height;
            int totalWidth = bitmapWidth * columns;
            int totalHeight = bitmapHeight * rows;

            Bitmap stitchedBitmap = new Bitmap(totalWidth, totalHeight);

            using (Graphics graafix = Graphics.FromImage(stitchedBitmap))
            {
                for (int column = 0; column < columns; column++)
                {
                    for (int row = 0; row < rows; row++)
                    {
                        int x = column * bitmapWidth;
                        int y = row * bitmapHeight;
                        Bitmap bitmap = bitmaps[column, row];
                        graafix.DrawImage(bitmap, new Point(x, y));
                    }
                }
            }
            return stitchedBitmap;
        }


    }
}
