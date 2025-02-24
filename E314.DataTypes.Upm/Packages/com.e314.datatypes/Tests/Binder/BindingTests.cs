using System;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DataTypes.Tests
{

[TestFixture]
internal sealed class BindingTests
{
	private ICapacityStrategy _capacityStrategy;

	[SetUp]
	public void SetUp()
	{
		_capacityStrategy = new CapacityStrategy();
	}

	[Test]
	public void Constructor_InitializesWithValidParameters()
	{
		// Arrange
		const string key = "TestKey";

		// Act
		var binding = new Binding<string, string>(_capacityStrategy, key);

		// Assert
		Assert.That(binding.Key, Is.EqualTo(key));
		Assert.That(binding.Count, Is.EqualTo(0));
		Assert.That(binding.Values, Is.Empty);
	}

	[Test]
	public void Constructor_ThrowsException()
	{
		// Arrange
		const string key = "TestKey";

		// Act & Assert
		Assert.Throws<ArgNullException>(() => _ = new Binding<string, string>(null, key));
		Assert.Throws<ArgNullException>(() => _ = new Binding<string, string>(_capacityStrategy, null));
	}

	[Test]
	public void To_AddsValueToBinding()
	{
		// Arrange
		const string key = "TestKey";
		const string value = "TestValue";
		var binding = new Binding<string, string>(_capacityStrategy, key);

		// Act
		binding.To(value);

		// Assert
		Assert.That(binding.Count, Is.EqualTo(1));
		Assert.That(binding.Values, Contains.Item(value));
	}

	[Test]
	public void To_ThrowsExceptionForNullValue()
	{
		// Arrange
		const string key = "TestKey";
		var binding = new Binding<string, string>(_capacityStrategy, key);

		// Act & Assert
		Assert.Throws<ArgNullException>(() => binding.To(null));
	}

	[Test]
	public void ClearValues_ClearsAllValues()
	{
		// Arrange
		const string key = "TestKey";
		var binding = new Binding<string, string>(_capacityStrategy, key);
		binding.To("Value1");
		binding.To("Value2");

		// Act
		binding.ClearValues();

		// Assert
		Assert.That(binding.Count, Is.EqualTo(0));
		Assert.That(binding.Values, Is.Empty);
	}

	[Test]
	public void Dispose_ReleasesResources()
	{
		// Arrange
		const string key = "TestKey";
		var disposableValue = new TestDisposable();
		var binding = new Binding<string, TestDisposable>(_capacityStrategy, key);
		binding.To(disposableValue);

		// Act
		binding.Dispose();

		// Assert
		Assert.That(disposableValue.IsDisposed, Is.True);
		Assert.That(binding.Count, Is.EqualTo(0));
	}

	[Test]
	public void UseAfterDispose_ThrowsException()
	{
		// Arrange
		const string key = "TestKey";
		var binding = new Binding<string, string>(_capacityStrategy, key);
		binding.Dispose();

		// Act & Assert
		Assert.Throws<ObjDisposedException>(() => binding.To("Value"));
	}

	[Test]
	public void Key_ReturnsCorrectValue()
	{
		// Arrange
		const string key = "TestKey";
		var binding = new Binding<string, string>(_capacityStrategy, key);

		// Act & Assert
		Assert.That(binding.Key, Is.EqualTo(key));
	}

	[Test]
	public void Count_ReturnsCorrectValueAfterAddingMultipleValues()
	{
		// Arrange
		const string key = "TestKey";
		var binding = new Binding<string, string>(_capacityStrategy, key);

		// Act
		binding.To("Value1");
		binding.To("Value2");
		binding.To("Value3");

		// Assert
		Assert.That(binding.Count, Is.EqualTo(3));
	}

	[Test]
	public void Values_ReturnsCorrectCollection()
	{
		// Arrange
		const string key = "TestKey";
		var binding = new Binding<string, string>(_capacityStrategy, key);

		// Act
		binding.To("Value1");
		binding.To("Value2");

		// Assert
		Assert.That(binding.Values, Is.EquivalentTo(new[] { "Value1", "Value2" }));
	}
}

public class TestDisposable : IDisposable
{
	public bool IsDisposed { get; private set; }

	public void Dispose()
	{
		IsDisposed = true;
		GC.SuppressFinalize(this);
	}
}

}