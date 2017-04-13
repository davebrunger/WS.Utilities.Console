namespace WS.Utilities.Console
{
    public interface IOutputWriter
    {
        void WriteLine(string line, bool highlight = false);

        void WriteErrorLine(string line);
    }
}
