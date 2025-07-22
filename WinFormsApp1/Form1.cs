using OfficeOpenXml; // EPPlus
using System;
using System.Collections.Generic; // Para List<T>
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ReadAndConsolidateExcel; // MUY IMPORTANTE: para File.Exists y Path.Combine
// using OfficeOpenXml; // Ya lo pusimos en el constructor para LicenseContext, no es estrictamente necesario aquí arriba si no usas tipos de EPPlus directamente en Form1
using System.ComponentModel; // Para BackgroundWorker
using Microsoft.Office.Interop.Excel; // Para Excel Interop
using System.Runtime.InteropServices; // Para Marshal.ReleaseComObject

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private BackgroundWorker conversionWorker; //Para la conversión XLS->XLSX
        private BackgroundWorker consolidationWorker; // Para la consolidación por lote

        public Form1()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            conversionWorker = new BackgroundWorker();
            conversionWorker.WorkerReportsProgress = true;
            conversionWorker.DoWork += ConversionWorker_DoWork;
            conversionWorker.ProgressChanged += ConversionWorker_ProgressChanged;
            conversionWorker.RunWorkerCompleted += ConversionWorker_RunWorkerCompleted;

            // Inicializar el BackgroundWorker para la consolidación por lote
            consolidationWorker = new BackgroundWorker();
            consolidationWorker.WorkerReportsProgress = true;
            consolidationWorker.DoWork += ConsolidationWorker_DoWork;
            consolidationWorker.ProgressChanged += ConsolidationWorker_ProgressChanged;
            consolidationWorker.RunWorkerCompleted += ConsolidationWorker_RunWorkerCompleted;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnBrowseSourceFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogSource.ShowDialog() == DialogResult.OK)
            {
                txtSourceFilePath.Text = openFileDialogSource.FileName;
                lblStatus.Text = "Archivo de origen seleccionado.";
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            string sourceFilePath = txtSourceFilePath.Text;
            string yearInput = txtYear.Text;

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(sourceFilePath))
            {
                MessageBox.Show("Por favor, selecciona un archivo de liquidación de origen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error: No se seleccionó archivo de origen.";
                return;
            }

            if (!File.Exists(sourceFilePath)) // Necesitarás: using System.IO;
            {
                MessageBox.Show($"El archivo de origen especificado no existe: {sourceFilePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error: El archivo de origen no existe.";
                return;
            }

            if (string.IsNullOrWhiteSpace(yearInput) || !int.TryParse(yearInput, out int yearNumber) || yearInput.Length != 4)
            {
                MessageBox.Show("Por favor, ingresa un año válido (4 dígitos).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error: Año no válido.";
                txtYear.Focus();
                return;
            }
            string processingYear = yearInput;

            lblStatus.Text = $"Procesando archivo: {Path.GetFileName(sourceFilePath)} para el año {processingYear}...";
            this.Cursor = Cursors.WaitCursor; // Cambiar cursor a espera

            System.Windows.Forms.Application.DoEvents(); // Forzar actualización de UI

            try
            {
                var reader = new ExcelDataReader();
                LiquidacionData? liquidacion = reader.LeerLiquidacion(sourceFilePath);

                if (liquidacion == null)
                {
                    MessageBox.Show("No se pudieron leer los datos de la liquidación. Revisa la consola o el log para más detalles.", "Error de Lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Error al leer la liquidación.";
                    return;
                }

                // Definir ruta del archivo de destino
                string destinationFileName = "Consolidado_Liquidaciones.xlsx";
                // Guardar en la carpeta "Mis Documentos" del usuario
                string destinationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), destinationFileName);

                lblStatus.Text = $"Escribiendo en: {destinationFilePath} (Hoja: {processingYear})...";
                System.Windows.Forms.Application.DoEvents();

                var writer = new ExcelDataWriter();
                var dataList = new List<LiquidacionData> { liquidacion };
                bool success = writer.EscribirConsolidado(dataList, destinationFilePath, processingYear);

                if (success)
                {
                    MessageBox.Show($"Liquidación procesada y guardada exitosamente en:\n{destinationFilePath}\n(Hoja: {processingYear})", "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "Proceso completado exitosamente.";
                }
                else
                {
                    MessageBox.Show("Ocurrió un error al guardar el archivo consolidado.", "Error de Escritura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Error al guardar el consolidado.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado durante el procesamiento:\n{ex.Message}", "Error General", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error inesperado en el procesamiento.";
            }
            finally
            {
                this.Cursor = Cursors.Default; // Restaurar cursor
            }
        }

        private void btnBrowseXlsSource_Click(object sender, EventArgs e)
        {
            // Asumo que tienes un FolderBrowserDialog llamado folderBrowserDialog1 en tu formulario
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtXlsSourceFolder.Text = folderBrowserDialog1.SelectedPath;
                lstConversionLog.Items.Clear(); // Asumo ListBox llamado lstConversionLog
                lstConversionLog.Items.Add($"Carpeta de origen .xls seleccionada: {txtXlsSourceFolder.Text}");
            }
        }

        private void btnStartConversion_Click(object sender, EventArgs e)
        {
            string sourceFolder = txtXlsSourceFolder.Text;

            if (string.IsNullOrWhiteSpace(sourceFolder) || !Directory.Exists(sourceFolder))
            {
                MessageBox.Show("Por favor, selecciona una carpeta de origen válida.", "Error de Carpeta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string destinationFolder = Path.Combine(sourceFolder, "XLSX_Convertidos");
            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                    lstConversionLog.Items.Add($"Carpeta de destino creada: {destinationFolder}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear la carpeta de destino '{destinationFolder}': {ex.Message}", "Error de Carpeta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (conversionWorker.IsBusy)
            {
                MessageBox.Show("Ya hay un proceso de conversión en curso.", "Proceso Activo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            lstConversionLog.Items.Add("Iniciando conversión de archivos .xls a .xlsx...");
            btnStartConversion.Enabled = false;
            btnBrowseXlsSource.Enabled = false; // Deshabilitar también este botón
            this.Cursor = Cursors.WaitCursor;

            List<string> taskData = new List<string> { sourceFolder, destinationFolder };
            conversionWorker.RunWorkerAsync(taskData);
        }

        //Limi

        private void ConversionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> folders = e.Argument as List<string>;
            string sourceFolder = folders[0];
            string destinationFolder = folders[1];
            int filesConverted = 0;
            int filesWithError = 0;
            string[] xlsFiles = Directory.GetFiles(sourceFolder, "*.xls");

            if (xlsFiles.Length == 0)
            {
                conversionWorker.ReportProgress(0, "No se encontraron archivos .xls en la carpeta seleccionada.");
                e.Result = new Tuple<int, int>(0, 0);
                return;
            }

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.DisplayAlerts = false;

                for (int i = 0; i < xlsFiles.Length; i++)
                {
                    string xlsFilePath = xlsFiles[i];
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(xlsFilePath);
                    string xlsxFilePath = Path.Combine(destinationFolder, fileNameWithoutExtension + ".xlsx");
                    Workbook workbook = null;

                    conversionWorker.ReportProgress((i * 100) / xlsFiles.Length, $"Convirtiendo: {Path.GetFileName(xlsFilePath)}");

                    try
                    {
                        workbook = excelApp.Workbooks.Open(xlsFilePath);
                        workbook.SaveAs(xlsxFilePath, XlFileFormat.xlOpenXMLWorkbook); // 51 es xlOpenXMLWorkbook
                        filesConverted++;
                        conversionWorker.ReportProgress(((i + 1) * 100) / xlsFiles.Length, $"Convertido: {Path.GetFileName(xlsxFilePath)}");
                    }
                    catch (Exception ex)
                    {
                        filesWithError++;
                        conversionWorker.ReportProgress(((i + 1) * 100) / xlsFiles.Length, $"ERROR en {Path.GetFileName(xlsFilePath)}: {ex.Message.Split('\n')[0]}");
                    }
                    finally
                    {
                        workbook?.Close(false);
                        if (workbook != null) Marshal.ReleaseComObject(workbook);
                    }
                }
                e.Result = new Tuple<int, int>(filesConverted, filesWithError);
            }
            catch (Exception ex)
            {
                // Error general del proceso (ej: no se pudo iniciar Excel)
                conversionWorker.ReportProgress(0, $"ERROR general del proceso: {ex.Message}");
                e.Result = new Tuple<int, int>(filesConverted, filesWithError); // Retorna lo que se haya alcanzado
            }
            finally
            {
                excelApp?.Quit();
                if (excelApp != null) Marshal.ReleaseComObject(excelApp);
                // Forzar liberación de memoria para objetos COM
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void ConversionWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                lstConversionLog.Items.Add(e.UserState.ToString());
                lstConversionLog.SelectedIndex = lstConversionLog.Items.Count - 1; // Auto-scroll
            }
            // Aquí podrías actualizar una ProgressBar si la añades: progressBarConversion.Value = e.ProgressPercentage;
        }

        private void ConversionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Default;
            btnStartConversion.Enabled = true;
            btnBrowseXlsSource.Enabled = true;

            if (e.Error != null)
            {
                lstConversionLog.Items.Add($"ERROR MAYOR EN EL PROCESO: {e.Error.Message}");
                MessageBox.Show($"Ocurrió un error mayor durante la conversión: {e.Error.Message}", "Error Crítico de Conversión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Tuple<int, int> result = e.Result as Tuple<int, int>;
                string summary = $"Conversión finalizada. Archivos convertidos: {result?.Item1 ?? 0}. Archivos con error: {result?.Item2 ?? 0}.";
                lstConversionLog.Items.Add(summary);
                MessageBox.Show(summary, "Conversión Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            lstConversionLog.SelectedIndex = lstConversionLog.Items.Count - 1;


        }

        private void btnBrowseConsolidationFolder_Click(object sender, EventArgs e)
        {
            // Usamos el folderBrowserDialog2 que creaste
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                txtConsolidationSourceFolder.Text = folderBrowserDialog2.SelectedPath;
                lstConsolidationLog.Items.Clear();
                lstConsolidationLog.Items.Add($"Carpeta de origen seleccionada: {txtConsolidationSourceFolder.Text}");
            }
        }

        private void btnStartBatchConsolidation_Click(object sender, EventArgs e)
        {
            string sourceFolder = txtConsolidationSourceFolder.Text;
            string yearInput = txtConsolidationYear.Text;

            if (string.IsNullOrWhiteSpace(sourceFolder) || !Directory.Exists(sourceFolder))
            {
                MessageBox.Show("Por favor, selecciona una carpeta de origen válida.", "Error de Carpeta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(yearInput) || !int.TryParse(yearInput, out _) || yearInput.Length != 4)
            {
                MessageBox.Show("Por favor, ingresa un año válido (4 dígitos).", "Error de Año", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtConsolidationYear.Focus();
                return;
            }

            if (consolidationWorker.IsBusy)
            {
                MessageBox.Show("Ya hay un proceso de consolidación en curso.", "Proceso Activo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            lstConsolidationLog.Items.Clear();
            lstConsolidationLog.Items.Add("Iniciando consolidación por lote...");
            btnStartBatchConsolidation.Enabled = false;
            btnBrowseConsolidationFolder.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            // Definir la ruta del archivo de destino
            string destinationFileName = "Consolidado_Liquidaciones.xlsx";
            string destinationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), destinationFileName);

            // Pasar los datos necesarios
            List<object> taskData = new List<object> { sourceFolder, yearInput, destinationFilePath };
            consolidationWorker.RunWorkerAsync(taskData);
        }

        // --- MÉTODOS DEL BACKGROUNDWORKER PARA LA CONSOLIDACIÓN POR LOTE ---

        private void ConsolidationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> taskData = e.Argument as List<object>;
            string sourceFolder = taskData[0].ToString();
            string processingYear = taskData[1].ToString();
            string destinationFilePath = taskData[2].ToString();

            var allData = new List<LiquidacionData>();
            var reader = new ExcelDataReader();
            int filesWithError = 0;

            string[] xlsxFiles = Directory.GetFiles(sourceFolder, "*.xlsx");

            if (xlsxFiles.Length == 0)
            {
                consolidationWorker.ReportProgress(0, "No se encontraron archivos .xlsx en la carpeta seleccionada.");
                e.Result = new Tuple<int, int>(0, 0); // (procesados, errores)
                return;
            }

            for (int i = 0; i < xlsxFiles.Length; i++)
            {
                string filePath = xlsxFiles[i];
                consolidationWorker.ReportProgress(0, $"Leyendo archivo: {Path.GetFileName(filePath)}...");
                try
                {
                    LiquidacionData? liquidacion = reader.LeerLiquidacion(filePath);
                    if (liquidacion != null)
                    {
                        allData.Add(liquidacion);
                    }
                    else
                    {
                        filesWithError++;
                        consolidationWorker.ReportProgress(0, $"ADVERTENCIA: No se pudieron leer datos de {Path.GetFileName(filePath)}.");
                    }
                }
                catch (Exception ex)
                {
                    filesWithError++;
                    consolidationWorker.ReportProgress(0, $"ERROR al leer {Path.GetFileName(filePath)}: {ex.Message.Split('\n')[0]}");
                }
            }

            if (allData.Any())
            {
                consolidationWorker.ReportProgress(0, "Escribiendo datos en el archivo consolidado...");
                var writer = new ExcelDataWriter();
                bool success = writer.EscribirConsolidado(allData, destinationFilePath, processingYear);
                if (!success)
                {
                    // Marcar todos los archivos como erróneos si la escritura falla
                    filesWithError = xlsxFiles.Length;
                    consolidationWorker.ReportProgress(0, "ERROR: No se pudo escribir en el archivo de destino.");
                }
            }

            e.Result = new Tuple<int, int>(allData.Count, filesWithError);
        }

        private void ConsolidationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Actualiza el log de la consolidación. Asumo que el ListBox se llama lstConsolidationLog
            if (e.UserState != null)
            {
                lstConsolidationLog.Invoke(new System.Action(() =>
                {
                    lstConsolidationLog.Items.Add(e.UserState.ToString());
                    lstConsolidationLog.SelectedIndex = lstConsolidationLog.Items.Count - 1;
                }));
            }
        }

        private void ConsolidationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Actualiza la UI al finalizar. Asumo que los botones se llaman btnStartBatchConsolidation y btnBrowseConsolidationFolder
            this.Cursor = Cursors.Default;
            btnStartBatchConsolidation.Enabled = true;
            btnBrowseConsolidationFolder.Enabled = true;

            if (e.Error != null)
            {
                lstConsolidationLog.Invoke(new System.Action(() => lstConsolidationLog.Items.Add($"ERROR MAYOR EN EL PROCESO: {e.Error.Message}")));
                MessageBox.Show($"Ocurrió un error mayor durante la consolidación por lote: {e.Error.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Tuple<int, int> result = e.Result as Tuple<int, int>;
                string summary = $"Consolidación por lote finalizada. Archivos procesados con éxito: {result?.Item1 ?? 0}. Archivos con error: {result?.Item2 ?? 0}.";
                lstConsolidationLog.Invoke(new System.Action(() => lstConsolidationLog.Items.Add(summary)));
                MessageBox.Show(summary, "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


    }
}
