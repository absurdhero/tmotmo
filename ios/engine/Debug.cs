
public class Debug
{
    const bool debug = true;

    public static void Log(string msg) {
        if (debug)
            System.Console.WriteLine(msg);
    }
}

