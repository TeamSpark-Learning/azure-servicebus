using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Shared
{
    public static class Bootstrap
    {
        public static async Task Lab01Async(string queueName)
        {
            await Helpers.EnsureQueueExistsAsync(new QueueDescription(queueName));
        }
        
        public static async Task Lab02Async(string queueName, string queueNameDead)
        {
            await Helpers.EnsureQueueExistsAsync(new QueueDescription(queueNameDead));
            await Helpers.EnsureQueueExistsAsync(new QueueDescription(queueName)
            {
                ForwardDeadLetteredMessagesTo = queueNameDead,
                LockDuration = TimeSpan.FromSeconds(5),
                DefaultMessageTimeToLive = TimeSpan.FromSeconds(20)
            });
        }
        
        public static async Task Lab03Async(string topicName, string subscriptionName1, string subscriptionName2)
        {
            await Helpers.EnsureTopicExistsAsync(new TopicDescription(topicName));
            await Helpers.EnsureSubscriptionExistsAsync(new SubscriptionDescription(topicName, subscriptionName1));
            await Helpers.EnsureSubscriptionExistsAsync(new SubscriptionDescription(topicName, subscriptionName2));
        }
        
        public static async Task Lab04Async(string topicName, string subscriptionName1, string subscriptionName2, string subscriptionName3)
        {
            await Helpers.EnsureTopicExistsAsync(new TopicDescription(topicName));
            await Helpers.EnsureSubscriptionExistsAsync(new SubscriptionDescription(topicName, subscriptionName1));
            await Helpers.EnsureSubscriptionExistsAsync(new SubscriptionDescription(topicName, subscriptionName2));
            await Helpers.EnsureSubscriptionRuleExistsAsync(topicName, subscriptionName2, new RuleDescription
            {
                Name = "odd-only",
                Filter = new CorrelationFilter { Properties = { new KeyValuePair<string, object>("number", "odd") }}
            });
            await Helpers.EnsureSubscriptionExistsAsync(new SubscriptionDescription(topicName, subscriptionName3));
            await Helpers.EnsureSubscriptionRuleExistsAsync(topicName, subscriptionName3, new RuleDescription
            {
                Name = "even-only",
                Filter = new CorrelationFilter { Properties = { new KeyValuePair<string, object>("number", "even") }}
            });
        }
        
        public static async Task Lab05Async(string topicName, string subscriptionName1, string subscriptionName2, string subscriptionName3)
        {
            await Helpers.EnsureTopicExistsAsync(new TopicDescription(topicName));
            await Helpers.EnsureSubscriptionExistsAsync(new SubscriptionDescription(topicName, subscriptionName1));
            await Helpers.EnsureSubscriptionRuleExistsAsync(topicName, subscriptionName1, new RuleDescription
            {
                Name = "small-size",
                Filter = new SqlFilter("tShirt > 0 AND tShirt < 120")
            });
            await Helpers.EnsureSubscriptionExistsAsync(new SubscriptionDescription(topicName, subscriptionName2));
            await Helpers.EnsureSubscriptionRuleExistsAsync(topicName, subscriptionName2, new RuleDescription
            {
                Name = "medium-size",
                Filter = new SqlFilter("tShirt >= 120 AND tShirt <= 250")
            });
            await Helpers.EnsureSubscriptionExistsAsync(new SubscriptionDescription(topicName, subscriptionName3));
            await Helpers.EnsureSubscriptionRuleExistsAsync(topicName, subscriptionName3, new RuleDescription
            {
                Name = "large-size",
                Filter = new SqlFilter("tShirt > 250")
            });
        }
    }
}