using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Linq;

namespace SystemOfLinearEquationsCalculator
{
    public partial class MainWindow
    {
        private int _calculationMethod;
        private int _size;
        private Matrix _matrix;
        private double[] _subMatrix;
        private double[] _results;

        public MainWindow()
        {
            InitializeComponent();

            for (var i = 2; i <= 9; i++)
            {
                SizeSelector.Items.Add(new ComboBoxItem { Content = i });
            }

            _size = 2;
            _calculationMethod = 0;
            SystemActions.CreateSystem(_size, SystemGrid);
        }

        private void SizeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SystemGrid == null) return;

            ClearFields();
            _size = int.Parse(((ComboBoxItem)SizeSelector.SelectedItem).Content.ToString());
            SystemActions.CreateSystem(_size, SystemGrid);
        }

        private void KramerMethod_Click(object sender, RoutedEventArgs e)
        {
            _calculationMethod = 0;
            MethodName.Text = "Calculating with Kramer method";
        }

        private void GaussMethodWithSingleCoefficientsButton_Click(object sender, RoutedEventArgs e)
        {
            _calculationMethod = 1;
            MethodName.Text = "Calculating with Gauss method with single coefficients";
        }

        private void GaussMethodWithMainElementButton_Click(object sender, RoutedEventArgs e)
        {
            _calculationMethod = 2;
            MethodName.Text = "Calculating with Gauss method with main element";
        }

        private void GenerateSystemButton_Click(object sender, RoutedEventArgs e)
        {
            (_matrix, _subMatrix) = SystemActions.GenerateSystem(_size);
            SystemActions.FillSystem(_matrix, _subMatrix, _size, SystemGrid);
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();

            if (!Validation.IsValidNumbersFormat(_size * 2 - 1, _size * 2 + 1, SystemGrid)) return;

            (_matrix, _subMatrix) = SystemActions.ReadSystemValues(_size, SystemGrid);

            if (!Validation.IsValidSystem(_matrix)) return;

            _results = new double[_size];
            int iterationsAmount;
            
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            switch (_calculationMethod)
            {
                case 0:
                    (_results, iterationsAmount) = 
                        Calculations.KramerMethod(_matrix.Clone(), _subMatrix.ToArray(), _size);
                    break;
                case 1:
                    (_results, iterationsAmount) = 
                        Calculations.GaussMethodWithSingleCoefficients(_matrix.Clone(), _subMatrix.ToArray(), _size);
                    break;
                case 2:
                    (_results, iterationsAmount) =
                        Calculations.GaussMethodWithMainElement(_matrix.Clone(), _subMatrix.ToArray(), _size);
                    break;
                default:
                    MessageBox.Show("Error: Wrong method of calculations");
                    return;
            }
            
            stopwatch.Stop();
            
            if (!Validation.IsValidResults(_results)) return;
            
            for (var i = 0; i < _size; i++)
            {
                _results[i] = Math.Round(_results[i], 3);
            }
            
            if (_size == 2)
            {
                GraphicCalculations.GraphicalSolution(_matrix, _subMatrix, _results, Results);
                GraphicCalculations.ShowSystemOfCoordinates(_matrix, _subMatrix, _results, FullSystemOfCoordinates,
                    SystemOfCoordinates);
            }
            else
            {
                for (var i = 0; i < _size; i++)
                {
                    Results.Text += "x" + (i + 1) + " = " + _results[i] + "\n";
                }
            }
            
            Results.Text += $"\nAmount of iterations: {iterationsAmount}\n" +
                            $"Calculating time: {stopwatch.Elapsed.TotalSeconds} seconds\n";
            WriteToFilePanel.Visibility = Visibility.Visible;
        }

        private void ClearSystemButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemGrid == null)
                return;

            ClearFields();
            SystemActions.CreateSystem(_size, SystemGrid);
        }

        private void WriteSystemToFile_Click(object sender, RoutedEventArgs e)
        {
            var fileName = FileName.Text;
            SystemToFile.WriteToTheFile(_matrix, _subMatrix, _results, fileName);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ClearFields()
        {
            GraphicCalculations.ClearSystemOfCoordinates(FullSystemOfCoordinates, SystemOfCoordinates);
            WriteToFilePanel.Visibility = Visibility.Hidden;
            Results.Text = "";
            FileName.Text = "Solutions";
        }
    }
}