using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

namespace SystemOfLinearEquationsCalculator
{
    public static class Validation
    {
        public static bool IsValidNumbersFormat(int rows, int columns, Grid system)
        {
            var isValidMatrix = true;
            
            for (var i = 0; i < rows; i += 2) 
            {
                for (var j = 0; j < columns; j += 2)
                {
                    var element = system.Children.Cast<UIElement>()
                        .FirstOrDefault(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);

                    if (!(element is TextBox textBox))
                        continue;
                    
                    if (double.TryParse(textBox.Text, out _))
                    {
                        if (textBox.Text.Contains(" ") == false)
                        {
                            textBox.BorderBrush = Brushes.Gray;
                            continue;
                        } 
                        
                        MessageBox.Show("Error: values contains spaces");
                    }
                    
                    MessageBox.Show("Error: values are not in double format");
                    textBox.BorderBrush = Brushes.Red;
                    isValidMatrix = false;
                }
            }
            
            return isValidMatrix;
        }
        
        public static bool IsValidSystem(double[,] matrix)
        {
            var simpleOperationsAmount = 0;
            if (Calculations.CalculateDeterminant(matrix, ref simpleOperationsAmount) != 0)
                return true;
            
            MessageBox.Show("Error: determinant equal 0");
            
            return false;
        }
        
        public static bool IsValidResults(double[] results)
        {
            var isValidResults = results.All(result => !double.IsNaN(result) 
                && !double.IsPositiveInfinity(result) && !double.IsNegativeInfinity(result) 
                && result < double.MaxValue && result > double.MinValue);
            
            if (!isValidResults)
                MessageBox.Show("Error: results are not in double format");
            
            return isValidResults;
        }
        
        public static bool IsValidFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var isValidFileName = !fileName.Any(ch => invalidChars.Contains(ch));

            if (!isValidFileName)
                MessageBox.Show("Error: invalid file name");

            return isValidFileName;
        }
    }
}
