namespace ConsoleApp_CancellationToken
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            CancellationToken token = cts.Token;

            Task.Run(async () =>
            {
                for (int i = 1; i <= 100; i++)
                {
                Console.WriteLine($"{i} : {token.IsCancellationRequested} - {token.CanBeCanceled}");

                    await Task.Delay(1000);

                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Canceled : {0}", token.ToString());
                        return;
                    }
                }

            }, token);

            Console.ReadLine();

            cts.Cancel();

            Console.ReadLine();
        }
    }
}
