using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Shared
{
    public static class Helpers
    {
        public static async Task EnsureQueueExistsAsync(QueueDescription queueDescription)
        {
            var sbManagement = new ManagementClient(Configs.SbConnectionString);
            
            var queues = await sbManagement.GetQueuesAsync();
            if (!queues.Any(x => x.Path.Equals(queueDescription.Path)))
            {
                Console.WriteLine($"Creating queue: {queueDescription.Path}");
                await sbManagement.CreateQueueAsync(queueDescription);
            }
        }
        
        public static async Task EnsureTopicExistsAsync(TopicDescription topicDescription)
        {
            var sbManagement = new ManagementClient(Configs.SbConnectionString);
            
            var topics = await sbManagement.GetTopicsAsync();
            if (!topics.Any(x => x.Path.Equals(topicDescription.Path)))
            {
                Console.WriteLine($"Creating topic: {topicDescription.Path}");
                await sbManagement.CreateTopicAsync(topicDescription);
            }
        }
        
        public static async Task EnsureSubscriptionExistsAsync(SubscriptionDescription subscriptionDescription)
        {
            var sbManagement = new ManagementClient(Configs.SbConnectionString);

            var subscriptions = await sbManagement.GetSubscriptionsAsync(subscriptionDescription.TopicPath);
            if (!subscriptions.Any(x => x.SubscriptionName.Equals(subscriptionDescription.SubscriptionName)))
            {
                Console.WriteLine($"Creating subscription: {subscriptionDescription.SubscriptionName}");
                await sbManagement.CreateSubscriptionAsync(subscriptionDescription.TopicPath, subscriptionDescription.SubscriptionName);
            }
        }
        
        public static async Task EnsureSubscriptionRuleExistsAsync(string topicName, string subscriptionName, RuleDescription ruleDescription)
        {
            var sbManagement = new ManagementClient(Configs.SbConnectionString);

            var rules = await sbManagement.GetRulesAsync(topicName, subscriptionName);

            var defaultRule = rules.SingleOrDefault(x => x.Name.Equals(RuleDescription.DefaultRuleName));
            if (defaultRule != null)
            {
                Console.WriteLine($"Removing subscription rule: {defaultRule.Name}");
                await sbManagement.DeleteRuleAsync(topicName, subscriptionName, defaultRule.Name);
            }

            if (!rules.Any(x => x.Name.Equals(ruleDescription.Name)))
            {
                Console.WriteLine($"Creating subscription rule: {ruleDescription.Name}");
                await sbManagement.CreateRuleAsync(topicName, subscriptionName, ruleDescription);
            }
        }
        
        public static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            
            return Task.CompletedTask;
        }
    }
}