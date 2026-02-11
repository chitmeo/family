
using System.Runtime.Serialization;

using Dev.Exceptions;

namespace Dev.Tests.Exceptions;

public class EnumExtensionsTest
{
    private enum TestEnum
    {
        [EnumMember(Value = "custom_value")]
        WithAttribute,

        WithoutAttribute,

        [EnumMember(Value = "another_custom")]
        AnotherWithAttribute
    }

    [Fact]
    public void GetEnumMemberValue_WithEnumMemberAttribute_ReturnsAttributeValue()
    {
        // Arrange
        var enumValue = TestEnum.WithAttribute;

        // Act
        var result = enumValue.GetEnumMemberValue();

        // Assert
        Assert.Equal("custom_value", result);
    }

    [Fact]
    public void GetEnumMemberValue_WithoutEnumMemberAttribute_ReturnsEnumName()
    {
        // Arrange
        var enumValue = TestEnum.WithoutAttribute;

        // Act
        var result = enumValue.GetEnumMemberValue();

        // Assert
        Assert.Equal("WithoutAttribute", result);
    }

    [Fact]
    public void GetEnumMemberValue_AnotherAttributeValue_ReturnsCorrectValue()
    {
        // Arrange
        var enumValue = TestEnum.AnotherWithAttribute;

        // Act
        var result = enumValue.GetEnumMemberValue();

        // Assert
        Assert.Equal("another_custom", result);
    }
}