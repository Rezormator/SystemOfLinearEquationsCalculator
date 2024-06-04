using System;

namespace SystemOfLinearEquationsCalculator
{
    public static class Calculations
    {
        public static (double[], int) KramerMethod(Matrix matrix, double[] subMatrix, int size)
        {
            var iterationsAmount = 0;
            var results = new double[size];
            
            var determinant = matrix.CalculateDeterminant(ref iterationsAmount);

            for (var i = 0; i < size; i++)
            {
                iterationsAmount++;

                var matrixCopy = matrix.Clone(ref iterationsAmount);
                var subKramerMatrix = SubKramerMatrix(matrixCopy, subMatrix, i, ref iterationsAmount);
                results[i] = subKramerMatrix.CalculateDeterminant(ref iterationsAmount) / determinant;
            }
            
            return (results, iterationsAmount);
        }
        
        public static (double[], int) GaussMethodWithSingleCoefficients(Matrix matrix, double[] subMatrix, int size)
        {
            var iterationsAmount = 0;
            var results = new double[size];
            
            if (matrix[0, 0] == 0)
            {
                for (var i = 1; i < size; i++)
                {
                    iterationsAmount++;
                    
                    if (matrix[i, 0] == 0) continue;

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

                if (i == size - 1) continue;

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

        public static (double[], int) GaussMethodWithMainElement(Matrix matrix, double[] subMatrix, int size)
        {
            var iterationsAmount = 0;
            var results = new double[size];
            
            var maxValuesCols = new int[size];

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
                    
                    if (j != maxValuesCols[i]) sum += matrix[i, j] * results[j];
                }

                results[maxValuesCols[i]] = (subMatrix[i] - sum) / matrix[i, maxValuesCols[i]];
            }

            return (results, iterationsAmount);
        }
        
        private static Matrix SubKramerMatrix(Matrix matrix, double[] subMatrix, int col, ref int iterationsAmount)
        {
            for (var i = 0; i < matrix.Rows; i++)
            {
                iterationsAmount++;
                matrix[i, col] = subMatrix[i];
            }

            return matrix;
        }

        private static (int, int, double) FindMaxElement(Matrix matrix, int start, ref int iterationsAmount)
        {
            var size = matrix.Rows;
            var max = Math.Abs(matrix[start, 0]);
            int row = start, col = 0;

            for (var i = start; i < size; i++)
            {
                iterationsAmount++;
                
                for (var j = 0; j < size; j++)
                {
                    iterationsAmount++;
                    
                    if (Math.Abs(matrix[i, j]) < Math.Abs(max)) continue;

                    max = matrix[i, j];
                    row = i;
                    col = j;
                }
            }

            return (row, col, max);
        }
    }
}
