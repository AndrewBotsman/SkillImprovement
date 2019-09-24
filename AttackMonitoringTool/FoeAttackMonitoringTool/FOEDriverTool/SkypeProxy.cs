using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOEDriverTool
{
    public class SkypeProxy
    {
        // 1. SkypeNet
        private readonly string AttackStarted = "Началось нападение.";

        public void SendMessage(string messageText)
        {
            var msg = $"Внимание!!! {messageText}. {AttackStarted}";
            MessageBox.Show(msg);
        }
    }
}
