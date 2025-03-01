using System;
using System.Collections;
using System.Collections.Generic;
using E314.Protect;

namespace E314.DataTypes
{

/// <summary>
/// Represents a provider that creates and manages a list of instances from multiple instance providers.
/// Implements the <see cref="IInstanceProvider"/> interface.
/// </summary>
public sealed class ListInstanceProvider : IInstanceProvider
{
	private readonly IReadOnlyCollection<IInstanceProvider> _providers;
	private readonly Type _type;

	/// <summary>
	/// Initializes a new instance of the <see cref="ListInstanceProvider"/> class with the specified providers and type.
	/// </summary>
	/// <param name="providers">A collection of instance providers used to create the list. Must not be null or empty, and must not contain null elements.</param>
	/// <param name="type">The type of elements in the list. Must not be null.</param>
	/// <exception cref="ArgNullException">Thrown if <paramref name="providers"/> or <paramref name="type"/> is null.</exception>
	/// <exception cref="ArgException">Thrown if <paramref name="providers"/> is empty or contains null elements.</exception>
	public ListInstanceProvider(IReadOnlyCollection<IInstanceProvider> providers, Type type)
	{
		Requires.NotNull(providers, nameof(providers));
		Requires.NotEmpty(providers, nameof(providers));
		Requires.NoNullElements(providers, nameof(providers));
		Requires.NotNull(type, nameof(type));
		_providers = providers;
		_type = type;
	}

	/// <summary>
	/// Disposes of all managed instance providers.
	/// </summary>
	public void Dispose()
	{
		foreach (var instanceProvider in _providers) instanceProvider.Dispose();
	}

	/// <summary>
	/// Creates and returns a list of instances provided by the underlying instance providers.
	/// </summary>
	/// <returns>A list of instances of the specified type.</returns>
	/// <remarks>
	/// The returned list is dynamically created using reflection based on the specified type.
	/// Each instance provider in the collection contributes one element to the list.
	/// </remarks>
	public object GetInstance()
	{
		var genericType = typeof(List<>).MakeGenericType(_type);
		var resultList = (IList) Activator.CreateInstance(genericType, _providers.Count);

		foreach (var instanceProvider in _providers)
		{
			var instance = instanceProvider.GetInstance();
			resultList!.Add(instance);
		}

		return resultList;
	}
}

}