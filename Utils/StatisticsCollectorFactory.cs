using System.Runtime.InteropServices;
using StatisticsCollector.Models;

namespace StatisticsCollector.Utils;

public static class StatisticsCollectorFactory
{
    public static IStatisticsCollectorStrategy CreateCollector()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
            return new WindowsStatisticsCollectorStrategy();
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
            return new UnixStatisticsCollectorStrategy();
        
        throw new NotSupportedException(nameof(RuntimeInformation.OSDescription));
    }
}