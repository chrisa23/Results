namespace Results.Tests;

[TestFixture]
public class ImplicitTests
{

    [Test]
    public void ImplicitConvert()
    {
        var result = GetResult1();
        Assert.IsTrue(result);
        Assert.That(result.Value, Is.EqualTo("Test"));
        
        var resultError = GetResultError();
        Assert.IsFalse(resultError);
        Assert.That(resultError.Error, Is.TypeOf<Exception>());
        
    }

    private Result<string, Exception> GetResult1()
    {
        return "Test";
    }
    
    private Result<string, Exception> GetResultError()
    {
        return new Exception();
    }
}