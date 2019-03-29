using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Shared;

namespace Lab01.Producer
{
    class Program
    {
        
        private static readonly string _queueName = "lab01";
        private static IQueueClient _queue;
        
        private static readonly int _messagesToSend = 10;
        
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        
        static async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Lab01 - Producer ({Configs.PID})");

            await Bootstrap.Lab01Async(_queueName);
            
            _queue = new QueueClient(Configs.SbFailoverConnectionString, _queueName);
            
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