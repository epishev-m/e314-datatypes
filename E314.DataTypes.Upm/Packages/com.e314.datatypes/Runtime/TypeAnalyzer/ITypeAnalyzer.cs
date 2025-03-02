using System;

namespace E314.DataTypes
{

/// <summary>
/// Interface for analyzing types. Provides a method to analyze a type with specified flags.
/// </summary>
public interface ITypeAnalyzer
{
	/// <summary>
	/// Analyzes the specified type based on the provided flags.
	/// </summary>
	/// <param name="type">The type to analyze.</param>
	/// <param name="flags">Flags indicating which members of the type to analyze.</param>
	/// <returns>A result object containing the analyzed members of the type.</returns>
	TypeAnalysisResult Analyze(Type type, TypeAnalysisFlags flags = TypeAnalysisFlags.All);

	void ClearCache();
}

}