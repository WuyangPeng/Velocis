using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Game.Scripts.Main.Editor
{
    public class CSCeleritasGenerator : EditorWindow
    {
        [MenuItem("Tools/Generate CSCeleritas")]
        public static void Generate()
        {
            var generator = new CsCeleritasGeneratorInstance();
            generator.Run();
        }
    }

    public class CsCeleritasGeneratorInstance
    {
        private Dictionary<string, ProtoMessage> _messages = new Dictionary<string, ProtoMessage>();
        private string _protoRootAbs;

        public void Run()
        {
            _protoRootAbs = Path.Combine(Application.dataPath, "Game/proto");
            ParseAllProtos(_protoRootAbs);

            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using Celeritas.Proto;");
            sb.AppendLine("using Celeritas.Proto.Client;");
            sb.AppendLine("using Celeritas.Proto.Common;");
            sb.AppendLine("using ProtoBuf;");
            sb.AppendLine();
            sb.AppendLine("namespace Game.Scripts.Main.Runtime.Network.Packet");
            sb.AppendLine("{");
            sb.AppendLine("    [Serializable]");
            sb.AppendLine("    [ProtoContract(Name = @\"CSCeleritas\")]");
            sb.AppendLine("    public class CSCeleritas : CSPacketBase");
            sb.AppendLine("    {");
            sb.AppendLine("        public CSCeleritas()");
            sb.AppendLine("        {");
            sb.AppendLine("            Common = new header();");
            sb.AppendLine("            Celeritas = new celeritas");
            sb.AppendLine("            {");
            sb.AppendLine("                CeleritasRequest = new request");
            sb.AppendLine("                {");
            sb.AppendLine("                    Client = new client_request()");
            sb.AppendLine("                }");
            sb.AppendLine("            };");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public override int Id => 100;");
            sb.AppendLine();
            sb.AppendLine("        public header Common { get; set; }");
            sb.AppendLine("        public celeritas Celeritas { get; set; }");
            sb.AppendLine();

            // 从 client_request 开始生成方法
            var rootMsg = FindMessage("client_request");
            if (rootMsg != null)
            {
                GenerateMethods(sb, rootMsg, new List<ProtoField>(), "Celeritas.CeleritasRequest.Client");
            }
            else
            {
                Debug.LogError("Could not find client_request message.");
            }

            sb.AppendLine("        public override void Clear()");
            sb.AppendLine("        {");
            sb.AppendLine("            Common = new header();");
            sb.AppendLine("            Celeritas = new celeritas");
            sb.AppendLine("            {");
            sb.AppendLine("                CeleritasRequest = new request");
            sb.AppendLine("                {");
            sb.AppendLine("                    Client = new client_request()");
            sb.AppendLine("                }");
            sb.AppendLine("            };");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            string finalPath = Path.Combine(Application.dataPath, "Game/Scripts/Main/Runtime/Network/Packet/CSCeleritas.cs");
            File.WriteAllText(finalPath, sb.ToString());
            AssetDatabase.Refresh();
            Debug.Log("CSCeleritas generated successfully.");
        }

        private void GenerateMethods(StringBuilder sb, ProtoMessage msg, List<ProtoField> path, string propertyPath)
        {
            foreach (var field in msg.Fields)
            {
                if (!string.IsNullOrEmpty(field.OneOfGroup))
                {
                    var currentPath = new List<ProtoField>(path);
                    currentPath.Add(field);

                    string methodName = GetMethodName(currentPath);
                    string fieldType = field.Type;
                    string returnType = GetCSharpTypeName(fieldType);
                    string fieldPropertyName = ToPascalCase(field.Name);
                    string newPropertyPath = propertyPath + "." + fieldPropertyName;

                    sb.AppendLine($"        public {returnType} {methodName}()");
                    sb.AppendLine("        {");
                    
                    if (path.Count > 0)
                    {
                        string parentMethodName = GetMethodName(path);
                        sb.AppendLine($"            {parentMethodName}();");
                    }

                    sb.AppendLine($"            {newPropertyPath} = new {returnType}();");
                    sb.AppendLine();
                    sb.AppendLine($"            return {newPropertyPath};");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    var childMsg = FindMessage(fieldType);
                    if (childMsg != null)
                    {
                        GenerateMethods(sb, childMsg, currentPath, newPropertyPath);
                    }
                }
            }
        }

        private string GetMethodName(List<ProtoField> path)
        {
            if (path.Count == 0) return "";
            
            string first = GetCleanTypeName(path[0].Type);
            
            if (path.Count == 1)
            {
                return "Set" + first;
            }
            else
            {
                string last = GetCleanTypeName(path[path.Count - 1].Type);
                return "Set" + first + last;
            }
        }

        private string GetCleanTypeName(string typeName)
        {
            if (typeName.Contains("."))
            {
                typeName = typeName.Substring(typeName.LastIndexOf('.') + 1);
            }

            if (typeName.EndsWith("_request"))
            {
                typeName = typeName.Substring(0, typeName.Length - "_request".Length);
            }

            if (typeName.StartsWith("client_"))
            {
                string withoutClient = typeName.Substring("client_".Length);
                if (withoutClient == "player")
                {
                    typeName = withoutClient;
                }
            }

            return ToPascalCase(typeName);
        }

        private string GetCSharpTypeName(string protoType)
        {
            if (protoType.Contains("."))
            {
                return protoType.Substring(protoType.LastIndexOf('.') + 1);
            }
            return protoType;
        }

        private string ToPascalCase(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var parts = str.Split('_');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                {
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
                }
            }
            return string.Join("", parts);
        }

        private ProtoMessage FindMessage(string name)
        {
            if (_messages.ContainsKey(name)) return _messages[name];

            foreach (var kvp in _messages)
            {
                if (kvp.Key.EndsWith("." + name) || kvp.Key == name)
                {
                    return kvp.Value;
                }
                
                var shortName = kvp.Key;
                if (shortName.Contains(".")) shortName = shortName.Substring(shortName.LastIndexOf('.') + 1);
                
                if (shortName == name) return kvp.Value;
            }
            return null;
        }

        private void ParseAllProtos(string rootPath)
        {
            var files = Directory.GetFiles(rootPath, "*.proto", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                ParseProtoFile(file);
            }
        }

        private void ParseProtoFile(string path)
        {
            string content = File.ReadAllText(path);
            string package = "";
            
            var packageMatch = Regex.Match(content, @"package\s+([\w\.]+);");
            if (packageMatch.Success)
            {
                package = packageMatch.Groups[1].Value;
            }

            content = Regex.Replace(content, @"//.*", "");
            
            var lines = content.Split('\n');
            ProtoMessage currentMessage = null;
            string currentOneOf = null;

            foreach (var line in lines)
            {
                string l = line.Trim();
                if (string.IsNullOrEmpty(l)) continue;

                if (l.StartsWith("message "))
                {
                    var match = Regex.Match(l, @"message\s+(\w+)\s*\{?");
                    if (match.Success)
                    {
                        string msgName = match.Groups[1].Value;
                        string fullName = string.IsNullOrEmpty(package) ? msgName : package + "." + msgName;
                        currentMessage = new ProtoMessage { Name = fullName, Package = package };
                        _messages[fullName] = currentMessage;
                    }
                }
                else if (l.StartsWith("oneof "))
                {
                    var match = Regex.Match(l, @"oneof\s+(\w+)\s*\{?");
                    if (match.Success)
                    {
                        currentOneOf = match.Groups[1].Value;
                        if (currentMessage != null)
                        {
                            currentMessage.OneOfs.Add(currentOneOf);
                        }
                    }
                }
                else if (l.Contains("}"))
                {
                    if (currentOneOf != null)
                    {
                        if (l.Contains("}")) currentOneOf = null;
                    }
                    else if (currentMessage != null)
                    {
                        // 消息结束
                    }
                }
                else if (currentMessage != null)
                {
                    var fieldMatch = Regex.Match(l, @"^([\w\.]+)\s+(\w+)\s*=\s*\d+;");
                    if (fieldMatch.Success)
                    {
                        var field = new ProtoField
                        {
                            Type = fieldMatch.Groups[1].Value,
                            Name = fieldMatch.Groups[2].Value,
                            OneOfGroup = currentOneOf
                        };
                        currentMessage.Fields.Add(field);
                    }
                }
            }
        }
    }

    public class ProtoMessage
    {
        public string Name { get; set; }
        public string Package { get; set; }
        public List<ProtoField> Fields { get; set; } = new List<ProtoField>();
        public List<string> OneOfs { get; set; } = new List<string>();
    }

    public class ProtoField
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string OneOfGroup { get; set; }
    }
}
