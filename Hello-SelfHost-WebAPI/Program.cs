using System; 
using Hello_SelfHost_WebAPI.Start;

namespace Hello_SelfHost_WebAPI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var runner = new ApiRunner();
            runner.Start();
            Console.WriteLine("Press ENTER to exit the app.");
            Console.ReadLine();
            runner.Stop();
        }
    }
}