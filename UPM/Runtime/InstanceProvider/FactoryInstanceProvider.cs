using E314.Protect;

namespace E314.DataTypes
{

/// <summary>
/// Represents a provider that manages instances created by a factory.
/// Implements the <see cref="IInstanceProvider"/> interface.
/// </summary>
public sealed class FactoryInstanceProvider : IInstanceProvider
{
	private readonly IInstanceProvider _instanceProvider;
	private IFactory _factory;

	/// <summary>
	/// Initializes a new instance of the <see cref="FactoryInstanceProvider"/> class with the specified instance provider.
	/// </summary>
	/// <param name="instanceProvider">The provider used to retrieve the factory instance. Must not be null.</param>
	/// <exception cref="ArgNullException">Thrown if <paramref name="instanceProvider"/> is null.</exception>
	public FactoryInstanceProvider(IInstanceProvider instanceProvider)
	{
		Requires.NotNull(instanceProvider, nameof(instanceProvider), this);
		_instanceProvider = instanceProvider;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FactoryInstanceProvider"/> class with the specified factory.
	/// </summary>
	/// <param name="factory">The factory used to create instances. Must not be null.</param>
	/// <exception cref="ArgNullException">Thrown if <paramref name="factory"/> is null.</exception>
	public FactoryInstanceProvider(IFactory factory)
	{
		Requires.NotNull(factory, nameof(factory), this);
		_factory = factory;
	}

	/// <summary>
	/// Disposes of the managed instance provider and the factory if they implement <see cref="IDisposable"/>.
	/// </summary>
	public void Dispose()
	{
		_instanceProvider?.Dispose();
		_factory?.Dispose();
	}

	/// <summary>
	/// Retrieves an instance created by the factory.
	/// If the factory has not been initialized yet, it is retrieved from the instance provider.
	/// </summary>
	/// <returns>An instance created by the factory.</returns>
	public object GetInstance()
	{
		_factory ??= (IFactory) _instanceProvider.GetInstance();
		var instance = _factory.Create();
		return instance;
	}
}

}