﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Clover
{
    internal static class CsvAnalyzer
    {
        /// <summary>
        /// Reads completely through a CSV stream to determine encoding, separator, field count and row count. 
        /// Uses fallbackEncoding if there is no BOM. Throws DecoderFallbackException if there are invalid characters in the stream.
        /// Returns the separator whose average field count is closest to its max field count.
        /// </summary>
        public static void Analyze(Stream stream, char[] separators, Encoding fallbackEncoding, int analyzeInitialCsvRows, out int fieldCount, out char autodetectSeparator, out Encoding autodetectEncoding, out int bomLength, out int rowCount)
        {
            int bufferSize = 1024;
            int probeSize = 16;
            byte[] buffer = new byte[bufferSize];
            int bytesRead = stream.Read(buffer, 0, probeSize);

            autodetectEncoding = GetEncodingFromBom(buffer, out bomLength);
            if (autodetectEncoding == null)
            {
                autodetectEncoding = fallbackEncoding;
            }

            if (separators == null || separators.Length == 0)
            {
                separators = new char[] { '\0' };
            }

            SeparatorInfo[] separatorInfos = new SeparatorInfo[separators.Length];
            for (int i = 0; i < separators.Length; i++)
            {
                separatorInfos[i] = new SeparatorInfo();
                separatorInfos[i].Buffer = new CsvParser(separators[i], autodetectEncoding);
            }

            AnalyzeCsvRows(stream, buffer, bytesRead, bomLength, analyzeInitialCsvRows, separators, separatorInfos);

            FlushSeparatorsBuffers(separators, separatorInfos);

            SeparatorInfo bestSeparatorInfo = separatorInfos[0];
            char bestSeparator = separators[0];
            double bestDistance = double.MaxValue;

            for (int i = 0; i < separators.Length; i++)
            {
                char separator = separators[i];
                SeparatorInfo separatorInfo = separatorInfos[i];

                // Row has one column if there are no separators, there must be at least one separator to count
                if (separatorInfo.RowCount == 0 || separatorInfo.MaxFieldCount <= 1)
                {
                    continue;
                }

                double average = separatorInfo.SumFieldCount / (double)separatorInfo.RowCount;
                double dist = separatorInfo.MaxFieldCount - average;

                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    bestSeparator = separator;
                    bestSeparatorInfo = separatorInfo;
                }
            }

            autodetectSeparator = bestSeparator;
            fieldCount = bestSeparatorInfo.MaxFieldCount;
            rowCount = analyzeInitialCsvRows == 0 ? bestSeparatorInfo.RowCount : -1;
        }

        private static void AnalyzeCsvRows(Stream inputStream, byte[] buffer, int initialBytesRead, int bomLength, int analyzeInitialCsvRows, char[] separators, SeparatorInfo[] separatorInfos)
        {
            ParseSeparatorsBuffer(buffer, bomLength, initialBytesRead - bomLength, separators, separatorInfos);

            if (IsMinNumberOfRowAnalyzed(analyzeInitialCsvRows, separatorInfos))
            {
                return;
            }

            while (inputStream.Position < inputStream.Length)
            {
                int bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                ParseSeparatorsBuffer(buffer, 0, bytesRead, separators, separatorInfos);
                if (IsMinNumberOfRowAnalyzed(analyzeInitialCsvRows, separatorInfos))
                {
                    return;
                }
            }
        }

        private static bool IsMinNumberOfRowAnalyzed(
            int analyzeInitialCsvRows,
            SeparatorInfo[] separatorInfos)
        {
            if (analyzeInitialCsvRows > 0)
            {
                foreach (SeparatorInfo separatorInfo in separatorInfos)
                {
                    if (separatorInfo.RowCount >= analyzeInitialCsvRows)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void ParseSeparatorsBuffer(byte[] bytes, int offset, int count, char[] separators, SeparatorInfo[] separatorInfos)
        {
            for (int i = 0; i < separators.Length; i++)
            {
                char separator = separators[i];
                SeparatorInfo separatorInfo = separatorInfos[i];

                separatorInfo.Buffer.ParseBuffer(bytes, offset, count, out List<List<string>> rows);

                foreach (List<string> row in rows)
                {
                    separatorInfo.MaxFieldCount = Math.Max(separatorInfo.MaxFieldCount, row.Count);
                    separatorInfo.SumFieldCount += row.Count;
                    separatorInfo.RowCount++;
                }
            }
        }

        private static void FlushSeparatorsBuffers(char[] separators, SeparatorInfo[] separatorInfos)
        {
            for (int i = 0; i < separators.Length; i++)
            {
                char separator = separators[i];
                SeparatorInfo separatorInfo = separatorInfos[i];

                separatorInfo.Buffer.Flush(out List<List<string>> rows);

                foreach (List<string> row in rows)
                {
                    separatorInfo.MaxFieldCount = Math.Max(separatorInfo.MaxFieldCount, row.Count);
                    separatorInfo.SumFieldCount += row.Count;
                    separatorInfo.RowCount++;
                }
            }
        }

        private static Encoding GetEncodingFromBom(byte[] bom, out int bomLength)
        {
            Encoding[] encodings = new Encoding[]
            {
                Encoding.Unicode, Encoding.BigEndianUnicode, Encoding.UTF8
            };

            foreach (Encoding encoding in encodings)
            {
                if (IsEncodingPreamble(bom, encoding, out int length))
                {
                    bomLength = length;
                    return encoding;
                }
            }

            bomLength = 0;
            return null;
        }

        private static bool IsEncodingPreamble(byte[] bom, Encoding encoding, out int bomLength)
        {
            bomLength = 0;
            byte[] preabmle = encoding.GetPreamble();
            if (preabmle.Length > bom.Length)
                return false;
            int i = 0;
            for (; i < preabmle.Length; i++)
            {
                if (preabmle[i] != bom[i])
                    return false;
            }

            bomLength = i;
            return true;
        }

        private class SeparatorInfo
        {
            public int MaxFieldCount { get; set; }

            public int SumFieldCount { get; set; }

            public int RowCount { get; set; }

            public CsvParser Buffer { get; set; }
        }
    }
}
