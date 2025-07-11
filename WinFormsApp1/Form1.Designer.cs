namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtSourceFilePath = new TextBox();
            btnBrowseSourceFile = new Button();
            txtYear = new TextBox();
            label2 = new Label();
            btnProcess = new Button();
            lblStatus = new Label();
            openFileDialogSource = new OpenFileDialog();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F);
            label1.Location = new Point(19, 28);
            label1.Name = "label1";
            label1.Size = new Size(181, 32);
            label1.TabIndex = 0;
            label1.Text = "Archivo Origen ";
            // 
            // txtSourceFilePath
            // 
            txtSourceFilePath.Location = new Point(19, 63);
            txtSourceFilePath.Name = "txtSourceFilePath";
            txtSourceFilePath.ReadOnly = true;
            txtSourceFilePath.Size = new Size(710, 23);
            txtSourceFilePath.TabIndex = 1;
            // 
            // btnBrowseSourceFile
            // 
            btnBrowseSourceFile.Location = new Point(654, 92);
            btnBrowseSourceFile.Name = "btnBrowseSourceFile";
            btnBrowseSourceFile.Size = new Size(75, 23);
            btnBrowseSourceFile.TabIndex = 2;
            btnBrowseSourceFile.Text = "Examinar ..";
            btnBrowseSourceFile.UseVisualStyleBackColor = true;
            btnBrowseSourceFile.Click += btnBrowseSourceFile_Click;
            // 
            // txtYear
            // 
            txtYear.Location = new Point(19, 127);
            txtYear.MaxLength = 4;
            txtYear.Name = "txtYear";
            txtYear.Size = new Size(710, 23);
            txtYear.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F);
            label2.Location = new Point(19, 92);
            label2.Name = "label2";
            label2.Size = new Size(134, 32);
            label2.TabIndex = 3;
            label2.Text = "Año (XXXX)";
            // 
            // btnProcess
            // 
            btnProcess.Location = new Point(19, 173);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(710, 23);
            btnProcess.TabIndex = 5;
            btnProcess.Text = "Procesar Liquidacion";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += btnProcess_Click;
            // 
            // lblStatus
            // 
            lblStatus.BorderStyle = BorderStyle.Fixed3D;
            lblStatus.Location = new Point(19, 217);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(710, 162);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Listo";
            // 
            // openFileDialogSource
            // 
            openFileDialogSource.FileName = "openFileDialogSource";
            openFileDialogSource.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
            openFileDialogSource.Title = "Seleccionar Archivo de Liquidación";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblStatus);
            Controls.Add(btnProcess);
            Controls.Add(txtYear);
            Controls.Add(label2);
            Controls.Add(btnBrowseSourceFile);
            Controls.Add(txtSourceFilePath);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Read and Consolidate Excel";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtSourceFilePath;
        private Button btnBrowseSourceFile;
        private TextBox txtYear;
        private Label label2;
        private Button btnProcess;
        private Label lblStatus;
        private OpenFileDialog openFileDialogSource;
    }
}
