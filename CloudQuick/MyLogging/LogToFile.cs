namespace CloudQuick.MyLogging
{
    public class LogToFile : IMyLogger
    {
        public void Log(string messge)
        {
            Console.WriteLine(messge);
            Console.WriteLine("Logtofile");
            //write your own logic to save the logs to file

        }
    }
}
