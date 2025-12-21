using Game.Scripts.Main.Runtime.Procedure;
using GameFramework;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Main.Editor.BuildEvent.Generator
{
    public static class DataTableGeneratorMenu
    {
        [MenuItem("Velocis/Generate DataTables")]
        private static void GenerateDataTables()
        {
            foreach (var dataTableName in ProcedurePreload.DataTableNames)
            {
                if (!GenerateDataTables(dataTableName))
                {
                    break;
                }
            }

            AssetDatabase.Refresh();
        }

        private static bool GenerateDataTables(string dataTableName)
        {
            var dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
            if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
            {
                Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                return false;
            }

            DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
            DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            
            return true;
        }
    }
}