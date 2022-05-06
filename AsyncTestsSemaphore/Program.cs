// See https://aka.ms/new-console-template for more information
int total = 100;
int semaphoreCount = 5;
Console.WriteLine($"Fazendo {total} ações de 1 segundo");
//Linear(total);
await TaskWhenAllAsync(total);
await TaskWhenAllWithSem(total, semaphoreCount);

void Linear(int total)
{
    Console.WriteLine($"execução linear:");
    System.Diagnostics.Stopwatch stopwatch = new();
    stopwatch.Start();
    for (int i = 0; i < total; i++)
    {
        Execute(i);
    }
    stopwatch.Stop();
    Console.WriteLine(stopwatch.Elapsed.ToString());
}

async Task TaskWhenAllAsync(int total)
{
    Console.WriteLine($"tasks whenall:");
    List<Task> tasks = new();
    System.Diagnostics.Stopwatch stopwatch = new();
    stopwatch.Start();
    for (int i = 0; i < total; i++)
    {
        tasks.Add(ExecuteAsync(i));
    }
    await Task.WhenAll(tasks);
    stopwatch.Stop();
    Console.WriteLine(stopwatch.Elapsed.ToString());
}

async Task TaskWhenAllWithSem(int x, int semaphoreCount)
{
    Console.WriteLine($"tasks whenall com limite de {semaphoreCount} tasks:");
    List<Task> tasks2 = new();
    System.Diagnostics.Stopwatch stopwatch = new();
    SemaphoreSlim sem = new(semaphoreCount);
    stopwatch.Start();
    for (int i = 0; i < total; i++)
    {
        tasks2.Add(ExecuteAsyncSemaphore(i, sem));
    }
    await Task.WhenAll(tasks2);
    stopwatch.Stop();
    Console.WriteLine(stopwatch.Elapsed.ToString());
}


async Task ExecuteAsyncSemaphore(int x, SemaphoreSlim sem)
{
    await sem.WaitAsync();
    try
    {
        await Task.Run(() =>
            Execute(x)); // pq execute não é async
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        sem.Release();
    }
}

async Task ExecuteAsync(int x)
{
    await Task.Run(() => 
    // pq execute não é async
        Execute(x)); 
}

void Execute(int x)
{
    Console.WriteLine($"fazendo {x}");
    Thread.Sleep(1000);
}