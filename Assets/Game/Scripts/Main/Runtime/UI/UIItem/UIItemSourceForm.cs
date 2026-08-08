using System.Collections.Generic;
// using Celeritas.Config.game;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIItem
{
    /// <summary>
    ///     道具获取途径面板。
    /// </summary>
    public class UIItemSourceForm : UGuiForm
    {
        [FormerlySerializedAs("itemIcon")] [Header("Top Info")] [SerializeField]
        private UIItemIcon uiItemIcon;

        [SerializeField] private Text textTitle;
        [SerializeField] private Text textItemName;

        [Header("Scroll Area")] [SerializeField]
        private ScrollRect scrollRectSources;

        [SerializeField] private Transform contentTransform;
        [SerializeField] private GameObject prefabSourceRow; // 列表项预制体
        [SerializeField] private GameObject objEmptyTip;
        private readonly List<GameObject> _activeRows = new();

        private object _config; // item_config

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _config = userData;
            RefreshUI();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            ClearRows();
            _config = null;
            base.OnClose(isShutdown, userData);
        }

        private void RefreshUI()
        {
            if (_config == null)
            {
                Close();
                return;
            }

            // 1. 顶部渲染
            if (uiItemIcon != null)
            {
                uiItemIcon.SetData(_config, 1);
            }

            if (textTitle != null)
            {
                textTitle.text = GameEntry.Localization.GetString("Item.SourceTitle");
            }

            // if (textItemName != null)
            // {
            //     textItemName.text = GameEntry.Localization.GetString(_config.NameKey);
            // }

            // 2. 清理旧行并填充获取来源
            ClearRows();
            // var sources = GetItemSources(_config.ItemTemplateId);

          /*  if (sources == null || sources.Count == 0)
            {
                if (objEmptyTip != null)
                {
                    objEmptyTip.SetActive(true);
                }

                if (scrollRectSources != null)
                {
                    scrollRectSources.gameObject.SetActive(false);
                }
            }
            else
            {
                if (objEmptyTip != null)
                {
                    objEmptyTip.SetActive(false);
                }

                if (scrollRectSources != null)
                {
                    scrollRectSources.gameObject.SetActive(true);
                }

                foreach (var source in sources)
                {
                    if (prefabSourceRow == null || contentTransform == null)
                    {
                        break;
                    }

                    var rowGo = Instantiate(prefabSourceRow, contentTransform);
                    rowGo.SetActive(true);
                    _activeRows.Add(rowGo);

                    // 填充单行数据
                    var textType = rowGo.transform.Find("TextType")?.GetComponent<Text>();
                    var textName = rowGo.transform.Find("TextName")?.GetComponent<Text>();
                    var btnGo = rowGo.transform.Find("BtnGo")?.GetComponent<Button>();

                    if (textType != null)
                    {
                        textType.text = $"[{source.SourceType}]";
                    }

                    if (textName != null)
                    {
                        textName.text = source.SourceName;
                    }

                    if (btnGo != null)
                    {
                        btnGo.onClick.RemoveAllListeners();
                        btnGo.onClick.AddListener(() => OnClickGoTo(source));
                    }
                }
            }*/
        }

        private void ClearRows()
        {
            foreach (var row in _activeRows)
            {
                if (row != null)
                {
                    Destroy(row);
                }
            }

            _activeRows.Clear();
        }

        private void OnClickGoTo(ItemSourceData source)
        {
            Log.Info($"关闭面板并前往: {source.SourceType} - {source.SourceName} (路由/目标ID: {source.TargetSystemId})");

            // 关闭途径和详情等 UI
            Close();
            GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(UIFormId.ItemDetailsForm));

            // TODO: 调用路由/导航系统跳转至目标场景/界面
            // GameEntry.Event.Fire(this, ...);
        }

        public void OnClickClose()
        {
            Close();
        }

        /// <summary>
        ///     模拟获取配置表中的产出途径数据。
        /// </summary>
        private List<ItemSourceData> GetItemSources(int itemTemplateId)
        {
            // 示例：此处可以读取 luban 生成的掉落配置，为了演示暂提供模拟数据
            var list = new List<ItemSourceData>();
            if (itemTemplateId > 0)
            {
                list.Add(new ItemSourceData { SourceType = GameEntry.Localization.GetString("Item.SourceTypeCampaign"), SourceName = GameEntry.Localization.GetString("Item.SourceCampaignDemo"), TargetSystemId = 1001 });
                list.Add(new ItemSourceData { SourceType = GameEntry.Localization.GetString("Item.SourceTypeShop"), SourceName = GameEntry.Localization.GetString("Item.SourceShopDemo"), TargetSystemId = 2002 });
                list.Add(new ItemSourceData { SourceType = GameEntry.Localization.GetString("Item.SourceTypeActivity"), SourceName = GameEntry.Localization.GetString("Item.SourceActivityDemo"), TargetSystemId = 3003 });
            }

            return list;
        }
    }

    /// <summary>
    ///     单个获取来源的数据模型。
    /// </summary>
    public struct ItemSourceData
    {
        public string SourceType; // 战役 / 商店 / 活动
        public string SourceName; // 详情描述
        public int TargetSystemId; // 用于路由跳转的目标系统 ID
    }
}