using System.Text.Json;
using NUnit.Framework.Internal;

namespace Results.Tests;

public class UnitTests
{

    [Test]
    public void SystemJsonDeserializes()
    {
        var result  = Result.Ok;
        var serialized   = JsonSerializer.Serialize(result);
        var resultDS = JsonSerializer.Deserialize<Result>(serialized);
        Assert.IsNotNull(resultDS);
        Assert.IsTrue(resultDS);

        
        var result2     = Result<string>.Ok("True");
        var serialized2 = JsonSerializer.Serialize(result2);
        var result2DS   = JsonSerializer.Deserialize<Result>(serialized2);
        Assert.IsNotNull(result2DS);
        Assert.IsTrue(result2DS);
    }

    [Test]
    public void ImplicitBool()
    {
        var result1 = Result.Ok;
        Assert.IsTrue(result1);

        var result2 = Result<string>.Ok("True");
        Assert.IsTrue(result2);
        
        var result3 = Result<string, Exception>.Ok("True");
        Assert.IsTrue(result3);
    }

    [Test]
    public async Task ThenTest1()
    {
        Task<Result> F1() => Task.FromResult(Result.Ok);
        Task<Result> F2() => Task.FromResult(Result.Failed("Error"));

        var f      = F1().Then(F2).Then(F1);
        var result = await f;
        Assert.IsFalse(result);
        Assert.AreEqual("Error", result.Message);

    }
    
    [Test]
    public async Task ThenTest2()
    {
        Task<Result<string>> F1() => Task.FromResult(Result<string>.Ok("1"));

        Task<Result<int>> F2(string s) => 
            Task.FromResult(int.TryParse(s, out var i) ? Result<int>.Ok(i) : Result<int>.Failed("Error"));

        var f      = F1().Then(F2);
        var result = await f;
        Assert.IsTrue(result);
        Assert.AreEqual(1, result.Value);
    }

}