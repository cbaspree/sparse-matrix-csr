using System.Text;

namespace sparse_matrix_csr
{
    public class SparseMatrix
    {
        private int _rowCount;
        private int _columnCount;
        private int _nonZeroValues;

        private List<int> _values;
        private List<int> _columnIndices;
        private List<int> _rowPointers;

        public SparseMatrix()
        {
            _values = new List<int>();
            _columnIndices = new List<int>();
            _rowPointers = new List<int>();
        }

        public SparseMatrix(int rowCount, int columnCount)
        {
            _values = new List<int>();
            _columnIndices = new List<int>();
            _rowPointers = new List<int>();

            _rowCount = rowCount;
            _columnCount = columnCount;
            _rowPointers.Add(0);
        }

        public SparseMatrix(int[][] matrix)
        {
            _values = new List<int>();
            _columnIndices = new List<int>();
            _rowPointers = new List<int>();

            _rowCount = matrix.Length;
            _columnCount = matrix[0].Length;
            _rowPointers.Add(0);

            for (int r = 0; r < _rowCount; ++r)
            {
                for (int c = 0; c < _columnCount; ++c)
                {
                    if (matrix[r][c] != 0)
                    {
                        _values.Add(matrix[r][c]);
                        _columnIndices.Add(c);
                        _nonZeroValues++;
                    }
                }
                _rowPointers.Add(NonZeroValues);
            }
        }

        public int RowCount { get => _rowCount; }
        public int ColumnCount { get => _columnCount; }
        public int NonZeroValues { get => _nonZeroValues; }
        public List<int> Values { get => _values; }
        public List<int> ColumnIndices { get => _columnIndices; }
        public List<int> RowPointers { get => _rowPointers; }

        public SparseMatrix Add(SparseMatrix matrix)
        {
            if (_rowCount != matrix.RowCount)
            {
                throw new ArgumentException("Matrices must have the same number of rows");
            }

            if (_columnCount != matrix.ColumnCount)
            {
                throw new ArgumentException("Matrices must have the same number of columns");
            }

            SparseMatrix result = new SparseMatrix(matrix.RowCount, matrix.ColumnCount);

            for (int r = 0; r < _rowCount; ++r)
            {
                int rowStart = _rowPointers[r];
                int rowEnd = _rowPointers[r + 1];
                int otherRowStart = matrix.RowPointers[r];
                int otherRowEnd = matrix.RowPointers[r + 1];

                while (rowStart < rowEnd && otherRowStart < otherRowEnd)
                {
                    int column = _columnIndices[rowStart];
                    int otherColumn = matrix.ColumnIndices[otherRowStart];

                    if (column < otherColumn)
                    {
                        result.AddValue(_values[rowStart], column);
                        ++rowStart;
                    }
                    else if (column > otherColumn)
                    {
                        result.AddValue(matrix.Values[otherRowStart], otherColumn);
                        ++otherRowStart;
                    }
                    else
                    {
                        result.AddValue(
                            _values[rowStart] + matrix.Values[otherRowStart],
                            column);

                        ++rowStart;
                        ++otherRowStart;
                    }
                }

                if (rowStart >= rowEnd)
                {
                    for (int i = otherRowStart; i < otherRowEnd; ++i)
                    {
                        result.AddValue(matrix.Values[i], matrix.ColumnIndices[i]);
                    }
                }
                else
                {
                    for (int i = rowStart; i < rowEnd; ++i)
                    {
                        result.AddValue(_values[i], _columnIndices[i]);
                    }
                }

                result.RowPointers.Add(result.NonZeroValues);
            }

            return result;
        }

        public SparseMatrix Transpose()
        {
            SparseMatrix result = new SparseMatrix(_rowCount, _columnCount);

            // Add "empty" elements to values
            for (int i = 0; i < _nonZeroValues; ++i)
            {
                result.Values.Add(0);
                result.ColumnIndices.Add(0);
            }

            // Count non zero values per column
            int[] countPerColumn = new int[_columnCount];
            for (int i = 0; i < _nonZeroValues; ++i)
            {
                countPerColumn[_columnIndices[i]]++;
            }

            // Fill new row pointers
            for (int i = 0; i < _columnCount; ++i)
            {
                result.RowPointers.Add(result.RowPointers[i] + countPerColumn[i]);
            }

            for (int r = 0; r < _rowCount; ++r)
            {
                int rowStart = _rowPointers[r];
                int rowEnd = _rowPointers[r + 1];

                for (int c = rowStart; c < rowEnd; ++c)
                {
                    int column = _columnIndices[c];
                    int valueIndex = result.RowPointers[column + 1] - countPerColumn[column];
                    result.Values[valueIndex] = _values[c];
                    result.ColumnIndices[valueIndex] = r;
                    countPerColumn[column]--;
                }
            }

            return result;
        }

        public void Print()
        {
            Console.WriteLine(FormatForPrinting("Values", _values));
            Console.WriteLine(FormatForPrinting("Column Indices", _columnIndices));
            Console.WriteLine(FormatForPrinting("Row Pointers", _rowPointers));
            Console.WriteLine();
        }

        public void AddValue(int value, int columnIndex)
        {
            _values.Add(value);
            _columnIndices.Add(columnIndex);
            ++_nonZeroValues;
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
