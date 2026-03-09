namespace Dev.Tests;

public class CommonHelperTests
{
    [Fact]
    public void GenerateRandomDigitCode_WithValidLength_ReturnsStringOfCorrectLength()
    {
        // Arrange
        var length = 6;

        // Act
        var result = CommonHelper.GenerateRandomDigitCode(length);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(length, result.Length);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public void GenerateRandomDigitCode_WithVariousLengths_ReturnsCorrectLength(int length)
    {
        // Act
        var result = CommonHelper.GenerateRandomDigitCode(length);

        // Assert
        Assert.Equal(length, result.Length);
    }

    [Fact]
    public void GenerateRandomDigitCode_WithZeroLength_ReturnsEmptyString()
    {
        // Act
        var result = CommonHelper.GenerateRandomDigitCode(0);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GenerateRandomDigitCode_WithNegativeLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var length = -1;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            CommonHelper.GenerateRandomDigitCode(length));
        Assert.Equal("length", exception.ParamName);
        Assert.Contains("must be non-negative", exception.Message);
    }

    [Fact]
    public void GenerateRandomDigitCode_OnlyContainsDigits()
    {
        // Arrange
        var length = 10;

        // Act
        var result = CommonHelper.GenerateRandomDigitCode(length);

        // Assert
        Assert.Matches(@"^\d+$", result);
    }

    [Fact]
    public void GenerateRandomDigitCode_GeneratesDifferentCodes()
    {
        // Arrange
        var length = 6;
        var codes = new HashSet<string>();

        // Act - Generate 100 codes
        for (int i = 0; i < 100; i++)
        {
            codes.Add(CommonHelper.GenerateRandomDigitCode(length));
        }

        // Assert - Should have generated multiple unique codes (very high probability)
        Assert.True(codes.Count > 1, "Generated codes should be random and different");
    }

    [Fact]
    public void GenerateRandomDigitCode_AllDigitsAreValid()
    {
        // Arrange
        var length = 50;

        // Act
        var result = CommonHelper.GenerateRandomDigitCode(length);

        // Assert
        foreach (char c in result)
        {
            Assert.True(char.IsDigit(c), $"Character '{c}' is not a digit");
            Assert.InRange(c, '0', '9');
        }
    }

    [Fact]
    public void DefaultFileProvider_CanBeSetAndGet()
    {
        // Arrange
        var originalValue = CommonHelper.DefaultFileProvider;

        try
        {
            // Act
            CommonHelper.DefaultFileProvider = null;

            // Assert
            Assert.Null(CommonHelper.DefaultFileProvider);
        }
        finally
        {
            // Cleanup - Restore original value
            CommonHelper.DefaultFileProvider = originalValue;
        }
    }
}
