namespace Game.Scripts.Main.Editor.Luban
{
    public class LubanInfo
    {
        private readonly string _command;
        private string _argument;

        public LubanInfo(string command, string argument)
        {
            _command = command;
            _argument = argument;
        }

        public string GetArgument()
        {
            return _argument;
        }

        public string GetCommand()
        {
            return _command;
        }

        public void AddArgument(string arg)
        {
            _argument += arg;
        }
    }
}