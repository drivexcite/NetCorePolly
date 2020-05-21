# Boundded Concurrency and Resiliency for .NET Apps

Thi repository is intended to provide a starting point to implement concurrency and resiliency strategies for .NET async tasks. 
It includes two projects, a server and a client.

## Server
A mock of a REST API that returns a big list of documents, with server-side pagination.

## Client
A console application that consumes the documents from the server, page by page and then process them somehow.

## Stage 1. Sequential
This is the stage of the code contained in this repo. In this stage, the client retrieves the elements from the server, sequentially page by page. 

### Disadvantages
When every requests makes expensive data access calls (simulating artificially in the server by delaying by at least 60 ms), retrieving sequentially severely slow downs the performance of the client for the processing operation. Notice that even when the execution of every page retrieving operation is invoked asynchronously without blocking the thread, the calls to the service are still executed sequentially.

```csharp
for (var page = 0; page < pages; page++)
{
    var currentPage = page;
    await ProcessSolrDocuments(currentPage, pageSize);

    completedSuccessfully++;
}
```

## Stage 2. Unbounded parallelism
In this stage, the code has been changed to use the Task Parellel Library to retrieve all the tasks and then await on them until they all complete.
```csharp
var tasks = new List<Task<List<LegacyDocument>>>();

for (var page = 0; page < pages; page++)
{
    var currentPage = page;
    tasks.Add(ProcessSolrDocuments(currentPage, pageSize));
}

await Task.WhenAll(tasks);
```
### Disadvantages
Notice that with this implementation, the number of tasks that execute simultaneously, is not under the control of the developer, but bounded only by the number of threads in the CLR thread pool. Because of this reason, it may be possible to overwhelm the server and get a higher failure rate than what would otherwise be desirable.

## Stage 3. Bounded parallelism with `SemaphoreSlim`.
In this stage, we have to introduce a global Semaphore that all the individual tasks can use to synchronize the execution and achieve bounded parallelism. Also notice that the code in the main method, doesn't change respect to the code shown for stage 1.

```csharp
private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(10);

public static async Task<List<LegacyDocument>> ProcessSolrDocuments(int page, int pageSize)
{
    await Semaphore.WaitAsync();

    try
    {
        // work!
    }
    finally
    {
        Semaphore.Release();
    }
}
```

### Disadvantages
The main disadvantage of this method is that now the tasks have knowledge of the fact that they run on a multi-threaded environment, also, sharing the global semaphore may be difficult when these tasks are distributed acrross many layers.

## Stage 4. Bounded parellelism with Bulkhead policy.
In this stage, we rely on the Polly Library to delegate the invocation of the work to a policy object. The policy will take care of limit the number of concurrent tasks using configuration.

```csharp
var concurrentExecutionPolicy = Policy.BulkheadAsync(10, pages);

for (var page = 0; page < pages; page++)
{
    var currentPage = page;
    var task = concurrentExecutionPolicy.ExecuteAsync(async () => await ProcessSolrDocuments(currentPage, pageSize));

    tasks.Add(task);
}

await Task.WhenAll(tasks);
```

Notice that the code that implements the actual work doesn't need to be aware that is executing in parallel.



