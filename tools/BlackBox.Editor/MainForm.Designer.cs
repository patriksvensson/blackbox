namespace BlackBox.Editor
{
    partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this._menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._menuNew = new System.Windows.Forms.ToolStripMenuItem();
			this._menuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this._menuSave = new System.Windows.Forms.ToolStripMenuItem();
			this._menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this._menuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.testConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._menuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this._statusStrip = new System.Windows.Forms.StatusStrip();
			this._tabControl = new System.Windows.Forms.TabControl();
			this._tabPageHierarchy = new System.Windows.Forms.TabPage();
			this._hierarchyEditor = new BlackBox.Editor.HierarchyEditor();
			this._tabPageXml = new System.Windows.Forms.TabPage();
			this._xmlEditor = new BlackBox.Editor.XmlEditor();
			this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this._menuStrip.SuspendLayout();
			this._tabControl.SuspendLayout();
			this._tabPageHierarchy.SuspendLayout();
			this._tabPageXml.SuspendLayout();
			this.SuspendLayout();
			// 
			// _menuStrip
			// 
			this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this._menuStrip.Location = new System.Drawing.Point(0, 0);
			this._menuStrip.Name = "_menuStrip";
			this._menuStrip.Size = new System.Drawing.Size(521, 24);
			this._menuStrip.TabIndex = 0;
			this._menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuNew,
            this._menuOpen,
            this.toolStripSeparator,
            this._menuSave,
            this._menuSaveAs,
            this.toolStripSeparator2,
            this._menuExit});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// _menuNew
			// 
			this._menuNew.Image = ((System.Drawing.Image)(resources.GetObject("_menuNew.Image")));
			this._menuNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._menuNew.Name = "_menuNew";
			this._menuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this._menuNew.Size = new System.Drawing.Size(146, 22);
			this._menuNew.Text = "&New";
			this._menuNew.Click += new System.EventHandler(this._menuNew_Click);
			// 
			// _menuOpen
			// 
			this._menuOpen.Image = ((System.Drawing.Image)(resources.GetObject("_menuOpen.Image")));
			this._menuOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._menuOpen.Name = "_menuOpen";
			this._menuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this._menuOpen.Size = new System.Drawing.Size(146, 22);
			this._menuOpen.Text = "&Open";
			this._menuOpen.Click += new System.EventHandler(this._menuOpen_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
			// 
			// _menuSave
			// 
			this._menuSave.Enabled = false;
			this._menuSave.Image = ((System.Drawing.Image)(resources.GetObject("_menuSave.Image")));
			this._menuSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._menuSave.Name = "_menuSave";
			this._menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this._menuSave.Size = new System.Drawing.Size(146, 22);
			this._menuSave.Text = "&Save";
			this._menuSave.Click += new System.EventHandler(this._menuSave_Click);
			// 
			// _menuSaveAs
			// 
			this._menuSaveAs.Name = "_menuSaveAs";
			this._menuSaveAs.Size = new System.Drawing.Size(146, 22);
			this._menuSaveAs.Text = "Save &As";
			this._menuSaveAs.Click += new System.EventHandler(this._menuSaveAs_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
			// 
			// _menuExit
			// 
			this._menuExit.Name = "_menuExit";
			this._menuExit.Size = new System.Drawing.Size(146, 22);
			this._menuExit.Text = "E&xit";
			this._menuExit.Click += new System.EventHandler(this._menuExit_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			this.editToolStripMenuItem.Visible = false;
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Enabled = false;
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.undoToolStripMenuItem.Text = "&Undo";
			// 
			// redoToolStripMenuItem
			// 
			this.redoToolStripMenuItem.Enabled = false;
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.redoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.redoToolStripMenuItem.Text = "&Redo";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Enabled = false;
			this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
			this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.cutToolStripMenuItem.Text = "Cu&t";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Enabled = false;
			this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
			this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.copyToolStripMenuItem.Text = "&Copy";
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Enabled = false;
			this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
			this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.pasteToolStripMenuItem.Text = "&Paste";
			// 
			// debugToolStripMenuItem
			// 
			this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testConfigurationToolStripMenuItem});
			this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
			this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.debugToolStripMenuItem.Text = "Debug";
			// 
			// testConfigurationToolStripMenuItem
			// 
			this.testConfigurationToolStripMenuItem.Enabled = false;
			this.testConfigurationToolStripMenuItem.Name = "testConfigurationToolStripMenuItem";
			this.testConfigurationToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.testConfigurationToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.testConfigurationToolStripMenuItem.Text = "Test Configuration...";
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			this.toolsToolStripMenuItem.Visible = false;
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Enabled = false;
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
			this.optionsToolStripMenuItem.Text = "&Options...";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuAbout});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// _menuAbout
			// 
			this._menuAbout.Name = "_menuAbout";
			this._menuAbout.Size = new System.Drawing.Size(116, 22);
			this._menuAbout.Text = "&About...";
			this._menuAbout.Click += new System.EventHandler(this._menuAbout_Click);
			// 
			// _statusStrip
			// 
			this._statusStrip.Location = new System.Drawing.Point(0, 374);
			this._statusStrip.Name = "_statusStrip";
			this._statusStrip.Size = new System.Drawing.Size(521, 22);
			this._statusStrip.TabIndex = 2;
			this._statusStrip.Text = "statusStrip1";
			// 
			// _tabControl
			// 
			this._tabControl.Controls.Add(this._tabPageHierarchy);
			this._tabControl.Controls.Add(this._tabPageXml);
			this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tabControl.Location = new System.Drawing.Point(0, 24);
			this._tabControl.Name = "_tabControl";
			this._tabControl.SelectedIndex = 0;
			this._tabControl.Size = new System.Drawing.Size(521, 350);
			this._tabControl.TabIndex = 3;
			this._tabControl.SelectedIndexChanged += new System.EventHandler(this.OnTabPageSelectedIndexChanged);
			// 
			// _tabPageHierarchy
			// 
			this._tabPageHierarchy.Controls.Add(this._hierarchyEditor);
			this._tabPageHierarchy.Location = new System.Drawing.Point(4, 22);
			this._tabPageHierarchy.Name = "_tabPageHierarchy";
			this._tabPageHierarchy.Padding = new System.Windows.Forms.Padding(3);
			this._tabPageHierarchy.Size = new System.Drawing.Size(513, 324);
			this._tabPageHierarchy.TabIndex = 0;
			this._tabPageHierarchy.Text = "Hierarchy";
			this._tabPageHierarchy.UseVisualStyleBackColor = true;
			// 
			// _hierarchyEditor
			// 
			this._hierarchyEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this._hierarchyEditor.Location = new System.Drawing.Point(3, 3);
			this._hierarchyEditor.Name = "_hierarchyEditor";
			this._hierarchyEditor.Size = new System.Drawing.Size(507, 318);
			this._hierarchyEditor.TabIndex = 0;
			// 
			// _tabPageXml
			// 
			this._tabPageXml.Controls.Add(this._xmlEditor);
			this._tabPageXml.Location = new System.Drawing.Point(4, 22);
			this._tabPageXml.Name = "_tabPageXml";
			this._tabPageXml.Padding = new System.Windows.Forms.Padding(3);
			this._tabPageXml.Size = new System.Drawing.Size(513, 324);
			this._tabPageXml.TabIndex = 1;
			this._tabPageXml.Text = "XML";
			this._tabPageXml.UseVisualStyleBackColor = true;
			// 
			// _xmlEditor
			// 
			this._xmlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this._xmlEditor.Location = new System.Drawing.Point(3, 3);
			this._xmlEditor.Name = "_xmlEditor";
			this._xmlEditor.Size = new System.Drawing.Size(507, 318);
			this._xmlEditor.TabIndex = 0;
			this._xmlEditor.Synchronize += new System.EventHandler<BlackBox.Editor.LogConfigurationEventArgs>(this.OnXmlSynchronize);
			// 
			// _openFileDialog
			// 
			this._openFileDialog.Filter = "XML files|*.xml|Configuration files|*.config|All files|*.*";
			// 
			// _saveFileDialog
			// 
			this._saveFileDialog.Filter = "XML files|*.xml|Configuration files|*.config|All files|*.*";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(521, 396);
			this.Controls.Add(this._tabControl);
			this.Controls.Add(this._statusStrip);
			this.Controls.Add(this._menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this._menuStrip;
			this.Name = "MainForm";
			this.Text = "BlackBox Log Configuration Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this._menuStrip.ResumeLayout(false);
			this._menuStrip.PerformLayout();
			this._tabControl.ResumeLayout(false);
			this._tabPageHierarchy.ResumeLayout(false);
			this._tabPageXml.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _tabPageHierarchy;
        private System.Windows.Forms.TabPage _tabPageXml;
        private HierarchyEditor _hierarchyEditor;
        private XmlEditor _xmlEditor;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _menuNew;
        private System.Windows.Forms.ToolStripMenuItem _menuOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem _menuSave;
        private System.Windows.Forms.ToolStripMenuItem _menuSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _menuExit;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _menuAbout;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testConfigurationToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
    }
}

