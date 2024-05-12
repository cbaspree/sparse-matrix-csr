namespace sparse_matrix_csr
{
    internal class Program
    {
        static void Main(string[] args)
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
            sparseMatrix1.Print();

            SparseMatrix sparseMatrix2 = new SparseMatrix(matrix2);
            sparseMatrix2.Print();

            sparseMatrix2.Transpose().Print();
        }
    }
}
