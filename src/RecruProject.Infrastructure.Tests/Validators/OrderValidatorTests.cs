using AwesomeAssertions;
using RecruProject.Infrastructure.Validators;

namespace RecruProject.Infrastructure.Tests.Validators;

public class OrderValidatorTests
{
    private readonly OrderValidator _orderValidator;

    public OrderValidatorTests()
    {
        _orderValidator = new OrderValidator();
    }
    
    [Fact]
    public void IsValid_ShouldReturnTrue_WhenOrderIdIsGreaterThanZero()
    {
        // Arrange
        
        // Act
        var result = _orderValidator.IsValid(10);

        // Assert
        result.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void IsValid_ShouldReturnFalse_WhenOrderIdIsZeroOrSmallerThanZero(int orderId)
    {
        // Arrange
        
        // Act
        var result = _orderValidator.IsValid(orderId);

        // Assert
        result.Should().BeFalse();
    }
}