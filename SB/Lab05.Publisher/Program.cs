using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Shared;

namespace Lab05.Publisher
{
    class Program
    {
        private static readonly string _topicName = "lab05";
        private static readonly string _subscriptionName1 = "small";
        private static readonly string _subscriptionName2 = "medium";
        private static readonly string _subscriptionName3 = "large";
        private static ITopicClient _topic;
        
        private static readonly int _messagesToSend = 20;
        
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        
        static async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Lab05 - Publisher ({Configs.PID})");

            await Bootstrap.Lab05Async(_topicName, _subscriptionName1, _subscriptionName2, _subscriptionName3);
            
            _topic = new TopicClient(Configs.SbConnectionString, _topicName);
            
            for (var i = 0; i < _messagesToSend; i++)
            {
                var messageBody = $"pid {Configs.PID} message {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                var size = 0;
                switch (i % 4)
                {
                    case 0:
                        size = 100;
                        break;
                    case 1:
                        size = 200;
                        break;
                    case 2:
                        size = 300;
                        break;
                }
                
                message.UserProperties.Add("tShirt", size);
                
                Console.WriteLine($"Sending message: {messageBody} size: {size}");
                
                await _topic.SendAsync(message);
            }
            
            Console.WriteLine("Done!");
            Console.ResetColor();
        }
    }
}