using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Shared;

namespace Lab02.Producer
{
    class Program
    {
        private static readonly string _queueName = "lab02";
        private static readonly string _queueNameDead = "lab02-dead";
        private static IQueueClient _queue;
        
        private static readonly int _messagesToSend = 10;
        
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        
        static async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Lab02 - Producer ({Configs.PID})");

            await Bootstrap.Lab02Async(_queueName, _queueNameDead);
            
            _queue = new QueueClient(Configs.SbConnectionString, _queueName);
            
            for (var i = 0; i < _messagesToSend; i++)
            {
                var messageBody = $"pid {Configs.PID} message {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                
                Console.WriteLine($"Sending message: {messageBody}");
                
                await _queue.SendAsync(message);
            }

            await _queue.CloseAsync();
            
            Console.WriteLine("Done!");
            Console.ResetColor();
        }
    }
}