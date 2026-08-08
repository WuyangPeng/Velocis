using System.Collections.Generic;
using Game.Scripts.Main.Runtime.SaveData;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIForm;

namespace Game.Scripts.Main.Runtime.Procedure
{
    public interface IProcedureFormHost
    {
        void OpenUIForm(UIFormId form);

        void RemoveUIForm(UIFormId formId);
    }

    public interface IProcedureMenuHost : IProcedureFormHost
    {
        void LoadGame();

        void StartGame();

        void LoadHeadData();

        bool HasHeadData(int index);

        List<HeadSaveData> GetHeadData();
    }

    public interface IProcedureCreateHost : IProcedureFormHost
    {
        void EnterGame();

        void ReturnMenu();

        void SaveData();
    }
}
