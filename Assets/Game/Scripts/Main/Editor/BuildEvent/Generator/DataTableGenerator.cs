using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GameFramework;
using UnityEngine;

namespace Game.Scripts.Main.Editor.BuildEvent.Generator
{
    /// <summary>
    ///     数据表生成器。
    ///     <para>这是一个编辑器工具类，用于根据文本格式的数据表（.txt）自动生成二进制数据文件（.bytes）和对应的C#数据结构代码（DR*.cs）。</para>
    ///     <para>主要流程：</para>
    ///     <para>1. 读取位于 Assets/Game/DataTables 的 .txt 文件。</para>
    ///     <para>2. 将其转换为高效的二进制 .bytes 文件，供游戏运行时读取。</para>
    ///     <para>3. 根据数据表结构和模板（DataTableCodeTemplate.txt），生成强类型的C#类，方便在代码中安全、便捷地访问数据。</para>
    /// </summary>
    public static class DataTableGenerator
    {
        private const string DataTablePath = "Assets/Game/DataTables";
        private const string CSharpCodePath = "Assets/Game/Scripts/Main/Runtime/DataTable";
        private const string CSharpCodeTemplateFileName = "Assets/Game/Configs/DataTableCodeTemplate.txt";
        private static readonly Regex EndWithNumberRegex = new(@"\d+$");
        private static readonly Regex NameRegex = new("^[A-Z][A-Za-z0-9_]*$");

        /// <summary>
        ///     创建并配置一个数据表处理器。
        /// </summary>
        /// <param name="dataTableName">数据表名称（不含扩展名）。</param>
        /// <returns>配置好的数据表处理器实例。</returns>
        public static DataTableProcessor CreateDataTableProcessor(string dataTableName)
        {
            return new DataTableProcessor(Utility.Path.GetRegularPath(Path.Combine(DataTablePath, dataTableName + ".txt")),
                Encoding.UTF8,
                1,
                2,
                null,
                3,
                4,
                1);
        }

        /// <summary>
        ///     检查原始数据表的有效性，主要是校验字段命名是否规范。
        /// </summary>
        /// <param name="dataTableProcessor">已创建的数据表处理器。</param>
        /// <param name="dataTableName">要检查的数据表名称。</param>
        /// <returns>如果数据有效，则返回 true；否则返回 false。</returns>
        public static bool CheckRawData(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            for (var index = 0; index < dataTableProcessor.RawColumnCount; ++index)
            {
                var name = dataTableProcessor.GetName(index);
                if (!CheckRawData(dataTableName, name))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CheckRawData(string dataTableName, string name)
        {
            if (string.IsNullOrEmpty(name) || name == "#")
            {
                return true;
            }

            if (NameRegex.IsMatch(name))
            {
                return true;
            }

            Debug.LogWarning(Utility.Text.Format("Check raw data failure. DataTableName='{0}' Name='{1}'", dataTableName, name));
            return false;
        }

        /// <summary>
        ///     根据数据表处理器，生成二进制数据文件（.bytes）。
        /// </summary>
        /// <param name="dataTableProcessor">已创建的数据表处理器。</param>
        /// <param name="dataTableName">要生成的数据表名称。</param>
        public static void GenerateDataFile(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            var binaryDataFileName = Utility.Path.GetRegularPath(Path.Combine(DataTablePath, dataTableName + ".bytes"));
            if (!dataTableProcessor.GenerateDataFile(binaryDataFileName) && File.Exists(binaryDataFileName))
            {
                File.Delete(binaryDataFileName);
            }
        }

        /// <summary>
        ///     根据数据表处理器，生成对应的C#代码文件（DR*.cs）。
        /// </summary>
        /// <param name="dataTableProcessor">已创建的数据表处理器。</param>
        /// <param name="dataTableName">要生成的数据表名称。</param>
        public static void GenerateCodeFile(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            dataTableProcessor.SetCodeTemplate(CSharpCodeTemplateFileName, Encoding.UTF8);
            dataTableProcessor.SetCodeGenerator(DataTableCodeGenerator);

            var csharpCodeFileName = Utility.Path.GetRegularPath(Path.Combine(CSharpCodePath, "DR" + dataTableName + ".cs"));
            if (!dataTableProcessor.GenerateCodeFile(csharpCodeFileName, Encoding.UTF8, dataTableName) &&
                File.Exists(csharpCodeFileName))
            {
                File.Delete(csharpCodeFileName);
            }
        }

        /// <summary>
        ///     数据表代码生成的具体实现回调方法。
        ///     <para>此方法会替换代码模板文件中的特定占位符（如 __DATA_TABLE_CLASS_NAME__）为实际生成的内容。</para>
        /// </summary>
        /// <param name="dataTableProcessor">数据表处理器。</param>
        /// <param name="codeContent">用于构建代码内容的 StringBuilder 对象。</param>
        /// <param name="userData">用户自定义数据，此处为数据表名称。</param>
        private static void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData)
        {
            var dataTableName = (string)userData;

            codeContent.Replace("__DATA_TABLE_CREATE_TIME__", DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            codeContent.Replace("__DATA_TABLE_NAME_SPACE__", "Game.Scripts.Main.Runtime.DataTable");
            codeContent.Replace("__DATA_TABLE_CLASS_NAME__", "DR" + dataTableName);
            codeContent.Replace("__DATA_TABLE_COMMENT__", dataTableProcessor.GetValue(0, 1) + "。");
            codeContent.Replace("__DATA_TABLE_ID_COMMENT__", "获取" + dataTableProcessor.GetComment(dataTableProcessor.IdColumn) + "。");
            codeContent.Replace("__DATA_TABLE_PROPERTIES__", GenerateDataTableProperties(dataTableProcessor));
            codeContent.Replace("__DATA_TABLE_PARSER__", GenerateDataTableParser(dataTableProcessor));
            codeContent.Replace("__DATA_TABLE_PROPERTY_ARRAY__", GenerateDataTablePropertyArray(dataTableProcessor));
        }

        /// <summary>
        ///     为数据表生成所有公开属性的C#代码。
        /// </summary>
        /// <param name="dataTableProcessor">数据表处理器。</param>
        /// <returns>生成的属性C#代码字符串。</returns>
        private static string GenerateDataTableProperties(DataTableProcessor dataTableProcessor)
        {
            var stringBuilder = new StringBuilder();
            var firstProperty = true;
            for (var index = 0; index < dataTableProcessor.RawColumnCount; index++)
            {
                if (GenerateDataTableProperties(dataTableProcessor, index, firstProperty, stringBuilder))
                {
                    firstProperty = false;
                }
            }

            return stringBuilder.ToString();
        }

        private static bool GenerateDataTableProperties(DataTableProcessor dataTableProcessor, int index, bool firstProperty, StringBuilder stringBuilder)
        {
            if (dataTableProcessor.IsCommentColumn(index))
            {
                // 注释列
                return false;
            }

            if (dataTableProcessor.IsIdColumn(index))
            {
                // 编号列
                return false;
            }

            if (!firstProperty)
            {
                stringBuilder.AppendLine().AppendLine();
            }

            stringBuilder
                .AppendLine("        /// <summary>")
                .AppendFormat("        /// 获取{0}。", dataTableProcessor.GetComment(index)).AppendLine()
                .AppendLine("        /// </summary>")
                .AppendFormat("        public {0} {1}", dataTableProcessor.GetLanguageKeyword(index), dataTableProcessor.GetName(index)).AppendLine()
                .AppendLine("        {")
                .AppendLine("            get;")
                .AppendLine("            private set;")
                .Append("        }");

            return true;
        }

        /// <summary>
        ///     生成数据行解析相关的C#代码，包括从字符串（txt）和二进制（bytes）两种数据源的解析逻辑。
        /// </summary>
        /// <param name="dataTableProcessor">数据表处理器。</param>
        /// <returns>生成的解析逻辑C#代码字符串。</returns>
        private static string GenerateDataTableParser(DataTableProcessor dataTableProcessor)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(GenerateStringDataRowParser(dataTableProcessor));
            stringBuilder.AppendLine().AppendLine();
            stringBuilder.Append(GenerateBinaryDataRowParser(dataTableProcessor));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 生成从字符串解析数据行的C#代码。
        /// </summary>
        /// <param name="dataTableProcessor">数据表处理器。</param>
        /// <returns>生成的解析逻辑C#代码字符串。</returns>
        private static string GenerateStringDataRowParser(DataTableProcessor dataTableProcessor)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder
                .AppendLine("        public override bool ParseDataRow(string dataRowString, object userData)")
                .AppendLine("        {")
                .AppendLine("            var columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);")
                .AppendLine("            for (var i = 0; i < columnStrings.Length; i++)")
                .AppendLine("            {")
                .AppendLine("                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);")
                .AppendLine("            }")
                .AppendLine()
                .AppendLine("            var index = 0;");

            for (var i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    // 注释列
                    stringBuilder.AppendLine("            index++;");
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    // 编号列
                    stringBuilder.AppendLine("            m_Id = int.Parse(columnStrings[index++]);");
                    continue;
                }

                if (dataTableProcessor.IsSystem(i))
                {
                    var languageKeyword = dataTableProcessor.GetLanguageKeyword(i);
                    if (languageKeyword == "string")
                    {
                        stringBuilder.AppendFormat("            {0} = columnStrings[index++];", dataTableProcessor.GetName(i)).AppendLine();
                    }
                    else
                    {
                        stringBuilder.AppendFormat("            {0} = {1}.Parse(columnStrings[index++]);", dataTableProcessor.GetName(i), languageKeyword).AppendLine();
                    }
                }
                else
                {
                    stringBuilder.AppendFormat("            {0} = DataTableExtension.Parse{1}(columnStrings[index++]);", dataTableProcessor.GetName(i), dataTableProcessor.GetType(i).Name).AppendLine();
                }
            }

            stringBuilder.AppendLine()
                .AppendLine("            GeneratePropertyArray();")
                .AppendLine("            return true;")
                .Append("        }");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 生成从二进制解析数据行的C#代码。
        /// </summary>
        /// <param name="dataTableProcessor">数据表处理器。</param>
        /// <returns>生成的解析逻辑C#代码字符串。</returns>
        private static string GenerateBinaryDataRowParser(DataTableProcessor dataTableProcessor)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder
                .AppendLine("        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)")
                .AppendLine("        {")
                .AppendLine("            using (var memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))")
                .AppendLine("            {")
                .AppendLine("                using (var binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))")
                .AppendLine("                {");

            for (var i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    // 注释列
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    // 编号列
                    stringBuilder.AppendLine("                    m_Id = binaryReader.Read7BitEncodedInt32();");
                    continue;
                }

                var languageKeyword = dataTableProcessor.GetLanguageKeyword(i);
                if (languageKeyword is "int" or "uint" or "long" or "ulong")
                {
                    stringBuilder.AppendFormat("                    {0} = binaryReader.Read7BitEncoded{1}();", dataTableProcessor.GetName(i), dataTableProcessor.GetType(i).Name).AppendLine();
                }
                else
                {
                    stringBuilder.AppendFormat("                    {0} = binaryReader.Read{1}();", dataTableProcessor.GetName(i), dataTableProcessor.GetType(i).Name).AppendLine();
                }
            }

            stringBuilder
                .AppendLine("                }")
                .AppendLine("            }")
                .AppendLine()
                .AppendLine("            GeneratePropertyArray();")
                .AppendLine("            return true;")
                .Append("        }");

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     工具方法：将字符串的第一个字符转换为小写。
        /// </summary>
        /// <param name="input">输入字符串。</param>
        /// <returns>转换后的字符串。</returns>
        private static string FirstCharToLower(string input)
        {
            return string.IsNullOrEmpty(input) ? input : char.ToLowerInvariant(input[0]) + (input.Length > 1 ? input[1..] : "");
        }

        /// <summary>
        ///     为以数字结尾的属性组（如 Prop1, Prop2）生成数组和访问器方法。
        ///     <para>例如，如果数据表中有 Reward1, Reward2, Reward3，此方法会生成一个 Reward 数组和 GetReward(id), GetRewardAt(index) 等辅助方法。</para>
        /// </summary>
        /// <param name="dataTableProcessor">数据表处理器。</param>
        /// <returns>生成的属性数组及相关方法的C#代码字符串。</returns>
        private static string GenerateDataTablePropertyArray(DataTableProcessor dataTableProcessor)
        {
            var propertyCollections = new List<PropertyCollection>();
            for (var i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    // 注释列
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    // 编号列
                    continue;
                }

                var name = dataTableProcessor.GetName(i);
                if (!EndWithNumberRegex.IsMatch(name))
                {
                    continue;
                }

                var propertyCollectionName = EndWithNumberRegex.Replace(name, string.Empty);
                var id = int.Parse(EndWithNumberRegex.Match(name).Value);

                var propertyCollection = propertyCollections.FirstOrDefault(pc => pc.Name == propertyCollectionName);

                if (propertyCollection == null)
                {
                    propertyCollection = new PropertyCollection(propertyCollectionName, dataTableProcessor.GetLanguageKeyword(i));
                    propertyCollections.Add(propertyCollection);
                }

                propertyCollection.AddItem(id, name);
            }

            var stringBuilder = new StringBuilder();
            var firstProperty = true;
            foreach (var propertyCollection in propertyCollections)
            {
                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    stringBuilder.AppendLine().AppendLine();
                }

                stringBuilder
                    .AppendFormat("        private KeyValuePair<int, {1}>[] {0};", FirstCharToLower(propertyCollection.Name), propertyCollection.LanguageKeyword).AppendLine()
                    .AppendLine()
                    .AppendFormat("        public int {0}Count => {1}.Length;", propertyCollection.Name, FirstCharToLower(propertyCollection.Name)).AppendLine()
                    .AppendLine()
                    .AppendFormat("        public {1} Get{0}(int id)", propertyCollection.Name, propertyCollection.LanguageKeyword).AppendLine()
                    .AppendLine("        {")
                    .AppendFormat("            foreach (var i in {0})", FirstCharToLower(propertyCollection.Name))
                    .AppendLine()
                    .AppendLine("            {")
                    .AppendLine("                if (i.Key == id)")
                    .AppendLine("                {")
                    .AppendLine("                    return i.Value;")
                    .AppendLine("                }")
                    .AppendLine("            }")
                    .AppendLine()
                    .AppendFormat("            throw new GameFrameworkException(Utility.Text.Format(\"Get{0} with invalid id '{{0}}'.\", id));", propertyCollection.Name).AppendLine()
                    .AppendLine("        }")
                    .AppendLine()
                    .AppendFormat("        public {1} Get{0}At(int index)", propertyCollection.Name, propertyCollection.LanguageKeyword).AppendLine()
                    .AppendLine("        {")
                    .AppendFormat("            if (index < 0 || index >= {0}.Length)", FirstCharToLower(propertyCollection.Name)).AppendLine()
                    .AppendLine("            {")
                    .AppendFormat("                throw new GameFrameworkException(Utility.Text.Format(\"Get{0}At with invalid index '{{0}}'.\", index));", propertyCollection.Name).AppendLine()
                    .AppendLine("            }")
                    .AppendLine()
                    .AppendFormat("            return {0}[index].Value;", FirstCharToLower(propertyCollection.Name))
                    .AppendLine()
                    .Append("        }");
            }

            if (propertyCollections.Count > 0)
            {
                stringBuilder.AppendLine().AppendLine();
            }

            stringBuilder
                .AppendLine("        private void GeneratePropertyArray()")
                .AppendLine("        {");

            firstProperty = true;
            foreach (var propertyCollection in propertyCollections)
            {
                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    stringBuilder.AppendLine().AppendLine();
                }

                stringBuilder
                    .AppendFormat("            {0} = new KeyValuePair<int, {1}>[]", FirstCharToLower(propertyCollection.Name), propertyCollection.LanguageKeyword).AppendLine()
                    .AppendLine("            {");

                var itemCount = propertyCollection.ItemCount;
                for (var i = 0; i < itemCount; i++)
                {
                    var item = propertyCollection.GetItem(i);
                    stringBuilder.AppendFormat("                new ({0}, {1}),", item.Key.ToString(), item.Value).AppendLine();
                }

                stringBuilder.Append("            };");
            }

            stringBuilder
                .AppendLine()
                .Append("        }");

            return stringBuilder.ToString();
        }
    }
}