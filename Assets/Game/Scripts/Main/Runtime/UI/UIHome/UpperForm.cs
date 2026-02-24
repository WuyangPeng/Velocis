using System;
using System.Collections.Generic;
using System.Linq;
using Celeritas.Config;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.GameModule.Develop;
using Game.Scripts.Main.Runtime.GameModule.Item;
using Game.Scripts.Main.Runtime.GameModule.Role;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using GameFramework.Event;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class UpperForm : UGuiForm
    {
        [SerializeField] private AvatarItem avatarItem;

        [SerializeField] private TMP_Text roleName;

        [SerializeField] private TMP_Text roleLevel;

        [SerializeField] private TMP_Text vipLevel;

        [SerializeField] private List<ResourceTextMapping> resourceTextMappings = new();
        private readonly Dictionary<currency_type, TMP_Text> _resourceTextDict = new();

        private ProcedureHome _procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureHome = (ProcedureHome)GetCurrentProcedure();
            if (_procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open UpperForm.");
            }

            var avatarModule = GameEntry.ModuleComponent.GetModule<AvatarModule>();
            var selectedAvatar = avatarModule.GetSelectedAvatar();
            if (selectedAvatar != null)
            {
                var avatarConfig = GameEntry.GameConfig.GetGameConfig().GetTables().AvatarConfigContainer.Get(selectedAvatar.Inventory.TemplateId);
                if (avatarConfig != null)
                {
                    avatarItem.SetSprite(avatarConfig.IconRes);
                }
            }

            var frameModule = GameEntry.ModuleComponent.GetModule<FrameModule>();
            var selectedFrame = frameModule.GetSelectedFrame();
            if (selectedFrame != null)
            {
                var frameConfig = GameEntry.GameConfig.GetGameConfig().GetTables().FrameConfigContainer.Get(selectedFrame.Inventory.TemplateId);
                if (frameConfig != null)
                {
                    avatarItem.SetFrameSprite(frameConfig.IconRes);
                }
            }

            var roleModule = GameEntry.ModuleComponent.GetModule<RoleModule>();
            roleName.text = roleModule.GetFullName();

            var roleDevelopModule = GameEntry.ModuleComponent.GetModule<RoleDevelopModule>();
            roleLevel.text = roleDevelopModule.GetLevel().ToString();

            var vipDevelopModule = GameEntry.ModuleComponent.GetModule<VipDevelopModule>();
            vipLevel.text = vipDevelopModule.GetLevel().ToString();

            _resourceTextDict.Clear();
            foreach (var mapping in resourceTextMappings.Where(mapping => mapping.textComponent != null && !_resourceTextDict.ContainsKey(mapping.resourceType)))
            {
                _resourceTextDict[mapping.resourceType] = mapping.textComponent;
            }

            var customModule = GameEntry.ModuleComponent.GetModule<CustomModule>();
            foreach (var kvp in _resourceTextDict)
            {
                kvp.Value.text = NumberFormatter.FormatNumber(customModule.GetItemCount(kvp.Key));
            }

            GameEntry.Event.Subscribe(ChangeNameEventArgs.EventId, OnChangeName);
            GameEntry.Event.Subscribe(ChangeLevelEventArgs.EventId, OnChangeLevel);
            GameEntry.Event.Subscribe(ChangeCustomEventArgs.EventId, OnChangeCustom);
        }

        private void OnChangeCustom(object sender, GameEventArgs e)
        {
            var changeCustomEventArgs = (ChangeCustomEventArgs)e;
            var resourceType = (currency_type)changeCustomEventArgs.ItemId;
            var customModule = GameEntry.ModuleComponent.GetModule<CustomModule>();

            if (_resourceTextDict.TryGetValue(resourceType, out var textComponent))
            {
                textComponent.text = NumberFormatter.FormatNumber(customModule.GetItemCount(resourceType));
            }
        }

        private void OnChangeLevel(object sender, GameEventArgs e)
        {
            var changeLevelEventArgs = (ChangeLevelEventArgs)e;

            switch (changeLevelEventArgs.SystemType)
            {
                case develop_system_type.role:
                {
                    var roleDevelopModule = GameEntry.ModuleComponent.GetModule<RoleDevelopModule>();
                    roleLevel.text = roleDevelopModule.GetLevel().ToString();
                    break;
                }
                case develop_system_type.vip:
                {
                    var vipDevelopModule = GameEntry.ModuleComponent.GetModule<VipDevelopModule>();
                    vipLevel.text = vipDevelopModule.GetLevel().ToString();
                    break;
                }
            }
        }

        private void OnChangeName(object sender, GameEventArgs e)
        {
            var roleModule = GameEntry.ModuleComponent.GetModule<RoleModule>();
            roleName.text = roleModule.GetFullName();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(ChangeNameEventArgs.EventId, OnChangeName);
            GameEntry.Event.Unsubscribe(ChangeLevelEventArgs.EventId, OnChangeLevel);
            GameEntry.Event.Unsubscribe(ChangeCustomEventArgs.EventId, OnChangeCustom);

            _procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnPersonalInformationButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.PersonalInformationForm);
        }

        public void OnVipButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.VipForm);
        }

        [Serializable]
        public class ResourceTextMapping
        {
            public currency_type resourceType;
            public TMP_Text textComponent;
        }
    }
}