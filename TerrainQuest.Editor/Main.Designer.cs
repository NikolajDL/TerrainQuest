namespace TerrainQuest.Editor
{
    partial class Main
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
            this.renderingControl = new TerrainQuest.Editor.Rendering.RenderingControl();
            this.SuspendLayout();
            // 
            // renderingControl
            // 
            this.renderingControl.Location = new System.Drawing.Point(136, 80);
            this.renderingControl.Name = "renderingControl";
            this.renderingControl.Size = new System.Drawing.Size(721, 532);
            this.renderingControl.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 752);
            this.Controls.Add(this.renderingControl);
            this.Name = "Main";
            this.Text = "Terrain Quest Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private Rendering.RenderingControl renderingControl;
    }
}

