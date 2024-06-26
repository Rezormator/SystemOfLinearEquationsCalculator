﻿using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace SystemOfLinearEquationsCalculator
{
    public static class SystemActions
    {
        public static void CreateSystem(int size, Grid systemGrid)
        {
            systemGrid.Children.Clear();
            systemGrid.RowDefinitions.Clear();
            systemGrid.ColumnDefinitions.Clear();

            var rows = size * 2 - 1;
            var cols = size * 2 + 1;
            
            Enumerable.Range(0, rows).ToList().ForEach(_ => systemGrid.RowDefinitions.Add(new RowDefinition()));
            Enumerable.Range(0, cols).ToList().ForEach(_ => systemGrid.ColumnDefinitions.Add(new ColumnDefinition()));
            
            for (var i = 0; i <= rows; i += 2)
            {
                for (var j = 0; j < cols; j += 2)
                {
                    var textBox = new TextBox { FontSize = 16, Width = 80, BorderBrush = Brushes.Gray };
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                    systemGrid.Children.Add(textBox);
                }
                
                for (var j = 1; j <= rows; j += 2)
                { 
                    var textBlock = new TextBlock 
                    {
                        FontSize = 16, 
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center, 
                        Foreground = Brushes.White,
                        Margin = new Thickness(5, 0, 5, 0) 
                    };
                    
                    textBlock.Text = (j != rows) ? "x" + (j / 2 + 1) + " +" : "x" + (j / 2 + 1) + " =";
                    
                    Grid.SetRow(textBlock, i);
                    Grid.SetColumn(textBlock, j);
                    systemGrid.Children.Add(textBlock);
                }
            }

            for (var i = 1; i <= rows; i += 2)
            {
                var textBlock = new TextBlock();
                Grid.SetRow(textBlock, i);
                Grid.SetColumn(textBlock, 1);
                systemGrid.Children.Add(textBlock);
            }
        }
        
        public static (Matrix, double[]) ReadSystemValues(int size, Grid systemGrid)
        {
            var rows = size * 2 - 1;
            var cols = size * 2 + 1;
            
            var matrix = new Matrix(size, size);
            var subMatrix = new double[size];
            
            for (var i = 0; i < rows; i += 2)
            {
                for (var j = 0; j < rows; j += 2)
                {
                    var element = systemGrid.Children.Cast<UIElement>()
                        .FirstOrDefault(ele => Grid.GetRow(ele) == i && Grid.GetColumn(ele) == j);

                    if (!(element is TextBox textBox)) continue;

                    matrix[i / 2, j / 2] = Math.Round(double.Parse(textBox.Text), 3);
                }
            }

            for (var i = 0; i < rows; i += 2)
            {
                var element = systemGrid.Children.Cast<UIElement>()
                    .FirstOrDefault(ele => Grid.GetRow(ele) == i && Grid.GetColumn(ele) == cols - 1);

                if (!(element is TextBox textBox)) continue;

                subMatrix[i / 2] = Math.Round(double.Parse(textBox.Text), 3);
            }
            
            return (matrix, subMatrix);
        }
        
        public static void FillSystem(Matrix matrix, double[] subMatrix, int size, Grid systemGrid)
        {
            var rows = size * 2 - 1;
            var cols = size * 2 + 1;
            
            for (var i = 0; i < rows; i += 2)
            {
                for (var j = 0; j < rows; j += 2)
                {
                    var element = systemGrid.Children.Cast<UIElement>()
                        .FirstOrDefault(ele => Grid.GetRow(ele) == i && Grid.GetColumn(ele) == j);

                    if (!(element is TextBox textBox)) continue;

                    textBox.Text = matrix[i / 2, j / 2].ToString("0.000000000");
                }
            }

            for (var i = 0; i < rows; i += 2)
            {
                var element = systemGrid.Children.Cast<UIElement>()
                    .FirstOrDefault(ele => Grid.GetRow(ele) == i && Grid.GetColumn(ele) == cols - 1);

                if (!(element is TextBox textBox)) continue;

                textBox.Text = subMatrix[i / 2].ToString("0.000000000");
            }
        }
        
        public static (Matrix, double[]) GenerateSystem(int size)
        {
            var matrix = new Matrix(size, size);
            var subMatrix = new double[size];
            var iterations = 0;

            do
            {
                var random = new Random();

                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        matrix[i, j] = Math.Round(random.NextDouble() * 2000 - 1000, 9);
                    }
                    
                    subMatrix[i] = Math.Round(random.NextDouble() * 2000 - 1000, 9);
                }
                
            } while (matrix.CalculateDeterminant(ref iterations) == 0);

            return (matrix, subMatrix);
        }
    }
}
