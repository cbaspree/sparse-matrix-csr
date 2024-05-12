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

        public SparseMatrix Add(SparseMatrix other)
        {
            if (_rowCount != other.RowCount)
            {
                throw new ArgumentException("Matrices must have the same number of rows");
            }

            if (_columnCount != other.ColumnCount)
            {
                throw new ArgumentException("Matrices must have the same number of columns");
            }

            SparseMatrix result = new SparseMatrix(other.RowCount, other.ColumnCount);

            for (int r = 0; r < _rowCount; ++r)
            {
                int rowStart = _rowPointers[r];
                int rowEnd = _rowPointers[r + 1];
                int otherRowStart = other.RowPointers[r];
                int otherRowEnd = other.RowPointers[r + 1];

                while (rowStart < rowEnd && otherRowStart < otherRowEnd)
                {
                    int column = _columnIndices[rowStart];
                    int otherColumn = other.ColumnIndices[otherRowStart];

                    if (column < otherColumn)
                    {
                        result.AddValue(_values[rowStart], column);
                        ++rowStart;
                    }
                    else if (column > otherColumn)
                    {
                        result.AddValue(other.Values[otherRowStart], otherColumn);
                        ++otherRowStart;
                    }
                    else
                    {
                        result.AddValue(
                            _values[rowStart] + other.Values[otherRowStart],
                            column);

                        ++rowStart;
                        ++otherRowStart;
                    }
                }

                if (rowStart >= rowEnd)
                {
                    for (int i = otherRowStart; i < otherRowEnd; ++i)
                    {
                        result.AddValue(other.Values[i], other.ColumnIndices[i]);
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

        public SparseMatrix Multiply(SparseMatrix other)
        {
            if (_columnCount != other.RowCount)
            {
                throw new ArgumentException("The number of columns of the first matrix must be equal to the number of rows of the second matrix.");
            }

            SparseMatrix transposedMatrix = other.Transpose();
            SparseMatrix result = new SparseMatrix(_rowCount, other.ColumnCount);
            int valuesPerRow = 0;

            for (int row = 0; row < _rowCount; ++row)
            {
                int rowStart = _rowPointers[row];
                int rowEnd = _rowPointers[row + 1];

                for (int column = 0; column < other.ColumnCount; ++column)
                {
                    int rowIndex = rowStart;
                    int columnStart = transposedMatrix.RowPointers[column];
                    int columnEnd = transposedMatrix.RowPointers[column + 1];
                    int sum = 0;

                    while (rowIndex < rowEnd && columnStart < columnEnd)
                    {
                        int columnIndex = _columnIndices[rowIndex];
                        int otherColumnIndex = transposedMatrix.ColumnIndices[columnStart];

                        if (columnIndex == otherColumnIndex)
                        {
                            sum += _values[rowIndex] * transposedMatrix.Values[columnStart];
                            ++rowIndex;
                            ++columnStart;
                        }
                        else if (columnIndex < otherColumnIndex)
                        {
                            ++rowIndex;
                        }
                        else
                        {
                            ++columnStart;
                        }
                    }

                    if (sum > 0)
                    {
                        result.AddValue(sum, column);
                        ++valuesPerRow;
                    }
                }

                result.RowPointers.Add(valuesPerRow);
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
