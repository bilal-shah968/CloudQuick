namespace CloudQuick.MyLogging
{
    public class LogToServerMemory : IMyLogger
    {
        public void Log(string messge)
        {
            Console.WriteLine(messge);
            Console.WriteLine("Logto Server memory");
            //write your own logic to save the logs to Server memory

        }
    }
}
