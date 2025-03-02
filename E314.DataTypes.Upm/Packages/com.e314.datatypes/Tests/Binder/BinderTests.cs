using System;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DataTypes.Tests
{

[TestFixture]
internal sealed class BinderTests
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
		// Arrange & Act
		var binder = new Binder<string, string>(_capacityStrategy, 10);

		// Assert
		Assert.That(binder, Is.Not.Null);
	}

	[Test]
	public void Constructor_ThrowsExceptionForNullCapacityStrategy()
	{
		// Act & Assert
		Assert.Throws<ArgNullException>(() => _ = new Binder<string, string>(null, 10));
	}

	[Test]
	public void Bind_CreatesNewBindingForKey()
	{
		// Arrange
		const string key = "TestKey";
		var binder = new Binder<string, string>(_capacityStrategy, 10);

		// Act
		var binding = binder.Bind(key);

		// Assert
		Assert.That(binding, Is.Not.Null);
		Assert.That(binding.Key, Is.EqualTo(key));
	}

	[Test]
	public void Bind_ReturnsExistingBindingForKey()
	{
		// Arrange
		const string key = "TestKey";
		var binder = new Binder<string, string>(_capacityStrategy, 10);
		var firstBinding = binder.Bind(key);

		// Act
		var secondBinding = binder.Bind(key);

		// Assert
		Assert.That(secondBinding, Is.SameAs(firstBinding));
	}

	[Test]
	public void Unbind_RemovesBindingForKey()
	{
		// Arrange
		const string key = "TestKey";
		var binder = new Binder<string, string>(_capacityStrategy, 10);
		binder.Bind(key);

		// Act
		var result = binder.Unbind(key);

		// Assert
		Assert.That(result, Is.True);
		Assert.That(binder.GetBinding(key), Is.Null);
	}

	[Test]
	public void Unbind_ReturnsFalseForNonExistentKey()
	{
		// Arrange
		const string key = "NonExistentKey";
		var binder = new Binder<string, string>(_capacityStrategy, 10);

		// Act
		var result = binder.Unbind(key);

		// Assert
		Assert.That(result, Is.False);
	}

	[Test]
	public void GetBinding_ReturnsBindingForExistingKey()
	{
		// Arrange
		const string key = "TestKey";
		var binder = new Binder<string, string>(_capacityStrategy, 10);
		var binding = binder.Bind(key);

		// Act
		var result = binder.GetBinding(key);

		// Assert
		Assert.That(result, Is.SameAs(binding));
	}

	[Test]
	public void GetBinding_ReturnsNullForNonExistentKey()
	{
		// Arrange
		const string key = "NonExistentKey";
		var binder = new Binder<string, string>(_capacityStrategy, 10);

		// Act
		var result = binder.GetBinding(key);

		// Assert
		Assert.That(result, Is.Null);
	}

	[Test]
	public void Dispose_ReleasesAllResources()
	{
		// Arrange
		const string key = "TestKey";
		var binder = new Binder<string, TestDisposable>(_capacityStrategy, 10);
		var disposableValue = new TestDisposable();
		var binding = binder.Bind(key);
		binding.To(disposableValue);

		// Act
		binder.Dispose();

		// Assert
		Assert.That(disposableValue.IsDisposed, Is.True);
		Assert.Throws<ObjDisposedException>(() => _ = binder.GetBinding(key));
	}

	[Test]
	public void UseAfterDispose_ThrowsException()
	{
		// Arrange
		var binder = new Binder<string, string>(_capacityStrategy, 10);
		binder.Dispose();

		// Act & Assert
		Assert.Throws<ObjDisposedException>(() => binder.Bind("TestKey"));
	}

	private sealed class TestDisposable : IDisposable
	{
		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			IsDisposed = true;
		}
	}
}

}