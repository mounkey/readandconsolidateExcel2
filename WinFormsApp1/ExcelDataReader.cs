using OfficeOpenXml; // EPPlus
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReadAndConsolidateExcel
{
    public class ExcelDataReader
    {
        public LiquidacionData? LeerLiquidacion(string rutaArchivoOrigen)
        {
            if (!File.Exists(rutaArchivoOrigen))
            {
                Console.WriteLine($"Error: El archivo de origen no existe en la ruta: {rutaArchivoOrigen}");
                return null;
            }

            var data = new LiquidacionData();
            FileInfo fileInfo = new FileInfo(rutaArchivoOrigen);

            // Configurar el contexto de licencia para EPPlus si es necesario (para versiones > 5.x)
            // ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // o LicenseContext.Commercial

            try
            {
                using (var package = new ExcelPackage(fileInfo))
                {
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        Console.WriteLine("Error: El archivo de Excel de origen no contiene hojas.");
                        return null;
                    }

                    var worksheet = package.Workbook.Worksheets.First(); // Asumimos la primera hoja

                    // 1. PERIODO (Mes) y AÑO (para nombre de hoja, se pasa por fuera)
                    string periodoCompleto = worksheet.Cells["B8"].Text?.Trim() ?? string.Empty; // Estimada
                    if (!string.IsNullOrWhiteSpace(periodoCompleto))
                    {
                        // Ejemplo: "MES DE MARZO DE 2019"
                        var match = Regex.Match(periodoCompleto, @"MES DE (\w+) DE (\d{4})", RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            data.Periodo = match.Groups[1].Value.ToUpper(); // Ej: MARZO
                            // El año (match.Groups[2].Value) se usará para el nombre de la hoja de destino
                            // pero ya lo pedimos al usuario. Podemos usarlo para validar si coincide.
                        }
                        else
                        {
                            // Si no coincide el formato esperado, intentar tomar el primer texto que parezca mes
                            string[] partes = periodoCompleto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (partes.Length > 2 && partes[0].Equals("MES", StringComparison.OrdinalIgnoreCase) && partes[1].Equals("DE", StringComparison.OrdinalIgnoreCase))
                            {
                                data.Periodo = partes[2].ToUpper();
                            }
                            else
                            {
                                data.Periodo = "MES_NO_EXTRAIDO"; // O alguna indicación de error
                            }
                        }
                    }

                    // 2. RUT
                    data.Rut = worksheet.Cells["B12"].Text?.Trim(); // Estimada

                    // 3. NOMBRES (ApellidoPaterno, ApellidoMaterno, Nombres)
                    string nombreCompleto = worksheet.Cells["B11"].Text?.Trim() ?? string.Empty; // Estimada
                    if (!string.IsNullOrWhiteSpace(nombreCompleto))
                    {
                        string[] partesNombre = nombreCompleto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (partesNombre.Length > 0) data.ApellidoPaterno = partesNombre[0];
                        if (partesNombre.Length > 1) data.ApellidoMaterno = partesNombre[1];
                        if (partesNombre.Length > 2) data.Nombres = string.Join(" ", partesNombre.Skip(2));
                        else if (partesNombre.Length == 1) data.Nombres = partesNombre[0]; // Si solo hay una palabra, asumirla como nombre
                    }

                    data.SueldoBase = GetDecimalFromCell(worksheet, "D15");
                    data.CentroDeCosto = worksheet.Cells["B13"].Text?.Trim(); // Estimada
                    data.DiasTrabajados = GetIntFromCell(worksheet, "B15");
                    data.Vacaciones = GetDecimalFromCell(worksheet, "D16");
                    data.Vacaciones_dias = GetDecimalFromCell(worksheet, "B16");// Tratar "-" como 0 o nulo

                    data.IsapreFonasa = worksheet.Cells["A26"].Text?.Trim(); // Estimada (Nombre Institución)
                    data.Afp = worksheet.Cells["A25"].Text?.Trim(); // Estimada (Nombre Institución)

                    string porcentajeAfpText = worksheet.Cells["B25"].Text?.Replace("%", "").Trim() ?? string.Empty; // Estimada
                    if (decimal.TryParse(porcentajeAfpText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal porcAfp))
                        data.PorcentajeAfp = porcAfp;

                    string porcentajeSaludText = worksheet.Cells["B26"].Text?.Replace("%", "").Trim() ?? string.Empty; // Estimada
                    if (decimal.TryParse(porcentajeSaludText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal porcSalud))
                        data.PorcentajeSalud= porcSalud; // Asumimos que es el mismo porcentaje para AFP y Salud

                    string porcentajeCesantiaText = worksheet.Cells["B27"].Text?.Replace("%", "").Trim() ?? string.Empty; // Estimada
                    if (decimal.TryParse(porcentajeCesantiaText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal porcCesantia))
                        data.PorcentacjeCesantia= porcCesantia; // Asumimos que es el mismo porcentaje para AFP y Salud

                    // SUELDO MENSUAL - PENDIENTE CELDA ORIGEN
                    // data.SueldoMensual = GetDecimalFromCell(worksheet, "CELDA_SUELDO_MENSUAL"); 
                    data.SueldoMensual = data.SueldoBase; // Asunción temporal hasta tener la celda correcta

                    data.Gratificacion = GetDecimalFromCell(worksheet, "D18");
                    data.TotalImponible = GetDecimalFromCell(worksheet, "D19");

                    // TOTAL NO IMPONIBLE - Suma de Locomoción y Colación
                    decimal? locomocion = GetDecimalFromCell(worksheet, "D20");
                    decimal? colacion = GetDecimalFromCell(worksheet, "D21"); // GetDecimalFromCell maneja "-" como nulo
                    data.TotalNoImponible = (locomocion ?? 0) + (colacion ?? 0);
                    if (data.TotalNoImponible == 0 && locomocion == null && colacion == null) data.TotalNoImponible = null;


                    data.MontoAfp = GetDecimalFromCell(worksheet, "C25");
                    data.MontoSalud = GetDecimalFromCell(worksheet, "C26");
                    data.SeguroCesantia = GetDecimalFromCell(worksheet, "C27"); // GetDecimalFromCell maneja "-"
                    data.TotalDescuentos = GetDecimalFromCell(worksheet, "D29");
                    data.LiquidoAPagar = GetDecimalFromCell(worksheet, "D31");

                    // TRIBUTABLE - PENDIENTE CELDA ORIGEN
                    // data.Tributable = GetDecimalFromCell(worksheet, "CELDA_TRIBUTABLE");
                    data.Tributable = data.TotalImponible; // Asunción temporal

                    // Campos que se dejan en blanco/nulo por ahora:
                    data.Atraso = null;
                    data.Plan = null;
                    data.CargaFamiliar = null;
                    data.Apv1 = null;
                    data.Apv2 = null;
                    data.ImpuestoUnico = null;
                    data.TotalOtrosDescuentos = null;
                    data.Sis = null;
                    data.Mutual = null;
                    data.AporteSeguroCesantiaEmpleador = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error al leer el archivo de Excel: {ex.Message}");
                return null; // O lanzar la excepción si se prefiere un manejo más arriba
            }

            return data;
        }

        // Helper para convertir texto de celda a decimal, manejando comas, puntos y "-"
        private decimal? GetDecimalFromCell(ExcelWorksheet worksheet, string cellAddress)
        {
            string text = worksheet.Cells[cellAddress].Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text) || text == "-")
            {
                return null;
            }
            // Reemplazar comas de miles por nada, y punto decimal de Excel a punto decimal de InvariantCulture
            text = text.Replace(",", ""); // Asumimos que la coma es separador de miles
                                          // No es necesario reemplazar el punto si la configuración regional de Excel usa punto decimal.
                                          // Si Excel usa coma decimal, y los miles son puntos, la lógica debe cambiar.
                                          // Por ahora, esta heurística es común para muchos formatos de LatAm.

            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value))
            {
                return value;
            }
            else
            {
                Console.WriteLine($"Advertencia: No se pudo convertir '{worksheet.Cells[cellAddress].Text}' de la celda {cellAddress} a decimal.");
                return null;
            }
        }

        // Helper para convertir texto de celda a int
        private int? GetIntFromCell(ExcelWorksheet worksheet, string cellAddress)
        {
            string text = worksheet.Cells[cellAddress].Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text) || text == "-")
            {
                return null;
            }
            if (int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
            {
                return value;
            }
            else
            {
                Console.WriteLine($"Advertencia: No se pudo convertir '{worksheet.Cells[cellAddress].Text}' de la celda {cellAddress} a entero.");
                return null;
            }
        }
    }
}
