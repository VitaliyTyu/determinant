using System;
namespace determinant
{
    internal class Program
    {
        public static Random Rnd { get; set; } = new Random();
        public static void Main(string[] args)
        {
            //double[,] matrixD = new double[,] { { 5, 2, -1, 6 }, { 3, 4, -1, 5 }, { 0, 0, 0, 1 }, { -1, 2, -2, 0 } };
            double[,] matrixD = new double[,] { { 5, 2, -1 }, { 3, 4, -1 }, { 2, 2, 2 } };
            //double[,] matrixD = InitMatrixD();
            PrintMatrixD(matrixD);

            double[,] matrixDupd = GaussMatrix(matrixD);

            PrintMatrixD(matrixDupd);

            Console.WriteLine($"детерминант: {CountDeterminant(matrixDupd)}");
            Console.ReadLine();
        }

        static double[,] InitMatrixD()
        {
            Console.WriteLine("Введите размер матрицы:");
            int n = int.Parse(Console.ReadLine());
            var matrix = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = Rnd.Next(-10, 10);
                }
            }
            return matrix;
        }
        
        static void PrintMatrixD(double[,] matrix)
        {
            Console.WriteLine("Матрица:");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t\t");
                }
                Console.WriteLine();
            }
        }

        static int Determinant(int[,] matrix)
        {
                if (matrix.GetLength(0) == 1)
                    return matrix[0, 0];
                if (matrix.GetLength(0) == 2)
                    return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                //return matrix[0,0] * SmallerMatrix(matrix)
                int sign = 1, det=0;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    sign = 1;
                    if (i % 2 == 1)
                        sign = -1;
                    det += (sign * matrix[0, i] * Determinant(SmallerMatrix(matrix, i)));
                }

                return det;
        }
        
        static int[,] SmallerMatrix(int[,] matrix, int i)
        {
            int size = matrix.GetLength(1);
            int[,] smallMatrix = new int[size - 1,size - 1];//размер >1

            int a=0, b=0;
            for (int j = 0; j < size; j++)
            {
                if (j==0)
                    continue;
                for (int k = 0; k < size; k++)
                {
                    if (k == i)
                        continue;
                    smallMatrix[a, b++] = matrix[j, k];
                }

                a++;
                b = 0;
            }

            return smallMatrix;
        }
        
        static double[,] SmallerMatrixGauss(double[,] matrix)
        {
            // минор размерностью на 1 меньше
            int size = matrix.GetLength(1);
            double[,] smallMatrix = new double[size - 1,size - 1];//размер >1

            int a=0, b=0;
            for (int j = 1; j < size; j++)
            {
                for (int k = 1; k < size; k++)
                {
                    smallMatrix[a, b] = matrix[j, k];
                    b++;
                }
                a++;
                b = 0;
            }

            return smallMatrix;
        }

        static int sign = 1;//знак детерминанта
        
        static double[,] GaussMatrix(double[,] matrix)
        {
            if (matrix.GetLength(0) <= 1) 
                return matrix; // возврат матрица 1*1


            double mult;//множитель
            if (matrix[0, 0] == 0)
                matrix = SwapLines(matrix, 0);

            for (int i = 1; i < matrix.GetLength(1); i++)//перебор строк 
            {
                if (matrix[i, 0] != 0)
                {
                    mult = matrix[i, 0] / matrix[0, 0];
                    matrix[i, 0] = 0;
                    for (int j = 1; j < matrix.GetLength(0); j++)//перебор элементов в строке
                    {
                        matrix[i, j] -= matrix[0, j] * mult;
                    }
                }
                else
                {
                    matrix = SwapLines(matrix, i);
                    i--;
                }
                
            }

            double[,] newMatrix = GaussMatrix(SmallerMatrixGauss(matrix));

            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(0); j++)
                {
                    matrix[i + 1, j + 1] = newMatrix[i, j];
                }
            }

            return matrix;
        }

        static double[,] SwapLines(double[,] matrix, int a)
        {
            int notNull=-1;
            double temp=0;

            if (matrix.GetLength(0) < 2)
                return matrix;
            
            for (int i = 1; i < matrix.GetLength(0); i++) // поиск ненулевого элемента в столбце
                if (matrix[i, 0] != 0)
                {
                    notNull = i;
                    break;
                }

            if (notNull == -1) // ненулевых элементов нет
                return new double[,]{{0}};

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                temp = matrix[a, i];
                matrix[a, i] = matrix[notNull, i];
                matrix[notNull, i] = temp;
            }

            sign *= -1;//изменяем знак детерминанта при обмене строк
            return matrix;
        }

        public static double CountDeterminant(double[,] matrix)
        {
            //перемножение элементов главной диагонали
            double res = 1;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                res *= matrix[i, i];
            }

            return res * sign;
        }   
    }
}