using FOEDriverTool.Attribute;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FOEDriverTool
{
    public static class Utils
    {
        public static Bitmap GetEntereScreenshot(this IJavaScriptExecutor _driver)
        {
            Bitmap stitchedImage = null;
            try
            {
                try
                {
                    ((IJavaScriptExecutor)_driver).ExecuteScript("return document.body.offsetWidth");
                }
                catch
                { }

                long totalwidth1 = (long)((IJavaScriptExecutor)_driver).ExecuteScript("return document.body.offsetWidth");//documentElement.scrollWidth");
                long totalHeight1 = (long)((IJavaScriptExecutor)_driver).ExecuteScript("return  document.body.parentNode.scrollHeight");
                int totalWidth = (int)totalwidth1;
                int totalHeight = (int)totalHeight1;

                // Get the Size of the Viewport
                long viewportWidth1 = (long)((IJavaScriptExecutor)_driver).ExecuteScript("return document.body.clientWidth");//documentElement.scrollWidth");
                long viewportHeight1 = (long)((IJavaScriptExecutor)_driver).ExecuteScript("return window.innerHeight");//documentElement.scrollWidth");
                int viewportWidth = (int)viewportWidth1;
                int viewportHeight = (int)viewportHeight1;

                // Split the Screen in multiple Rectangles
                List<Rectangle> rectangles = new List<Rectangle>();
                // Loop until the Total Height is reached
                for (int i = 0; i < totalHeight; i += viewportHeight)
                {
                    int newHeight = viewportHeight;
                    // Fix if the Height of the Element is too big
                    if (i + viewportHeight > totalHeight)
                    {
                        newHeight = totalHeight - i;
                    }
                    // Loop until the Total Width is reached
                    for (int ii = 0; ii < totalWidth; ii += viewportWidth)
                    {
                        int newWidth = viewportWidth;
                        // Fix if the Width of the Element is too big
                        if (ii + viewportWidth > totalWidth)
                        {
                            newWidth = totalWidth - ii;
                        }

                        // Create and add the Rectangle
                        Rectangle currRect = new Rectangle(ii, i, newWidth, newHeight);
                        rectangles.Add(currRect);
                    }
                }

                // Build the Image
                stitchedImage = new Bitmap(totalWidth, totalHeight);
                // Get all Screenshots and stitch them together
                Rectangle previous = Rectangle.Empty;
                foreach (var rectangle in rectangles)
                {
                    // Calculate the Scrolling (if needed)
                    if (previous != Rectangle.Empty)
                    {
                        int xDiff = rectangle.Right - previous.Right;
                        int yDiff = rectangle.Bottom - previous.Bottom;
                        // Scroll
                        //selenium.RunScript(String.Format("window.scrollBy({0}, {1})", xDiff, yDiff));
                        ((IJavaScriptExecutor)_driver).ExecuteScript(String.Format("window.scrollBy({0}, {1})", xDiff, yDiff));
                        System.Threading.Thread.Sleep(200);
                    }

                    // Take Screenshot
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();

                    // Build an Image out of the Screenshot
                    Image screenshotImage;
                    using (MemoryStream memStream = new MemoryStream(screenshot.AsByteArray))
                    {
                        screenshotImage = Image.FromStream(memStream);
                    }

                    // Calculate the Source Rectangle
                    Rectangle sourceRectangle = new Rectangle(viewportWidth - rectangle.Width, viewportHeight - rectangle.Height, rectangle.Width, rectangle.Height);

                    // Copy the Image
                    using (Graphics g = Graphics.FromImage(stitchedImage))
                    {
                        g.DrawImage(screenshotImage, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                    }

                    // Set the Previous Rectangle
                    previous = rectangle;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error message: {ex.Message},\nCallStack: {ex.StackTrace}");
                Debug.WriteLine($"Error message: {ex.Message},\nCallStack: {ex.StackTrace}");
            }
            return stitchedImage;
        }

        public static void ClickLocation(this IJavaScriptExecutor _driver, Rectangle location)
        {
            try
            {
                var wd = (IWebDriver)_driver;
                Actions action = new Actions(wd);
                var body = wd.FindElement(By.XPath(".//body"));
                //action.MoveByOffset(location.X, location.Y).Perform();
                //Thread.Sleep(1000);
                action
                    .MoveToElement(body, location.X, location.Y)
                    .Click()
                    .Build()
                    .Perform();
                action.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error message: {ex.Message},\nCallStack: {ex.StackTrace}");
                Debug.WriteLine($"Error message: {ex.Message},\nCallStack: {ex.StackTrace}");
            }
        }

        public static void MovedownLocation(this IJavaScriptExecutor _driver)
        {
            try
            {
                int X = 100,
                    Y = 600;
                var wd = (IWebDriver)_driver;
                Actions action = new Actions(wd);
                var body = wd.FindElement(By.XPath(".//body"));
                var img = wd.FindElement(By.ClassName("raguImg"));
                
                //action
                //    .MoveByOffset(X, Y)
                //    .ClickAndHold()
                //    .MoveByOffset(X, Y - 400)
                //    .Click()
                //    .Build()
                //    .Perform();
                action
                    .MoveToElement(img)
                    //.ClickAndHold()
                    //.MoveByOffset(img.Location.X, img.Location.Y + 600)
                    .DragAndDropToOffset(img, X, Y)
                    .Build()
                    .Perform();
                //action.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error message: {ex.Message},\nCallStack: {ex.StackTrace}");
                Debug.WriteLine($"Error message: {ex.Message},\nCallStack: {ex.StackTrace}");
            }
        }

        public static void ZoomoutView(this IJavaScriptExecutor _driver)
        {
            try
            {
                var wd = (IWebDriver)_driver;
                Actions action = new Actions(wd);
                var html = wd.FindElement(By.TagName("html"));
                action
                    .SendKeys(html, Keys.Control + Keys.Add)
                    .Perform();
                //action.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error message: {ex.Message},\nCallStack: {ex.StackTrace}");
                Debug.WriteLine($"Error message: {ex.Message},\nCallStack: {ex.StackTrace}");
            }
        }

        public static string GetStringValue(Enum value)
        {
            string resultValue = null;
            var type = value.GetType();
            var fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if (attrs.Length > 0)
            {
                resultValue = attrs[0].Value;
            }

            return resultValue;
        }
    }
}
