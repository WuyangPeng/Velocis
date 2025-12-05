using System.Collections;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using Constant = Game.Scripts.Main.Runtime.Definition.Constant.Constant;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UICommon
{
    public static class UIExtension
    {
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            var time = 0f;
            var originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
        {
            var time = 0f;
            var originalValue = slider.value;
            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }

        public static bool HasUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroupName = null)
        {
            return uiComponent.HasUIForm((int)uiFormId, uiGroupName);
        }

        public static bool HasUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        {
            var dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            var drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                return false;
            }

            var assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (string.IsNullOrEmpty(uiGroupName))
            {
                return uiComponent.HasUIForm(assetName);
            }

            var uiGroup = uiComponent.GetUIGroup(uiGroupName);
            return uiGroup != null && uiGroup.HasUIForm(assetName);
        }

        public static UGuiForm GetUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroupName = null)
        {
            return uiComponent.GetUIForm((int)uiFormId, uiGroupName);
        }

        public static UGuiForm GetUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        {
            var dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            var drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                return null;
            }

            var assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);

            if (string.IsNullOrEmpty(uiGroupName))
            {
                var nullGroupNameUiForm = uiComponent.GetUIForm(assetName);
                if (nullGroupNameUiForm == null)
                {
                    return null;
                }

                return (UGuiForm)nullGroupNameUiForm.Logic;
            }

            var uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return null;
            }

            var uiForm = (UnityGameFramework.Runtime.UIForm)uiGroup.GetUIForm(assetName);
            if (uiForm == null)
            {
                return null;
            }

            return (UGuiForm)uiForm.Logic;
        }

        public static void CloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, UIFormId uiFormId, object userData = null)
        {
            return uiComponent.OpenUIForm((int)uiFormId, userData);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            var dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            var drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            var assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (drUIForm.AllowMultiInstance)
            {
                return uiComponent.OpenUIForm(assetName, drUIForm.UIGroupName, Constant.AssetPriority.UIFormAsset, drUIForm.PauseCoveredUIForm, userData);
            }

            if (uiComponent.IsLoadingUIForm(assetName))
            {
                return null;
            }

            if (uiComponent.HasUIForm(assetName))
            {
                return null;
            }

            return uiComponent.OpenUIForm(assetName, drUIForm.UIGroupName, Constant.AssetPriority.UIFormAsset, drUIForm.PauseCoveredUIForm, userData);
        }

        public static void OpenDialog(this UIComponent uiComponent, DialogParams dialogParams)
        {
            if (((ProcedureBase)GameEntry.Procedure.CurrentProcedure).UseNativeDialog)
            {
                OpenNativeDialog(dialogParams);
            }
            else
            {
                uiComponent.OpenUIForm(UIFormId.DialogForm, dialogParams);
            }
        }

        private static void OpenNativeDialog(DialogParams dialogParams)
        {
            // TODO：这里应该弹出原生对话框，先简化实现为直接按确认按钮
            dialogParams.OnClickConfirm?.Invoke(dialogParams.UserData);
        }
    }
}
