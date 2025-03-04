using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using E314.Protect;

namespace E314.DataTypes
{

/// <summary>
/// A class that analyzes types. Implements the <see cref="ITypeAnalyzer"/> interface.
/// </summary>
public sealed class TypeAnalyzer : ITypeAnalyzer
{
	/// <summary>
	/// Cache for storing previously analyzed types and their results.
	/// </summary>
	private static readonly Dictionary<Type, TypeAnalysisResult> Cache = new(100);

	/// <summary>
	/// Analyzes the specified type based on the provided flags.
	/// </summary>
	/// <param name="type">The type to analyze.</param>
	/// <param name="flags">Flags indicating which members of the type to analyze.</param>
	/// <returns>A result object containing the analyzed members of the type.</returns>
	public TypeAnalysisResult Analyze(Type type, TypeAnalysisFlags flags = TypeAnalysisFlags.All)
	{
		Requires.NotNull(type, nameof(type));

		if (!Cache.TryGetValue(type, out var result))
		{
			result = new TypeAnalysisResult(type);
			Cache[type] = result;
		}

		result.Update(flags);
		return result;
	}

	public void ClearCache()
	{
		Cache.Clear();
	}

	/// <summary>
	/// Retrieves the constructors of the specified type.
	/// </summary>
	/// <param name="type">The type to retrieve constructors from.</param>
	/// <returns>A read-only list of constructors declared by the type.</returns>
	internal static IReadOnlyList<ConstructorInfo> GetConstructors(Type type)
	{
		const BindingFlags bindingFlags = BindingFlags.Public | 
										BindingFlags.Instance | 
										BindingFlags.Static | 
										BindingFlags.DeclaredOnly;
		return type.GetConstructors(bindingFlags);
	}

	/// <summary>
	/// Retrieves the methods of the specified type.
	/// </summary>
	/// <param name="type">The type to retrieve methods from.</param>
	/// <returns>A read-only list of methods declared by the type, excluding special-name methods.</returns>
	internal static IReadOnlyList<MethodInfo> GetMethods(Type type)
	{
		const BindingFlags bindingFlags = BindingFlags.Public | 
										BindingFlags.Instance | 
										BindingFlags.Static | 
										BindingFlags.DeclaredOnly;
		return type.GetMethods(bindingFlags).Where(m => !m.IsSpecialName).ToList();
	}

	/// <summary>
	/// Retrieves the properties of the specified type.
	/// </summary>
	/// <param name="type">The type to retrieve properties from.</param>
	/// <returns>A read-only list of properties declared by the type.</returns>
	internal static IReadOnlyList<PropertyInfo> GetProperties(Type type)
	{
		const BindingFlags bindingFlags = BindingFlags.Public | 
										BindingFlags.Instance | 
										BindingFlags.Static | 
										BindingFlags.DeclaredOnly;
		return type.GetProperties(bindingFlags).ToList();
	}

	/// <summary>
	/// Retrieves the fields of the specified type.
	/// </summary>
	/// <param name="type">The type to retrieve fields from.</param>
	/// <returns>A read-only list of fields declared by the type.</returns>
	internal static IReadOnlyList<FieldInfo> GetFields(Type type)
	{
		const BindingFlags bindingFlags = BindingFlags.Public | 
										BindingFlags.Instance | 
										BindingFlags.Static | 
										BindingFlags.DeclaredOnly;
		return type.GetFields(bindingFlags).ToList();
	}
}

}