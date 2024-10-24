using System.ComponentModel.DataAnnotations;

using var cts = new CancellationTokenSource();

KickOff(cts);

Console.WriteLine("Press any key to abort");

while (!cts.IsCancellationRequested)
{
    var attribute = new RangeAttribute(typeof(DateTime), "1900-01-01", "2019-06-06 23:59:00");
    //var attribute = new ParsableRangeAttribute<DateTime>("1900-01-01", "2019-06-06 23:59:00");
    //var attribute = new LazyRangeAttribute<DateTime>("1900-01-01", "2019-06-06 23:59:00");
    var tasks = Enumerable.Range(0, 50).Select(_ => Task.Run(() => attribute.IsValid(DateTime.Now)));
    await Task.WhenAll(tasks);
}

return;

static void KickOff(CancellationTokenSource cancellationTokenSource)
{
    _ = Task.Run(() =>
    {
        Console.ReadKey();
        cancellationTokenSource.Cancel();
    });
}
