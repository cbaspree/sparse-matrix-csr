using System;
using System.Collections.Generic;
using System.Text;

namespace sparse_matrix_csr
{
    internal class SparseMatrix
    {
        private int _rowCount;
        private int _colCount;
        private int _nonZeroValues;

        private List<int> _values;
        private List<int> _columnIndices;
        private List<int> _rowPointers;

        public SparseMatrix(int[][] matrix)
        {
            // TODO: validate

            _values = new List<int>();
            _columnIndices = new List<int>();
            _rowPointers = new List<int>();

            _rowCount = matrix.Length;
            _colCount = matrix[0].Length;
            _rowPointers.Add(0);

            for (int r = 0; r < _rowCount; ++r)
            {
                for (int c = 0; c < _colCount; ++c)
                {
                    if (matrix[r][c] != 0)
                    {
                        _values.Add(matrix[r][c]);
                        _columnIndices.Add(c);
                        _nonZeroValues++;
                    }
                }
                _rowPointers.Add(_nonZeroValues);
            }
        }

        public List<int> Values { get => _values; }

        public void Print()
        {
            Console.WriteLine(FormatForPrinting("Values", _values));
            Console.WriteLine(FormatForPrinting("Column Indices", _columnIndices));
            Console.WriteLine(FormatForPrinting("Row Pointers", _rowPointers));
            Console.WriteLine();
        }

        private string FormatForPrinting(string title, List<int> values)
        {
            StringBuilder formattedValues = new StringBuilder($"{title}: [");
            for (int i = 0; i < values.Count; ++i)
            {
                formattedValues.Append(values[i]);
                if (i < values.Count - 1)
                {
                    formattedValues.Append(", ");
                }
            }
            formattedValues.Append("]");
            return formattedValues.ToString();
        }
    }
}
