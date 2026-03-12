using Celeritas.Proto.Client;

namespace Game.Scripts.Main.Runtime.GameModule.Develop
{
    public class DevelopData
    {
        public DevelopData()
        {
        }

        public DevelopData(int systemId, long instanceId, int level, long exp)
        {
            SystemId = systemId;
            InstanceId = instanceId;
            Level = level;
            Exp = exp;
        }

        public DevelopData(develop_data protoData)
        {
            if (protoData == null)
            {
                return;
            }

            SystemId = protoData.SystemId;
            InstanceId = protoData.InstanceId;
            Level = protoData.Level;
            Exp = protoData.Exp;
        }

        public int SystemId { get; set; }

        public long InstanceId { get; set; }

        public int Level { get; set; }

        public long Exp { get; set; }

        public DevelopData Clone()
        {
            return new DevelopData(SystemId, InstanceId, Level, Exp);
        }

        public void Reset()
        {
            SystemId = 0;
            InstanceId = 0;
            Level = 0;
            Exp = 0;
        }

        public override string ToString()
        {
            return $"DevelopData(SystemId={SystemId}, InstanceId={InstanceId}, Level={Level}, Exp={Exp})";
        }
    }
}