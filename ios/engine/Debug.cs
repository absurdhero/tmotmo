
public class Debug
{
    const bool debug = true;

    public static void Log(object msg) {
        if (debug)
            System.Console.WriteLine(msg);
    }
}

