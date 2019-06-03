using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FOEDriverTool
{
    class Program
    {
        private static IJavaScriptExecutor _driver;
        //private static readonly string DriverPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\.nuget\packages\selenium.webdriver.chromedriver\74.0.3729.6\driver\win32";
        private static readonly string DriverPath = Environment.CurrentDirectory + @"\..\..\Resources\driver";
        private static readonly string WorldPath = @"https://ru13.forgeofempires.com/game/index?ref=";

        static void Main(string[] args)
        {
            InitialLoad();
            StartProcess();
        }

        public static void InitialLoad()
        {
            _driver = new ChromeDriver(DriverPath);
            ((IWebDriver)_driver).Manage().Window.Maximize();
            var script = "window.location = \'" + WorldPath + "\'";
            ((IJavaScriptExecutor)_driver).ExecuteScript(script);
        }

        private static void StartProcess()
        {
            try
            {
                Login();
                OpenGVGMap();
                StartProcessing();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error message: {ex.Message},\nCallStack: {ex.StackTrace}");
                Debug.WriteLine($"Error message: {ex.Message},\nCallStack: {ex.StackTrace}");
            }
            finally
            {
                FinishProcess();
            }
        }

        private static void StartProcessing()
        {
            var afterLoadPics = new Dictionary<string, List<Bitmap>>
            {
                {
                    "IA",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.IA),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "EMA",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.EMA),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "HMA",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.HMA),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "CA",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.CA),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "IndA",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.IndA),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "PE",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.PE),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "ME",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.ME),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "PME",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.PME),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "CE",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.CE),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "T",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.T),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "F",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.F),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "AA",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.AA),
                        _driver.GetEntereScreenshot()
                    }

                },
            };

            //var moveDownAges = new List<string>
            //{
            //    "EMA",
            //    "HMA",
            //    "T",
            //    "AA",
            //};

            

            foreach (var item in afterLoadPics)
            {
                Debug.WriteLine($"Item: {item.Key}");

                //if (moveDownAges.Contains(item.Key))
                //{
                //    _driver.MovedownLocation();
                //    Thread.Sleep(1000);
                //}

                CatchItem(item.Value[0], item.Value[1]);
                Thread.Sleep(5000);
                CatchItem(new Bitmap(Resources.Back), _driver.GetEntereScreenshot());
                Thread.Sleep(1000);
            }
        }

        private static void OpenGVGMap()
        {
            var afterLoadPics = new Dictionary<string, List<Bitmap>>
            {
                {
                    "Close",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.Close),
                        _driver.GetEntereScreenshot()
                    }

                },
                {
                    "GVG",
                    new List<Bitmap>
                    {
                        new Bitmap(Resources.Gvg),
                        _driver.GetEntereScreenshot()
                    }

                },
            };

            foreach (var item in afterLoadPics)
            {
                CatchItem(item.Value[0], item.Value[1]);
                Thread.Sleep(1000);
            }

            _driver.ZoomoutView();
        }

        private static void CatchItem(Bitmap bmpToSearch, Bitmap bmpSource)
        {
            var location = ImageWorker.autoSearchBitmap(bmpToSearch, bmpSource);
            if (location.X != 0 && location.Y != 0)
            {
                Debug.WriteLine($"location: {location}");
                _driver.ClickLocation(location);
            }
            else
            {
                Debug.WriteLine($"location was not found: {location}");
            }

        }

        private static void Login()
        {
            var wd = ((IWebDriver)_driver);
            IWebElement loginUserIdTextBox = wd.FindElement(By.Id("login_userid")),
                loginPasswordTextBox = wd.FindElement(By.Id("login_password")),
                loginButton = wd.FindElement(By.Id("login_Login"));

            loginUserIdTextBox.Clear();
            loginUserIdTextBox.SendKeys("-LuckyStar-");
            loginPasswordTextBox.Clear();
            loginPasswordTextBox.SendKeys("funcrack4");
            loginButton.Click();
            Thread.Sleep(1000);
            IWebElement playButton = wd.FindElement(By.Name("play"));
            playButton.Click();
            Thread.Sleep(1000);
            IWebElement worldButton = wd.FindElements(By.TagName("a")).FirstOrDefault(_ => _.Text == "Норсил");
            worldButton.Click();
            Thread.Sleep(10000);
        }

        public static void FinishProcess()
        {
            Thread.Sleep(5000);
            ((IWebDriver)_driver).Quit();
        }
    }
}
