# Azure ServiceBus

## Lab 01 - messaging essentials

**Producer** - creates some messages and sent them to the queue.
**Consumer** - listens for new messages to appear within the queue and processes them.

### Scenario 01-a
Run producer and consumer simultaneously.

### Scenario 01-b
Run 3 consumer instances and 1 producer instance.

### Scenario 01-c
- Add `await Task.Delay(TimeSpan.FromSeconds(5))`to consumer.
- Set `MaxConcurrentCalls = 3` for consumer.
- Run producer and consumer simultaneously.

## Lab 02 - dead messages


## Lab 03 - publisher / subscriber


## Lab 04 - publisher / subscriber with simple filtering


## Lab 05 - publisher / subscriber with sql filtering

