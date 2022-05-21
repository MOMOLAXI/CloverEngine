using System.Data;
using System.IO;

namespace Clover
{
    public static partial class CloverEngine
    {
        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static DataSet ReadExcel(string filePath, ExcelDataSetConfiguration configuration = null)
        {
            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    return reader.AsDataSet(configuration);
                }
            }
        }
    }
}