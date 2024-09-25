using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using static System.Math;

namespace TeleprompterConsole;

internal class TelePrompterConfig
{
    public int DelayInMilliseconds { get; private set; } = 200;
    public void UpdateDelay(int increment) // negative to speed up
    {
        var newDelay = Min(DelayInMilliseconds + increment, 1000);
        newDelay = Max(newDelay, 20);
        DelayInMilliseconds = newDelay;
    }

    // private static int Max(int newDelay, int v)
    // {
    //     return newDelay > v ? newDelay : v;
    // }

    // private static int Min(int v1, int v2)
    // {
    //     return v1 < v2 ? v1 : v2;
    // }

    public bool Done { get; private set; }
    public void SetDone()
    {
        Done = true;
    }
}