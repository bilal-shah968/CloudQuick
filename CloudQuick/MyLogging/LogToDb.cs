namespace CloudQuick.MyLogging
{
    public class  LogToDb : IMyLogger
    {
        public void Log(string messge)
        {
            Console.WriteLine(messge);
            Console.WriteLine("LogtoDB");
            //write your own logic to save the logs to Db

        }
    }
}
