using System.IO;
using Clover;

namespace Clover
{
    internal class ExcelOpenXmlReader : ExcelDataReader<XlsxWorkbook, XlsxWorksheet>
    {
        public ExcelOpenXmlReader(Stream stream)
        {
            Document = new ZipWorker(stream);
            Workbook = new XlsxWorkbook(Document);

            // By default, the data reader is positioned on the first result.
            Reset();
        }

        private ZipWorker Document { get; set; }

        public override void Close()
        {
            base.Close();

            Document?.Dispose();
            Workbook = null;
            Document = null;
        }
    }
}
