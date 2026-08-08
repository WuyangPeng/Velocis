using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIItem
{
    /// <summary>
    ///     道具详情面板。
    /// </summary>
    public class UIItemDetailsForm : UGuiForm
    {
        [Header("Top Info")] [SerializeField] private UIItemIcon uiItemIcon;

        [SerializeField] private Text textName;
        [SerializeField] private Text textSlot;
        [SerializeField] private Text textDurability;
        [SerializeField] private GameObject objLockedBadge;
        [SerializeField] private GameObject objEquippedBadge;

        [Header("Middle Content")] [SerializeField]
        private Text textAttributes;

        [SerializeField] private Text textDescription;
        [SerializeField] private Button buttonSource;

        [Header("Action Buttons")] [SerializeField]
        private GameObject layoutConsumable;

        [SerializeField] private Button buttonUse;
        [SerializeField] private Button buttonBatchUse;

        [Header("Equipment Buttons")] [SerializeField]
        private GameObject layoutEquipment;

        [SerializeField] private Button buttonEquipOrUnequip;
        [SerializeField] private Text textEquipOrUnequipBtn;
        [SerializeField] private Button buttonLockOrUnlock;
        [SerializeField] private Text textLockOrUnlockBtn;
        [SerializeField] private Button buttonDecompose;
        [SerializeField] private Button buttonRecast;

        //private item_config _config;
        private int _count;
        private int _durabilityCurrent = 100;
        private int _durabilityMax = 100;
        private string _equippedHeroName;
        private bool _isEquipped;
        private bool _isLocked;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            // 解析传递的数据
            if (userData is ItemDetailsParams param)
            {
                //_config = param.Config;
                _count = param.Count;
                _isLocked = param.IsLocked;
                _isEquipped = param.IsEquipped;
                _equippedHeroName = param.EquippedHeroName;
                _durabilityCurrent = param.DurabilityCurrent;
                _durabilityMax = param.DurabilityMax;
            }

            RefreshUI();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            // _config = null;
            base.OnClose(isShutdown, userData);
        }

        private void RefreshUI()
        {
            /*  if (_config == null)
              {
                  Close();
                  return;
              }

              // 1. 顶部基础信息
              if (uiItemIcon != null)
              {
                  uiItemIcon.SetData(_config, 1, _isLocked, _isEquipped);
              }

              if (textName != null)
              {
                  textName.text = GameEntry.Localization.GetString(_config.NameKey);
                  textName.color = GetQualityColor(_config.Quality);
              }

              if (textSlot != null)
              {
                  textSlot.text = _config.ItemType == item_type.equipment ? GameEntry.Localization.GetString("Item.SlotWeapon") : GetTypeName(_config.ItemType);
              }

              if (textDurability != null)
              {
                  if (_config.ItemType == item_type.equipment)
                  {
                      textDurability.gameObject.SetActive(true);
                      textDurability.text = GameEntry.Localization.GetString("Item.DurabilityFormat", _durabilityCurrent.ToString(), _durabilityMax.ToString());
                  }
                  else
                  {
                      textDurability.gameObject.SetActive(false);
                  }
              }

              if (objLockedBadge != null)
              {
                  objLockedBadge.SetActive(_isLocked);
              }

              if (objEquippedBadge != null)
              {
                  objEquippedBadge.SetActive(_isEquipped);
                  if (_isEquipped && !string.IsNullOrEmpty(_equippedHeroName))
                  {
                      // 可以设置已装备给某位将领的文本
                  }
              }

              // 2. 中部展示属性与描述
              if (textAttributes != null)
              {
                  if (_config.ItemType == item_type.equipment)
                  {
                      textAttributes.gameObject.SetActive(true);
                      textAttributes.text = GameEntry.Localization.GetString("Item.AttributePhysicsAttack") + "：+150\n" + GameEntry.Localization.GetString("Item.AttributeCritRate") + "：+5.5% (" + GameEntry.Localization.GetString("Item.AttributeRandomAffix") + ")";
                  }
                  else
                  {
                      textAttributes.gameObject.SetActive(false);
                  }
              }

              if (textDescription != null)
              {
                  textDescription.text = GameEntry.Localization.GetString(_config.DescriptionKey);
              }

              // 3. 根据道具类型动态显隐按钮区
              if (_config.ItemType == item_type.consumable)
              {
                  layoutConsumable.SetActive(true);
                  layoutEquipment.SetActive(false);
              }
              else if (_config.ItemType == item_type.equipment)
              {
                  layoutConsumable.SetActive(false);
                  layoutEquipment.SetActive(true);

                  // 更新装备按钮文本
                  if (textEquipOrUnequipBtn != null)
                  {
                      textEquipOrUnequipBtn.text = GameEntry.Localization.GetString(_isEquipped ? "Item.ActionUnequip" : "Item.ActionEquip");
                  }

                  if (textLockOrUnlockBtn != null)
                  {
                      textLockOrUnlockBtn.text = GameEntry.Localization.GetString(_isLocked ? "Item.ActionUnlock" : "Item.ActionLock");
                  }
              }
              else
              {
                  layoutConsumable.SetActive(false);
                  layoutEquipment.SetActive(false);
              }
              */
        }

        /*   private Color GetQualityColor(quality_type quality)
           {
               switch (quality)
               {
                   case quality_type.common: return Color.white;
                   case quality_type.uncommon: return new Color(0.3f, 0.8f, 0.3f);
                   case quality_type.rare: return new Color(0.2f, 0.6f, 1f);
                   case quality_type.epic: return new Color(0.7f, 0.2f, 0.9f);
                   case quality_type.legendary: return new Color(1f, 0.6f, 0f);
                   case quality_type.mythic: return Color.red;
                   default: return Color.white;
               }
           }

           private string GetTypeName(item_type type)
           {
               switch (type)
               {
                   case item_type.consumable: return GameEntry.Localization.GetString("Item.TypeConsumable");
                   case item_type.equipment: return GameEntry.Localization.GetString("Item.TypeEquipment");
                   case item_type.resource: return GameEntry.Localization.GetString("Item.TypeResource");
                   default: return GameEntry.Localization.GetString("Item.TypeUnknown");
               }
           }*/

        #region UI Button Callbacks

        public void OnClickClose()
        {
            Close();
        }

        public void OnClickSource()
        {
            // 点击弹出获取途径面板
            //GameEntry.UI.OpenUIForm(UIFormId.ItemSourceForm, _config);
        }

        public void OnClickUse()
        {
            //Log.Info($"使用道具: {_config.NameKey}");
            // 执行使用道具逻辑...
            Close();
        }

        public void OnClickBatchUse()
        {
            // Log.Info($"批量使用道具: {_config.NameKey}");
            // 执行批量使用逻辑...
            Close();
        }

        public void OnClickEquipOrUnequip()
        {
            _isEquipped = !_isEquipped;
            Log.Info(_isEquipped ? "穿戴装备" : "卸下装备");
            RefreshUI();
        }

        public void OnClickLockOrUnlock()
        {
            _isLocked = !_isLocked;
            Log.Info(_isLocked ? "锁定装备" : "解锁装备");
            RefreshUI();
        }

        public void OnClickDecompose()
        {
            // 点击分解，弹出安全确认与分解弹窗
            var param = new ItemConfirmDialogParams
            {
                //Config = _config,
                IsLocked = _isLocked,
                DecomposeResultDescription = "生铁 x10, 铜钱 x100"
            };
            GameEntry.UI.OpenUIForm(UIFormId.ItemConfirmDialogForm, param);
        }

        public void OnClickRecast()
        {
            Log.Info("重铸装备");
        }

        #endregion
    }

    /// <summary>
    ///     道具详情面板的初始化参数。
    /// </summary>
    public class ItemDetailsParams
    {
        // public item_config Config;
        public int Count;
        public int DurabilityCurrent = 100;
        public int DurabilityMax = 100;
        public string EquippedHeroName;
        public bool IsEquipped;
        public bool IsLocked;
    }
}