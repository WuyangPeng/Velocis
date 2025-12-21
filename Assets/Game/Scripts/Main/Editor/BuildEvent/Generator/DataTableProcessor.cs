using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game.Scripts.Main.Runtime.RuntimeException;
using GameFramework;
using UnityEngine;

namespace Game.Scripts.Main.Editor.BuildEvent.Generator
{
    public sealed partial class DataTableProcessor
    {
        private const string CommentLineSeparator = "#";
        private static readonly char[] DataSplitSeparators = { '\t' };
        private static readonly char[] DataTrimSeparators = { '\"' };
        private readonly string[] _commentRow;

        private readonly DataProcessor[] _dataProcessor;
        private readonly string[] _defaultValueRow;

        private readonly string[] _nameRow;
        private readonly string[][] _rawValues;
        private readonly string[] _strings;
        private DataTableCodeGenerator _codeGenerator;

        private string _codeTemplate;

        public DataTableProcessor(string dataTableFileName,
            Encoding encoding,
            int nameRow,
            int typeRow,
            int? defaultValueRow,
            int? commentRow,
            int contentStartRow,
            int idColumn)
        {
            ValidateFileName(dataTableFileName);
            _rawValues = ParseRawValues(dataTableFileName, encoding);

            ValidateRowIndices(nameRow, typeRow, defaultValueRow, commentRow, contentStartRow, idColumn);

            _nameRow = _rawValues[nameRow];
            _defaultValueRow = defaultValueRow.HasValue ? _rawValues[defaultValueRow.Value] : null;
            _commentRow = commentRow.HasValue ? _rawValues[commentRow.Value] : null;
            ContentStartRow = contentStartRow;
            IdColumn = idColumn;

            _dataProcessor = CreateDataProcessors(typeRow);
            _strings = CollectStrings(contentStartRow);

            _codeTemplate = null;
            _codeGenerator = null;
        }

        private int RawRowCount => _rawValues.Length;

        public int RawColumnCount => _rawValues.Length > 0 ? _rawValues[0].Length : 0;

        private int StringCount => _strings.Length;

        private int ContentStartRow { get; }

        public int IdColumn { get; }

        private static void ValidateFileName(string dataTableFileName)
        {
            if (string.IsNullOrEmpty(dataTableFileName))
            {
                throw new GameException("Data table file name is invalid.");
            }

            if (!dataTableFileName.EndsWith(".txt", StringComparison.Ordinal))
            {
                throw new GameException(Utility.Text.Format("Data table file '{0}' is not a txt.", dataTableFileName));
            }

            if (!File.Exists(dataTableFileName))
            {
                throw new GameException(Utility.Text.Format("Data table file '{0}' is not exist.", dataTableFileName));
            }
        }

        private static string[][] ParseRawValues(string dataTableFileName, Encoding encoding)
        {
            var lines = File.ReadAllLines(dataTableFileName, encoding);
            var rawValues = new List<string[]>();
            var rawColumnCount = 0;
            for (var index = 0; index < lines.Length; ++index)
            {
                var rawValue = ParseRawValues(dataTableFileName, lines, index, rawColumnCount);
                if (index == 0)
                {
                    rawColumnCount = rawValue.Length;
                }

                rawValues.Add(rawValue);
            }

            return rawValues.ToArray();
        }

        private static string[] ParseRawValues(string dataTableFileName, string[] lines, int index, int rawColumnCount)
        {
            var rawValue = lines[index].Split(DataSplitSeparators);
            for (var j = 0; j < rawValue.Length; ++j)
            {
                rawValue[j] = rawValue[j].Trim(DataTrimSeparators);
            }

            if (index != 0 && rawValue.Length != rawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Data table file '{0}', raw Column is '{2}', but line '{1}' column is '{3}'.",
                    dataTableFileName,
                    index,
                    rawColumnCount,
                    rawValue.Length));
            }

            return rawValue;
        }

        private void ValidateRowIndices(int nameRow, int typeRow, int? defaultValueRow, int? commentRow, int contentStartRow, int idColumn)
        {
            if (nameRow < 0)
            {
                throw new GameException(Utility.Text.Format("Name row '{0}' is invalid.", nameRow));
            }

            if (typeRow < 0)
            {
                throw new GameException(Utility.Text.Format("Type row '{0}' is invalid.", typeRow));
            }

            if (contentStartRow < 0)
            {
                throw new GameException(Utility.Text.Format("Content start row '{0}' is invalid.", contentStartRow));
            }

            if (idColumn < 0)
            {
                throw new GameException(Utility.Text.Format("Id column '{0}' is invalid.", idColumn));
            }

            if (nameRow >= RawRowCount)
            {
                throw new GameException(Utility.Text.Format("Name row '{0}' >= raw row count '{1}' is not allow.", nameRow, RawRowCount));
            }

            if (typeRow >= RawRowCount)
            {
                throw new GameException(Utility.Text.Format("Type row '{0}' >= raw row count '{1}' is not allow.", typeRow, RawRowCount));
            }

            if (defaultValueRow >= RawRowCount)
            {
                throw new GameException(Utility.Text.Format("Default value row '{0}' >= raw row count '{1}' is not allow.", defaultValueRow.Value, RawRowCount));
            }

            if (commentRow >= RawRowCount)
            {
                throw new GameException(Utility.Text.Format("Comment row '{0}' >= raw row count '{1}' is not allow.", commentRow.Value, RawRowCount));
            }

            if (contentStartRow > RawRowCount)
            {
                throw new GameException(Utility.Text.Format("Content start row '{0}' > raw row count '{1}' is not allow.", contentStartRow, RawRowCount));
            }

            if (idColumn >= RawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Id column '{0}' >= raw column count '{1}' is not allow.", idColumn, RawColumnCount));
            }
        }

        private DataProcessor[] CreateDataProcessors(int typeRow)
        {
            var rawValue = _rawValues[typeRow];
            var dataProcessors = new DataProcessor[RawColumnCount];
            for (var index = 0; index < RawColumnCount; ++index)
            {
                if (index == IdColumn)
                {
                    dataProcessors[index] = DataProcessorUtility.GetDataProcessor("id");
                }
                else
                {
                    dataProcessors[index] = DataProcessorUtility.GetDataProcessor(rawValue[index]);
                }
            }

            return dataProcessors;
        }

        private string[] CollectStrings(int contentStartRow)
        {
            var strings = new Dictionary<string, int>(StringComparer.Ordinal);
            for (var index = contentStartRow; index < RawRowCount; ++index)
            {
                if (IsCommentRow(index))
                {
                    continue;
                }

                CollectStrings(strings, index);
            }

            return strings
                .OrderBy(value => value.Key)
                .ThenByDescending(value => value.Value)
                .Select(value => value.Key)
                .ToArray();
        }

        private void CollectStrings(Dictionary<string, int> strings, int index)
        {
            for (var column = 0; column < RawColumnCount; ++column)
            {
                if (_dataProcessor[column].LanguageKeyword != "string")
                {
                    continue;
                }

                var str = _rawValues[index][column];
                if (!strings.TryAdd(str, 1))
                {
                    ++strings[str];
                }
            }
        }

        public bool IsIdColumn(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _dataProcessor[rawColumn].IsId;
        }

        private bool IsCommentRow(int rawRow)
        {
            if (rawRow < 0 || rawRow >= RawRowCount)
            {
                throw new GameException(Utility.Text.Format("Raw row '{0}' is out of range.", rawRow));
            }

            return GetValue(rawRow, 0).StartsWith(CommentLineSeparator, StringComparison.Ordinal);
        }

        public bool IsCommentColumn(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return string.IsNullOrEmpty(GetName(rawColumn)) || _dataProcessor[rawColumn].IsComment;
        }

        public string GetName(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return IsIdColumn(rawColumn) ? "Id" : _nameRow[rawColumn];
        }

        public bool IsSystem(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _dataProcessor[rawColumn].IsSystem;
        }

        public Type GetType(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _dataProcessor[rawColumn].Type;
        }

        public string GetLanguageKeyword(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _dataProcessor[rawColumn].LanguageKeyword;
        }

        private string GetDefaultValue(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _defaultValueRow?[rawColumn];
        }

        public string GetComment(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _commentRow?[rawColumn];
        }

        public string GetValue(int rawRow, int rawColumn)
        {
            if (rawRow < 0 || rawRow >= RawRowCount)
            {
                throw new GameException(Utility.Text.Format("Raw row '{0}' is out of range.", rawRow));
            }

            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new GameException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return _rawValues[rawRow][rawColumn];
        }

        public string GetString(int index)
        {
            if (index < 0 || index >= StringCount)
            {
                throw new GameException(Utility.Text.Format("String index '{0}' is out of range.", index));
            }

            return _strings[index];
        }

        public int GetStringIndex(string str)
        {
            for (var i = 0; i < StringCount; i++)
            {
                if (_strings[i] == str)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool GenerateDataFile(string outputFileName)
        {
            if (string.IsNullOrEmpty(outputFileName))
            {
                throw new GameException("Output file name is invalid.");
            }

            try
            {
                DoGenerateDataFile(outputFileName);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Parse data table '{0}' failure, exception is '{1}'.", outputFileName, exception));
                return false;
            }
        }

        private void DoGenerateDataFile(string outputFileName)
        {
            using var fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
            using var binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8);
            for (var rawRow = ContentStartRow; rawRow < RawRowCount; ++rawRow)
            {
                if (IsCommentRow(rawRow))
                {
                    continue;
                }

                var bytes = GetRowBytes(outputFileName, rawRow);
                if (bytes == null)
                {
                    throw new GameException(Utility.Text.Format("Get row bytes failure. OutputFileName='{0}' RawRow='{1}'", outputFileName, rawRow));
                }

                binaryWriter.Write7BitEncodedInt32(bytes.Length);
                binaryWriter.Write(bytes);
            }

            Debug.Log(Utility.Text.Format("Parse data table '{0}' success.", outputFileName));
        }

        public bool SetCodeTemplate(string codeTemplateFileName, Encoding encoding)
        {
            try
            {
                DoSetCodeTemplate(codeTemplateFileName, encoding);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Set code template '{0}' failure, exception is '{1}'.", codeTemplateFileName, exception));
                return false;
            }
        }

        private void DoSetCodeTemplate(string codeTemplateFileName, Encoding encoding)
        {
            _codeTemplate = File.ReadAllText(codeTemplateFileName, encoding);
            Debug.Log(Utility.Text.Format("Set code template '{0}' success.", codeTemplateFileName));
        }

        public void SetCodeGenerator(DataTableCodeGenerator codeGenerator)
        {
            _codeGenerator = codeGenerator;
        }

        public bool GenerateCodeFile(string outputFileName, Encoding encoding, object userData = null)
        {
            if (string.IsNullOrEmpty(_codeTemplate))
            {
                throw new GameException("You must set code template first.");
            }

            if (string.IsNullOrEmpty(outputFileName))
            {
                throw new GameException("Output file name is invalid.");
            }

            try
            {
                DoGenerateCodeFile(outputFileName, encoding, userData);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Generate code file '{0}' failure, exception is '{1}'.", outputFileName, exception));
                return false;
            }
        }

        private void DoGenerateCodeFile(string outputFileName, Encoding encoding, object userData)
        {
            var stringBuilder = new StringBuilder(_codeTemplate);
            _codeGenerator?.Invoke(this, stringBuilder, userData);

            using var fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
            using var stream = new StreamWriter(fileStream, encoding);
            stream.Write(stringBuilder.ToString());

            Debug.Log(Utility.Text.Format("Generate code file '{0}' success.", outputFileName));
        }

        private byte[] GetRowBytes(string outputFileName, int rawRow)
        {
            using var memoryStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8);
            for (var rawColumn = 0; rawColumn < RawColumnCount; ++rawColumn)
            {
                if (!GetRowBytes(outputFileName, rawRow, rawColumn, binaryWriter))
                {
                    return null;
                }
            }

            return memoryStream.ToArray();
        }

        private bool GetRowBytes(string outputFileName, int rawRow, int rawColumn, BinaryWriter binaryWriter)
        {
            if (IsCommentColumn(rawColumn))
            {
                return true;
            }

            try
            {
                _dataProcessor[rawColumn].WriteToStream(this, binaryWriter, GetValue(rawRow, rawColumn));
            }
            catch
            {
                if (!HandleParseRowError(outputFileName, rawRow, rawColumn, binaryWriter))
                {
                    return false;
                }
            }

            return true;
        }

        private bool HandleParseRowError(string outputFileName, int rawRow, int rawColumn, BinaryWriter binaryWriter)
        {
            if (_dataProcessor[rawColumn].IsId || string.IsNullOrEmpty(GetDefaultValue(rawColumn)))
            {
                Debug.LogError(Utility.Text.Format("Parse raw value failure. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'",
                    outputFileName,
                    rawRow,
                    rawColumn,
                    GetName(rawColumn),
                    GetLanguageKeyword(rawColumn),
                    GetValue(rawRow, rawColumn)));
                
                return false;
            }

            Debug.LogWarning(Utility.Text.Format("Parse raw value failure, will try default value. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'",
                outputFileName,
                rawRow,
                rawColumn,
                GetName(rawColumn),
                GetLanguageKeyword(rawColumn),
                GetValue(rawRow, rawColumn)));

            try
            {
                _dataProcessor[rawColumn].WriteToStream(this, binaryWriter, GetDefaultValue(rawColumn));
                return true;
            }
            catch
            {
                Debug.LogError(Utility.Text.Format("Parse default value failure. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'",
                    outputFileName,
                    rawRow,
                    rawColumn,
                    GetName(rawColumn),
                    GetLanguageKeyword(rawColumn),
                    GetComment(rawColumn)));
                return false;
            }
        }
    }
}