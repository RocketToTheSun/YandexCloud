namespace YandexCloud.INIT.Infrastructure
{
    public interface IConsoleReader
    {
        DateTime ReadDateTime(string message);
        string ReadString(string message);
    }
}