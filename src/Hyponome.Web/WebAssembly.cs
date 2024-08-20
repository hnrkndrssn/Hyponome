using System;
using System.Reflection;

namespace Hyponome.Web;

public static class HyponomeWeb
{
    public static Assembly Assembly => typeof(HyponomeWeb).Assembly;
    public static string Version = GetVersion();
    public static string ApplicationName = Assembly.GetName().Name ?? nameof(HyponomeWeb);

    static string GetVersion() 
        => Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyInformationalVersionAttribute)) is AssemblyInformationalVersionAttribute assemblyInformationalVersion
    ? assemblyInformationalVersion.InformationalVersion
    : Assembly.GetName().Version?.ToString() ?? "0.0.0.0-local";
}