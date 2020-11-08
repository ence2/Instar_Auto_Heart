using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstarAutoHeart
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Config.Instance.Load("Data\\config.json");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var instarAutoHeart = new InstarAutoHeart();
            Manager.Instance.Init(ref instarAutoHeart);

            Application.Run(instarAutoHeart);
        }
    }
}
