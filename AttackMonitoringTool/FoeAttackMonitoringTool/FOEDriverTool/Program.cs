﻿using FOEDriverTool.Enumaration;
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
            /*Func<string, List<Bitmap>> getAge = age =>
            {
                return new List<Bitmap>
                    {
                        (Bitmap)Resources.ResourceManager.GetObject(age),
                        _driver.GetEntereScreenshot()
                    };
            }; */

            var afterLoadPics = new Dictionary<string, List<Bitmap>>
            {
                {
                    Utils.GetStringValue(Age.IA),
                    GetAge(Utils.GetStringValue(Age.IA))
                },
                {
                    Utils.GetStringValue(Age.EMA),
                    GetAge(Utils.GetStringValue(Age.EMA))
                },
                {
                    Utils.GetStringValue(Age.HMA),
                    GetAge(Utils.GetStringValue(Age.HMA))
                },
                {
                    Utils.GetStringValue(Age.CA),
                    GetAge(Utils.GetStringValue(Age.CA))
                },
                {
                    Utils.GetStringValue(Age.IndA),
                    GetAge(Utils.GetStringValue(Age.IndA))
                },
                {
                    Utils.GetStringValue(Age.PE),
                    GetAge(Utils.GetStringValue(Age.PE))
                },
                {
                    Utils.GetStringValue(Age.ME),
                    GetAge(Utils.GetStringValue(Age.ME))
                },
                {
                    Utils.GetStringValue(Age.PME),
                    GetAge(Utils.GetStringValue(Age.PME))
                },
                {
                    Utils.GetStringValue(Age.CE),
                    GetAge(Utils.GetStringValue(Age.CE))
                },
                {
                    Utils.GetStringValue(Age.T),
                    GetAge(Utils.GetStringValue(Age.T))
                },
                {
                    Utils.GetStringValue(Age.F),
                    GetAge(Utils.GetStringValue(Age.F))
                },
                {
                    Utils.GetStringValue(Age.AA),
                    GetAge(Utils.GetStringValue(Age.AA))
                },
            };

            var moveDownAges = new List<string>
            {
                Utils.GetStringValue(Age.EMA),
                Utils.GetStringValue(Age.HMA),
                Utils.GetStringValue(Age.T),
                Utils.GetStringValue(Age.AA),
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
                NotifyAttack();

                // Back to GvG map
                CatchItem(new Bitmap(Resources.Back), _driver.GetEntereScreenshot());
                Thread.Sleep(1000);
            }
        }

        private static void NotifyAttack()
        {
            
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
