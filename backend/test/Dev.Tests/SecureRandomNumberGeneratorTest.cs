namespace Dev.Tests;


public class SecureRandomNumberGeneratorTest
{
    [Fact]
    public void Next_ShouldReturnNonNegativeValue()
    {
        using var rng = new SecureRandomNumberGenerator();
        var result = rng.Next();
        Assert.True(result >= 0);
    }

    [Fact]
    public void Next_WithMaxValue_ShouldReturnValueInRange()
    {
        using var rng = new SecureRandomNumberGenerator();
        var maxValue = 100;
        var result = rng.Next(maxValue);
        Assert.InRange(result, 0, maxValue);
    }

    [Fact]
    public void Next_WithMinAndMaxValue_ShouldReturnValueInRange()
    {
        using var rng = new SecureRandomNumberGenerator();
        var minValue = 10;
        var maxValue = 100;
        var result = rng.Next(minValue, maxValue);
        Assert.InRange(result, minValue, maxValue);
    }

    [Fact]
    public void Next_WithMinGreaterThanMax_ShouldThrowArgumentOutOfRangeException()
    {
        using var rng = new SecureRandomNumberGenerator();
        Assert.Throws<ArgumentOutOfRangeException>(() => rng.Next(100, 10));
    }

    [Fact]
    public void NextDouble_ShouldReturnValueBetweenZeroAndOne()
    {
        using var rng = new SecureRandomNumberGenerator();
        var result = rng.NextDouble();
        Assert.InRange(result, 0.0, 1.0);
    }

    [Fact]
    public void GetBytes_ShouldFillArrayWithRandomBytes()
    {
        using var rng = new SecureRandomNumberGenerator();
        var data = new byte[10];
        rng.GetBytes(data);
        Assert.NotEqual(new byte[10], data);
    }

    [Fact]
    public void GetNonZeroBytes_ShouldFillArrayWithNonZeroBytes()
    {
        using var rng = new SecureRandomNumberGenerator();
        var data = new byte[10];
        rng.GetNonZeroBytes(data);
        Assert.DoesNotContain<byte>(0, data);
    }

    [Fact]
    public void Dispose_ShouldAllowMultipleCalls()
    {
        var rng = new SecureRandomNumberGenerator();
        rng.Dispose();
        rng.Dispose(); // Should not throw
    }
}