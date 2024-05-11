using sparse_matrix_csr;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class SparseMatrixTests
    {
        [Test]
        public void TestConstructorFrom2DArray()
        {
            int[][] matrix =
            [
                [5, 0, 0, 0],
                [0, 8, 0, 0],
                [0, 0, 3, 0],
                [0, 6, 0, 0],
            ];

            List<int> expectedValues = new List<int>() { 5, 8, 3, 6 };
            List<int> expectedColumnIndices = new List<int>() { 0, 1, 2, 1 };
            List<int> expectedRowPointers = new List<int>() { 0, 1, 2, 3, 4 };

            SparseMatrix sparseMatrix = new SparseMatrix(matrix);

            Assert.IsTrue(expectedValues.SequenceEqual(sparseMatrix.Values));
            Assert.IsTrue(expectedColumnIndices.SequenceEqual(sparseMatrix.ColumnIndices));
            Assert.IsTrue(expectedRowPointers.SequenceEqual(sparseMatrix.RowPointers));
        }

        [Test]
        public void TestMatrixAdditionDifferentRowCount()
        {
            int[][] matrix1 =
            [
                [5, 0, 0, 0],
                [0, 8, 0, 0],
                [0, 0, 3, 0],
                [0, 6, 0, 0],
            ];

            int[][] matrix2 =
            [
                [10, 20, 0, 0],
                [0, 30, 0, 40],
                [0, 0, 50, 60],
            ];

            SparseMatrix sparseMatrix1 = new SparseMatrix(matrix1);
            SparseMatrix sparseMatrix2 = new SparseMatrix(matrix2);

            var exception = Assert.Throws<ArgumentException>(
                () => { sparseMatrix1.Add(sparseMatrix2); });

            Assert.That(exception.Message, Is.EqualTo("Matrices must have the same number of rows"));
        }

        [Test]
        public void TestMatrixAdditionDifferentColumnCount()
        {
            int[][] matrix1 =
            [
                [5, 0, 0, 0],
                [0, 8, 0, 0],
                [0, 0, 3, 0],
                [0, 6, 0, 0],
            ];

            int[][] matrix2 =
            [
                [10, 20, 0, 0, 0, 0],
                [0, 30, 0, 40, 0, 0],
                [0, 0, 50, 60, 70, 0],
                [0, 0, 0, 0, 0, 80],
            ];

            SparseMatrix sparseMatrix1 = new SparseMatrix(matrix1);
            SparseMatrix sparseMatrix2 = new SparseMatrix(matrix2);

            var exception = Assert.Throws<ArgumentException>(
                () => { sparseMatrix1.Add(sparseMatrix2); });

            Assert.That(exception.Message, Is.EqualTo("Matrices must have the same number of columns"));
        }

        [Test]
        public void TestMatrixAddition()
        {
            int[][] matrix1 =
            [
                [0, 0, 0, 0],
                [5, 8, 0, 0],
                [0, 0, 3, 0],
                [0, 6, 0, 0],
            ];

            int[][] matrix2 =
            [
                [1, 0, 5, 0],
                [2, 3, 0, 0],
                [4, 0, 0, 1],
                [0, 0, 2, 0],
            ];

            SparseMatrix sparseMatrix1 = new SparseMatrix(matrix1);
            SparseMatrix sparseMatrix2 = new SparseMatrix(matrix2);

            SparseMatrix result = sparseMatrix1.Add(sparseMatrix2);

            List<int> expectedValues = new List<int>() { 1, 5, 7, 11, 4, 3, 1, 6, 2 };
            List<int> expectedColumnIndices = new List<int>() { 0, 2, 0, 1, 0, 2, 3, 1, 2 };
            List<int> expectedRowPointers = new List<int>() { 0, 2, 4, 7, 9 };

            Assert.IsNotNull(result);
            Assert.IsTrue(expectedValues.SequenceEqual(result.Values));
            Assert.IsTrue(expectedColumnIndices.SequenceEqual(result.ColumnIndices));
            Assert.IsTrue(expectedRowPointers.SequenceEqual(result.RowPointers));
        }
    }
}