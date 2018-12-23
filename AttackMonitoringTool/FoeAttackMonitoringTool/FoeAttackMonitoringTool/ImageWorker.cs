using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoeAttackMonitoringTool
{
    public static class ImageWorker
    {
        public static Bitmap CaptureActiveWindow()
        {
            // Shot size = screen size
            Size shotSize = Screen.PrimaryScreen.Bounds.Size;

            // the upper left point in the screen to start shot
            // 0,0 to get the shot from upper left point
            Point upperScreenPoint = new Point(0, 0);

            // the upper left point in the image to put the shot
            Point upperDestinationPoint = new Point(0, 0);

            // create image to get the shot in it
            Bitmap shot = new Bitmap(shotSize.Width, shotSize.Height);

            // new Graphics instance 
            Graphics graphics = Graphics.FromImage(shot);

            // get the shot by Graphics class 
            graphics.CopyFromScreen(upperScreenPoint, upperDestinationPoint, shotSize);

            // return the image
            return shot;
        }

        public static bool ImageContainsTemplate(Bitmap sourceImage, Bitmap template)
        {
            //const Int32 divisor = 4;
            //const Int32 epsilon = 10;

            //ExhaustiveTemplateMatching etm = new ExhaustiveTemplateMatching(0.9f);

            //TemplateMatch[] tm = etm.ProcessImage(
            //    new ResizeNearestNeighbor(template.Width / divisor, template.Height / divisor).Apply(template),
            //    new ResizeNearestNeighbor(bmp.Width / divisor, bmp.Height / divisor).Apply(bmp)
            //    );

            //if (tm.Length == 1)
            //{
            //    Rectangle tempRect = tm[0].Rectangle;

            //    if (Math.Abs(bmp.Width / divisor - tempRect.Width) < epsilon
            //        &&
            //        Math.Abs(bmp.Height / divisor - tempRect.Height) < epsilon)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }

        public static Bitmap GetScreenshot(this Control c)
        {
            return GetScreenshot(new Rectangle(c.PointToScreen(Point.Empty), c.Size));
        }

        public static Bitmap GetScreenshot(Rectangle bounds)
        {
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            return bitmap;
        }

        public static Rectangle autoSearchBitmap(Bitmap bitmap1, Bitmap bitmap2)
        {
            Rectangle location = Rectangle.Empty;
            for (int i = 0; i <= 20; i++) //toleranceTrackBar.Maximum
            {
                //toleranceTrackBar.Value = i;
                //toleranceTrackBar.Refresh();
                double tolerance = Convert.ToDouble(i) / 100.0;

                location = searchBitmap(bitmap1, bitmap2, tolerance);

                if (location.Width != 0)
                    break;
            }
            return location;
        }

        private static Rectangle searchBitmap(Bitmap smallBmp, Bitmap bigBmp, double tolerance)
        {
            BitmapData smallData =
              smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData bigData =
              bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride;
            int bigStride = bigData.Stride;

            int bigWidth = bigBmp.Width;
            int bigHeight = bigBmp.Height - smallBmp.Height + 1;
            int smallWidth = smallBmp.Width * 3;
            int smallHeight = smallBmp.Height;

            Rectangle location = Rectangle.Empty;
            int margin = Convert.ToInt32(255.0 * tolerance);

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0;
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int smallOffset = smallStride - smallBmp.Width * 3;
                int bigOffset = bigStride - bigBmp.Width * 3;

                bool matchFound = true;

                for (int y = 0; y < bigHeight; y++)
                {
                    for (int x = 0; x < bigWidth; x++)
                    {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        //Look for the small picture.
                        for (int i = 0; i < smallHeight; i++)
                        {
                            int j = 0;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++)
                            {
                                //With tolerance: pSmall value should be between margins.
                                int inf = pBig[0] - margin;
                                int sup = pBig[0] + margin;
                                if (sup < pSmall[0] || inf > pSmall[0])
                                {
                                    matchFound = false;
                                    break;
                                }

                                pBig++;
                                pSmall++;
                            }

                            if (!matchFound) break;

                            //We restore the pointers.
                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            //Next rows of the small and big pictures.
                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        //If match found, we return.
                        if (matchFound)
                        {
                            location.X = x;
                            location.Y = y;
                            location.Width = smallBmp.Width;
                            location.Height = smallBmp.Height;
                            break;
                        }
                        //If no match found, we restore the pointers and continue.
                        else
                        {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }

                    if (matchFound) break;

                    pBig += bigOffset;
                }
            }

            bigBmp.UnlockBits(bigData);
            smallBmp.UnlockBits(smallData);

            return location;
        }

        #region Save Dialog
        public const string DEFAULT_IMAGESAVEFILEDIALOG_TITLE = "Save image";
        public const string DEFAULT_IMAGESAVEFILEDIALOG_FILTER = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg|Bitmap Image (*.bmp)|*.bmp|GIF Image (*.gif)|*.gif";

        public const string CUSTOMPLACES_COMPUTER = "0AC0837C-BBF8-452A-850D-79D08E667CA7";
        public const string CUSTOMPLACES_DESKTOP = "B4BFCC3A-DB2C-424C-B029-7FE99A87C641";
        public const string CUSTOMPLACES_DOCUMENTS = "FDD39AD0-238F-46AF-ADB4-6C85480369C7";
        public const string CUSTOMPLACES_PICTURES = "33E28130-4E1E-4676-835A-98395C3BC3BB";
        public const string CUSTOMPLACES_PUBLICPICTURES = "B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5";
        public const string CUSTOMPLACES_RECENT = "AE50C081-EBD2-438A-8655-8A092E34987A";

        public static SaveFileDialog GetImageSaveFileDialog(
          string title = DEFAULT_IMAGESAVEFILEDIALOG_TITLE,
          string filter = DEFAULT_IMAGESAVEFILEDIALOG_FILTER)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Title = title;
            dialog.Filter = filter;


            /* //this seems to throw error on Windows Server 2008 R2, must be for Windows Vista only
            dialog.CustomPlaces.Add(CUSTOMPLACES_COMPUTER);
            dialog.CustomPlaces.Add(CUSTOMPLACES_DESKTOP);
            dialog.CustomPlaces.Add(CUSTOMPLACES_DOCUMENTS);
            dialog.CustomPlaces.Add(CUSTOMPLACES_PICTURES);
            dialog.CustomPlaces.Add(CUSTOMPLACES_PUBLICPICTURES);
            dialog.CustomPlaces.Add(CUSTOMPLACES_RECENT);
            */

            return dialog;
        }

        public static void ShowSaveFileDialog(this Image image, IWin32Window owner = null)
        {
            using (SaveFileDialog dlg = GetImageSaveFileDialog())
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                    image.Save(dlg.FileName);
        }
        #endregion
    }
}
