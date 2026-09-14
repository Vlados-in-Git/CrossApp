using System;
using System.Runtime.InteropServices;

namespace Core;

public record EnvironmentInfo(
    string App,
    string Student,
    string OSDescription,
    string OSVersion,
    string ProcessArchitecture,
    string DotNetVersion,
    string RuntimeFramework,
    string BaseDirectory,
    string CurrentDirectory,
    string Domain
);

public static class EnvironmentService
{
    public static EnvironmentInfo GetEnvironmentInfo()
    {
        return new EnvironmentInfo(
            App: "CrossApp - Cross-platform programming practicum",
            Student: "Vlad, Group 33",
            OSDescription: RuntimeInformation.OSDescription,
            OSVersion: Environment.OSVersion.ToString(),
            ProcessArchitecture: RuntimeInformation.ProcessArchitecture.ToString(),
            DotNetVersion: Environment.Version.ToString(),
            RuntimeFramework: RuntimeInformation.FrameworkDescription,
            BaseDirectory: AppContext.BaseDirectory,
            CurrentDirectory: Environment.CurrentDirectory,
            Domain: "Warehouse (products, batches, stock, movements)"
        );
    }
}
