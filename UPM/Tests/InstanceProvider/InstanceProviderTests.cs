using System;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DataTypes.Tests
{

[TestFixture]
internal sealed class InstanceProviderTests
{
	[Test]
	public void Constructor_ThrowsException_WhenInstanceIsNull()
	{
		// Act & Assert
		Assert.Throws<ArgNullException>(() => _ = new InstanceProvider(null));
	}

	[Test]
	public void Constructor_CreatesInstanceProvider_WhenInstanceIsNotNull()
	{
		// Arrange
		var instance = new object();

		// Act
		var provider = new InstanceProvider(instance);

		// Assert
		Assert.That(provider, Is.Not.Null);
	}

	[Test]
	public void GetInstance_ReturnsCorrectInstance()
	{
		// Arrange
		var instance = new object();
		var provider = new InstanceProvider(instance);

		// Act
		var result = provider.GetInstance();

		// Assert
		Assert.That(result, Is.SameAs(instance));
	}

	[Test]
	public void Dispose_CallsDisposeOnInstance_WhenInstanceIsDisposable()
	{
		// Arrange
		var disposableMock = new DisposableMock();
		var provider = new InstanceProvider(disposableMock);

		// Act
		provider.Dispose();

		// Assert
		Assert.That(disposableMock.IsDisposed, Is.True);
	}

	[Test]
	public void Dispose_DoesNotThrow_WhenInstanceIsNotDisposable()
	{
		// Arrange
		var instance = new object();
		var provider = new InstanceProvider(instance);

		// Act & Assert
		Assert.DoesNotThrow(() => provider.Dispose());
	}

	#region Nested

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