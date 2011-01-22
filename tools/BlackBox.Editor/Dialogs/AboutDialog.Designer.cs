namespace BlackBox.Editor
{
    partial class AboutDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.label2 = new System.Windows.Forms.Label();
            this._linkHomepage = new System.Windows.Forms.LinkLabel();
            this._buttonClose = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this._labelBuildDate = new System.Windows.Forms.Label();
            this.headerPanel1 = new BlackBox.Editor.HeaderPanel();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Copyright © 2011 Patrik Svensson";
            // 
            // _linkHomepage
            // 
            this._linkHomepage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._linkHomepage.AutoSize = true;
            this._linkHomepage.Location = new System.Drawing.Point(12, 296);
            this._linkHomepage.Name = "_linkHomepage";
            this._linkHomepage.Size = new System.Drawing.Size(134, 13);
            this._linkHomepage.TabIndex = 3;
            this._linkHomepage.TabStop = true;
            this._linkHomepage.Text = "http://blackbox-project.org";
            this._linkHomepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkHomepage_LinkClicked);
            // 
            // _buttonClose
            // 
            this._buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonClose.Location = new System.Drawing.Point(306, 291);
            this._buttonClose.Name = "_buttonClose";
            this._buttonClose.Size = new System.Drawing.Size(75, 23);
            this._buttonClose.TabIndex = 4;
            this._buttonClose.Text = "Close";
            this._buttonClose.UseVisualStyleBackColor = true;
            this._buttonClose.Click += new System.EventHandler(this._buttonClose_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(15, 103);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(366, 182);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // _labelBuildDate
            // 
            this._labelBuildDate.AutoSize = true;
            this._labelBuildDate.BackColor = System.Drawing.Color.Transparent;
            this._labelBuildDate.Location = new System.Drawing.Point(14, 61);
            this._labelBuildDate.Name = "_labelBuildDate";
            this._labelBuildDate.Size = new System.Drawing.Size(77, 13);
            this._labelBuildDate.TabIndex = 7;
            this._labelBuildDate.Text = "[BUILD-DATE]";
            this._labelBuildDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // headerPanel1
            // 
            this.headerPanel1.BackColor = System.Drawing.Color.White;
            this.headerPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel1.DrawGradient = false;
            this.headerPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerPanel1.GradientEndColor = System.Drawing.Color.CornflowerBlue;
            this.headerPanel1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.headerPanel1.GradientStartColor = System.Drawing.Color.White;
            this.headerPanel1.Icon = null;
            this.headerPanel1.InnerPadding = 0;
            this.headerPanel1.Location = new System.Drawing.Point(0, 0);
            this.headerPanel1.Name = "headerPanel1";
            this.headerPanel1.Size = new System.Drawing.Size(393, 53);
            this.headerPanel1.TabIndex = 5;
            this.headerPanel1.Title = "BlackBox Log Configuration Editor";
            // 
            // AboutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 326);
            this.Controls.Add(this._labelBuildDate);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.headerPanel1);
            this.Controls.Add(this._buttonClose);
            this.Controls.Add(this._linkHomepage);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Load += new System.EventHandler(this.AboutDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel _linkHomepage;
        private System.Windows.Forms.Button _buttonClose;
        private HeaderPanel headerPanel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label _labelBuildDate;
    }
}