using System.Threading;

public static class AnimationThread
{
    private static readonly Thread AnimationThr;

    static AnimationThread()
    {
        AnimationThr = new Thread(Runner);
    }

    private static void Runner(object obj)
    {
        
    }

    public static void Start()
    {
        AnimationThr.Start();
    }
}
