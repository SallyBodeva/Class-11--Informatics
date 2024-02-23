﻿using System.IO;

namespace P24_MatrixManipulation

{
    internal class Program
    {
        static void Main()
        {
            int[,] matrix = ReadMatrix(@"C:\Users\EliteBook\Desktop\test.txt");
            Console.WriteLine(IsSymmetric(matrix));
            Console.WriteLine(DiagonalDiff(matrix));
        }
        public static int[,] ReadMatrix(string pathName)
        {
            int rows = File.ReadAllLines(pathName).Length;
            int col = File.ReadAllLines(pathName)[0].Split(" ").Length;
            int[,] matrix = null;
            try
            {
                matrix = new int[rows, col];
                if (rows!=col)
                {
                    throw new Exception();
                }

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    string line = File.ReadAllLines(pathName)[i];
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        matrix[i, j] = int.Parse(line.Split(' ')[j]);
                    }
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Invalid data.");
            }
            return matrix;

        }
        public static bool IsSymmetric(int[,] matrix)
        {
            bool isSemmetric = true;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (i != j && matrix[i, j] != matrix[j,i])
                    {
                        isSemmetric = false;
                        return isSemmetric;
                    }
                }
            }
            return isSemmetric;
        }
        public static int SumFirstMainDiagonal(int[,] matrix) 
        {
            int firstDiag = 0;
            try
            {
                if (matrix.GetLength(0) != matrix.GetLength(1))
                {
                    throw new Exception();
                }
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (i == j)
                        {
                            firstDiag += matrix[i, j];
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invlid data.");
            }
            return firstDiag;

        }
        public static int SumSecMainDiagonal(int[,] matrix)
        {
            int sum = 0;
            try
            {
                if (matrix.GetLength(0) != matrix.GetLength(1))
                {
                    throw new Exception();
                }
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    sum += matrix[i, matrix.GetLength(0) - i - 1];
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Incorrect data.");
            }
          
            return sum;
        }

        public static int DiagonalDiff(int[,] matrix)
        {
            return Math.Abs((SumFirstMainDiagonal(matrix)-SumSecMainDiagonal(matrix)));
        }
    }
}
