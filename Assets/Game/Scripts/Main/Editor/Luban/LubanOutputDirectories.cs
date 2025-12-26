namespace Game.Scripts.Main.Editor.Luban
{
    public class LubanOutputDirectories
    {
        public LubanOutputDirectories(string codeOutputDirectory, string dataOutputDirectory)
        {
            CodeOutputDirectory = codeOutputDirectory;
            DataOutputDirectory = dataOutputDirectory;
        }

        public string CodeOutputDirectory { get; }
        public string DataOutputDirectory { get; }
    }
}