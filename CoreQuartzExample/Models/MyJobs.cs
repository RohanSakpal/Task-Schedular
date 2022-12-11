using System;
namespace CoreQuartzExample.Models
{
    public class MyJobs
    {
        public MyJobs(Type type, string expression)
        {
            Common.Logs($"MyJobs at " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"), "MyJob"+DateTime.Now.ToString("hhmmss"));
            Type = type;
            Expression = expression;
        }
        public Type Type { get;}
        public string Expression { get;}
    }
}
