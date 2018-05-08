# Tiger.Types

## What It Is

Tiger.Types is a library of useful types for C#, ones that are sometimes included by default in other languages. These types enable and include advanced operations that encapsulate boilerplate logic. These include, but are not limited to:

- The type `Option<TSome>`, which represents the concept of “a value” or “no value” in a way that is more type-safe than returning `null`. This maps to failable operations where failure delivers no specific information.
- The type `Either<TLeft, TRight>`, which represents the concept of “a value” or “a different value” in a way that is more type-safe than always throwing an exception. This maps to operations that can return a value upon success, or a detailed error upon failure.
- Advanced operations on `Task<T>`, which allows transformation of values while remaining within the `Task<T>` context.

## Why You Want It

These types and operations allow you to treat more operations in your .NET application as [functors] or [monads], which frequently represent operations in more type-safe ways than .NET conventions.

[functors]: https://en.wikipedia.org/wiki/Map_\(higher-order_function\)#Generalization
[monads]: https://en.wikipedia.org/wiki/Monad_\(functional_programming\)#Motivating_examples

Let’s use `Option<TSome>` for an example. In the following code, we’ll write a short method that converts a string to all upper-case.

```csharp
public string ToAllUpperCase(string input)
{
  return input.ToUpper();
}
```

(It is a trivial example, since the capability is built into the `string` type, but it will do.)

There is already a somewhat major error that could occur: If `input` is `null`, then the method will throw `NullReferenceException`. This is a bug, and the type system did nothing to help us detect it. Our method can accept any `string` value *and* the kind-of-value `null`. We can check for `null`, and throw `ArgumentNullException`, but let’s say that we need to allow the concept of “no value” into the method.

```csharp
public string ToAllUpperCase(string input)
{
  if (input == null)
  {
    return null;
  }
  return input.ToUpper();
}
```

This looks better, but is even worse! Our callers still believe that we can only return actual `string` values, but we can also return `null`. Now we are checking for `null`, *and* our callers must check, as well. This is where `Option<TSome>` comes in. Similar to the .NET concept of `Nullable<T>` (also written as `T?`), `Option<TSome>` allows us to explicitly advertise the concept of “no value” to our callers.

The pattern that emerges is this: If we get no value, return no value. If we get a value, we process it. This pattern is built into the type, and it is called `Map`. That allows us to write the original verison of `ToAllUpperCase` defined above, the one that operates only on values, and call it like this:

```csharp
potentialString.Map(s => ToAllUpperCase(s));
```

(Of course, our caller could also write `potentialString.Map(s => s.ToUpper())`, but then we’re out of our jobs!)

Here, the type of `potentialString` is explicitly `Option<string>`, and the return type of `Map` is explicitly `Option<string>`. This is almost identical to `Select` on `IEnumerable<T>`: If the sequence is empty, we get back an empty sequence. If the sequence has elements, the elements are transformed. For `Option<TSome>` the “empty” state is called <i>None</i>, and the “has elements” state is called <i>Some</i>. There are many such useful operations on optional values. Here’s an abridged list:

- `Map`: Given a value of `Func<TSome, U>`, returns an `Option<U>` in the same state as the input value. <small>Aliased to `Select`, from the BCL.</small>

- `Bind`: Given a value of `Func<TSome, Option<U>>`, calculates an `Option<Option<U>>` and flattens it to `Option<U>` before returning. <small>Aliased to `SelectMany`, from the BCL.</small>

- `Match` <small>(Value-Returning)</small>: Associates a value of `Func<U>` with the <i>None</i> state and a value of `Func<TSome, U>` with the <i>Some</i> state, and invokes the function that matches the input value’s state.

- `Match` <small>(Action-Performing)</small>: Associates a value of `Action` with the <i>None</i> state and a value of `Action<TSome>` with the <i>Some</i> state, and invokes the function that matches the input value’s state.

- `Filter`: Given a predicate value of `Func<TSome, bool>`, returns the original value if it is in the <i>Some</i> state and its <i>Some</i> value passes the predicate; otherwise, returns an `Option<TSome>` in the <i>None</i> state. <small>Aliased to `Where`, from the BCL.</small>

- `Let`: Given a value of `Action<TSome>`, performs that action if the original value is in the <i>Some</i> state. <small>Aliased to `ForEach`, from the Interactive Extensions.</small>

- `Tap`: Given a value of `Action<TSome>`, performs that action if the original value is in the <i>Some</i> state, then returns the original value – most useful for chained methods. <small>Aliased to `Do`, from the Interactive Extensions.</small>

### A Note on Aliases

Many of the methods on these types are aliased to LINQ-standard names. This is for reasons of developer familiarity and activating certain C# features. For example, implementing `Select`, `SelectMany`, and `Where` allows the LINQ query syntax to be used. Using `Option<TSome>` again:

```csharp
var left = Option.From(3); // Some(3)
var right = Option.From(4); // Some(4)

var sum = from l in left
          from r in right
          select l + r; // Some(7)
```

However, if either of the input values is in the <i>None</i> state, the operation fails.

```csharp
var left = Option<int>.None; // None
var right = Option.From(4); // Some(4)

var sum = from l in left
          from r in right
          select l + r; // None
```

Additionally, implementing `GetEnumerator` allows an `Option<TSome>` to be used with the `foreach` statement, which will execute its body only if the optional value is in the <i>Some</i> state.

```csharp
foreach (var value in Option.From("world"))
{
  Console.WriteLine($"Hello, {value}!"); // Hello, world!
}

foreach (var value in Option<string>.None)
{
  Console.WriteLine($"Hello, {value}!"); // <not executed>
}
```

This result can also be accomplished with the `Let` operation.

### A Note on `null`

Most of this library is allergic to `null`. It advertises where `null` is allowed, and where it is not – heavily tilted to the latter. If returning `null` from a passed function would violate the semantics of an operation, then that operation will throw an uncatchable exception. For example, the contract of `Map` is that it will only return an optional value in the <i>None</i> state if the original value is in the <i>None</i> state. However, if returning `null` from the passed function were allowed, that would put the returned value from an original value in the <i>Some</i> state into the <i>None</i> state. This should be refactored into a method of type `Func<TSome, Option<U>>`, and used with the `Bind` operation.

## How You Develop It

This project is using the standard [`dotnet`] build tool. A brief primer:

[`dotnet`]: https://dot.net

- Restore NuGet dependencies: `dotnet restore`
- Build the entire solution: `dotnet build`
- Run all unit tests: `dotnet test`
- Pack for publishing: `dotnet pack -o "$(pwd)/artifacts"`

The parameter `--configuration` (shortname `-c`) can be supplied to the `build`, `test`, and `pack` steps with the following meaningful values:

- “Debug” (the default)
- “Release”

This repository is attempting to use the [GitFlow] branching methodology. Results may be mixed, please be aware.

[GitFlow]: http://jeffkreeftmeijer.com/2010/why-arent-you-using-git-flow/

## Thank You

Seriously, though. Thank you for using this software. The author hopes it performs admirably for you.
