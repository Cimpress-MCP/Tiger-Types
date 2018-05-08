### What's new in 5.0.0 (Released 2018-05-08)

* The library has been refactored completely, consuming much of the functionality of the deprecated Tiger.ErrorTypes.
* See [the wiki][] for migration details.
* The library now supports multiple target frameworks.
* Relevant types have been marked as `readonly`, increasing performance in some scenarios.
  * Accordingly, many methods have been annotated with `in`.
* The `Error` type has been promoted from Tiger.ErrorTypes.
* The `Try<TErr, TOk>` type has arrived, providing the functionality of an optional either.

[the wiki]: https://github.com/Cimpress-MCP/Tiger-Types/wiki/Migration-to-5.0

### What's new in 4.0.0 (Released 2017-04-10)

* Types that obey the monad laws have grown a `Join` method.
* The `Bind` operation on many types has been made more efficient.
* Asynchronous operations have been made significantly more efficient.
* Collections have learned some new tricks, such as folding asynchronously.
* Equality operations have been made slightly more efficient.
* Many extension methods have been moved into a more easily imported namespace.
* The project has moved to the latest versions of tools in the .NET ecosystem, and refactorings have been made accordingly.

### What's new in 3.1.0 (Released 2016-11-21)

* The `Split` method of `Either` has learned a few convenient overloads.

### What's new in 3.0.0 (Released 2016-11-14)

* Thanks to the magic of the .NET Standard, Tiger libraries are available to a wider set of platforms than ever before. The project now targets .NET Standard 1.0.
* The `Either` static class has learned from `Option.None` and can indirectly create `Either` instances without specifying type parameters.
* The main types of the library have had their struct layouts specified, leading to reduced memory usage when boxed.
* LINQPad's custom dumping has been upgraded to the new method, leading to improved display and debugging.
  * This and other improvements have led to a moderate reduction in the size of the library's assembly.
* Debugger proxies have been attached to the main types of the library, vastly improving debugging.
* Potentially confusing methods of creating `Option` instances have been removed.
* Methods have been added that allow monadic operations to be performed through `Task`s.
* A `Unit` type, a type with only one value, has been added.
  * Some `void`-returning methods have been modified to return `Unit`. This value may safely be ignored.
* `IEnumerable<T>` has learned a few monadic operations.

### What's new in 2.3.0 (Released 2016-10-28)

* `Option` and `Either` values can be created by splitting a conventional value on a condition.

### What's new in 2.2.1 (Released 2016-10-18)

* An `Either` value may be cast to its Left type.

### What's new in 2.2.0 (Released 2016-08-22)

* A method of optional dictionary value retrieval has been added.

### What's new in 2.1.0 (Released 2016-08-19)

* A Union type has been added, which is an unbiased union of two types.

### What's new in 2.0.0 (Released 2016-07-19)

* A greater number of methods have been marked as pure.
* Equality comparisons have been made simpler.
* The `Some` static method has been removed from `Option`, as its usage was identical to `From`, and saved no cycles.
* Arguments to `Map` or `Bind` that could lead to a violation of laws now throw uncatchable exceptions. Circumstances under which these exceptions occur are to be considered "[boneheaded][]", and represent a bug in the calling code.
* The `null`-safety of some methods has been adjusted to match reality.
* A greater number of `Either` methods will panic upon being passed an `Either` in the Bottom state.
* The performance of a number of methods has been slightly improved by reducing indirect method calls.
* `Option` and `Either` may now be converted from the one to the other, and from the other to the one.
* `Option` method parameter names have been adjusted to be parallel with those of other Tiger Types.

[boneheaded]: https://blogs.msdn.microsoft.com/ericlippert/2008/09/10/vexing-exceptions/

### What's new in 1.1.0 (Released 2016-07-12)

* ReSharper has been re-educated on the notion of purity, and will now correctly recognize annotated methods as pure.

### What's new in 1.0.0 (Released 2016-07-07)

* Everything!
