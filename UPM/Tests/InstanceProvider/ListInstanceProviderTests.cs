using System.Collections;
using System.Collections.Generic;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DataTypes.Tests
{

[TestFixture]
internal sealed class ListInstanceProviderTests
{
	[Test]
	public void Constructor_ThrowsException_WhenProvidersIsNull()
	{
		// Arrange
		// ReSharper disable once CollectionNeverUpdated.Local
		var emptyProviders = new List<IInstanceProvider>();
		var providers = new List<IInstanceProvider> { null };
		var mockProvider = new MockInstanceProvider();

		// Act & Assert
		Assert.Throws<ArgNullException>(() => _ = new ListInstanceProvider(null, typeof(string)));
		Assert.Throws<ArgException>(() => _ = new ListInstanceProvider(emptyProviders, typeof(string)));
		Assert.Throws<ArgException>(() => _ = new ListInstanceProvider(providers, typeof(string)));
		Assert.Throws<ArgNullException>(() => _ = new ListInstanceProvider(new[] { mockProvider }, null));
	}

	[Test]
	public void GetInstance_ReturnsListOfCorrectTypeAndSize()
	{
		// Arrange
		var mockProvider1 = new MockInstanceProvider(new object());
		var mockProvider2 = new MockInstanceProvider(new object());
		var providers = new List<IInstanceProvider> { mockProvider1, mockProvider2 };
		var provider = new ListInstanceProvider(providers, typeof(object));

		// Act
		var result = provider.GetInstance();

		// Assert
		Assert.That(result, Is.InstanceOf(typeof(IList)));
		var resultList = (IList)result;
		Assert.That(resultList.Count, Is.EqualTo(2));
	}

	[Test]
	public void GetInstance_ReturnsInstancesFromProviders()
	{
		// Arrange
		var instance1 = new object();
		var instance2 = new object();
		var mockProvider1 = new MockInstanceProvider(instance1);
		var mockProvider2 = new MockInstanceProvider(instance2);
		var providers = new List<IInstanceProvider> { mockProvider1, mockProvider2 };
		var provider = new ListInstanceProvider(providers, typeof(object));

		// Act
		var result = provider.GetInstance();

		// Assert
		var resultList = (IList)result;
		Assert.That(resultList[0], Is.SameAs(instance1));
		Assert.That(resultList[1], Is.SameAs(instance2));
	}

	[Test]
	public void Dispose_CallsDisposeOnAllProviders()
	{
		// Arrange
		var disposableProvider1 = new MockInstanceProvider();
		var disposableProvider2 = new MockInstanceProvider();
		var providers = new List<IInstanceProvider> { disposableProvider1, disposableProvider2 };
		var provider = new ListInstanceProvider(providers, typeof(object));

		// Act
		provider.Dispose();

		// Assert
		Assert.That(disposableProvider1.IsDisposed, Is.True);
		Assert.That(disposableProvider2.IsDisposed, Is.True);
	}

	#region Nested

	private sealed class MockInstanceProvider : IInstanceProvider
	{
		private readonly object _instance;

		public MockInstanceProvider(object instance = null)
		{
			_instance = instance ?? new object();
		}

		public bool IsDisposed { get; private set; }

		public object GetInstance()
		{
			return _instance;
		}

		public void Dispose()
		{
			IsDisposed = true;
		}
	}

	#endregion
}

}