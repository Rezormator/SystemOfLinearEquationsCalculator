using System.IO;
using System.Windows;

namespace SystemOfLinearEquationsCalculator
{
    public static class SystemToFile
    {
        public static void WriteToTheFile(double[,] matrix, double[] subMatrix, double[] results, string fileName)
        {
            if (!Validation.IsValidFileName(fileName))
                return;
            
            var directory = Directory.GetCurrentDirectory();
            directory = directory.Replace("\\SystemOfLinearEquationsCalculator\\bin\\Debug", "");
            var filePath = Path.Combine(directory, $"{fileName}.txt");

            try
            {
                using (var writer = File.AppendText(filePath))
                {
                    writer.WriteLine("System:");
                    for (var i = 0; i < subMatrix.Length; i++)
                    {
                        for (var j = 0; j < subMatrix.Length; j++)
                            writer.Write("+ (" + matrix[i, j] + ")x" + (j + 1) + " ");

                        writer.WriteLine("= " + subMatrix[i]);
                    }

                    writer.WriteLine("\nSolutions:");
                    for (var i = 0; i < subMatrix.Length; i++)
                        writer.WriteLine("x" + (i + 1) + " = " + results[i]);

                    writer.WriteLine("\n");
                    
                    MessageBox.Show("System write to file");
                }
            } 
            catch
            {
                MessageBox.Show("Error: can't write system to file");
            }
        }
    }
}
