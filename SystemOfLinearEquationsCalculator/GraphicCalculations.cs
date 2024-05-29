using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System;

namespace SystemOfLinearEquationsCalculator
{
    public static class GraphicCalculations
    {
        public static void GraphicalSolution(Matrix matrix, double[] subMatrix, double[] results, TextBlock textBlock)
        {
            if (matrix[0, 0] == 0)
            {
                (matrix[0, 0], matrix[1, 0]) = (matrix[1, 0], matrix[0, 0]);
                (matrix[0, 1], matrix[1, 1]) = (matrix[1, 1], matrix[0, 1]);
                (subMatrix[0], subMatrix[1]) = (subMatrix[1], subMatrix[0]);
            }
            
            var calculations = new double[5];
            calculations[0] = matrix[1, 0] / matrix[0, 0];
            calculations[1] = calculations[0] * subMatrix[0];
            calculations[2] = calculations[0] * matrix[0, 1];
            calculations[3] = matrix[1, 1] - calculations[2];
            calculations[4] = subMatrix[1] - calculations[1];
            
            textBlock.Text = 
                $"\n{matrix[0, 0]}x + {matrix[0, 1]}y = {subMatrix[0]}\n" +
                $"{matrix[1, 0]}x + {matrix[1, 1]}y = {subMatrix[1]}\n\n" +
                $"x = ({subMatrix[0]} - {matrix[0, 1]}y) / {matrix[0, 0]}\n" +
                $"{matrix[1, 0]}x + {matrix[1, 1]}y = {subMatrix[1]}\n\n" +
                $"x = ({subMatrix[0]} - {matrix[0, 1]}y) / {matrix[0, 0]}\n" +
                $"{matrix[1, 0]} * ({subMatrix[0]} - {matrix[0, 1]}y) / {matrix[0, 0]} + {matrix[1, 1]}y = {subMatrix[1]}\n\n" +
                $"x = ({subMatrix[0]} - {matrix[0, 1]}y) / {matrix[0, 0]}\n" +
                $"{calculations[1]} - {calculations[2]}y + {matrix[1, 1]}y = {subMatrix[1]}\n\n" +
                $"x = ({subMatrix[0]} - {matrix[0, 1]}y) / {matrix[0, 0]}\n" +
                $"y = ({subMatrix[1]} - {calculations[1]}) / ({matrix[1, 1]} - {calculations[2]})\n\n" +
                $"x = ({subMatrix[0]} - {matrix[0, 1]} * {results[1]}) / {matrix[0, 0]}\n" +
                $"y = {results[1]}\n\n" +
                $"x = {results[0]}\n" +
                $"y = {results[1]}\n";
        }
        
        public static void ShowSystemOfCoordinates(Matrix matrix, double[] subMatrix, double[] results, Canvas fullSystem, Canvas system)
        {
            fullSystem.Height = 400;
            fullSystem.Width = 400;

            for (var i = -200; i <= 200; i += 10)
            {
                AddLine(-200, 200, i, i, system, Brushes.Gray, 1);
                AddLine(i, i, -200, 200, system, Brushes.Gray, 1);
            }

            AddLine(-200, 200, 0, 0, system, Brushes.Black, 2);
            AddLine(0, 0, 200, -200, system, Brushes.Black, 2);
            AddLine(-5, 0, -190, -200, system, Brushes.Black, 2);
            AddLine(5, 0, -190, -200, system, Brushes.Black, 2);
            AddLine(190, 200, -5, 0, system, Brushes.Black, 2);
            AddLine(190, 200, 5, 0, system, Brushes.Black, 2);

            AddTextBlock("y", 11, -208, system, Brushes.Red);
            AddTextBlock("x", 191, -28, system, Brushes.Blue);
            
            var sizeDif = Math.Abs(results[0]) >= 200 || Math.Abs(results[1]) >= 200 
                ? 1 + Math.Ceiling(Math.Max(Math.Abs(results[0]), Math.Abs(results[1])) / 200)
                : 0.01 + Math.Ceiling(Math.Max(Math.Abs(results[0]), Math.Abs(results[1]))) / 200;
            
            system.Children.Add(BuildLine(matrix[0, 0], matrix[0, 1], subMatrix[0] / sizeDif));
            system.Children.Add(BuildLine(matrix[1, 0], matrix[1, 1], subMatrix[1] / sizeDif));
            
            AddLine(-5, 5, -results[1] / sizeDif, -results[1] / sizeDif, system, Brushes.Black, 2);
            AddLine(results[0] / sizeDif, results[0] / sizeDif, -5, 5, system, Brushes.Black, 2);

            var y = -(results[1] / sizeDif);
            AddTextBlock($"{results[1]:0.###E+0}", 11, y < 0 ? y : y -25, system, Brushes.Red);
            var x = results[0] / sizeDif - 25;
            AddTextBlock($"{results[0]:0.###E+0}", x > 0 ? x -60 : x + 25, -28, system, Brushes.Blue);
            
            var point = new Ellipse { Stroke = Brushes.Green, Height = 10, Width = 10 };
            Canvas.SetLeft(point, results[0] / sizeDif - point.Width / 2);
            Canvas.SetTop(point, -results[1] / sizeDif - point.Height / 2);
            system.Children.Add(point);
        }
        
        public static void ClearSystemOfCoordinates(Canvas fullSystem, Canvas system)
        {
            fullSystem.Height = 0;
            fullSystem.Width = 0;
            system.Children.Clear();
        }

        private static void AddLine(double x1, double x2, double y1, double y2, Panel system, Brush color, double thickness)
        {
            var line = new Line { Stroke = color, StrokeThickness = thickness, X1 = x1, X2 = x2, Y1 = y1, Y2 = y2 };
            system.Children.Add(line);
        }
        
        private static void AddTextBlock(string text, double x, double y, Panel system, Brush color)
        {
            var textBlock = new TextBlock {Text = text, FontSize = 16, Foreground = color};
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            system.Children.Add(textBlock);
        }
        
        private static Line BuildLine(double a, double b, double c)
        {
            var line = new Line { Stroke = Brushes.Black, StrokeThickness = 2 };
            bool change;

            if (a < b)
            {
                line.X1 = -200;
                line.Y1 = -(c - a * -200) / b;
                line.X2 = 200;
                line.Y2 = -(c - a * 200) / b;
            }
            else
            {
                line.Y1 = 200;
                line.X1 = (c - b * -200) / a;
                line.Y2 = -200;
                line.X2 = (c - b * 200) / a;
            }
            
            (line.Y1, change) = SubBuildLine(line.Y1);
            if (change) line.X1 = (c - b * -line.Y1) / a;
            
            (line.X1, change) = SubBuildLine(line.X1);
            if (change) line.Y1 = -(c - a * line.X1) / b;
            
            (line.Y2, change) = SubBuildLine(line.Y2);
            if (change) line.X2 = (c - b * -line.Y2) / a;
            
            (line.X2, change) = SubBuildLine(line.X2);
            if (change) line.Y2 = -(c - a * line.X2) / b;
            
            return line;
        }

        private static (double, bool) SubBuildLine(double number)=>
            number > 200 ? (200, true) :
            number < -200 ? (-200, true) :
            (number, false);
    }
}
