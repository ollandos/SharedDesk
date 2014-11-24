using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharedDesk
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // TODO: enable login
            //Application.Run(new loginForm());

            // For asses like John & Tatsuya for "testing"!
            Application.Run(new Form1("rea@th.com","1"));
        }
    }
}
