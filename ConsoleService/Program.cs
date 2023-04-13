// See https://aka.ms/new-console-template for more information
using PongService;
using System.Threading;

namespace ConsoleService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var pongService = PongHTTPService.GetInstance();

            Task.Run(async () =>
            {
                await pongService.ServerListenerAsync();
            });


            while (true)
            {
                Console.WriteLine(PongHTTPService.GetInstance().VoiceModule.Id);
                Console.WriteLine(PongHTTPService.GetInstance().VoiceModule.RacketDirection);
                Console.WriteLine(PongHTTPService.GetInstance().VoiceModule.Velocity);

                await Task.Delay(100);
                Console.Clear();
            }
        }
    }
}
