using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Shared;

namespace Lab05.Subscriber
{
    class Program
    {
        private static readonly string _topicName = "lab05";
        private static readonly string _subscriptionName1 = "small";
        private static readonly string _subscriptionName2 = "medium";
        private static readonly string _subscriptionName3 = "large";
        
        private static ISubscriptionClient _subscription;
        
        static void Main(string[] args)
        {
            MainAsync(args.FirstOrDefault() ?? _subscriptionName1).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string subscriptionName)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Lab05 - Subscriber for '{subscriptionName}' ({Configs.PID})");
            
            await Bootstrap.Lab05Async(_topicName, _subscriptionName1, _subscriptionName2, _subscriptionName3);
            
            _subscription = new SubscriptionClient(Configs.SbConnectionString, _topicName, subscriptionName);
            
            var messageHandlerOptions = new MessageHandlerOptions(Helpers.ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            
            _subscription.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            
            Console.WriteLine("Press 'enter' to close app");
            Console.ReadLine();
            Console.ResetColor();

            await _subscription.CloseAsync();
        }
        
        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message!" +
                              $"SequenceNumber: {message.SystemProperties.SequenceNumber} " +
                              $"Body: {Encoding.UTF8.GetString(message.Body)} " +
                              $"Number: {message.UserProperties["tShirt"]}");

            await _subscription.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}