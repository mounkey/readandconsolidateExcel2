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
            openFileDialogSource = new OpenFileDialog();
            groupBox1 = new GroupBox();
            lstConversionLog = new ListBox();
            btnStartConversion = new Button();
            txtXlsxDestinationFolder = new TextBox();
            btnBrowseXlsSource = new Button();
            txtXlsSourceFolder = new TextBox();
            label3 = new Label();
            folderBrowserDialog1 = new FolderBrowserDialog();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            lblStatus = new Label();
            btnProcess = new Button();
            txtYear = new TextBox();
            label2 = new Label();
            btnBrowseSourceFile = new Button();
            txtSourceFilePath = new TextBox();
            label1 = new Label();
            tabPage2 = new TabPage();
            lstConsolidationLog = new ListBox();
            btnStartBatchConsolidation = new Button();
            txtConsolidationYear = new TextBox();
            label5 = new Label();
            btnBrowseConsolidationFolder = new Button();
            txtConsolidationSourceFolder = new TextBox();
            label6 = new Label();
            folderBrowserDialog2 = new FolderBrowserDialog();
            groupBox1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // openFileDialogSource
            // 
            openFileDialogSource.FileName = "openFileDialogSource";
            openFileDialogSource.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
            openFileDialogSource.Title = "Seleccionar Archivo de Liquidación";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lstConversionLog);
            groupBox1.Controls.Add(btnStartConversion);
            groupBox1.Controls.Add(txtXlsxDestinationFolder);
            groupBox1.Controls.Add(btnBrowseXlsSource);
            groupBox1.Controls.Add(txtXlsSourceFolder);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(19, 314);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(710, 322);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // lstConversionLog
            // 
            lstConversionLog.FormattingEnabled = true;
            lstConversionLog.ItemHeight = 15;
            lstConversionLog.Location = new Point(6, 173);
            lstConversionLog.Name = "lstConversionLog";
            lstConversionLog.Size = new Size(698, 139);
            lstConversionLog.TabIndex = 9;
            // 
            // btnStartConversion
            // 
            btnStartConversion.Location = new Point(6, 141);
            btnStartConversion.Name = "btnStartConversion";
            btnStartConversion.Size = new Size(698, 26);
            btnStartConversion.TabIndex = 8;
            btnStartConversion.Text = "Iniciar Conversión";
            btnStartConversion.UseVisualStyleBackColor = true;
            btnStartConversion.Click += btnStartConversion_Click;
            // 
            // txtXlsxDestinationFolder
            // 
            txtXlsxDestinationFolder.Location = new Point(6, 112);
            txtXlsxDestinationFolder.Name = "txtXlsxDestinationFolder";
            txtXlsxDestinationFolder.ReadOnly = true;
            txtXlsxDestinationFolder.Size = new Size(698, 23);
            txtXlsxDestinationFolder.TabIndex = 6;
            // 
            // btnBrowseXlsSource
            // 
            btnBrowseXlsSource.Location = new Point(493, 83);
            btnBrowseXlsSource.Name = "btnBrowseXlsSource";
            btnBrowseXlsSource.Size = new Size(211, 23);
            btnBrowseXlsSource.TabIndex = 5;
            btnBrowseXlsSource.Text = "Seleccionar Carpeta Origen";
            btnBrowseXlsSource.UseVisualStyleBackColor = true;
            btnBrowseXlsSource.Click += btnBrowseXlsSource_Click;
            // 
            // txtXlsSourceFolder
            // 
            txtXlsSourceFolder.Location = new Point(6, 54);
            txtXlsSourceFolder.Name = "txtXlsSourceFolder";
            txtXlsSourceFolder.ReadOnly = true;
            txtXlsSourceFolder.Size = new Size(698, 23);
            txtXlsSourceFolder.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 18F);
            label3.Location = new Point(6, 19);
            label3.Name = "label3";
            label3.Size = new Size(275, 32);
            label3.TabIndex = 3;
            label3.Text = "Carpeta con archivos .xls";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(20, 7);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(730, 301);
            tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(lblStatus);
            tabPage1.Controls.Add(btnProcess);
            tabPage1.Controls.Add(txtYear);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(btnBrowseSourceFile);
            tabPage1.Controls.Add(txtSourceFilePath);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(722, 273);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Consolidar Archivo Individua";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.BorderStyle = BorderStyle.Fixed3D;
            lblStatus.Location = new Point(-5, 188);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(710, 63);
            lblStatus.TabIndex = 13;
            lblStatus.Text = "Listo";
            // 
            // btnProcess
            // 
            btnProcess.Location = new Point(-5, 144);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(710, 23);
            btnProcess.TabIndex = 12;
            btnProcess.Text = "Procesar Liquidacion";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += btnBrowseConsolidationFolder_Click;
            // 
            // txtYear
            // 
            txtYear.Location = new Point(-5, 98);
            txtYear.MaxLength = 4;
            txtYear.Name = "txtYear";
            txtYear.Size = new Size(710, 23);
            txtYear.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F);
            label2.Location = new Point(-5, 63);
            label2.Name = "label2";
            label2.Size = new Size(134, 32);
            label2.TabIndex = 10;
            label2.Text = "Año (XXXX)";
            // 
            // btnBrowseSourceFile
            // 
            btnBrowseSourceFile.Location = new Point(630, 63);
            btnBrowseSourceFile.Name = "btnBrowseSourceFile";
            btnBrowseSourceFile.Size = new Size(75, 23);
            btnBrowseSourceFile.TabIndex = 9;
            btnBrowseSourceFile.Text = "Examinar ..";
            btnBrowseSourceFile.UseVisualStyleBackColor = true;
            btnBrowseSourceFile.Click += btnBrowseSourceFile_Click;
            // 
            // txtSourceFilePath
            // 
            txtSourceFilePath.Location = new Point(-5, 34);
            txtSourceFilePath.Name = "txtSourceFilePath";
            txtSourceFilePath.ReadOnly = true;
            txtSourceFilePath.Size = new Size(710, 23);
            txtSourceFilePath.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F);
            label1.Location = new Point(-5, -1);
            label1.Name = "label1";
            label1.Size = new Size(181, 32);
            label1.TabIndex = 7;
            label1.Text = "Archivo Origen ";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(lstConsolidationLog);
            tabPage2.Controls.Add(btnStartBatchConsolidation);
            tabPage2.Controls.Add(txtConsolidationYear);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(btnBrowseConsolidationFolder);
            tabPage2.Controls.Add(txtConsolidationSourceFolder);
            tabPage2.Controls.Add(label6);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(722, 273);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Consolidar Archivo Por Lote (Carpeta)";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstConsolidationLog
            // 
            lstConsolidationLog.FormattingEnabled = true;
            lstConsolidationLog.ItemHeight = 15;
            lstConsolidationLog.Location = new Point(6, 184);
            lstConsolidationLog.Name = "lstConsolidationLog";
            lstConsolidationLog.Size = new Size(710, 79);
            lstConsolidationLog.TabIndex = 20;
            // 
            // btnStartBatchConsolidation
            // 
            btnStartBatchConsolidation.Location = new Point(6, 155);
            btnStartBatchConsolidation.Name = "btnStartBatchConsolidation";
            btnStartBatchConsolidation.Size = new Size(710, 23);
            btnStartBatchConsolidation.TabIndex = 19;
            btnStartBatchConsolidation.Text = "Iniciar Consolidación por Lote";
            btnStartBatchConsolidation.UseVisualStyleBackColor = true;
            btnStartBatchConsolidation.Click += btnStartBatchConsolidation_Click;
            // 
            // txtConsolidationYear
            // 
            txtConsolidationYear.Location = new Point(6, 109);
            txtConsolidationYear.MaxLength = 4;
            txtConsolidationYear.Name = "txtConsolidationYear";
            txtConsolidationYear.Size = new Size(710, 23);
            txtConsolidationYear.TabIndex = 18;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 18F);
            label5.Location = new Point(6, 74);
            label5.Name = "label5";
            label5.Size = new Size(134, 32);
            label5.TabIndex = 17;
            label5.Text = "Año (XXXX)";
            // 
            // btnBrowseConsolidationFolder
            // 
            btnBrowseConsolidationFolder.Location = new Point(590, 74);
            btnBrowseConsolidationFolder.Name = "btnBrowseConsolidationFolder";
            btnBrowseConsolidationFolder.Size = new Size(126, 23);
            btnBrowseConsolidationFolder.TabIndex = 16;
            btnBrowseConsolidationFolder.Text = "Examinar Carpeta ...";
            btnBrowseConsolidationFolder.UseVisualStyleBackColor = true;
            btnBrowseConsolidationFolder.Click += btnBrowseConsolidationFolder_Click;
            // 
            // txtConsolidationSourceFolder
            // 
            txtConsolidationSourceFolder.Location = new Point(6, 45);
            txtConsolidationSourceFolder.Name = "txtConsolidationSourceFolder";
            txtConsolidationSourceFolder.ReadOnly = true;
            txtConsolidationSourceFolder.Size = new Size(710, 23);
            txtConsolidationSourceFolder.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 18F);
            label6.Location = new Point(6, 10);
            label6.Name = "label6";
            label6.Size = new Size(183, 32);
            label6.TabIndex = 14;
            label6.Text = "Carpeta Origen ";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 657);
            Controls.Add(tabControl1);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Read and Consolidate Excel";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private OpenFileDialog openFileDialogSource;
        private GroupBox groupBox1;
        private Button btnStartConversion;
        private TextBox txtXlsxDestinationFolder;
        private Button btnBrowseXlsSource;
        private TextBox txtXlsSourceFolder;
        private Label label3;
        private FolderBrowserDialog folderBrowserDialog1;
        private ListBox lstConversionLog;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Label lblStatus;
        private Button btnProcess;
        private TextBox txtYear;
        private Label label2;
        private Button btnBrowseSourceFile;
        private TextBox txtSourceFilePath;
        private Label label1;
        private TabPage tabPage2;
        private Button btnStartBatchConsolidation;
        private TextBox txtConsolidationYear;
        private Label label5;
        private Button btnBrowseConsolidationFolder;
        private TextBox txtConsolidationSourceFolder;
        private Label label6;
        private ListBox lstConsolidationLog;
        private FolderBrowserDialog folderBrowserDialog2;
    }
}
