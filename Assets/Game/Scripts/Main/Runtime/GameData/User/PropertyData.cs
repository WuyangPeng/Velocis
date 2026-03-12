using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameEnum;

namespace Game.Scripts.Main.Runtime.GameData.User
{
    public class PropertyData
    {
        private readonly Dictionary<BasePropertyType, int> _baseProperty = new();
        private readonly Dictionary<DefaultPropertyType, int> _defaultProperty = new();
        private readonly Dictionary<MartialArtsType, int> _martialArts = new();

        private readonly Dictionary<SpiritualType, int> _spiritual = new();

        private readonly Dictionary<TechniqueType, int> _technique = new();

        public int GetTechnique(TechniqueType techniqueType)
        {
            return _technique.GetValueOrDefault(techniqueType, 0);
        }

        public int GetBaseProperty(BasePropertyType basePropertyType)
        {
            return _baseProperty.GetValueOrDefault(basePropertyType, 0);
        }

        public int GetSpiritual(SpiritualType spiritualType)
        {
            return _spiritual.GetValueOrDefault(spiritualType, 0);
        }

        public int GetDefaultProperty(DefaultPropertyType defaultPropertyType)
        {
            return _defaultProperty.GetValueOrDefault(defaultPropertyType, 0);
        }

        public int GetMartialArts(MartialArtsType martialArtsType)
        {
            return _martialArts.GetValueOrDefault(martialArtsType, 0);
        }

        public void AddBaseProperty(int propertyId)
        {
            _baseProperty[(BasePropertyType)propertyId] = GetBaseProperty((BasePropertyType)propertyId) + 1;
        }

        public void ReduceBaseProperty(int propertyId)
        {
            _baseProperty[(BasePropertyType)propertyId] = GetBaseProperty((BasePropertyType)propertyId) - 1;
        }

        public void AddSpiritual(int spiritualId)
        {
            _spiritual[(SpiritualType)spiritualId] = GetSpiritual((SpiritualType)spiritualId) + 1;
        }

        public void ReduceSpiritual(int spiritualId)
        {
            _spiritual[(SpiritualType)spiritualId] = GetSpiritual((SpiritualType)spiritualId) - 1;
        }

        public void AddMartialArts(int martialArtsId)
        {
            _martialArts[(MartialArtsType)martialArtsId] = GetMartialArts((MartialArtsType)martialArtsId) + 1;
        }

        public void ReduceMartialArts(int martialArtsId)
        {
            _martialArts[(MartialArtsType)martialArtsId] = GetMartialArts((MartialArtsType)martialArtsId) - 1;
        }

        public void AddTechnique(int techniqueId)
        {
            _technique[(TechniqueType)techniqueId] = GetTechnique((TechniqueType)techniqueId) + 1;
        }

        public void ReduceTechnique(int techniqueId)
        {
            _technique[(TechniqueType)techniqueId] = GetTechnique((TechniqueType)techniqueId) - 1;
        }

        public void Init()
        {
            _baseProperty.Clear();
            _defaultProperty.Clear();
            _spiritual.Clear();
            _martialArts.Clear();
            _technique.Clear();
        }
    }
}