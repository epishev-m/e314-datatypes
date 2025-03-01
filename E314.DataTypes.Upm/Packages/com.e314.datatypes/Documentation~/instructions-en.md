# E314.DataTypes

## Description

The `E314.DataTypes` module provides structures and data types.

## Content tree

- [E314.DataTypes](#e314datatypes)
  - [Description](#description)
  - [Content tree](#content-tree)
  - [CapacityStrategy](#capacitystrategy)
    - [Recommendations](#recommendations)
    - [Usage Example](#usage-example)
  - [FastList](#fastlist)
    - [Recommendations](#recommendations-1)
    - [Usage Example](#usage-example-1)
  - [Binder](#binder)
    - [Key Features](#key-features)
    - [Recommendations](#recommendations-2)
    - [Usage Example](#usage-example-2)
  - [InstanceProvider](#instanceprovider)
    - [Recommendations](#recommendations-3)
    - [Usage Example](#usage-example-3)
  - [SingletonInstanceProvider](#singletoninstanceprovider)
    - [Recommendations](#recommendations-4)
    - [Usage Example](#usage-example-4)
  - [ListInstanceProvider](#listinstanceprovider)
    - [Recommendations](#recommendations-5)
    - [Usage Example](#usage-example-5)
  - [FactoryInstanceProvider](#factoryinstanceprovider)
    - [Recommendations](#recommendations-6)
    - [Usage Example](#usage-example-6)

## CapacityStrategy

A strategy for managing the capacity of collections.
This class provides a method to calculate capacity based on its current value and the required size.
The new capacity is selected from a predefined array containing numbers close to powers of two minus one `(2^n - 1)`.
These values ensure efficient memory allocation and minimize hash collisions.

### Recommendations

- Use for dynamic collections when you need efficient memory allocation or minimization of hash collisions.
- Estimate the required size in advance to avoid frequent memory reallocations.
- The maximum capacity is limited to 1,048,575. For larger data, use a different strategy.
- If there is a risk of exceeding the maximum capacity, handle the `InvOpException` exception.
- Implement the `ICapacityStrategy` interface if a different strategy implementation is needed.

### Usage Example

``` csharp
var strategy = new CapacityStrategy();
int requiredSize = 100;
int newCapacity = strategy.CalculateCapacity(0, requiredSize);
Console.WriteLine($"New capacity: {newCapacity}");
// New capacity: 127
```

## FastList

A high-performance list implementation.
The class provides methods for fast addition, removal, and access to elements with minimal overhead.
Uses `ArrayPool<T>`, which minimizes memory allocation and reduces pressure on the garbage collector.
Supports capacity management strategy through the `ICapacityStrategy` interface. By default, `CapacityStrategy` is used.
Supports enumeration using `foreach`.

### Recommendations

- Suitable for scenarios where the list size changes frequently.
- If the approximate number of elements is known, initialize the list with an appropriate initial capacity to avoid frequent memory reallocations.
- Ensure that indices are within valid bounds when using methods such as `RemoveAt` or accessing by index.
- Always call the `Dispose` method to return arrays to the pool and release resources.
- If the default strategy does not suit your needs, create a custom implementation of the `ICapacityStrategy` interface.

### Usage Example

``` csharp
var list = new FastList<int>();

list.Add(10);
list.Add(20);
list.Add(30);

Console.WriteLine($"Count: {list.Count}");
// Count: 3

Console.WriteLine($"Element at index 1: {list[1]}"); 
// Element at index 1: 20

bool isRemoved = list.RemoveAt(1);
// true
Console.WriteLine($"Count after removal: {list.Count}");
// Count after removal: 2

list.Clear();
Console.WriteLine($"Count after clear: {list.Count}");
// Count after clear: 0

list.Dispose();
```

## Binder

The `Binder<TKey, TValue>` class manages bindings between keys and values, implementing the `IBinder<TKey, TValue>` interface.
Provides methods for creating, retrieving, and removing bindings.
Supports capacity management strategy through the `ICapacityStrategy` interface.

### Key Features

- Creating bindings: Creates a new binding for a key or returns an existing one.
- Removing bindings: Removes the binding for a specified key and releases associated resources.
- Retrieving bindings: Returns the binding for a key if it exists, or `null` if the key is absent.
- Releasing resources: Implements the `IDisposable` interface to release all resources associated with bindings.

### Recommendations

- Suitable for scenarios where you need to create and remove relationships between objects (e.g., in event systems, dependencies, or data injection).
- If the approximate number of bindings is known, specify the initial capacity during initialization to minimize memory reallocations.
- Ensure that keys are not `null`, as this will throw an `ArgNullException`.
- Ensure that the object has not been disposed (`Dispose`) before performing operations.
- Always call the `Dispose` method to release all bindings and associated resources.
- Methods support chaining (Fluent interface), making the code more readable and compact.

### Usage Example

``` csharp
var capacityStrategy = new CapacityStrategy();
var binder = new Binder<string, object>(10, capacityStrategy);

binder.Bind("Key1").To(new object());
var binding = binder.GetBinding("Key1");
bool isUnbound = binder.Unbind("Key1");

binder.Dispose();
```

## InstanceProvider

`InstanceProvider` is designed to manage a single instance of an object.
It ensures that the instance will be properly disposed of if it implements the `IDisposable` interface.

### Recommendations

- Use when you need to ensure that an object will be correctly disposed of after use.
- Suitable for managing existing objects.
- Ensure that the passed object is not `null`, as this will throw an exception.

### Usage Example

``` csharp
var disposableObject = new DisposableResource();
using var provider = new InstanceProvider(disposableObject);
var instance = provider.GetInstance();
// ...
```

## SingletonInstanceProvider

`SingletonInstanceProvider` ensures the creation and management of a single instance of an object (Singleton pattern).
The instance is created only on the first request and reused in subsequent calls.

### Recommendations

- Use when you need to ensure that only one instance of an object exists in the system.
- Suitable for managing global resources such as configurations, loggers, or services.
- Ensure that the passed instance provider (`IInstanceProvider`) is not `null`.

### Usage Example

``` csharp
var mockProvider = new MockInstanceProvider(new object());
using var singletonProvider = new SingletonInstanceProvider(mockProvider);
var instance1 = singletonProvider.GetInstance();
var instance2 = singletonProvider.GetInstance();
// instance1 and instance2 will be the same object
```

## ListInstanceProvider

`ListInstanceProvider` creates and manages a list of instances, each provided by a separate provider.
The list is created dynamically based on the specified type.

### Recommendations

- Use when you need to combine multiple instances into a collection, such as for managing a list of dependencies or services.
- Ensure that the collection of providers is not empty and does not contain `null`.
- Specify the correct type of list elements to avoid errors during collection creation.

### Usage Example

``` csharp
var providers = new List<IInstanceProvider>
{
    new MockInstanceProvider(new object()),
    new MockInstanceProvider(new object())
};
using var listProvider = new ListInstanceProvider(providers, typeof(object));
var list = (IList)listProvider.GetInstance();
// ...
```

## FactoryInstanceProvider

`FactoryInstanceProvider` manages the creation of instances via a factory.
The factory is initialized only on the first instance request.

### Recommendations

- Use when you need to create instances "on the fly" using a factory.
- Suitable for scenarios where objects are created dynamically, such as depending on the system state.
- Ensure that the factory implements the `IFactory` interface and correctly creates instances.

### Usage Example

``` csharp
var factoryProvider = new MockInstanceProvider(new MockFactory());
using var provider = new FactoryInstanceProvider(factoryProvider);
var instance = provider.GetInstance();
// ...
```
