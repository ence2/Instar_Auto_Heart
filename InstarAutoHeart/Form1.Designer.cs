namespace InstarAutoHeart
{
    partial class InstarAutoHeart
    {
        public bool isInit = false;
        public bool isLogined = false;

        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPW = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbLogConsole = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbTags = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbExceptTags = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(14, 64);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(330, 23);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "로그인";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(49, 8);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(295, 21);
            this.tbID.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "PW";
            // 
            // tbPW
            // 
            this.tbPW.Location = new System.Drawing.Point(49, 37);
            this.tbPW.Name = "tbPW";
            this.tbPW.Size = new System.Drawing.Size(295, 21);
            this.tbPW.TabIndex = 4;
            this.tbPW.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "LogConsole";
            // 
            // tbLogConsole
            // 
            this.tbLogConsole.Location = new System.Drawing.Point(10, 150);
            this.tbLogConsole.Multiline = true;
            this.tbLogConsole.Name = "tbLogConsole";
            this.tbLogConsole.ReadOnly = true;
            this.tbLogConsole.Size = new System.Drawing.Size(333, 394);
            this.tbLogConsole.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbTags);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(356, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(122, 536);
            this.panel1.TabIndex = 7;
            // 
            // tbTags
            // 
            this.tbTags.Location = new System.Drawing.Point(3, 26);
            this.tbTags.Multiline = true;
            this.tbTags.Name = "tbTags";
            this.tbTags.Size = new System.Drawing.Size(117, 509);
            this.tbTags.TabIndex = 1;
            this.tbTags.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "우선탐색Tag";
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(15, 90);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(159, 39);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "작업 시작";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.Start_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(180, 90);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(163, 39);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "작업 중단";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbExceptTags);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Location = new System.Drawing.Point(484, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(122, 536);
            this.panel2.TabIndex = 8;
            // 
            // tbExceptTags
            // 
            this.tbExceptTags.Location = new System.Drawing.Point(3, 26);
            this.tbExceptTags.Multiline = true;
            this.tbExceptTags.Name = "tbExceptTags";
            this.tbExceptTags.Size = new System.Drawing.Size(117, 509);
            this.tbExceptTags.TabIndex = 1;
            this.tbExceptTags.TextChanged += new System.EventHandler(this.tbExceptTags_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "탐색제외Tag";
            // 
            // InstarAutoHeart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 556);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbLogConsole);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbPW);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label1);
            this.Name = "InstarAutoHeart";
            this.Text = "InstarAutoHeart";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnLogin;
        public System.Windows.Forms.TextBox tbID;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tbPW;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox tbLogConsole;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox tbTags;
        public System.Windows.Forms.Button btnStart;
        public System.Windows.Forms.Button btnStop;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.TextBox tbExceptTags;
        public System.Windows.Forms.Label label5;
    }
}

