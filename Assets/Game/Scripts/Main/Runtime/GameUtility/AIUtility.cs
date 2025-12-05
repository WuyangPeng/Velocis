using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Game.Scripts.Main.Runtime.Definition.Enum;
using Game.Scripts.Main.Runtime.Entity;
using Game.Scripts.Main.Runtime.Entity.EntityLogic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.GameUtility
{
    /// <summary>
    /// AI 工具类。
    /// </summary>
    public static class AIUtility
    {
        private static readonly Dictionary<CampPair, RelationType> CampPairToRelation = new();
        private static readonly Dictionary<(CampType, RelationType), CampType[]> CampAndRelationToCamps = new();

        static AIUtility()
        {
            CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Player), RelationType.Friendly);
            CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Enemy), RelationType.Hostile);
            CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Neutral), RelationType.Neutral);
            CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Player2), RelationType.Hostile);
            CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Enemy2), RelationType.Hostile);
            CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Neutral2), RelationType.Neutral);

            CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Enemy), RelationType.Friendly);
            CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Neutral), RelationType.Neutral);
            CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Player2), RelationType.Hostile);
            CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Enemy2), RelationType.Hostile);
            CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Neutral2), RelationType.Neutral);

            CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Neutral), RelationType.Neutral);
            CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Player2), RelationType.Neutral);
            CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Enemy2), RelationType.Neutral);
            CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Neutral2), RelationType.Hostile);

            CampPairToRelation.Add(new CampPair(CampType.Player2, CampType.Player2), RelationType.Friendly);
            CampPairToRelation.Add(new CampPair(CampType.Player2, CampType.Enemy2), RelationType.Hostile);
            CampPairToRelation.Add(new CampPair(CampType.Player2, CampType.Neutral2), RelationType.Neutral);

            CampPairToRelation.Add(new CampPair(CampType.Enemy2, CampType.Enemy2), RelationType.Friendly);
            CampPairToRelation.Add(new CampPair(CampType.Enemy2, CampType.Neutral2), RelationType.Neutral);

            CampPairToRelation.Add(new CampPair(CampType.Neutral2, CampType.Neutral2), RelationType.Neutral);
        }

        /// <summary>
        /// 获取两个阵营之间的关系。
        /// </summary>
        /// <param name="first">阵营一。</param>
        /// <param name="second">阵营二。</param>
        /// <returns>阵营间关系。</returns>
        public static RelationType GetRelation(CampType first, CampType second)
        {
            if (first > second)
            {
                (first, second) = (second, first);
            }

            if (CampPairToRelation.TryGetValue(new CampPair(first, second), out var relationType))
            {
                return relationType;
            }

            Log.Warning("Unknown relation between '{0}' and '{1}'.", first.ToString(), second.ToString());
            return RelationType.Unknown;
        }

        /// <summary>
        /// 获取和指定具有特定关系的所有阵营。
        /// </summary>
        /// <param name="camp">指定阵营。</param>
        /// <param name="relation">关系。</param>
        /// <returns>满足条件的阵营数组。</returns>
        public static CampType[] GetCamps(CampType camp, RelationType relation)
        {
            var key = (camp, relation);
            if (CampAndRelationToCamps.TryGetValue(key, out var result))
            {
                return result;
            }

            // TODO: GC Alloc
            var campTypes = Enum.GetValues(typeof(CampType));

            // TODO: GC Alloc
            result = campTypes.Cast<object>().Select((t, i) => (CampType)campTypes.GetValue(i)).Where(campType => GetRelation(camp, campType) == relation).ToArray();
            CampAndRelationToCamps[key] = result;

            return result;
        }

        /// <summary>
        /// 获取实体间的距离。
        /// </summary>
        /// <returns>实体间的距离。</returns>
        public static float GetDistance(Entity.EntityLogic.Entity fromEntity, Entity.EntityLogic.Entity toEntity)
        {
            var fromTransform = fromEntity.CachedTransform;
            var toTransform = toEntity.CachedTransform;
            return (toTransform.position - fromTransform.position).magnitude;
        }

        public static void PerformCollision(TargetableObject entity, Entity.EntityLogic.Entity other)
        {
            if (entity == null || other == null)
            {
                return;
            }

            var target = other as TargetableObject;
            if (target != null)
            {
                PerformCollision(entity, target);
                return;
            }

            var bullet = other as Bullet;
            if (bullet != null)
            {
                PerformCollision(entity, bullet);
            }

        }

        private static void PerformCollision(TargetableObject entity, Bullet bullet)
        {
            var entityImpactData = entity.GetImpactData();
            var bulletImpactData = bullet.GetImpactData();
            if (GetRelation(entityImpactData.Camp, bulletImpactData.Camp) == RelationType.Friendly)
            {
                return;
            }

            var entityDamageHp = CalcDamageHp(bulletImpactData.Attack, entityImpactData.Defense);

            entity.ApplyDamage(bullet, entityDamageHp);
            Base.GameEntry.Entity.HideEntity(bullet);
        }

        private static void PerformCollision(TargetableObject entity, TargetableObject target)
        {
            var entityImpactData = entity.GetImpactData();
            var targetImpactData = target.GetImpactData();
            if (GetRelation(entityImpactData.Camp, targetImpactData.Camp) == RelationType.Friendly)
            {
                return;
            }

            var entityDamageHp = CalcDamageHp(targetImpactData.Attack, entityImpactData.Defense);
            var targetDamageHp = CalcDamageHp(entityImpactData.Attack, targetImpactData.Defense);

            var delta = Mathf.Min(entityImpactData.Hp - entityDamageHp, targetImpactData.Hp - targetDamageHp);
            if (delta > 0)
            {
                entityDamageHp += delta;
                targetDamageHp += delta;
            }

            entity.ApplyDamage(target, entityDamageHp);
            target.ApplyDamage(entity, targetDamageHp);
        }

        private static int CalcDamageHp(int attack, int defense)
        {
            if (attack <= 0)
            {
                return 0;
            }

            if (defense < 0)
            {
                defense = 0;
            }

            return attack * attack / (attack + defense);
        }

        [StructLayout(LayoutKind.Auto)]
        private readonly struct CampPair : IEquatable<CampPair>
        {
            public CampPair(CampType first, CampType second)
            {
                First = first;
                Second = second;
            }

            private CampType First { get; }

            private CampType Second { get; }

            public bool Equals(CampPair other)
            {
                return First == other.First && Second == other.Second;
            }

            public override bool Equals(object obj)
            {
                return obj is CampPair other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine((int)First, (int)Second);
            }
        }
    }
}
