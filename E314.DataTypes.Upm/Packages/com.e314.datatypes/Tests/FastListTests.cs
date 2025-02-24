using System.Collections.Generic;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DataTypes.Tests
{

[TestFixture]
internal sealed class FastListTests
{
	private FastList<int> _list;

	[SetUp]
	public void SetUp()
	{
		_list = new FastList<int>(capacity: 4);
	}

	[TearDown]
	public void TearDown()
	{
		_list.Dispose();
	}

	[Test]
	public void Add_AddsElementsToList()
	{
		// Arrange
		const int expectedCount = 3;

		// Act
		_list.Add(10);
		_list.Add(20);
		_list.Add(30);

		// Assert
		Assert.That(_list.Count, Is.EqualTo(expectedCount));
		Assert.That(_list[0], Is.EqualTo(10));
		Assert.That(_list[1], Is.EqualTo(20));
		Assert.That(_list[2], Is.EqualTo(30));
	}

	[Test]
	public void Add_IncreasesCapacityWhenNeeded()
	{
		// Arrange
		const int elementsToAdd = 5;

		// Act
		for (var i = 0; i < elementsToAdd; i++) _list.Add(i);

		// Assert
		Assert.That(_list.Count, Is.EqualTo(elementsToAdd));
		Assert.That(_list.Capacity, Is.GreaterThanOrEqualTo(elementsToAdd));
	}

	[Test]
	public void RemoveAt_RemovesElementAtIndex()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);
		_list.Add(30);

		// Act
		bool result = _list.RemoveAt(1);

		// Assert
		Assert.That(result, Is.True);
		Assert.That(_list.Count, Is.EqualTo(2));
		Assert.That(_list[0], Is.EqualTo(10));
		Assert.That(_list[1], Is.EqualTo(30));
	}

	[Test]
	public void RemoveAt_ReturnsFalseForInvalidIndex()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);

		// Act
		bool result = _list.RemoveAt(5);

		// Assert
		Assert.That(result, Is.False);
		Assert.That(_list.Count, Is.EqualTo(2));
	}

	[Test]
	public void Indexer_AccessesElementsByIndex()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);

		// Act & Assert
		Assert.That(_list[0], Is.EqualTo(10));
		Assert.That(_list[1], Is.EqualTo(20));
	}

	[Test]
	public void Indexer_SetsValueAtIndex()
	{
		// Arrange
		_list.Add(10);

		// Act
		_list[0] = 20;

		// Assert
		Assert.That(_list[0], Is.EqualTo(20));
	}

	[Test]
	public void Clear_ClearsAllElements()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);

		// Act
		_list.Clear();

		// Assert
		Assert.That(_list.Count, Is.EqualTo(0));
	}

	[Test]
	public void GetEnumerator_EnumeratesElements()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);
		_list.Add(30);

		// Act
		var elements = new List<int>();
		foreach (int item in _list) elements.Add(item);

		// Assert
		Assert.That(elements, Is.EquivalentTo(new[] { 10, 20, 30 }));
	}

	[Test]
	public void Constructor_ThrowsExceptionForInvalidCapacity()
	{
		// Arrange & Act & Assert
		Assert.Throws<ArgOutOfRangeException>(() => _ = new FastList<int>(capacity: 0));
	}

	[Test]
	public void Dispose_ReleasesResources()
	{
		// Arrange
		_list.Add(10);

		// Act
		_list.Dispose();

		// Assert
		Assert.That(_list.Count, Is.EqualTo(0));
		Assert.That(_list.Capacity, Is.EqualTo(0));
	}
}

}