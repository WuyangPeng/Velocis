using Game.Scripts.Main.Runtime.Procedure;
using GameFramework;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Main.Editor.BuildEvent.Generator
{
    public sealed class DataTableGeneratorMenu
    {
        [MenuItem("Rise of History/Generate DataTables")]
        private static void GenerateDataTables()
        {
            foreach (var dataTableName in ProcedurePreload.DataTableNames)
            {
                var dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }
    }
}
