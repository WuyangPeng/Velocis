using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIItem
{
    /// <summary>
    ///     安全确认与分解二次弹窗。
    /// </summary>
    public class UIItemConfirmDialogForm : UGuiForm
    {
        [Header("Warning Elements")] [SerializeField]
        private Text textTitle;

        [SerializeField] private Text textWarningPrompt;

        [FormerlySerializedAs("sourceItemIcon")] [Header("Target & Output Display")] [SerializeField]
        private UIItemIcon sourceUIItemIcon;

        [SerializeField] private Text textOutputDetails;

        [Header("Security Verification")] [SerializeField]
        private Toggle toggleConfirmRisk;

        [SerializeField] private InputField inputFieldVerifyText;
        [SerializeField] private GameObject objInputVerifyArea;

        [Header("Action Buttons")] [SerializeField]
        private Button buttonConfirm;

        [SerializeField] private Button buttonCancel;

        // private item_config _config;
        private string _decomposeResultDescription;
        private bool _isLocked;
        private bool _needTextInputVerification;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            if (userData is ItemConfirmDialogParams param)
            {
                // _config = param.Config;
                _isLocked = param.IsLocked;
                _decomposeResultDescription = param.DecomposeResultDescription;
            }

            // 检查是否需要输入验证文本。橙色品质及以上装备或锁定装备进行操作时，需要手动输入以拦截。
            //  _needTextInputVerification = _isLocked || (_config != null && _config.Quality >= quality_type.legendary);

            if (toggleConfirmRisk != null)
            {
                toggleConfirmRisk.isOn = false;
                toggleConfirmRisk.onValueChanged.RemoveAllListeners();
                toggleConfirmRisk.onValueChanged.AddListener(_ => RefreshConfirmButtonState());
            }

            if (inputFieldVerifyText != null)
            {
                inputFieldVerifyText.text = string.Empty;
                inputFieldVerifyText.onValueChanged.RemoveAllListeners();
                inputFieldVerifyText.onValueChanged.AddListener(_ => RefreshConfirmButtonState());
            }

            RefreshUI();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            //_config = null;
            base.OnClose(isShutdown, userData);
        }

        private void RefreshUI()
        {
            /* if (_config == null)
             {
                 Close();
                 return;
             }

             // 1. 设置来源物品图标与产出详情描述
             if (sourceUIItemIcon != null)
             {
                 sourceUIItemIcon.SetData(_config, 1, _isLocked);
             }

             if (textOutputDetails != null)
             {
                 textOutputDetails.text = GameEntry.Localization.GetString("Item.DecomposeOutputTitle", _decomposeResultDescription);
             }

             // 2. 判断是否展示输入验证码区域
             if (objInputVerifyArea != null)
             {
                 objInputVerifyArea.SetActive(_needTextInputVerification);
             }

             if (textWarningPrompt != null)
             {
                 textWarningPrompt.text = GameEntry.Localization.GetString(_isLocked ? "Item.DecomposeWarningLocked" : "Item.DecomposeWarningNormal");
             }

             RefreshConfirmButtonState();*/
        }

        private void RefreshConfirmButtonState()
        {
            if (buttonConfirm == null)
            {
                return;
            }

            var isCheckboxOk = toggleConfirmRisk == null || toggleConfirmRisk.isOn;
            var isTextInputOk = true;

            if (_needTextInputVerification && inputFieldVerifyText != null)
            {
                // 输入文本必须完全匹配“确认分解”
                isTextInputOk = inputFieldVerifyText.text.Trim() == GameEntry.Localization.GetString("Item.ConfirmVerifyText");
            }

            buttonConfirm.interactable = isCheckboxOk && isTextInputOk;
        }

        public void OnClickConfirm()
        {
            // Log.Info($"向服务器发送请求：分解道具 {_config.NameKey}");

            // 播放一个简单的二次确认成功动效或直接完成处理
            // GameEntry.Event.Fire(this, ...);

            // 关闭分解弹窗和详情界面
            Close();
            GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(UIFormId.ItemDetailsForm));
        }

        public void OnClickCancel()
        {
            Close();
        }
    }

    /// <summary>
    ///     安全确认与分解界面的入参。
    /// </summary>
    public class ItemConfirmDialogParams
    {
        // public item_config Config;
        public string DecomposeResultDescription;
        public bool IsLocked;
    }
}