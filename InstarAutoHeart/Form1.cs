using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstarAutoHeart
{
    public partial class InstarAutoHeart : Form
    {
        public delegate void LogDelegate(string msg);

        public LogDelegate logDelegate;

        public InstarAutoHeart()
        {
            InitializeComponent();

            logDelegate = new LogDelegate(SendLog);

            this.tbID.Text = Config.Instance.Data.ID;
            this.tbPW.Text = Config.Instance.Data.PW;
        }

        public void SendLog(string msg)
        {
            if (this.tbLogConsole.InvokeRequired)
            {
                var d = logDelegate;
                Invoke(d, new object[] { msg });
            }
            else
            {
                this.tbLogConsole.AppendText(DateTime.Now.ToString("[MM/dd HH:mm:ss] ") + msg);
                this.tbLogConsole.AppendText(Environment.NewLine);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tbID.Text) || string.IsNullOrEmpty(tbPW.Text))
            {
                SendLog("로그인 실패 : 아이디 or 패스워드 똑바로 입력 해주세요");
                return;
            }

            btnLogin.Enabled = false;

            if (false == Manager.Instance.Login(tbID.Text, tbPW.Text))
            {
                btnLogin.Enabled = true;
                return;
            }

            btnStart.Enabled = true;
            SendLog("인스타그램 로그인 성공");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isInit)
                return;

            var tb = (System.Windows.Forms.TextBox)sender;
            Config.Instance.Data.priorityTags.Clear();
            foreach (var item in tb.Text.Replace("\r\n", ",").Split(','))
            {
                Config.Instance.Data.priorityTags.Add(item);
            }

            Config.Instance.Save();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (!isInit)
                return;

            btnStart.Enabled = false;

            Manager.Instance.WorkStart();

            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;

            Manager.Instance.WorkStop();

            SendLog("진행중 작업 중단 요청");

            btnStart.Enabled = true;
        }

        private void tbExceptTags_TextChanged(object sender, EventArgs e)
        {
            if (!isInit)
                return;

            var tb = (System.Windows.Forms.TextBox)sender;
            Config.Instance.Data.alreadySerached.Clear();
            foreach (var item in tb.Text.Replace("\r\n", ",").Split(','))
            {
                Config.Instance.Data.alreadySerached.Add(item);
            }

            Config.Instance.Save();
        }
    }
}
