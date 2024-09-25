namespace TeleprompterConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        await RunTeleprompter();
    }

    private static async Task RunTeleprompter()
    {
        var config = new TelePrompterConfig();
        var displayTask = ShowTeleprompter(config);

        var speedTask = GetInput(config);
        await Task.WhenAny(displayTask, speedTask);
    }

    private static async Task ShowTeleprompter(TelePrompterConfig config)
    {
        var words = ReadFrom("sampleQuotes.txt");
        foreach (var word in words)
        {
            Console.Write(word);
            if (!string.IsNullOrWhiteSpace(word))
            {
                //var pause = Task.Delay(200);
                //pause.Wait();
                // the two lines above are an anti-pattern
                // Synchronously waiting on a task is not ideal.
                await Task.Delay(200);
            }
        }
        config.SetDone();
    }

    static IEnumerable<string> ReadFrom(string file)
    {
        string? line;
        using ( var reader = File.OpenText(file))
        {
            while((line = reader.ReadLine()) != null)
            {
                // yield return line; //IEnumerable is an iterator method, and contains a yield return statement
                // commented to yeild individual words instead

                var words = line.Split(' ');
                var lineLength = 0;
                foreach (var word in words)
                {
                    yield return word + " ";
                    lineLength += word.Length;
                    if (lineLength > 70)
                    {
                        yield return Environment.NewLine;
                        lineLength = 0;
                    }
                }
                yield return Environment.NewLine;
            }
        }
    }

    private static async Task GetInput(TelePrompterConfig config)
    {
        var delay = 200;
        Action work = () =>
        {
            do {
                var key = Console.ReadKey(true);
                if (key.KeyChar == '>')
                {
                    delay -= 10;
                }
                else if (key.KeyChar == '<')
                {
                    delay += 10;
                }
                else if (key.KeyChar == 'X' || key.KeyChar == 'x')
                {
                    break;
                }
            } while (!config.Done);
        };
        await Task.Run(work);
    }
}