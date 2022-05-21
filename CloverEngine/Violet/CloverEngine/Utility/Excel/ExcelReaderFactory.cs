using System.IO;
using Clover;

namespace Clover
{
    /// <summary>
    /// The ExcelReader Factory
    /// </summary>
    public static class ExcelReaderFactory
    {
        const string DirectoryEntryWorkbook = "Workbook";
        const string DirectoryEntryBook = "Book";
        const string DirectoryEntryEncryptedPackage = "EncryptedPackage";
        const string DirectoryEntryEncryptionInfo = "EncryptionInfo";

        /// <summary>
        /// Creates an instance of <see cref="ExcelBinaryReader"/> or <see cref="ExcelOpenXmlReader"/>
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="configuration">The configuration object.</param>
        /// <returns>The excel data reader.</returns>
        public static IExcelDataReader CreateReader(Stream fileStream, ExcelReaderConfiguration configuration = null)
        {
            configuration ??= ExcelReaderConfiguration.Default();

            if (configuration.LeaveOpen)
            {
                fileStream = new LeaveOpenStream(fileStream);
            }

            byte[] probe = new byte[8];
            fileStream.Seek(0, SeekOrigin.Begin);
            fileStream.Read(probe, 0, probe.Length);
            fileStream.Seek(0, SeekOrigin.Begin);

            if (CompoundDocument.IsCompoundDocument(probe))
            {
                // Can be BIFF5-8 or password protected OpenXml
                CompoundDocument document = new CompoundDocument(fileStream);
                if (TryGetWorkbook(fileStream, document, out Stream stream))
                {
                    return new ExcelBinaryReader(stream, configuration.Password, configuration.FallbackEncoding);
                }

                if (TryGetEncryptedPackage(fileStream, document, configuration.Password, out stream))
                {
                    return new ExcelOpenXmlReader(stream);
                }

                throw new ExcelReaderException(Errors.ErrorStreamWorkbookNotFound);
            }

            if (XlsWorkbook.IsRawBiffStream(probe))
            {
                return new ExcelBinaryReader(fileStream, configuration.Password, configuration.FallbackEncoding);
            }

            if (probe[0] == 0x50 && probe[1] == 0x4B)
            {
                // zip files start with 'PK'
                return new ExcelOpenXmlReader(fileStream);
            }

            throw new HeaderException(Errors.ErrorHeaderSignature);
        }

        /// <summary>
        /// Creates an instance of <see cref="ExcelBinaryReader"/>
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="configuration">The configuration object.</param>
        /// <returns>The excel data reader.</returns>
        public static IExcelDataReader CreateBinaryReader(Stream fileStream,
            ExcelReaderConfiguration configuration = null)
        {
            if (configuration == null)
            {
                configuration = new ExcelReaderConfiguration();
            }

            if (configuration.LeaveOpen)
            {
                fileStream = new LeaveOpenStream(fileStream);
            }

            byte[] probe = new byte[8];
            fileStream.Seek(0, SeekOrigin.Begin);
            fileStream.Read(probe, 0, probe.Length);
            fileStream.Seek(0, SeekOrigin.Begin);

            if (CompoundDocument.IsCompoundDocument(probe))
            {
                CompoundDocument document = new CompoundDocument(fileStream);
                if (TryGetWorkbook(fileStream, document, out Stream stream))
                {
                    return new ExcelBinaryReader(stream, configuration.Password, configuration.FallbackEncoding);
                }
                else
                {
                    throw new ExcelReaderException(Errors.ErrorStreamWorkbookNotFound);
                }
            }
            else if (XlsWorkbook.IsRawBiffStream(probe))
            {
                return new ExcelBinaryReader(fileStream, configuration.Password, configuration.FallbackEncoding);
            }
            else
            {
                throw new HeaderException(Errors.ErrorHeaderSignature);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="ExcelOpenXmlReader"/>
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="configuration">The reader configuration -or- <see langword="null"/> to use the default configuration.</param>
        /// <returns>The excel data reader.</returns>
        public static IExcelDataReader CreateOpenXmlReader(Stream fileStream,
            ExcelReaderConfiguration configuration = null)
        {
            if (configuration == null)
            {
                configuration = new ExcelReaderConfiguration();
            }

            if (configuration.LeaveOpen)
            {
                fileStream = new LeaveOpenStream(fileStream);
            }

            byte[] probe = new byte[8];
            fileStream.Seek(0, SeekOrigin.Begin);
            fileStream.Read(probe, 0, probe.Length);
            fileStream.Seek(0, SeekOrigin.Begin);

            // Probe for password protected compound document or zip file
            if (CompoundDocument.IsCompoundDocument(probe))
            {
                CompoundDocument document = new CompoundDocument(fileStream);
                if (TryGetEncryptedPackage(fileStream, document, configuration.Password, out Stream stream))
                {
                    return new ExcelOpenXmlReader(stream);
                }

                throw new ExcelReaderException(Errors.ErrorCompoundNoOpenXml);
            }

            if (probe[0] == 0x50 && probe[1] == 0x4B)
            {
                // Zip files start with 'PK'
                return new ExcelOpenXmlReader(fileStream);
            }

            throw new HeaderException(Errors.ErrorHeaderSignature);
        }

        /// <summary>
        /// Creates an instance of ExcelCsvReader
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="configuration">The reader configuration -or- <see langword="null"/> to use the default configuration.</param>
        /// <returns>The excel data reader.</returns>
        public static IExcelDataReader CreateCsvReader(Stream fileStream, ExcelReaderConfiguration configuration = null)
        {
            if (configuration == null)
            {
                configuration = new ExcelReaderConfiguration();
            }

            if (configuration.LeaveOpen)
            {
                fileStream = new LeaveOpenStream(fileStream);
            }

            return new ExcelCsvReader(fileStream, configuration.FallbackEncoding, configuration.AutodetectSeparators,
                configuration.AnalyzeInitialCsvRows);
        }

        private static bool TryGetWorkbook(Stream fileStream, CompoundDocument document, out Stream stream)
        {
            CompoundDirectoryEntry workbookEntry = document.FindEntry(DirectoryEntryWorkbook, DirectoryEntryBook);
            if (workbookEntry != null)
            {
                if (workbookEntry.EntryType != STGTY.STGTY_STREAM)
                {
                    throw new ExcelReaderException(Errors.ErrorWorkbookIsNotStream);
                }

                stream = new CompoundStream(document, fileStream, workbookEntry.StreamFirstSector,
                    (int) workbookEntry.StreamSize, workbookEntry.IsEntryMiniStream, false);
                return true;
            }

            stream = null;
            return false;
        }

        private static bool TryGetEncryptedPackage(Stream fileStream, CompoundDocument document, string password,
            out Stream stream)
        {
            CompoundDirectoryEntry encryptedPackage = document.FindEntry(DirectoryEntryEncryptedPackage);
            CompoundDirectoryEntry encryptionInfo = document.FindEntry(DirectoryEntryEncryptionInfo);

            if (encryptedPackage == null || encryptionInfo == null)
            {
                stream = null;
                return false;
            }

            byte[] infoBytes = document.ReadStream(fileStream, encryptionInfo.StreamFirstSector,
                (int) encryptionInfo.StreamSize, encryptionInfo.IsEntryMiniStream);
            EncryptionInfo encryption = EncryptionInfo.Create(infoBytes);

            if (encryption.VerifyPassword("VelvetSweatshop"))
            {
                // Magic password used for write-protected workbooks
                password = "VelvetSweatshop";
            }
            else if (password == null || !encryption.VerifyPassword(password))
            {
                throw new InvalidPasswordException(Errors.ErrorInvalidPassword);
            }

            byte[] secretKey = encryption.GenerateSecretKey(password);
            CompoundStream packageStream = new CompoundStream(document, fileStream, encryptedPackage.StreamFirstSector,
                (int) encryptedPackage.StreamSize, encryptedPackage.IsEntryMiniStream, false);

            stream = encryption.CreateEncryptedPackageStream(packageStream, secretKey);
            return true;
        }
    }
}