using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor.ResourceTools;

namespace Game.Scripts.Main.Editor.ResourceTools
{
    /// <summary>
    ///     Sync common gameplay asset folders into ResourceCollection so new prefabs/tables
    ///     do not need to be hand-edited into ResourceCollection.xml.
    /// </summary>
    public static class ResourceCollectionSyncMenu
    {
        private static readonly FolderMapping[] Mappings =
        {
            new("Assets/Game/UI/UIForms", "UI/UIForms", "UI", true, new[] { ".prefab" }),
            new("Assets/Game/DataTables", "DataTables", "GameData", true, new[] { ".txt", ".bytes" }),
            new("Assets/Game/HotfixDll/Hotfix", "HotfixDll", "GameData", true, new[] { ".bytes" }),
            new("Assets/Game/Sounds", "Sounds", "Resources", true, new[] { ".wav", ".mp3", ".ogg" }),
            new("Assets/Game/Music", "Music", "Resources", true, new[] { ".wav", ".mp3", ".ogg" }),
            new("Assets/Game/Fonts", "Fonts", "UI", true, new[] { ".ttf", ".otf", ".asset" }),
            new("Assets/Game/Configs", "Configs", "GameData", true, new[] { ".txt", ".bytes" }),
            new("Assets/Game/Scenes", "Scenes", "Resources", true, new[] { ".unity" }),
            new("Assets/Game/Entities", "Entities", "Resources", true, new[] { ".prefab" }),
            new("Assets/Game/Textures/Icon/Help", "Textures", "Resources", true, new[] { ".png" })
        };

        [MenuItem("Velocis/Resource/Sync Collection From Folders", priority = 100)]
        public static void Sync()
        {
            var collection = new ResourceCollection();
            if (!collection.Load())
            {
                Debug.LogError("Load ResourceCollection.xml failed.");
                return;
            }

            var assigned = 0;
            var skipped = 0;
            var failed = 0;

            foreach (var mapping in Mappings)
            {
                EnsureResource(collection, mapping);

                var guids = AssetDatabase.FindAssets(string.Empty, new[] { mapping.folder });
                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (string.IsNullOrEmpty(assetPath) || Directory.Exists(assetPath))
                    {
                        continue;
                    }

                    if (!EndsWithAny(assetPath, mapping.extensions))
                    {
                        continue;
                    }

                    if (assetPath.EndsWith(".asmdef") || assetPath.Contains("/Editor/"))
                    {
                        continue;
                    }

                    var existing = collection.GetAsset(guid);
                    if (existing != null)
                    {
                        skipped++;
                        continue;
                    }

                    if (collection.AssignAsset(guid, mapping.resourceName, null))
                    {
                        assigned++;
                    }
                    else
                    {
                        failed++;
                        Debug.LogWarning($"Assign failed: {assetPath} -> {mapping.resourceName}");
                    }
                }
            }

            // Localization dictionaries: one resource per language folder path already used by AssetUtility.
            SyncLocalizationDictionaries(collection, ref assigned, ref skipped, ref failed);

            // Localization help files: one resource per help text file.
            SyncLocalizationHelp(collection, ref assigned, ref skipped, ref failed);

            // Luban binary configurations: one unpacked resource per configuration file.
            SyncBinConfigs(collection, ref assigned, ref skipped, ref failed);

            if (!collection.Save())
            {
                Debug.LogError("Save ResourceCollection.xml failed.");
                return;
            }

            AssetDatabase.Refresh();
            Debug.Log($"ResourceCollection sync done. Assigned={assigned}, AlreadyAssigned={skipped}, Failed={failed}. Reminder: run Resource Builder, then rebuild player.");
        }

        private static void SyncLocalizationDictionaries(ResourceCollection collection, ref int assigned, ref int skipped, ref int failed)
        {
            const string root = "Assets/Game/Localization";
            if (!AssetDatabase.IsValidFolder(root))
            {
                return;
            }

            var dictionaryGuids = AssetDatabase.FindAssets("Default t:TextAsset", new[] { root });
            foreach (var guid in dictionaryGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!assetPath.EndsWith("/Dictionaries/Default.xml") && !assetPath.EndsWith("/Dictionaries/Default.bytes"))
                {
                    continue;
                }

                // Resource name = path without Assets/Game/ and extension: Localization/ChineseSimplified/Dictionaries/Default
                var relative = assetPath.Substring("Assets/Game/".Length);
                var resourceName = Path.ChangeExtension(relative, null).Replace('\\', '/');

                if (!collection.HasResource(resourceName, null))
                {
                    collection.AddResource(resourceName, null, null, LoadType.LoadFromFile, false);
                }

                if (collection.GetAsset(guid) != null)
                {
                    skipped++;
                    continue;
                }

                if (collection.AssignAsset(guid, resourceName, null))
                {
                    assigned++;
                }
                else
                {
                    failed++;
                    Debug.LogWarning($"Assign failed: {assetPath} -> {resourceName}");
                }
            }
        }

        private static void SyncLocalizationHelp(ResourceCollection collection, ref int assigned, ref int skipped, ref int failed)
        {
            const string root = "Assets/Game/Localization";
            if (!AssetDatabase.IsValidFolder(root))
            {
                return;
            }

            var helpGuids = AssetDatabase.FindAssets("t:TextAsset", new[] { root });
            foreach (var guid in helpGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if ((!assetPath.Contains("/Help/") && !assetPath.Contains("/Loading/")) || !assetPath.EndsWith(".txt"))
                {
                    continue;
                }

                // Resource name = path without Assets/Game/ and extension: Localization/ChineseSimplified/Help/help_desc_xxx
                var relative = assetPath.Substring("Assets/Game/".Length);
                var resourceName = Path.ChangeExtension(relative, null).Replace('\\', '/');

                if (!collection.HasResource(resourceName, null))
                {
                    collection.AddResource(resourceName, null, null, LoadType.LoadFromFile, false);
                }

                if (collection.GetAsset(guid) != null)
                {
                    skipped++;
                    continue;
                }

                if (collection.AssignAsset(guid, resourceName, null))
                {
                    assigned++;
                }
                else
                {
                    failed++;
                    Debug.LogWarning($"Assign failed: {assetPath} -> {resourceName}");
                }
            }
        }

        private static void SyncBinConfigs(ResourceCollection collection, ref int assigned, ref int skipped, ref int failed)
        {
            const string root = "Assets/Game/Bin";
            if (!AssetDatabase.IsValidFolder(root))
            {
                return;
            }

            var binGuids = AssetDatabase.FindAssets(string.Empty, new[] { root });
            foreach (var guid in binGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(assetPath) || Directory.Exists(assetPath) || !assetPath.EndsWith(".bytes"))
                {
                    continue;
                }

                // Resource name = path without Assets/Game/ and extension: Bin/filename
                var relative = assetPath.Substring("Assets/Game/".Length);
                var resourceName = Path.ChangeExtension(relative, null).Replace('\\', '/');

                if (!collection.HasResource(resourceName, null))
                {
                    collection.AddResource(resourceName, null, null, LoadType.LoadFromFile, false);
                }

                if (collection.GetAsset(guid) != null)
                {
                    skipped++;
                    continue;
                }

                if (collection.AssignAsset(guid, resourceName, null))
                {
                    assigned++;
                }
                else
                {
                    failed++;
                    Debug.LogWarning($"Assign failed: {assetPath} -> {resourceName}");
                }
            }
        }

        private static void EnsureResource(ResourceCollection collection, FolderMapping mapping)
        {
            if (collection.HasResource(mapping.resourceName, null))
            {
                return;
            }

            collection.AddResource(
                mapping.resourceName,
                null,
                mapping.fileSystem,
                LoadType.LoadFromFile,
                mapping.packed,
                mapping.packed ? new[] { "Base" } : null);
        }

        private static bool EndsWithAny(string path, IReadOnlyList<string> extensions)
        {
            foreach (var extension in extensions)
            {
                if (path.EndsWith(extension))
                {
                    return true;
                }
            }

            return false;
        }

        private readonly struct FolderMapping
        {
            public readonly string folder;
            public readonly string resourceName;
            public readonly string fileSystem;
            public readonly bool packed;
            public readonly string[] extensions;

            public FolderMapping(string folder, string resourceName, string fileSystem, bool packed, string[] extensions)
            {
                this.folder = folder;
                this.resourceName = resourceName;
                this.fileSystem = fileSystem;
                this.packed = packed;
                this.extensions = extensions;
            }
        }
    }
}