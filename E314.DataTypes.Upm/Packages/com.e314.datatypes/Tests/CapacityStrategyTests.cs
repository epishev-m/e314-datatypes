using E314.Exceptions;
using NUnit.Framework;

namespace E314.DataTypes.Tests
{

[TestFixture]
public class CapacityStrategyTests
{
	private readonly CapacityStrategy _strategy = new();

	[Test]
	public void CalculateCapacity_WhenCurrentCapacityIsSufficient_ReturnsCurrentCapacity()
	{
		// Arrange
		const int currentCapacity = 100;
		const int requiredSize = 50;

		// Act
		int newCapacity = _strategy.CalculateCapacity(currentCapacity, requiredSize);

		// Assert
		Assert.That(newCapacity, Is.EqualTo(currentCapacity));
	}

	[Test]
	public void
		CalculateCapacity_WhenRequiredSizeExceedsCurrentCapacity_ReturnsNextCapacityFromArray()
	{
		// Arrange
		const int currentCapacity = 10;
		const int requiredSize = 20;

		// Act
		int newCapacity = _strategy.CalculateCapacity(currentCapacity, requiredSize);

		// Assert
		Assert.That(newCapacity, Is.EqualTo(31));
	}

	[Test]
	public void CalculateCapacity_WhenRequiredSizeMatchesArrayValue_ReturnsExactMatch()
	{
		// Arrange
		const int currentCapacity = 10;
		const int requiredSize = 63;

		// Act
		int newCapacity = _strategy.CalculateCapacity(currentCapacity, requiredSize);

		// Assert
		Assert.That(newCapacity, Is.EqualTo(63));
	}

	[Test]
	public void CalculateCapacity_WhenRequiredSizeExceedsMaximumCapacity_ThrowsInvOpException()
	{
		// Arrange
		const int currentCapacity = 1_000_000;
		const int requiredSize = 2_000_000;

		// Act & Assert
		var ex = Assert.Throws<InvOpException>(() =>
			_strategy.CalculateCapacity(currentCapacity, requiredSize));
		Assert.That(ex!.Message, Does.Contain("Required capacity is too large."));
		Assert.That(ex.ErrorData.ToString(), Does
			.Contain($"CurrentCapacity = {currentCapacity}")
			.And.Contain($"RequiredSize = {requiredSize}"));
	}

	[Test]
	public void CalculateCapacity_WhenRequiredSizeIsMinimum_ReturnsSmallestCapacity()
	{
		// Arrange
		const int currentCapacity = 0;
		const int requiredSize = 1;

		// Act
		int newCapacity = _strategy.CalculateCapacity(currentCapacity, requiredSize);

		// Assert
		Assert.That(newCapacity, Is.EqualTo(3));
	}

	[Test]
	public void CalculateCapacity_WhenRequiredSizeIsMaximum_ReturnsLargestCapacity()
	{
		// Arrange
		const int currentCapacity = 1_000_000;
		const int requiredSize = 1_048_575;

		// Act
		int newCapacity = _strategy.CalculateCapacity(currentCapacity, requiredSize);

		// Assert
		Assert.That(newCapacity, Is.EqualTo(1_048_575));
	}
}

}