using System.Reflection;

namespace SkyRoute_Tests.Support;

internal sealed record RaceReport(
    int Attempts,
    int Succeeded,
    int Rejected,
    int Faulted,
    IReadOnlyDictionary<string, int> FaultBreakdown,
    TimeSpan Elapsed);

/// <summary>Reads "did it succeed?" off whatever CreateBookingAsync returns (bool, ServiceResponse&lt;T&gt;, ...).</summary>
internal static class ResultInspector
{
    private static readonly string[] Names = { "Success", "IsSuccess", "Succeeded", "IsSuccessful", "Ok" };

    public static bool IsSuccess(object? result)
    {
        if (result is bool flag) return flag;
        if (result is null) return false;

        foreach (var name in Names)
        {
            var property = result.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (property is not null && property.PropertyType == typeof(bool))
            {
                return (bool)property.GetValue(result)!;
            }
        }

        // Fallback: if the type has exactly one public bool property, that is the success flag.
        var bools = result.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(bool)).ToList();
        if (bools.Count == 1) return (bool)bools[0].GetValue(result)!;

        throw new InvalidOperationException(
            $"Don't know how to read success from {result.GetType().FullName}. Edit ResultInspector.IsSuccess.");
    }
}