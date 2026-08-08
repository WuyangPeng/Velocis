// 创建时间：2026-08-07
// 修改时间：2026-08-07

using System.Reflection;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Hotfix.HotfixBusiness.Tools.Dropdown
{
    /// <summary>
    /// 下拉菜单（TMP_Dropdown）箭头旋转与列表高度补偿组件
    /// 负责在下拉菜单展开/收起时旋转箭头图标，并自动补偿 Viewport 内边距导致的列表高度不足问题
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownArrowRotator : MonoBehaviour
    {
        /// <summary> 箭头图标的 RectTransform </summary>
        [SerializeField] private RectTransform arrowRectTransform;
        
        /// <summary> 绑定的 TMP_Dropdown 组件 </summary>
        [SerializeField] private TMP_Dropdown dropdown;
        
        /// <summary> 下拉列表模板的 RectTransform </summary>
        [SerializeField] private RectTransform templateRt;
        
        /// <summary> 视口（Viewport）节点的 RectTransform </summary>
        [SerializeField] private RectTransform viewportRt;
        
        /// <summary> 内容容器（Content）节点的 RectTransform </summary>
        [SerializeField] private RectTransform contentRt;
        
        /// <summary> TMP_Dropdown 私有字段 m_Dropdown 的反射引用 </summary>
        private FieldInfo _dropdownListField;

        /// <summary> 运行时生成的下拉列表 GameObject 引用 </summary>
        private GameObject _listGo;

        /// <summary>
        /// 初始化反射字段引用
        /// </summary>
        private void Awake()
        {
            _dropdownListField = typeof(TMP_Dropdown).GetField("m_Dropdown", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// 帧更新：监听下拉菜单展开/关闭状态，旋转箭头并动态补全列表 Viewport 高度
        /// </summary>
        private void Update()
        {
            if (!dropdown)
            {
                return;
            }

            if (!_listGo)
            {
                if (_dropdownListField != null)
                {
                    _listGo = _dropdownListField.GetValue(dropdown) as GameObject;
                }
            }
            else if (!_listGo.activeInHierarchy)
            {
                // TMP 会销毁/重建列表，关掉后下次重新补偿高度
                _listGo = null;
            }

            if (_listGo)
            {
                EnsureListFitsViewportPadding(_listGo.transform as RectTransform);
            }

            if (!arrowRectTransform)
            {
                return;
            }

            var isExpanded = _listGo && _listGo.activeInHierarchy;
            var targetZ = isExpanded ? 180f : 0f;

            var currentZ = arrowRectTransform.localEulerAngles.z;
            if (!Mathf.Approximately(currentZ, targetZ))
            {
                arrowRectTransform.localRotation = Quaternion.Euler(0f, 0f, targetZ);
            }
        }

        /// <summary>
        /// TMP_Dropdown 会把展开高度收成 Content 高度，但 Viewport 的上下内边距不在计算内，
        /// 导致可视区域少掉一段。这里按 Viewport 上下 inset 把列表加高，保留金边缩进的同时显示全部选项。
        /// </summary>
        /// <param name="listRt">展开下拉列表根节点的 RectTransform</param>
        private void EnsureListFitsViewportPadding(RectTransform listRt)
        {
            if (!listRt || !templateRt || !viewportRt || !contentRt)
            {
                return;
            }

            var viewportIndex = viewportRt.GetSiblingIndex();
            var contentIndex = contentRt.GetSiblingIndex();

            var viewport = listRt.GetChild(viewportIndex) as RectTransform;
            var content = viewport ? viewport.GetChild(contentIndex) as RectTransform : null;
            if (!viewport || !content)
            {
                return;
            }

            var verticalInset = GetVerticalInset(viewport);
            if (verticalInset <= 0f)
            {
                return;
            }

            var targetHeight = content.rect.height + verticalInset;
            if (targetHeight > listRt.rect.height + 0.1f)
            {
                listRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
            }
        }

        /// <summary>
        /// 计算 Viewport 节点的上下内边距（Vertical Inset）总和
        /// </summary>
        /// <param name="viewport">Viewport 节点的 RectTransform</param>
        /// <returns>上下内边距总高度（像素）</returns>
        private static float GetVerticalInset(RectTransform viewport)
        {
            // stretch 满父节点时，sizeDelta.y 为负表示上下合计内边距
            if (Mathf.Approximately(viewport.anchorMin.y, 0f) && Mathf.Approximately(viewport.anchorMax.y, 1f))
            {
                return Mathf.Max(0f, -viewport.sizeDelta.y);
            }

            return Mathf.Max(0f, viewport.offsetMin.y) + Mathf.Max(0f, -viewport.offsetMax.y);
        }
    }
}