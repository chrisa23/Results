using System.Text;

namespace Results;

public static class ResultExtensions
{
    public static Result Aggregate(this IEnumerable<Result> results)
    {
        var success = true;
        var sb      = new StringBuilder();
        foreach (var result in results)
        {
            success &= result.Success;
            if (!result.Success)
                sb.AppendLine(result.Message);
        }

        if (success) return Result.Ok;
        return Result.Failed(sb.ToString());
    }
    
    public static async Task<Result> Then(this Task<Result> result, Func<Task<Result>> func)
    {
        var r = await result;
        if (!r) return Result.Failed(r.Message);
        return await func();
    }

    public static async Task<Result<T2>> Then<T1, T2>(this Task<Result<T1>> result, Func<T1, Task<Result<T2>>> func)
    {
        var r = await result;
        if (!r) return Result<T2>.Failed(r.Message);
        return await func(r.Value);
    }
    
}