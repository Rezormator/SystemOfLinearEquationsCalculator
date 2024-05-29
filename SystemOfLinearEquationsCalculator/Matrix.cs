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
        
        public Matrix Clone()
        {
            var copyArray = new Matrix(Rows, Columns);

            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    copyArray[i, j] = Data[i, j];
                }
            }
            
            return copyArray;
        }
    }
}