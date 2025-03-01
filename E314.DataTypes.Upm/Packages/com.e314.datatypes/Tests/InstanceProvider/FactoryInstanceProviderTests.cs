using System;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DataTypes.Tests
{

[TestFixture]
internal sealed class FactoryInstanceProviderTests
{
	[Test]
	public void Constructor_ThrowsException_WhenInstanceProviderIsNull()
	{
		// Act & Assert
		Assert.Throws<ArgNullException>(() => _ = new FactoryInstanceProvider(null));
	}

	[Test]
	public void GetInstance_ReturnsInstanceFromFactory()
	{
		// Arrange
		var mockFactory = new MockFactory();
		var mockProvider = new MockInstanceProvider(mockFactory);
		var provider = new FactoryInstanceProvider(mockProvider);

		// Act
		var instance = provider.GetInstance();

		// Assert
		Assert.That(instance, Is.Not.Null);
		Assert.That(instance, Is.TypeOf<object>());
	}

	[Test]
	public void GetInstance_InitializesFactoryOnlyOnce()
	{
		// Arrange
		var mockFactory = new MockFactory();
		var mockProvider = new MockInstanceProvider(mockFactory);
		var provider = new FactoryInstanceProvider(mockProvider);

		// Act
		provider.GetInstance();
		provider.GetInstance();

		// Assert
		Assert.That(mockProvider.CreateCalled, Is.EqualTo(1));
	}

	[Test]
	public void Dispose_CallsDisposeOnInstanceProvider_WhenInstanceProviderIsDisposable()
	{
		// Arrange
		var mockFactory = new MockFactory();
		var disposableProvider = new MockInstanceProvider(mockFactory);
		var provider = new FactoryInstanceProvider(disposableProvider);

		// Act
		provider.GetInstance();
		provider.Dispose();

		// Assert
		Assert.That(disposableProvider.IsDisposed, Is.True);
	}

	[Test]
	public void Dispose_CallsDisposeOnFactory_WhenFactoryIsDisposable()
	{
		// Arrange
		var mockFactory = new MockFactory();
		var mockProvider = new MockInstanceProvider(mockFactory);
		var provider = new FactoryInstanceProvider(mockProvider);

		// Act
		provider.GetInstance(); // Инициализация фабрики
		provider.Dispose();

		// Assert
		Assert.That(mockFactory.IsDisposed, Is.True);
	}

	#region Nested

	private sealed class MockInstanceProvider : IInstanceProvider
	{
		private readonly IFactory _factory;

		public MockInstanceProvider(IFactory factory)
		{
			_factory = factory;
		}

		public int CreateCalled { get; private set; }

		public bool IsDisposed { get; private set; }

		public object GetInstance()
		{
			CreateCalled++;
			return _factory;
		}

		public void Dispose()
		{
			IsDisposed = true;
		}
	}

	private sealed class MockFactory : IFactory
	{
		public bool IsDisposed { get; private set; }

		public object Create()
		{
			return new object();
		}

		public void Dispose()
		{
			IsDisposed = true;
		}
	}

	#endregion
}

}