using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Clover;

namespace Clover
{
    internal class CsvWorksheet : IWorksheet
    {
        public CsvWorksheet(Stream stream, Encoding fallbackEncoding, char[] autodetectSeparators, int analyzeInitialCsvRows)
        {
            Stream = stream;
            Stream.Seek(0, SeekOrigin.Begin);
            try
            {
                // Try as UTF-8 first, or use BOM if present
                CsvAnalyzer.Analyze(Stream, autodetectSeparators, Encoding.UTF8, analyzeInitialCsvRows, out int fieldCount, out char separator, out Encoding encoding, out int bomLength, out int rowCount);
                FieldCount = fieldCount;
                AnalyzedRowCount = rowCount;
                AnalyzedPartial = analyzeInitialCsvRows > 0;
                Encoding = encoding;
                Separator = separator;
                BomLength = bomLength;
            }
            catch (DecoderFallbackException)
            {
                // If cannot parse as UTF-8, try fallback encoding
                Stream.Seek(0, SeekOrigin.Begin);

                CsvAnalyzer.Analyze(Stream, autodetectSeparators, fallbackEncoding, analyzeInitialCsvRows, out int fieldCount, out char separator, out Encoding encoding, out int bomLength, out int rowCount);
                FieldCount = fieldCount;
                AnalyzedRowCount = rowCount;
                AnalyzedPartial = analyzeInitialCsvRows > 0;
                Encoding = encoding;
                Separator = separator;
                BomLength = bomLength;
            }
        }

        public string Name => string.Empty;

        public string CodeName => null;

        public string VisibleState => null;

        public HeaderFooter HeaderFooter => null;

        public CellRange[] MergeCells => null;

        public int FieldCount { get; }

        public int RowCount
        {
            get
            {
                if (AnalyzedPartial)
                {
                    throw new InvalidOperationException("Cannot use RowCount with AnalyzeInitialCsvRows > 0");
                }

                return AnalyzedRowCount;
            }
        }

        public Stream Stream { get; }

        public Encoding Encoding { get; }

        public char Separator { get; }

        public Column[] ColumnWidths => null;

        private int BomLength { get; set; }

        private bool AnalyzedPartial { get; }

        private int AnalyzedRowCount { get; }

        public IEnumerable<Row> ReadRows()
        {
            int bufferSize = 1024;
            byte[] buffer = new byte[bufferSize];
            int rowIndex = 0;
            CsvParser csv = new CsvParser(Separator, Encoding);
            int skipBomBytes = BomLength;

            Stream.Seek(0, SeekOrigin.Begin);
            while (Stream.Position < Stream.Length)
            {
                int bytesRead = Stream.Read(buffer, 0, bufferSize);
                csv.ParseBuffer(buffer, skipBomBytes, bytesRead - skipBomBytes, out List<List<string>> bufferRows);

                skipBomBytes = 0; // Only skip bom on first iteration

                foreach (Row row in GetReaderRows(rowIndex, bufferRows))
                {
                    yield return row;
                }

                rowIndex += bufferRows.Count;
            }

            csv.Flush(out List<List<string>> flushRows);
            foreach (Row row in GetReaderRows(rowIndex, flushRows))
            {
                yield return row;
            }
        }

        private IEnumerable<Row> GetReaderRows(int rowIndex, List<List<string>> rows)
        {
            foreach (List<string> row in rows)
            {
                List<Cell> cells = new List<Cell>(row.Count);
                for (int index = 0; index < row.Count; index++)
                {
                    object value = row[index];
                    cells.Add(new Cell(index, value, new ExtendedFormat(), null));
                }

                yield return new Row(rowIndex, 12.75 /* 255 twips */, cells);

                rowIndex++;
            }
        }
    }
}
