namespace Game.Scripts.Main.Editor.Luban
{
    public class ProcessResult
    {
        public ProcessResult(int exitCode, string output, string error)
        {
            ExitCode = exitCode;
            Output = output;
            Error = error;
        }

        public int ExitCode { get; }
        public string Output { get; }
        public string Error { get; }
    }
}