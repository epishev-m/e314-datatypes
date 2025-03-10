using System;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DataTypes.Tests
{

[TestFixture]
public class FactoryTests
{
	[Test]
	public void Create_WithValidFunction_ReturnsObject()
	{
		// Arrange
		var expectedObject = new object();
		var factory = new Factory(() => expectedObject);

		// Act
		var result = factory.Create();

		// Assert
		Assert.That(result, Is.SameAs(expectedObject));
	}

	[Test]
	public void Create_WithStringFunction_ReturnsString()
	{
		// Arrange
		const string expectedString = "Test String";
		var factory = new Factory(() => expectedString);

		// Act
		var result = factory.Create();

		// Assert
		Assert.That(result, Is.EqualTo(expectedString));
	}

	[Test]
	public void Constructor_WithNullFunction_ThrowsArgNullException()
	{
		// Arrange & Act & Assert
		Assert.Throws<ArgNullException>(() => _ = new Factory(null));
	}

	[Test]
	public void Create_WithComplexObject_ReturnsInitializedObject()
	{
		// Arrange
		var factory = new Factory(() =>
		{
			var obj = new ComplexTestObject();
			obj.Initialize();
			return obj;
		});

		// Act
		var result = (ComplexTestObject)factory.Create();

		// Assert
		Assert.That(result.IsInitialized, Is.True);
	}

	[Test]
	public void Dispose_WithDisposableObject_DisposesCorrectly()
	{
		// Arrange
		var disposableObject = new DisposableTestObject();
		var factory = new Factory(() => disposableObject);

		// Act
		factory.Dispose();

		// Assert
		// In this case Dispose should not call Dispose on created objects,
		// because Factory only creates objects and does not manage their lifecycle
		Assert.That(disposableObject.IsDisposed, Is.False);
	}

	[Test]
	public void Create_CalledMultipleTimes_CreatesNewInstances()
	{
		// Arrange
		var factory = new Factory(() => new object());

		// Act
		var result1 = factory.Create();
		var result2 = factory.Create();

		// Assert
		Assert.That(result1, Is.Not.SameAs(result2));
	}

	#region Nested

	private class ComplexTestObject
	{
		public bool IsInitialized { get; private set; }

		public void Initialize()
		{
			IsInitialized = true;
		}
	}

	private class DisposableTestObject : IDisposable
	{
		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			IsDisposed = true;
		}
	}

	#endregion
}

}