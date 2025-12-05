using System.Text;

namespace Game.Scripts.Main.Editor.BuildEvent.Generator
{
    public delegate void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData);
}
