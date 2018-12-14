using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Shared;

namespace Lab03.Publisher
{
    class Program
    {
        private static readonly string _topicName = "lab03";
        private static readonly string _subscriptionName1 = "sub1";
        private static readonly string _subscriptionName2 = "sub2";
        private static ITopicClient _topic;
        
        private static readonly int _messagesToSend = 10;
        
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Lab03 - Publisher ({Configs.PID})");

            await Bootstrap.Lab03Async(_topicName, _subscriptionName1, _subscriptionName2);
            
            _topic = new TopicClient(Configs.SbConnectionString, _topicName);
            
            for (var i = 0; i < _messagesToSend; i++)
            {
                var messageBody = $"pid {Configs.PID} message {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                
                Console.WriteLine($"Sending message: {messageBody}");
                
                await _topic.SendAsync(message);
            }
            
            Console.WriteLine("Done!");
            Console.ResetColor();
        }
    }
}