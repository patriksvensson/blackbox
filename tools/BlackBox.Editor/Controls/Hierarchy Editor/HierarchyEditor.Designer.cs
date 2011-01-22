namespace BlackBox.Editor
{
    partial class HierarchyEditor
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
			this._splitContainer = new System.Windows.Forms.SplitContainer();
			this._tree = new BlackBox.Editor.HierarchyTree();
			this._propertyGrid = new System.Windows.Forms.PropertyGrid();
			((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
			this._splitContainer.Panel1.SuspendLayout();
			this._splitContainer.Panel2.SuspendLayout();
			this._splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// _splitContainer
			// 
			this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this._splitContainer.Location = new System.Drawing.Point(0, 0);
			this._splitContainer.Name = "_splitContainer";
			// 
			// _splitContainer.Panel1
			// 
			this._splitContainer.Panel1.Controls.Add(this._tree);
			// 
			// _splitContainer.Panel2
			// 
			this._splitContainer.Panel2.Controls.Add(this._propertyGrid);
			this._splitContainer.Size = new System.Drawing.Size(473, 392);
			this._splitContainer.SplitterDistance = 266;
			this._splitContainer.TabIndex = 0;
			// 
			// _tree
			// 
			this._tree.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tree.Location = new System.Drawing.Point(0, 0);
			this._tree.Name = "_tree";
			this._tree.Size = new System.Drawing.Size(266, 392);
			this._tree.TabIndex = 0;
			this._tree.HierarchyNodeSelected += new System.EventHandler<BlackBox.Editor.HierarchyNodeEventArgs>(this.NodeSelected);
			// 
			// _propertyGrid
			// 
			this._propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._propertyGrid.Location = new System.Drawing.Point(0, 0);
			this._propertyGrid.Name = "_propertyGrid";
			this._propertyGrid.Size = new System.Drawing.Size(203, 392);
			this._propertyGrid.TabIndex = 0;
			// 
			// HierarchyEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._splitContainer);
			this.Name = "HierarchyEditor";
			this.Size = new System.Drawing.Size(473, 392);
			this._splitContainer.Panel1.ResumeLayout(false);
			this._splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
			this._splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.PropertyGrid _propertyGrid;
        private HierarchyTree _tree;
    }
}
