using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

Console.OutputEncoding = Encoding.UTF8;

bool isJson = args.Length > 0 && args.Contains("--json", StringComparer.OrdinalIgnoreCase);

var sysInfo = new
{
    App = "CrossApp - Cross-platform programming practicum",
    Student = "Vlad, Group 33",
    OSDescription = RuntimeInformation.OSDescription,
    OSVersion = Environment.OSVersion.ToString(),
    ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
    DotNetVersion = Environment.Version.ToString(),
    RuntimeFramework = RuntimeInformation.FrameworkDescription,
    BaseDirectory = AppContext.BaseDirectory,
    CurrentDirectory = Environment.CurrentDirectory,
    Domain = "Warehouse (products, batches, stock, movements)"
};

if (isJson)
{
    string jsonString = JsonSerializer.Serialize(sysInfo, new JsonSerializerOptions { WriteIndented = true });
    Console.WriteLine(jsonString);
}
else
{
    Console.WriteLine(sysInfo.App);
    Console.WriteLine($"Student: {sysInfo.Student}");
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"OS (OSDescription)   : {sysInfo.OSDescription}");
    Console.WriteLine($"OS (Environment)     : {sysInfo.OSVersion}");
    Console.WriteLine($"Process Architecture : {sysInfo.ProcessArchitecture}");
    Console.WriteLine($".NET Version (CLR)   : {sysInfo.DotNetVersion}");
    Console.WriteLine($"Runtime              : {sysInfo.RuntimeFramework}");
    Console.WriteLine($"App Directory        : {sysInfo.BaseDirectory}");
    Console.WriteLine($"Current Directory    : {sysInfo.CurrentDirectory}");
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"Domain               : {sysInfo.Domain}");
}
