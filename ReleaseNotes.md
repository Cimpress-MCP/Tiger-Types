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
* Arguments to `Map` or `Bind` that could lead to a violation of laws now throw uncatchable exceptions.  Circumstances under which these exceptions occur are to be considered "[boneheaded](https://blogs.msdn.microsoft.com/ericlippert/2008/09/10/vexing-exceptions/)", and represent a bug in the calling code.
* The `null`-safety of some methods has been adjusted to match reality.
* A greater number of `Either` methods will panic upon being passed an `Either` in the Bottom state.
* The performance of a number of methods has been slightly improved by reducing indirect method calls.
* `Option` and `Either` may now be converted from the one to the other, and from the other to the one.
* `Option` method parameter names have been adjusted to be parallel with those of other Tiger Types.

### What's new in 1.1.0 (Released 2016-07-12)

* ReSharper has been re-educated on the notion of purity, and will now correctly recognize annotated methods as pure.

### What's new in 1.0.0 (Released 2016-07-07)

* Everything!
