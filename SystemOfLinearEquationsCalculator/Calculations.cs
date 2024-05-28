using System;

namespace SystemOfLinearEquationsCalculator
{
    public static class Calculations
    {
        public static (double[], int) KramerMethod(double[,] matrix, double[] subMatrix, int size)
        {
            var results = new double[size];
            var iterationsAmount = 0;
            var determinant = CalculateDeterminant(matrix, ref iterationsAmount);

            for (var i = 0; i < size; i++)
            {
                iterationsAmount++;
                
                results[i] = CalculateDeterminant(SubKramerMatrix(CopyMatrix(matrix, ref iterationsAmount), 
                    subMatrix, i, ref iterationsAmount), ref iterationsAmount) / determinant;
            }
            return (results, iterationsAmount);
        }
        
        public static (double[], int) GaussMethodWithSingleCoefficients(double[,] matrix, double[] subMatrix, int size)
        {
            var results = new double[size];
            var iterationsAmount = 0;
            
            if (matrix[0, 0] == 0)
            {
                for (var i = 1; i < size; i++)
                {
                    iterationsAmount++;
                    
                    if (matrix[i, 0] == 0)
                        continue;

                    (subMatrix[0], subMatrix[i]) = (subMatrix[i], subMatrix[0]);

                    for (var j = 0; j < size; j++)
                    {
                        iterationsAmount++;
                        (matrix[0, j], matrix[i, j]) = (matrix[i, j], matrix[0, j]);
                    }
                    break;
                }
            }

            for (var i = 0; i < size; i++)
            {
                iterationsAmount++;
                
                var diagonalElement = matrix[i, i];
                subMatrix[i] /= diagonalElement;

                for (var k = i; k < size; k++)
                {
                    iterationsAmount++;
                    matrix[i, k] /= diagonalElement;
                }

                if (i == size - 1)
                    continue;

                for (var j = i + 1; j < size; j++)
                {
                    iterationsAmount++;
                    
                    var difference = -matrix[j, i] / matrix[i, i];
                    subMatrix[j] += subMatrix[i] * difference;

                    for (var k = 0; k < size; k++)
                    {
                        iterationsAmount++;
                        matrix[j, k] += matrix[i, k] * difference;
                    }
                }
            }

            for (var i = size - 1; i >= 0; i--)
            {
                iterationsAmount++;
                
                for (var j = size - 1; j > i; j--)
                {
                    iterationsAmount++;
                    subMatrix[i] -= matrix[i, j] * results[j];
                }

                results[i] = subMatrix[i] / matrix[i, i];
            }

            return (results, iterationsAmount);
        }

        public static (double[], int) GaussMethodWithMainElement(double[,] matrix, double[] subMatrix, int size)
        {
            var maxValuesCols = new int[size];
            var results = new double[size];
            var iterationsAmount = 0;

            for (var i = 0; i < size; i++)
            {
                iterationsAmount++;
                    
                var (maxRow, maxCol, max) = FindMaxElement(matrix, i, ref iterationsAmount);
                maxValuesCols[i] = maxCol;

                if (maxRow != i)
                {
                    (subMatrix[i], subMatrix[maxRow]) = (subMatrix[maxRow], subMatrix[i]);

                    for (var j = 0; j < size; j++)
                    {
                        iterationsAmount++;
                        (matrix[i, j], matrix[maxRow, j]) = (matrix[maxRow, j], matrix[i, j]);
                    }
                }

                for (var j = i + 1; j < size; j++)
                {
                    iterationsAmount++;
                    
                    var difference = -matrix[j, maxCol] / max;

                    for (var k = 0; k < size; k++)
                    {
                        iterationsAmount++;
                        matrix[j, k] += matrix[i, k] * difference;
                    }

                    subMatrix[j] += subMatrix[i] * difference;
                }
            }

            for (var i = size - 1; i >= 0; i--)
            {
                iterationsAmount++;

                var sum = 0.0;

                for (var j = 0; j < size; j++)
                {
                    iterationsAmount++;
                    
                    if (j != maxValuesCols[i])
                        sum += matrix[i, j] * results[j];
                }

                results[maxValuesCols[i]] = (subMatrix[i] - sum) / matrix[i, maxValuesCols[i]];
            }

            return (results, iterationsAmount);
        }
        
        public static double CalculateDeterminant(double[,] matrix, ref int iterationsAmount)
        {
            var size = matrix.GetLength(0);
            if (size == 1)
                return matrix[0, 0];

            double result = 0;

            for (var i = 0; i < size; i++)
            {
                iterationsAmount++;
                
                if (matrix[0, i] == 0)
                    continue;

                var minor = GetMinor(matrix, i, size, ref iterationsAmount);
                result += matrix[0, i] * Math.Pow(-1, i) * CalculateDeterminant(minor, ref iterationsAmount);
            }

            return result;
        }
        
        public static double[,] CopyMatrix(double[,] matrix, ref int iterationsAmount)
        {
            var size = matrix.GetLength(0);
            var matrixCopy = new double[size, size];

            for (var i = 0; i < size; i++)
            {
                iterationsAmount++;

                for (var j = 0; j < size; j++)
                {
                    iterationsAmount++;
                    matrixCopy[i, j] = matrix[i, j];
                }
            }

            return matrixCopy;
        }
        
        private static double[,] SubKramerMatrix(double[,] matrix, double[] subMatrix, int col, ref int iterationsAmount)
        {
            for (var i = 0; i < subMatrix.Length; i++)
            {
                iterationsAmount++;
                matrix[i, col] = subMatrix[i];
            }

            return matrix;
        }

        private static double[,] GetMinor(double[,] matrix, int col, int size, ref int iterationsAmount)
        {
            var minor = new double[size - 1, size - 1];

            for (int i = 0, p = 0; i < size; i++)
            {
                iterationsAmount++;
                
                if (i == 0)
                    continue;

                for (int j = 0, q = 0; j < size; j++)
                {
                    iterationsAmount++;

                    if (j != col)
                        minor[p, q++] = matrix[i, j];
                }

                p++;
            }

            return minor;
        }

        private static (int, int, double) FindMaxElement(double[,] matrix, int start, ref int iterationsAmount)
        {
            var size = matrix.GetLength(0);
            var max = Math.Abs(matrix[start, 0]);
            int row = start, col = 0;

            for (var i = start; i < size; i++)
            {
                iterationsAmount++;
                
                for (var j = 0; j < size; j++)
                {
                    iterationsAmount++;
                    
                    if (Math.Abs(matrix[i, j]) < Math.Abs(max))
                        continue;

                    max = matrix[i, j];
                    row = i;
                    col = j;
                }
            }

            return (row, col, max);
        }
    }
}
