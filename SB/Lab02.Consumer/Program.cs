using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Shared;

namespace Lab02.Consumer
{
    class Program
    {
        private static readonly string _queueName = "lab02";
        private static readonly string _queueNameDead = "lab02-dead";
        
        private static IQueueClient _queue;
        private static IQueueClient _queueDead;
        
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        
        static async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Lab02 - Consumer ({Configs.PID})");

            await Bootstrap.Lab02Async(_queueName, _queueNameDead);
            
            _queue = new QueueClient(Configs.SbFailoverConnectionString, _queueName);
            _queueDead = new QueueClient(Configs.SbFailoverConnectionString, _queueNameDead);
            
            var messageHandlerOptions = new MessageHandlerOptions(Helpers.ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queue.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            _queueDead.RegisterMessageHandler(ProcessDeadMessagesAsync, messageHandlerOptions);
            
            Console.WriteLine("Press 'enter' to close app");
            Console.ReadLine();
            Console.ResetColor();

            await _queue.CloseAsync();
        }
        
        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message!" +
                              $"SequenceNumber: {message.SystemProperties.SequenceNumber} " +
                              $"DeliveryCount: {message.SystemProperties.DeliveryCount} " +
                              $"Body: {Encoding.UTF8.GetString(message.Body)}");

            //await Task.Delay(TimeSpan.FromSeconds(5));
            await _queue.AbandonAsync(message.SystemProperties.LockToken);
        }
        
        static async Task ProcessDeadMessagesAsync(Message message, CancellationToken token)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[DEAD] ");
            Console.ForegroundColor = color;
            
            Console.WriteLine($"DeliveryCount: {message.SystemProperties.DeliveryCount} Body: {Encoding.UTF8.GetString(message.Body)}");

            await _queueDead.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}