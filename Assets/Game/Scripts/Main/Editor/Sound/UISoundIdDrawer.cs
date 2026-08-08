using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Game.Scripts.Main.Runtime.Sound;

namespace Game.Scripts.Main.Editor.Sound
{
    [CustomPropertyDrawer(typeof(UISoundIdAttribute))]
    public class UISoundIdDrawer : PropertyDrawer
    {
        private string[] _names;
        private int[] _ids;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            if (_names == null)
            {
                LoadSounds();
            }

            if (_ids == null || _ids.Length == 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            int currentIndex = System.Array.IndexOf(_ids, property.intValue);
            if (currentIndex < 0) currentIndex = 0;

            int newIndex = EditorGUI.Popup(position, label.text, currentIndex, _names);
            property.intValue = _ids[newIndex];
        }

        private void LoadSounds()
        {
            var namesList = new List<string>();
            var idsList = new List<int>();

            string filePath = Path.Combine(Application.dataPath, "Game", "DataTables", "UISound.txt");
            if (File.Exists(filePath))
            {
                // Read UISound.txt using UTF-16LE (System.Text.Encoding.Unicode)
                string[] lines = File.ReadAllLines(filePath, System.Text.Encoding.Unicode);
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;
                    string[] parts = line.Split('\t');
                    if (parts.Length >= 4)
                    {
                        if (int.TryParse(parts[1], out int id))
                        {
                            string desc = parts[2];
                            string asset = parts[3];
                            namesList.Add($"{id} - {desc} ({asset})");
                            idsList.Add(id);
                        }
                    }
                }
            }

            _names = namesList.ToArray();
            _ids = idsList.ToArray();
        }
    }
}
