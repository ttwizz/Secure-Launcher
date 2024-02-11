using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

static byte[] GenerateByte()
{
    int length = new Random().Next(2, 7);
    byte[] numArray = new byte[length];
    for (int index = 0; index < length; ++index)
    numArray[index] = 0;
    return numArray;
}

static string GenerateRandomString(int length)
{
    Random random = new Random();
    StringBuilder stringBuilder = new StringBuilder(length);
    for (int index = 0; index < length; ++index)
    stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz"[random.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz".Length)]);
    return stringBuilder.ToString();
}

static void ShuffleMD5(string file)
{
    int num = new FileInfo(file).Length > 1048576L ? 1048576 : 4096;
    byte[] numArray = GenerateByte();
    using (FileStream fileStream = new FileStream(file, (FileMode)6))
    fileStream.Write(numArray, 0, numArray.Length);
    using (MD5 md5 = MD5.Create())
    {
        using (FileStream fileStream = new FileStream(file, (FileMode)3, (FileAccess)1, (FileShare)1, num))
        Console.WriteLine("New MD5 hash: " + BitConverter.ToString(md5.ComputeHash(fileStream)).Replace("-", ""));
    }
}

static string ShuffleName(string currentDirectory, string oldFilePath)
{
    Path.GetFileNameWithoutExtension(oldFilePath);
    string randomString = GenerateRandomString(8);
    string newFilePath = Path.Combine(currentDirectory, randomString + ".exe");
    try
    {
        File.Move(oldFilePath, newFilePath);
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 2);
        interpolatedStringHandler.AppendLiteral("Renamed '");
        interpolatedStringHandler.AppendFormatted(oldFilePath);
        interpolatedStringHandler.AppendLiteral("' to '");
        interpolatedStringHandler.AppendFormatted(newFilePath);
        interpolatedStringHandler.AppendLiteral("'.");
        Console.WriteLine(interpolatedStringHandler.ToStringAndClear());
    }
    catch (IOException ex)
    {
        Console.WriteLine("An error occurred: " + ex.Message);
    }
    return newFilePath;
}

static void main()
{
    try
    {
        string baseDirectory = AppContext.BaseDirectory;
        string processName = Process.GetCurrentProcess().ProcessName;
        List<string> list = Enumerable.ToList(Enumerable.Where((IEnumerable<string>)Directory.GetFiles(baseDirectory, "*.exe"), (file => !string.Equals(Path.GetFileName(file), processName + ".exe"))));
        if (list.Count == 1)
        {
            Console.WriteLine("Found exe, obscuring.");
            Console.WriteLine(list[0]);
            Console.WriteLine("Shuffling Hash, one moment..");
            ShuffleMD5(list[0]);
            Console.WriteLine("Shuffling exe name, one moment..");
            string newFileName = ShuffleName(baseDirectory, list[0]);
            Console.WriteLine("Shuffling complete, launching exe.");
            Process.Start(new ProcessStartInfo()
            {
                FileName = newFileName,
                UseShellExecute = true
            });
        }
        else
        {
            Console.WriteLine("Couldn't find exe or too many exe files in folder.");
            Console.ReadLine();
        }
        Thread.Sleep(5000);
    }
    catch (Exception ex)
    {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
        interpolatedStringHandler.AppendLiteral("There was an error: ");
        interpolatedStringHandler.AppendFormatted(ex);
        Console.WriteLine(interpolatedStringHandler.ToStringAndClear());
        Console.ReadLine();
    }
}

main();