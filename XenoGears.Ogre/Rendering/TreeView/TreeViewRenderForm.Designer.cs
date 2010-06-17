namespace XenoGears.Ogre.Rendering.TreeView
{
    partial class TreeViewRenderForm
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
            this.Splitter = new System.Windows.Forms.SplitContainer();
            this.Tree = new System.Windows.Forms.TreeView();
            this.Details = new System.Windows.Forms.RichTextBox();
            this.Splitter.Panel1.SuspendLayout();
            this.Splitter.Panel2.SuspendLayout();
            this.Splitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // Splitter
            // 
            this.Splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.Splitter.Location = new System.Drawing.Point(0, 0);
            this.Splitter.Name = "Splitter";
            // 
            // Splitter.Panel1
            // 
            this.Splitter.Panel1.Controls.Add(this.Tree);
            // 
            // Splitter.Panel2
            // 
            this.Splitter.Panel2.Controls.Add(this.Details);
            this.Splitter.Size = new System.Drawing.Size(825, 556);
            this.Splitter.SplitterDistance = 426;
            this.Splitter.TabIndex = 0;
            // 
            // Tree
            // 
            this.Tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tree.Location = new System.Drawing.Point(0, 0);
            this.Tree.Name = "Tree";
            this.Tree.Size = new System.Drawing.Size(426, 556);
            this.Tree.TabIndex = 0;
            // 
            // Details
            // 
            this.Details.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Details.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Details.Location = new System.Drawing.Point(0, 0);
            this.Details.Name = "Details";
            this.Details.ReadOnly = true;
            this.Details.Size = new System.Drawing.Size(395, 556);
            this.Details.TabIndex = 0;
            this.Details.Text = "";
            // 
            // TreeViewRenderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 556);
            this.Controls.Add(this.Splitter);
            this.Name = "TreeViewRenderForm";
            this.Text = "Object graph";
            this.Splitter.Panel1.ResumeLayout(false);
            this.Splitter.Panel2.ResumeLayout(false);
            this.Splitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.SplitContainer Splitter;
        internal System.Windows.Forms.TreeView Tree;
        internal System.Windows.Forms.RichTextBox Details;
    }
}