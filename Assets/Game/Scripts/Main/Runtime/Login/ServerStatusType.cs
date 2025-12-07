namespace Game.Scripts.Main.Runtime.Login
{
    public enum ServerStatusType
    {
        Normal = 0, // 正常
        Busy = 1, // 繁忙
        Crowded = 2, // 拥挤
        Full = 3, // 爆满
        Maintenance = 4 // 维护
    }
}