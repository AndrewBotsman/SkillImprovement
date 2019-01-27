using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoeAttackMonitoringTool
{
    public partial class MonitoringTool : Form
    {
        //private Task SequenceTask;
        //BackgroundWorker worker;
        //private delegate void DELEGATE();
        // non-deterministic delay to let AJAX code run
        //const int AJAX_DELAY = 1000;
        // keep track of the main automation task
        //CancellationTokenSource mainCts;
        //Task mainTask = null;

        public Point WebBrowserLocation
        {
            get
            {
                return webBrowser.Location;
            }
        }

        public MonitoringTool()
        {
            InitializeComponent();

            #region comments
            //SetBrowserFeatureControl(); // set FEATURE_BROWSER_EMULATION first
            //worker = new BackgroundWorker();
            //this.Load += (s, e) =>
            //{
            //    // start the automation when form is loaded
            //    // timeout the whole automation task in 30s
            //    mainCts = new CancellationTokenSource(30000);
            //    mainTask = LoadingAsync(mainCts.Token).ContinueWith((completedTask) =>
            //    {
            //        Trace.WriteLine(String.Format("Automation task status: {0}", completedTask.Status.ToString()));
            //    }, TaskScheduler.FromCurrentSynchronizationContext());
            //};

            //this.FormClosing += (s, e) =>
            //{
            //    // cancel the automation if form closes
            //    if (this.mainTask != null && !this.mainTask.IsCompleted)
            //        mainCts.Cancel();
            //};
            #endregion
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            

            //buttonStart.Enabled = false;
            //try
            //{
            //    Thread thread = new Thread(StartUiProcess);
            //    thread.Start();
            //}
            //finally
            //{
            //    // Because we need the context here.
            //    buttonStart.Enabled = true;
            //}


            #region comments
            //try
            //{
            //    // start the automation when form is loaded
            //    // timeout the whole automation task in 30s
            //    mainCts = new CancellationTokenSource(30000);
            //    mainTask = StartAsync(mainCts.Token).ContinueWith((completedTask) =>
            //    {
            //        Trace.WriteLine(String.Format("Start task status: {0}", completedTask.Status.ToString()));
            //    }, TaskScheduler.FromCurrentSynchronizationContext());
            //}
            //finally
            //{
            //    if (this.mainTask != null && !this.mainTask.IsCompleted)
            //        mainCts.Cancel();
            //}



            //worker.DoWork += StartUI;
            //worker.RunWorkerAsync();

            //Thread start = new Thread(new ThreadStart(startProcess));
            //start.Start();

            //new Task(PrepareMonitoringArea).Start();
            //PrepareMonitoringArea();
            //StartWatching();
            #endregion
        }

        private void StartUI(object sender, DoWorkEventArgs e)
        {
            //var del = new DELEGATE(StartUiProcess);
            //this.Invoke(del);
        }

        private void StartUiProcess()
        {
            Action action = () =>
            {
                PrepareMonitoringArea();
                StartWatching();
            };

            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void startProcess()
        {
            if (webBrowser.InvokeRequired)
                webBrowser.Invoke((MethodInvoker)delegate ()
                {
                    PrepareMonitoringArea();
                    StartWatching();
                });
            else
            {
                PrepareMonitoringArea();
                StartWatching();
            }
        }

        private void StartWatching()
        {
            //MessageBox.Show("Start monotoring...");
            var bitmap1 = new Bitmap(Resources.AttackSign);
            var bitmap2 = new Bitmap(@"D:\System Storage\Development\Git\SkillImprovement\pics\Attack.png");

            var location = ImageWorker.autoSearchBitmap(bitmap1, bitmap2);
            //MessageBox.Show($"location: X => {location.X}; Y => {location.Y}");

        }

        private void PrepareMonitoringArea()
        {
            Application.DoEvents();
            //var playButton = webBrowser.Document.All.GetElementsByName("play");
            //playButton[0].InvokeMember("Click");
            //Thread.Sleep(1000);
            //int worlds = 0;
            //var timeStamp = DateTime.Now;
            //HtmlElementCollection worldList = null;

            //while (worlds < 1 && (DateTime.Now - timeStamp).Seconds < 10)
            //{
            //    worldList = webBrowser.Document.GetElementById("world_selection_list").All;
            //    Thread.Sleep(500);
            //    //MessageBox.Show($"Element world_selection_list:  {worldList.Count}");
            //    worlds = worldList.Count;
            //}
            ////var worldList = webBrowser.Document.GetElementById("world_selection_list").All.GetElementsByName("a");
            ////MessageBox.Show($"Element world_selection_list:  {worldList.Count}");

            //foreach (HtmlElement item in worldList)
            //{
            //    if (item.InnerText == "Норсил" && item.TagName == "A")
            //    {
            //        item.InvokeMember("Click");
            //        break;
            //    }
            //}


            var steps = new List<Bitmap>
            {
                Resources.StartGame,
                Resources.NorthilServer,
            };

            foreach (var step in steps)
            {
                //webBrowser.Refresh();

                var pattern = new Bitmap(step);
                var screenshot = webBrowser.GetScreenshot();
                FindClick(pattern, screenshot);
                Thread.Sleep(1000);
                this.Refresh();
            }
        }

        private static void FindClick(Bitmap bitmap1, Bitmap bitmap2)
        {
            var location = ImageWorker.autoSearchBitmap(bitmap1, bitmap2);
            var startTrack = DateTime.Now;
            while (location.X == 0 && location.Y == 0 && (DateTime.Now - startTrack).Seconds > 5)
            {
                location = ImageWorker.autoSearchBitmap(bitmap1, bitmap2);
                Thread.Sleep(500);
            }

            var position = new Utils.MousePoint
            {
                X = location.X + location.Width / 2,
                Y = location.Y + location.Height
            };
            //MessageBox.Show($"location: X => {location.X}; Y => {location.Y}");
            Utils.SetCursorPosition(position);

            Utils.MouseEvent(Utils.MouseEventFlags.LeftDown | Utils.MouseEventFlags.LeftUp);
            Thread.Sleep(30);
            //Utils.MouseEvent(Utils.MouseEventFlags.LeftUp);
            //Thread.Sleep(30);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopWatching();
        }

        private void StopWatching()
        {
            MessageBox.Show("Stop monotoring.");
        }

        // Another approach
        #region Async Nav
        //async Task NavigateAsync(CancellationToken ct, Action startNavigation, int timeout = Timeout.Infinite)
        //{
        //    var onloadTcs = new TaskCompletionSource<bool>();
        //    EventHandler onloadEventHandler = null;

        //    WebBrowserDocumentCompletedEventHandler documentCompletedHandler = delegate
        //    {
        //        // DocumentCompleted may be called several time for the same page,
        //        // beacuse of frames
        //        if (onloadEventHandler != null || onloadTcs == null || onloadTcs.Task.IsCompleted)
        //            return;

        //        // handle DOM onload event to make sure the document is fully loaded
        //        onloadEventHandler = (s, e) =>
        //            onloadTcs.TrySetResult(true);
        //        this.webBrowser.Document.Window.AttachEventHandler("onload", onloadEventHandler);
        //    };

        //    using (var cts = CancellationTokenSource.CreateLinkedTokenSource(ct))
        //    {
        //        if (timeout != Timeout.Infinite)
        //            cts.CancelAfter(Timeout.Infinite);

        //        using (cts.Token.Register(() => onloadTcs.TrySetCanceled(), useSynchronizationContext: true))
        //        {
        //            this.webBrowser.DocumentCompleted += documentCompletedHandler;
        //            try
        //            {
        //                startNavigation();
        //                // wait for DOM onload, throw if cancelled
        //                await onloadTcs.Task;
        //                ct.ThrowIfCancellationRequested();
        //                // let AJAX code run, throw if cancelled
        //                await Task.Delay(AJAX_DELAY, ct);
        //            }
        //            finally
        //            {
        //                this.webBrowser.DocumentCompleted -= documentCompletedHandler;
        //                if (onloadEventHandler != null)
        //                    this.webBrowser.Document.Window.DetachEventHandler("onload", onloadEventHandler);
        //            }
        //        }
        //    }
        //}

        //// Browser feature conntrol
        //void SetBrowserFeatureControl()
        //{
        //    // http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

        //    // FeatureControl settings are per-process
        //    var fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

        //    // make the control is not running inside Visual Studio Designer
        //    if (String.Compare(fileName, "devenv.exe", true) == 0 || String.Compare(fileName, "XDesProc.exe", true) == 0)
        //        return;

        //    SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, GetBrowserEmulationMode()); // Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
        //}

        //void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        //{
        //    using (var key = Registry.CurrentUser.CreateSubKey(
        //        String.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature),
        //        RegistryKeyPermissionCheck.ReadWriteSubTree))
        //    {
        //        key.SetValue(appName, (UInt32)value, RegistryValueKind.DWord);
        //    }
        //}

        //UInt32 GetBrowserEmulationMode()
        //{
        //    int browserVersion = 7;
        //    using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
        //        RegistryKeyPermissionCheck.ReadSubTree,
        //        System.Security.AccessControl.RegistryRights.QueryValues))
        //    {
        //        var version = ieKey.GetValue("svcVersion");
        //        if (null == version)
        //        {
        //            version = ieKey.GetValue("Version");
        //            if (null == version)
        //                throw new ApplicationException("Microsoft Internet Explorer is required!");
        //        }
        //        int.TryParse(version.ToString().Split('.')[0], out browserVersion);
        //    }

        //    UInt32 mode = 10000; // Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode. Default value for Internet Explorer 10.
        //    switch (browserVersion)
        //    {
        //        case 7:
        //            mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. Default value for applications hosting the WebBrowser Control.
        //            break;
        //        case 8:
        //            mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. Default value for Internet Explorer 8
        //            break;
        //        case 9:
        //            mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode. Default value for Internet Explorer 9.
        //            break;
        //        default:
        //            // use IE10 mode by default
        //            break;
        //    }

        //    return mode;
        //}

        //async Task LoadingAsync(CancellationToken ct)
        //{
        //    await NavigateAsync(ct, () => this.webBrowser.Navigate("https://ru13.forgeofempires.com/game/index?ref="), 10000); // timeout in 10s
        //    // page loaded, log the page's HTML
        //    Trace.WriteLine(GetBrowserDocumentHtml());
        //}

        //async Task StartAsync(CancellationToken ct)
        //{
        //    HtmlElementCollection play = webBrowser.Document.All.GetElementsByName("play");
        //    // throw if none or more than one element found
        //    HtmlElement btn = play.Cast<HtmlElement>().Single(
        //        el => el.Name == "play");

        //    ct.ThrowIfCancellationRequested();

        //    // simulate a click which causes navigation
        //    await NavigateAsync(ct, () => btn.InvokeMember("click"), 10000); // timeout in 10s

        //    // form submitted and new page loaded, log the page's HTML
        //    Trace.WriteLine(GetBrowserDocumentHtml());


        //    var worldList = webBrowser.Document.GetElementById("world_selection_list").All;
        //    HtmlElement world = worldList.Cast<HtmlElement>().Single(
        //        el => el.InnerText == "Норсил" && el.TagName == "A");

        //    //ct.ThrowIfCancellationRequested();

        //    // simulate a click which causes navigation
        //    await NavigateAsync(ct, () => world.InvokeMember("click"), 10000); // timeout in 10s
        //}

        //string GetBrowserDocumentHtml()
        //{
        //    return this.webBrowser.Document.GetElementsByTagName("html")[0].OuterHtml;
        //}

        #endregion

    }
}
