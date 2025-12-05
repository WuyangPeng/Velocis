using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.RuntimeException;

namespace Game.Scripts.Main.Runtime.LoadGame
{
    public abstract class LoadGameBase 
    {
        public abstract void LoadGame();

        public static LoadGameBase Create(InitGameType initGameType)
        {
            return initGameType switch
            {
                InitGameType.Begin => new BeginLoadGame(),
                InitGameType.Map => new MapLoadGame(),
                InitGameType.Family => new FamilyLoadGame(),
                InitGameType.Sect => new SectLoadGame(),
                InitGameType.Npc => new NpcLoadGame(),
                InitGameType.MartialArts => new MartialArtsLoadGame(),
                InitGameType.End => new NullLoadGame(),
                _ => throw new GameException($"InitGameType = {initGameType} is not exist.")
            };
        }
    }
}