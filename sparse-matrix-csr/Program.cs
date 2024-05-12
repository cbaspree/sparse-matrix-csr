namespace sparse_matrix_csr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[][] matrix1 =
            [
                [2, 13, 3, 4],
                [-9, 11, 6, 13],
                [3, -17, 15, 1],
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
