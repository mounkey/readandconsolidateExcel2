using OfficeOpenXml: // EPPlus

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;.
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
