namespace sparse_matrix_csr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[][] matrix1 =
            [
                [1, 0, 5, 0],
                [7, 11, 0, 0],
                [4, 0, 3, 1],
                [0, 6, 2, 0],
            ];

            int[][] matrix2 =
            [
                [10, 20, 0, 0, 0, 0],
                [0, 30, 0, 40, 0, 0],
                [0, 0, 50, 60, 70, 0],
                [0, 0, 0, 0, 0, 80],
            ];

            SparseMatrix sparseMatrix1 = new SparseMatrix(matrix1);
            sparseMatrix1.Print();

            SparseMatrix sparseMatrix2 = new SparseMatrix(matrix2);
            sparseMatrix2.Print();
        }
    }
}
