using Game.Scripts.Main.Runtime.GameEnum;
using System.Collections.Generic;

namespace Game.Scripts.Main.Runtime.GameData.User
{
    public class PropertyData
    {
        private readonly Dictionary<BasePropertyType, int> baseProperty = new();
        private readonly Dictionary<DefaultPropertyType, int> defaultProperty = new();

        private readonly Dictionary<SpiritualType, int> spiritual = new();
        private readonly Dictionary<MartialArtsType, int> martialArts = new();

        private readonly Dictionary<TechniqueType, int> technique = new();

        public int GetTechnique(TechniqueType techniqueType)
        {
            return technique.GetValueOrDefault(techniqueType, 0);
        }

        public int GetBaseProperty(BasePropertyType basePropertyType)
        {
            return baseProperty.GetValueOrDefault(basePropertyType, 0);
        }

        public int GetSpiritual(SpiritualType spiritualType)
        {
            return spiritual.GetValueOrDefault(spiritualType, 0);
        }

        public int GetDefaultProperty(DefaultPropertyType defaultPropertyType)
        {
            return defaultProperty.GetValueOrDefault(defaultPropertyType, 0);
        }

        public int GetMartialArts(MartialArtsType martialArtsType)
        {
            return martialArts.GetValueOrDefault(martialArtsType, 0);
        }

        public void AddBaseProperty(int propertyId)
        {
            baseProperty[(BasePropertyType)propertyId] = GetBaseProperty((BasePropertyType)propertyId) + 1;
        }

        public void ReduceBaseProperty(int propertyId)
        {
            baseProperty[(BasePropertyType)propertyId] = GetBaseProperty((BasePropertyType)propertyId) - 1;
        }

        public void AddSpiritual(int spiritualId)
        {
            spiritual[(SpiritualType)spiritualId] = GetSpiritual((SpiritualType)spiritualId) + 1;
        }

        public void ReduceSpiritual(int spiritualId)
        {
            spiritual[(SpiritualType)spiritualId] = GetSpiritual((SpiritualType)spiritualId) - 1;
        }

        public void AddMartialArts(int martialArtsId)
        {
            martialArts[(MartialArtsType)martialArtsId] = GetMartialArts((MartialArtsType)martialArtsId) + 1;
        }

        public void ReduceMartialArts(int martialArtsId)
        {
            martialArts[(MartialArtsType)martialArtsId] = GetMartialArts((MartialArtsType)martialArtsId) - 1;
        }

        public void AddTechnique(int techniqueId)
        {
            technique[(TechniqueType)techniqueId] = GetTechnique((TechniqueType)techniqueId) + 1;
        }

        public void ReduceTechnique(int techniqueId)
        {
            technique[(TechniqueType)techniqueId] = GetTechnique((TechniqueType)techniqueId) - 1;
        }

        public void Init()
        {
            baseProperty.Clear();
            defaultProperty.Clear();
            spiritual.Clear();
            martialArts.Clear();
            technique.Clear();
        }
    }
}