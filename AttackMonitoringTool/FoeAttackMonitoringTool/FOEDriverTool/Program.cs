using FOEDriverTool.Enumaration;
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

        private static List<Bitmap> GetAge(string age)
        {
            return new List<Bitmap>
                    {
                        (Bitmap)Resources.ResourceManager.GetObject(age),
                        _driver.GetEntereScreenshot()
                    };
        }

        private static void StartProcessing()
        {
            var afterLoadPics = new Dictionary<string, List<Bitmap>>
            {
                {
                    Utils.GetEnumStringValue(Age.IA),
                    GetAge(Utils.GetEnumStringValue(Age.IA))
                },
                {
                    Utils.GetEnumStringValue(Age.EMA),
                    GetAge(Utils.GetEnumStringValue(Age.EMA))
                },
                {
                    Utils.GetEnumStringValue(Age.HMA),
                    GetAge(Utils.GetEnumStringValue(Age.HMA))
                },
                {
                    Utils.GetEnumStringValue(Age.CA),
                    GetAge(Utils.GetEnumStringValue(Age.CA))
                },
                {
                    Utils.GetEnumStringValue(Age.IndA),
                    GetAge(Utils.GetEnumStringValue(Age.IndA))
                },
                {
                    Utils.GetEnumStringValue(Age.PE),
                    GetAge(Utils.GetEnumStringValue(Age.PE))
                },
                {
                    Utils.GetEnumStringValue(Age.ME),
                    GetAge(Utils.GetEnumStringValue(Age.ME))
                },
                {
                    Utils.GetEnumStringValue(Age.PME),
                    GetAge(Utils.GetEnumStringValue(Age.PME))
                },
                {
                    Utils.GetEnumStringValue(Age.CE),
                    GetAge(Utils.GetEnumStringValue(Age.CE))
                },
                {
                    Utils.GetEnumStringValue(Age.T),
                    GetAge(Utils.GetEnumStringValue(Age.T))
                },
                {
                    Utils.GetEnumStringValue(Age.F),
                    GetAge(Utils.GetEnumStringValue(Age.F))
                },
                {
                    Utils.GetEnumStringValue(Age.AA),
                    GetAge(Utils.GetEnumStringValue(Age.AA))
                },
            };

            var moveDownAges = new List<string>
            {
                Utils.GetEnumStringValue(Age.EMA),
                Utils.GetEnumStringValue(Age.HMA),
                Utils.GetEnumStringValue(Age.T),
                Utils.GetEnumStringValue(Age.AA),
            };

            foreach (var item in afterLoadPics)
            {
                Debug.WriteLine($"Item: {item.Key}");

                if (moveDownAges.Contains(item.Key))
                {
                    _driver.MovedownLocation();
                    Thread.Sleep(1000);
                }

                // Open age
                CatchItem(item.Value[0], item.Value[1]);
                Thread.Sleep(1000);

                // Verify attack and make alarm
                NotifyAttack(Utils.GetEnumDesription((Age)Enum.Parse(typeof(Age), item.Key)));

                // Back to GvG map
                CatchItem(new Bitmap(Resources.Back), _driver.GetEntereScreenshot());
                Thread.Sleep(1000);
            }
        }

        private static void NotifyAttack(string age)
        {
            var attackCaptured = ItemFound(new Bitmap(Resources.AttackSign), _driver.GetEntereScreenshot());
            var skype = new SkypeProxy();
            skype.SendMessage($"{age}");
        }

        private static void OpenGVGMap()
        {
            var afterLoadPics = new Dictionary<string, List<Bitmap>>
            {
                {
                    "Close",
                    GetAge("Close")

                },
                {
                    "GVG",
                    GetAge("GVG")

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

        private static bool ItemFound(Bitmap bmpToSearch, Bitmap bmpSource)
        {
            var location = ImageWorker.autoSearchBitmap(bmpToSearch, bmpSource);
            return location.X != 0 && location.Y != 0;
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
