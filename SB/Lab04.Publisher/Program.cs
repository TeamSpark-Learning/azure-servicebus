using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Shared;

namespace Lab04.Publisher
{
    class Program
    {
        private static readonly string _topicName = "lab04";
        private static readonly string _subscriptionName1 = "all-numbers";
        private static readonly string _subscriptionName2 = "odd-numbers";
        private static readonly string _subscriptionName3 = "even-numbers";
        private static ITopicClient _topic;
        
        private static readonly int _messagesToSend = 10;
        
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        
        static async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Lab04 - Publisher ({Configs.PID})");

            await Bootstrap.Lab04Async(_topicName, _subscriptionName1, _subscriptionName2, _subscriptionName3);
            
            _topic = new TopicClient(Configs.SbFailoverConnectionString, _topicName);
            
            for (var i = 0; i < _messagesToSend; i++)
            {
                var messageBody = $"pid {Configs.PID} message {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                
                message.UserProperties.Add("number", i % 2 == 0 ? "even" : "odd");
                
                Console.WriteLine($"Sending message: {messageBody}");
                
                await _topic.SendAsync(message);
            }
            
            Console.WriteLine("Done!");
            Console.ResetColor();
        }
    }
}