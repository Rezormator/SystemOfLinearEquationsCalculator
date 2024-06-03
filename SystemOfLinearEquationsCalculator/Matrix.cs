using System;

namespace SystemOfLinearEquationsCalculator
{
    public class Matrix
    {
        public int Rows { get; }
        public int Columns { get; }
        private double[,] Data { get; }
 
        public Matrix(int rows, int columns)
        {
            if (rows < 0) throw new ArgumentOutOfRangeException(nameof(rows));
            if (columns < 0) throw new ArgumentOutOfRangeException(nameof(columns));
            Rows = rows;
            Columns = columns;
            Data = new double[rows, columns];
        }
        
        public double this[int row, int column]
        {
            get
            {
                if (row >= Rows || row < 0 || column >= Columns || column < 0) throw new ArgumentOutOfRangeException();
                return Data[row, column];
            }
            set
            {
                if (row >= Rows || row < 0 || column >= Columns || column < 0) throw new ArgumentOutOfRangeException();
                Data[row, column] = value;
            }
        }
        
        public Matrix Clone(ref int iterationsAmount)
        {
            var copyArray = new Matrix(Rows, Columns);

            for (var i = 0; i < Rows; i++)
            {
                iterationsAmount++;
                    
                for (var j = 0; j < Columns; j++)
                {
                    iterationsAmount++;
                    copyArray[i, j] = Data[i, j];
                }
            }
            
            return copyArray;
        }
        
        public double CalculateDeterminant(ref int iterationsAmount)
        {
            if (Rows == 1) return Data[0, 0];

            double result = 0;

            for (var i = 0; i < Rows; i++)
            {
                iterationsAmount++;
                
                if (Data[0, i] == 0) continue;

                var minor = GetMinor(i, ref iterationsAmount);
                result += Data[0, i] * Math.Pow(-1, i) * minor.CalculateDeterminant(ref iterationsAmount);
            }
            
            return result;
        }
        
        private Matrix GetMinor(int col, ref int iterationsAmount)
        {
            var minor = new Matrix(Rows - 1, Columns - 1);

            for (int i = 0, p = 0; i < Rows; i++)
            {
                iterationsAmount++;
                
                if (i == 0) continue;

                for (int j = 0, q = 0; j < Rows; j++)
                {
                    iterationsAmount++;
                    if (j != col) minor[p, q++] = Data[i, j];
                }

                p++;
            }

            return minor;
        }
    }
}