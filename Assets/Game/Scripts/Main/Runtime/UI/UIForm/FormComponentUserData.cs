using Game.Scripts.Main.Runtime.UI.UICommon;

namespace Game.Scripts.Main.Runtime.UI.UIForm
{
    public class FormComponentUserData
    {
        public FormComponent FormComponent { get; }
        public UIFormId FormId { get; }

        public FormComponentUserData(FormComponent formComponent, UIFormId uiFormId)
        {
            FormComponent = formComponent;
            FormId = uiFormId;
        }
    }
} 