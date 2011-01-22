namespace BlackBox.Editor
{
    partial class XmlEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._buttonSynchronize = new System.Windows.Forms.ToolStripButton();
            this._editor = new ICSharpCode.TextEditor.TextEditorControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonSynchronize});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(473, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _buttonSynchronize
            // 
            this._buttonSynchronize.Image = global::BlackBox.Editor.Properties.Resources.Synchronize;
            this._buttonSynchronize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._buttonSynchronize.Name = "_buttonSynchronize";
            this._buttonSynchronize.Size = new System.Drawing.Size(91, 22);
            this._buttonSynchronize.Text = "Synchronize";
            this._buttonSynchronize.Click += new System.EventHandler(this._buttonSynchronize_Click);
            // 
            // _editor
            // 
            this._editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._editor.IsReadOnly = false;
            this._editor.Location = new System.Drawing.Point(0, 25);
            this._editor.Name = "_editor";
            this._editor.Size = new System.Drawing.Size(473, 367);
            this._editor.TabIndex = 1;
            // 
            // XmlEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._editor);
            this.Controls.Add(this.toolStrip1);
            this.Name = "XmlEditor";
            this.Size = new System.Drawing.Size(473, 392);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private ICSharpCode.TextEditor.TextEditorControl _editor;
        private System.Windows.Forms.ToolStripButton _buttonSynchronize;

    }
}
