using System;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.Entity.EntityData;
using Game.Scripts.Main.Runtime.Entity.EntityLogic;
using Game.Scripts.Main.Runtime.GameUtility;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Entity
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static EntityLogic.Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            var entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (EntityLogic.Entity)entity.Logic;
        }

        public static void HideEntity(this EntityComponent entityComponent, EntityLogic.Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, EntityLogic.Entity entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

        public static void ShowMyAircraft(this EntityComponent entityComponent, MyAircraftData data)
        {
            entityComponent.ShowEntity(typeof(MyAircraft), "Aircraft", Definition.Constant.Constant.AssetPriority.MyAircraftAsset, data);
        }

        public static void ShowAircraft(this EntityComponent entityComponent, AircraftData data)
        {
            entityComponent.ShowEntity(typeof(Aircraft), "Aircraft", Definition.Constant.Constant.AssetPriority.AircraftAsset, data);
        }

        public static void ShowThruster(this EntityComponent entityComponent, ThrusterData data)
        {
            entityComponent.ShowEntity(typeof(Thruster), "Thruster", Definition.Constant.Constant.AssetPriority.ThrusterAsset, data);
        }

        public static void ShowWeapon(this EntityComponent entityComponent, WeaponData data)
        {
            entityComponent.ShowEntity(typeof(Weapon), "Weapon", Definition.Constant.Constant.AssetPriority.WeaponAsset, data);
        }

        public static void ShowArmor(this EntityComponent entityComponent, ArmorData data)
        {
            entityComponent.ShowEntity(typeof(Armor), "Armor", Definition.Constant.Constant.AssetPriority.ArmorAsset, data);
        }

        public static void ShowBullet(this EntityComponent entityComponent, BulletData data)
        {
            entityComponent.ShowEntity(typeof(Bullet), "Bullet", Definition.Constant.Constant.AssetPriority.BulletAsset, data);
        }

        public static void ShowAsteroid(this EntityComponent entityComponent, AsteroidData data)
        {
            entityComponent.ShowEntity(typeof(Asteroid), "Asteroid", Definition.Constant.Constant.AssetPriority.AsteroiAsset, data);
        }

        public static void ShowEffect(this EntityComponent entityComponent, EffectData data)
        {
            entityComponent.ShowEntity(typeof(Effect), "Effect", Definition.Constant.Constant.AssetPriority.EffectAsset, data);
        }

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData.EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            var dtEntity = Base.GameEntry.DataTable.GetDataTable<DREntity>();
            var drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority, data);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
