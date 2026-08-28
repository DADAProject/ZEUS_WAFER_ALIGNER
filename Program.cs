using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eMachine
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AssemblyName a = Assembly.GetExecutingAssembly().GetName();
            string strProg = a.Name;

            bool bCreatedNew = false;
            System.Threading.Mutex Mx = new System.Threading.Mutex(true, strProg, out bCreatedNew);

            if (bCreatedNew)// 이중 실행 방지.
            {
                WinAPI.TimeBeginPeriod(1);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmMain());
            }
            else
            {
                MessageBox.Show(strProg + " is already running!", strProg, MessageBoxButtons.OK);
                return;
            }
        }




    }
}
