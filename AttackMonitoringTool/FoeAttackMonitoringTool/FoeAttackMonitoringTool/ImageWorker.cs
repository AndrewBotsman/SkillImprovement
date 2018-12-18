using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoeAttackMonitoringTool
{
    public class ImageWorker
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
            const Int32 divisor = 4;
            const Int32 epsilon = 10;

            ExhaustiveTemplateMatching etm = new ExhaustiveTemplateMatching(0.9f);

            TemplateMatch[] tm = etm.ProcessImage(
                new ResizeNearestNeighbor(template.Width / divisor, template.Height / divisor).Apply(template),
                new ResizeNearestNeighbor(bmp.Width / divisor, bmp.Height / divisor).Apply(bmp)
                );

            if (tm.Length == 1)
            {
                Rectangle tempRect = tm[0].Rectangle;

                if (Math.Abs(bmp.Width / divisor - tempRect.Width) < epsilon
                    &&
                    Math.Abs(bmp.Height / divisor - tempRect.Height) < epsilon)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
