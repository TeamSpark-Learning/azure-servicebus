using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Shared;

namespace Lab01.Consumer
{
    class Program
    {
        private static readonly string _queueName = "lab01";
        private static IQueueClient _queue;
        
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        
        static async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Lab01 - Consumer ({Configs.PID})");

            await Bootstrap.Lab01Async(_queueName);
            
            _queue = new QueueClient(Configs.SbConnectionString, _queueName);
            
            var messageHandlerOptions = new MessageHandlerOptions(Helpers.ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queue.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            
            Console.WriteLine("Press 'enter' to close app");
            Console.ReadLine();
            Console.ResetColor();
            
            await _queue.CloseAsync();
        }
        
        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message!" +
                              $"SequenceNumber: {message.SystemProperties.SequenceNumber}" +
                              $"Body: {Encoding.UTF8.GetString(message.Body)}");

            //await Task.Delay(TimeSpan.FromSeconds(5));
            
            await _queue.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}