using Game.Scripts.Main.Runtime.Definition.DataStruct;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.BuiltinData
{
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        [SerializeField]
        private TextAsset buildInfoTextAsset;

        [SerializeField]
        private TextAsset defaultDictionaryTextAsset;

        [SerializeField]
        private UpdateResourceForm updateResourceFormTemplate;

        public BuildInfo BuildInfo { get; private set; }

        public UpdateResourceForm UpdateResourceFormTemplate => updateResourceFormTemplate;

        public void InitBuildInfo()
        {
            if (buildInfoTextAsset == null || string.IsNullOrEmpty(buildInfoTextAsset.text))
            {
                Log.Info("Build info can not be found or empty.");
                return;
            }

            BuildInfo = Utility.Json.ToObject<BuildInfo>(buildInfoTextAsset.text);
            if (BuildInfo != null) return;
            Log.Warning("Parse build info failure.");
        }

        public void InitDefaultDictionary()
        {
            if (defaultDictionaryTextAsset == null || string.IsNullOrEmpty(defaultDictionaryTextAsset.text))
            {
                Log.Info("Default dictionary can not be found or empty.");
                return;
            }

            if (!Base.GameEntry.Localization.ParseData(defaultDictionaryTextAsset.text))
            {
                Log.Warning("Parse default dictionary failure.");
            }
        }
    }
}
