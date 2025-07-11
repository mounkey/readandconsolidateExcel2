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
// using OfficeOpenXml; // Ya lo pusimos en el constructor para LicenseContext, no es estrictamente necesario aqu� arriba si no usas tipos de EPPlus directamente en Form1

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
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

            // Validaciones b�sicas
            if (string.IsNullOrWhiteSpace(sourceFilePath))
            {
                MessageBox.Show("Por favor, selecciona un archivo de liquidaci�n de origen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error: No se seleccion� archivo de origen.";
                return;
            }

            if (!File.Exists(sourceFilePath)) // Necesitar�s: using System.IO;
            {
                MessageBox.Show($"El archivo de origen especificado no existe: {sourceFilePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error: El archivo de origen no existe.";
                return;
            }

            if (string.IsNullOrWhiteSpace(yearInput) || !int.TryParse(yearInput, out int yearNumber) || yearInput.Length != 4)
            {
                MessageBox.Show("Por favor, ingresa un a�o v�lido (4 d�gitos).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error: A�o no v�lido.";
                txtYear.Focus();
                return;
            }
            string processingYear = yearInput;

            lblStatus.Text = $"Procesando archivo: {Path.GetFileName(sourceFilePath)} para el a�o {processingYear}...";
            this.Cursor = Cursors.WaitCursor; // Cambiar cursor a espera
            Application.DoEvents(); // Forzar actualizaci�n de UI

            try
            {
                var reader = new ExcelDataReader();
                LiquidacionData? liquidacion = reader.LeerLiquidacion(sourceFilePath);

                if (liquidacion == null)
                {
                    MessageBox.Show("No se pudieron leer los datos de la liquidaci�n. Revisa la consola o el log para m�s detalles.", "Error de Lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Error al leer la liquidaci�n.";
                    return;
                }

                // Definir ruta del archivo de destino
                string destinationFileName = "Consolidado_Liquidaciones.xlsx";
                // Guardar en la carpeta "Mis Documentos" del usuario
                string destinationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), destinationFileName);

                lblStatus.Text = $"Escribiendo en: {destinationFilePath} (Hoja: {processingYear})...";
                Application.DoEvents();

                var writer = new ExcelDataWriter();
                var dataList = new List<LiquidacionData> { liquidacion };
                bool success = writer.EscribirConsolidado(dataList, destinationFilePath, processingYear);

                if (success)
                {
                    MessageBox.Show($"Liquidaci�n procesada y guardada exitosamente en:\n{destinationFilePath}\n(Hoja: {processingYear})", "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "Proceso completado exitosamente.";
                }
                else
                {
                    MessageBox.Show("Ocurri� un error al guardar el archivo consolidado.", "Error de Escritura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Error al guardar el consolidado.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurri� un error inesperado durante el procesamiento:\n{ex.Message}", "Error General", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error inesperado en el procesamiento.";
            }
            finally
            {
                this.Cursor = Cursors.Default; // Restaurar cursor
            }
        }

    }
}
