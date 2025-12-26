namespace Game.Scripts.Main.Runtime.GameModule.Base
{
    public abstract class BaseModule
    {
        public virtual bool IsLoad => true;

        public virtual void LoginFinish()
        {
        }
    }
}