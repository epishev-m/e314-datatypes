using System;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DataTypes.Tests
{

[TestFixture]
internal sealed class SingletonInstanceProviderTests
{
	[Test]
	public void Constructor_ThrowsException_WhenProvidersIsNull()
	{
		// Act & Assert
		Assert.Throws<ArgNullException>(() => _ = new SingletonInstanceProvider(null));
	}

	[Test]
	public void GetInstance_ReturnsSameInstance_OnMultipleCalls()
	{
		// Arrange
		var mockProvider = new MockInstanceProvider();
		var provider = new SingletonInstanceProvider(mockProvider);

		// Act
		var instance1 = provider.GetInstance();
		var instance2 = provider.GetInstance();

		// Assert
		Assert.That(instance1, Is.SameAs(instance2));
	}

	[Test]
	public void GetInstance_CreatesInstanceUsingInstanceProvider()
	{
		// Arrange
		var mockProvider = new MockInstanceProvider();
		var provider = new SingletonInstanceProvider(mockProvider);

		// Act
		var instance = provider.GetInstance();

		// Assert
		Assert.That(instance, Is.Not.Null);
		Assert.That(mockProvider.CreateCalled, Is.True);
	}

	[Test]
	public void Dispose_CallsDisposeOnInstance_WhenInstanceIsDisposable()
	{
		// Arrange
		var disposableMock = new DisposableMock();
		var mockProvider = new MockInstanceProvider(disposableMock);
		var provider = new SingletonInstanceProvider(mockProvider);

		// Act
		var instance = provider.GetInstance();
		provider.Dispose();

		// Assert
		Assert.That(instance, Is.EqualTo(disposableMock));
		Assert.That(disposableMock.IsDisposed, Is.True);
	}

	[Test]
	public void Dispose_CallsDisposeOnInstanceProvider_WhenProviderIsDisposable()
	{
		// Arrange
		var disposableProvider = new DisposableMock();
		var mockProvider = new MockInstanceProvider(disposableProvider);
		var provider = new SingletonInstanceProvider(mockProvider);

		// Act
		provider.Dispose();

		// Assert
		Assert.That(mockProvider.IsDisposed, Is.True);
	}

	[Test]
	public void Dispose_DoesNotThrow_WhenInstanceIsNotDisposable()
	{
		// Arrange
		var mockProvider = new MockInstanceProvider(new object());
		var provider = new SingletonInstanceProvider(mockProvider);

		// Act & Assert
		Assert.DoesNotThrow(() => provider.Dispose());
	}

	#region Nested

	private sealed class MockInstanceProvider : IInstanceProvider
	{
		private readonly object _instance;

		public MockInstanceProvider(object instance = null)
		{
			_instance = instance ?? new object();
		}

		public bool CreateCalled { get; private set; }

		public bool IsDisposed { get; private set; }

		public object GetInstance()
		{
			CreateCalled = true;
			return _instance;
		}

		public void Dispose()
		{
			IsDisposed = true;
			if (_instance is IDisposable disposable) disposable.Dispose();
		}
	}

	private sealed class DisposableMock : IDisposable
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