namespace caro27_033.GUI
{
    partial class frmScore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScore));
            dtgScoreBoard = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dtgScoreBoard).BeginInit();
            SuspendLayout();
            // 
            // dtgScoreBoard
            // 
            dtgScoreBoard.AllowUserToOrderColumns = true;
            dtgScoreBoard.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgScoreBoard.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgScoreBoard.BackgroundColor = SystemColors.ActiveCaption;
            dtgScoreBoard.BorderStyle = BorderStyle.Fixed3D;
            dtgScoreBoard.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgScoreBoard.Dock = DockStyle.Fill;
            dtgScoreBoard.EnableHeadersVisualStyles = false;
            dtgScoreBoard.Location = new Point(0, 0);
            dtgScoreBoard.MultiSelect = false;
            dtgScoreBoard.Name = "dtgScoreBoard";
            dtgScoreBoard.ReadOnly = true;
            dtgScoreBoard.RowHeadersWidth = 62;
            dtgScoreBoard.Size = new Size(330, 450);
            dtgScoreBoard.TabIndex = 0;
            // 
            // frmScore
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(330, 450);
            Controls.Add(dtgScoreBoard);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmScore";
            Text = "Bảng Xếp Hạng";
            Load += frmScore_Load;
            ((System.ComponentModel.ISupportInitialize)dtgScoreBoard).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dtgScoreBoard;
    }
}