using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.Reflection.BindingFlags;
using static System.StringComparison;
using static System.Threading.Tasks.Task;
using static Tiger.Types.Resources;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="Try{TErr, TOk}"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) }, QuietOnSuccess = true)]
    public static partial class TryTests
    {
        [Property(DisplayName = "The copy constructor copies.")]
        public static void CopyConstructor_Err(NonEmptyString err)
        {
            var expected = Try<string, Version>.From(err.Get);
            var actual = new Try<string, Version>(expected);

            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "The copy constructor copies.")]
        public static void CopyConstructor_Right(Version ok)
        {
            var expected = Try<string, Version>.From(ok);
            var actual = new Try<string, Version>(expected);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "The copy constructor copies.")]
        public static void CopyConstructor_None()
        {
            var expected = Try<string, Version>.None;
            var actual = new Try<string, Version>(expected);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Null nullable values create None Tries.")]
        public static void NullableFrom_NullOk_None()
        {
            var actual = Try.From<string, int>((int?)null);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Non-null nullable values create Ok Tries.")]
        public static void NullableFrom_ValueOk_Ok(int some)
        {
            var actual = Try.From<string, int>((int?)some);

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }

        [Fact(DisplayName = "Null nullable values create None Tries.")]
        public static void NullableFrom_NullErr_None()
        {
            var actual = Try.From<int, string>((int?)null);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Non-null nullable values create Ok Tries.")]
        public static void NullableFrom_ValueErr_Ok(int some)
        {
            var actual = Try.From<int, string>((int?)some);

            Assert.True(actual.IsErr);
            var innerValue = (int)actual;
            Assert.Equal(some, innerValue);
        }

        [Property(DisplayName = "An Err Try is in the Err state.")]
        public static void FromErrBoth_IsErr_Err(NonEmptyString err) =>
            Assert.True(Try<string, Version>.From(err.Get).IsErr);

        [Property(DisplayName = "An Err Try is in the Err state.")]
        public static void FromErrOne_IsErr_Err(NonEmptyString err) =>
            Assert.True(Try.From<string, Version>(err.Get).IsErr);

        [Property(DisplayName = "An Ok Try is not in the Err state.")]
        public static void FromOkBoth_IsErr_Ok(Version ok) =>
            Assert.False(Try<string, Version>.From(ok).IsErr);

        [Property(DisplayName = "An Ok Try is not in the Err state.")]
        public static void FromOkOne_IsErr_Ok(Version ok) =>
            Assert.False(Try.From<string, Version>(ok).IsErr);

        [Fact(DisplayName = "A None Try is not in the Err state.")]
        public static void NoneDefault_IsErr_None() =>
            Assert.False(default(Try<string, Version>).IsErr);

        [Fact(DisplayName = "A None Try is not in the Err state.")]
        public static void NoneField_IsErr_None() =>
            Assert.False(Try<string, Version>.None.IsErr);

        [Fact(DisplayName = "A None Try is not in the Err state.")]
        public static void NoneConstructor_IsErr_None() =>
            Assert.False(new Try<string, Version>().IsErr);

        [Fact(DisplayName = "A None Try is not in the Err state.")]
        public static void FromErrBoth_IsErr_None() =>
            Assert.False(Try<string, Version>.From(err: null).IsErr);

        [Fact(DisplayName = "A None Try is not in the Err state.")]
        public static void FromErrOne_IsErr_None() =>
            Assert.False(Try.From<string, Version>(err: null).IsErr);

        [Fact(DisplayName = "A None Try is not in the Err state.")]
        public static void FromOkBoth_IsErr_None() =>
            Assert.False(Try<string, Version>.From(ok: null).IsErr);

        [Fact(DisplayName = "A None Try is not in the Err state.")]
        public static void FromOkOne_IsErr_None() =>
            Assert.False(Try.From<string, Version>(ok: null).IsErr);

        [Property(DisplayName = "An Err Try is not in the Ok state.")]
        public static void FromErrBoth_IsOk_Err(NonEmptyString err) =>
            Assert.False(Try<string, Version>.From(err.Get).IsOk);

        [Property(DisplayName = "An Err Try is not in the Ok state.")]
        public static void FromErrOne_IsOk_Err(NonEmptyString err) =>
            Assert.False(Try.From<string, Version>(err.Get).IsOk);

        [Property(DisplayName = "An Ok Try is in the Ok state.")]
        public static void FromOkBoth_IsOk_Ok(Version ok) =>
            Assert.True(Try<string, Version>.From(ok).IsOk);

        [Property(DisplayName = "An Ok Try is in the Ok state.")]
        public static void FromOkOne_IsOk_Ok(Version ok) =>
            Assert.True(Try.From<string, Version>(ok).IsOk);

        [Fact(DisplayName = "A None Try is not in the Ok state.")]
        public static void NoneDefault_IsOk_None() =>
            Assert.False(default(Try<string, Version>).IsOk);

        [Fact(DisplayName = "A None Try is not in the Ok state.")]
        public static void NoneField_IsOk_None() =>
            Assert.False(Try<string, Version>.None.IsOk);

        [Fact(DisplayName = "A None Try is not in the Ok state.")]
        public static void NoneConstructor_IsOk_None() =>
            Assert.False(new Try<string, Version>().IsOk);

        [Fact(DisplayName = "A None Try is not in the Ok state.")]
        public static void FromErrBoth_IsOk_None() =>
            Assert.False(Try<string, Version>.From(err: null).IsOk);

        [Fact(DisplayName = "A None Try is not in the Ok state.")]
        public static void FromErrOne_IsOk_None() =>
            Assert.False(Try.From<string, Version>(err: null).IsOk);

        [Fact(DisplayName = "A None Try is not in the Ok state.")]
        public static void FromOkBoth_IsOk_None() =>
            Assert.False(Try<string, Version>.From(ok: null).IsOk);

        [Fact(DisplayName = "A None Try is not in the Ok state.")]
        public static void FromOkOne_IsOk_None() =>
            Assert.False(Try.From<string, Version>(ok: null).IsOk);

        [Property(DisplayName = "An Err Try is not in the None state.")]
        public static void FromErrBoth_IsNone_Err(NonEmptyString err) =>
            Assert.False(Try<string, Version>.From(err.Get).IsNone);

        [Property(DisplayName = "An Err Try is not in the Ok state.")]
        public static void FromErrOne_IsNone_Err(NonEmptyString err) =>
            Assert.False(Try.From<string, Version>(err.Get).IsNone);

        [Property(DisplayName = "An Ok Try is not in the None state.")]
        public static void FromOkBoth_IsNone_Ok(Version ok) =>
            Assert.False(Try<string, Version>.From(ok).IsNone);

        [Property(DisplayName = "An Ok Try is not in the None state.")]
        public static void FromOkOne_IsNone_Ok(Version ok) =>
            Assert.False(Try.From<string, Version>(ok).IsNone);

        [Fact(DisplayName = "A None Try is in the None state.")]
        public static void NoneDefault_IsNone_None() =>
            Assert.True(default(Try<string, Version>).IsNone);

        [Fact(DisplayName = "A None Try is in the None state.")]
        public static void NoneField_IsNone_None() =>
            Assert.True(Try<string, Version>.None.IsNone);

        [Fact(DisplayName = "A None Try is in the None state.")]
        public static void NoneConstructor_IsNone_None() =>
            Assert.True(new Try<string, Version>().IsNone);

        [Fact(DisplayName = "A None Try is in the None state.")]
        public static void FromErrBoth_IsNone_None() =>
            Assert.True(Try<string, Version>.From(err: null).IsNone);

        [Fact(DisplayName = "A None Try is in the None state.")]
        public static void FromErrOne_IsNone_None() =>
            Assert.True(Try.From<string, Version>(err: null).IsNone);

        [Fact(DisplayName = "A None Try is in the None state.")]
        public static void FromOkBoth_IsNone_None() =>
            Assert.True(Try<string, Version>.From(ok: null).IsNone);

        [Fact(DisplayName = "A None Try is in the None state.")]
        public static void FromOkOne_IsNone_None() =>
            Assert.True(Try.From<string, Version>(ok: null).IsNone);

        [Property(DisplayName = "Forcibly unwrapping an Err Try throws.")]
        public static void Value_Err_Throws(NonEmptyString err)
        {
            var actual = Record.Exception(() => Try<string, Version>.From(err.Get).Value);

            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(TryIsNotOk, ioe.Message, Ordinal);
        }

        [Fact(DisplayName = "Forcibly unwrapping a None Try throws.")]
        public static void Value_None_Throws()
        {
            var actual = Record.Exception(() => Try<string, Version>.None.Value);

            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(TryIsNotOk, ioe.Message, Ordinal);
        }

        [Property(DisplayName = "Forcibly unwrapping an Ok Try returns the Ok value.")]
        public static void Value_Ok(Version ok) =>
            Assert.Equal(ok, Try<string, Version>.From(ok).Value);

        [Property(DisplayName = "Coalescing a None Try with an alternative value returns the alternative value.")]
        public static void GetValueOrDefault_None() =>
            Assert.Equal(default, Try<string, Version>.None.GetValueOrDefault());

        [Property(DisplayName = "Coalescing an Err Try with an alternative value returns the alternative value.")]
        public static void GetValueOrDefault_Err(NonEmptyString err) =>
            Assert.Equal(default, Try<string, Version>.From(err.Get).GetValueOrDefault());

        [Property(DisplayName = "Coalescing an Ok Try with an alternative value returns the Ok value.")]
        public static void GetValueOrDefault_Ok(Version ok) =>
            Assert.Equal(ok, Try<string, Version>.From(ok).GetValueOrDefault());

        [Property(DisplayName = "Coalescing a Try with a null value throws.")]
        public static void ValueGetValueOrDefault_Null_Throws(Try<string, Version> option)
        {
            var actual = Record.Exception(() => option.GetValueOrDefault((Version)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("other", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Coalescing a None Try with an alternative value returns the alternative value.")]
        public static void ValueGetValueOrDefault_None(Version alternative) =>
            Assert.Equal(alternative, Try<string, Version>.None.GetValueOrDefault(alternative));

        [Property(DisplayName = "Coalescing an Err Try with an alternative value returns the alternative value.")]
        public static void ValueGetValueOrDefault_Err(NonEmptyString err, Version alternative) =>
            Assert.Equal(alternative, Try<string, Version>.From(err.Get).GetValueOrDefault(alternative));

        [Property(DisplayName = "Coalescing an Ok Try with an alternative value returns the Ok value.")]
        public static void ValueGetValueOrDefault_Ok(Version ok, Version alternative) =>
            Assert.Equal(ok, Try<string, Version>.From(ok).GetValueOrDefault(alternative));

        [Property(DisplayName = "Coalescing a Try with a null Func throws.")]
        public static void FuncGetValueOrDefault_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.GetValueOrDefault((Func<Version>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("other", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Coalescing a None Try with a func producing an alternative value returns the alternative value.")]
        public static void FuncGetValueOrDefault_None(Version alternative) =>
            Assert.Equal(alternative, Try<string, Version>.None.GetValueOrDefault(() => alternative));

        [Property(DisplayName = "Coalescing an Err Try with a func producing an alternative value returns the alternative value.")]
        public static void FuncGetValueOrDefault_Err(NonEmptyString err, Version alternative) =>
            Assert.Equal(alternative, Try<string, Version>.From(err.Get).GetValueOrDefault(() => alternative));

        [Property(DisplayName = "Coalescing an Ok Try with a func producing an alternative value returns the Ok value.")]
        public static void FuncGetValueOrDefault_Ok(Version ok, Version alternative) =>
            Assert.Equal(ok, Try<string, Version>.From(ok).GetValueOrDefault(() => alternative));

        [Property(DisplayName = "Coalescing a Try with a null task throws.")]
        public static async Task GetValueOrDefaultAsync_Null_Throws(Try<string, Version> @try)
        {
            var actual = await Record.ExceptionAsync(() => @try.GetValueOrDefaultAsync(null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("other", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Coalescing a None Try with a task producing an alternative value returns the alternative value.")]
        public static async Task GetValueOrDefaultAsync_None(Version alternative)
        {
            var actual = await Try<string, Version>.None
                .GetValueOrDefaultAsync(() => FromResult(alternative))
                .ConfigureAwait(false);

            Assert.Equal(alternative, actual);
        }

        [Property(DisplayName = "Coalescing an Err Try with a task producing an alternative value returns the alternative value.")]
        public static async Task GetValueOrDefaultAsync_Err(NonEmptyString err, Version alternative)
        {
            var actual = await Try<string, Version>.From(err.Get)
                .GetValueOrDefaultAsync(() => FromResult(alternative))
                .ConfigureAwait(false);

            Assert.Equal(alternative, actual);
        }

        [Property(DisplayName = "Coalescing an Ok Try with a func producing an alternative value returns the Ok value.")]
        public static async Task GetValueOrDefaultAsync_Ok(Version ok, Version alternative)
        {
            var actual = await Try<string, Version>.From(ok)
                .GetValueOrDefaultAsync(() => FromResult(alternative))
                .ConfigureAwait(false);

            Assert.Equal(ok, actual);
        }

        [Fact(DisplayName = "A None Try dumps to a None Try.")]
        public static void Dump_None()
        {
            var @try = Try<string, Version>.None;
            var actual = typeof(Try<string, Version>)
                .GetMethod("ToDump", Instance | NonPublic)
                .Invoke(@try, Array.Empty<object>());
            var properties = actual
                .GetType()
                .GetProperties();

            var property = Assert.Single(properties);
            Assert.NotNull(property);
            Assert.Equal("State", property.Name);
            var value = Assert.IsType<string>(property.GetValue(actual));
            Assert.Equal("None", value);
        }

        [Property(DisplayName = "An Err Try dumps to an Err Try.")]
        public static void Dump_Err(NonEmptyString err)
        {
            var option = Try<string, Version>.From(err.Get);
            var actual = typeof(Try<string, Version>)
                .GetMethod("ToDump", Instance | NonPublic)
                .Invoke(option, Array.Empty<object>());
            var properties = actual
                .GetType()
                .GetProperties();

            Assert.All(properties, Assert.NotNull);
            Assert.Collection(properties,
                pi =>
                {
                    Assert.Equal("State", pi.Name);
                    var value = Assert.IsType<string>(pi.GetValue(actual));
                    Assert.Equal("Err", value);
                },
                pi =>
                {
                    Assert.Equal("Value", pi.Name);
                    var value = Assert.IsType<string>(pi.GetValue(actual));
                    Assert.Equal(err.Get, value);
                });
        }

        [Property(DisplayName = "An Ok Try dumps to an Ok Try.")]
        public static void Dump_Ok(Version ok)
        {
            var option = Try<string, Version>.From(ok);
            var actual = typeof(Try<string, Version>)
                .GetMethod("ToDump", Instance | NonPublic)
                .Invoke(option, Array.Empty<object>());
            var properties = actual
                .GetType()
                .GetProperties();

            Assert.All(properties, Assert.NotNull);
            Assert.Collection(properties,
                pi =>
                {
                    Assert.Equal("State", pi.Name);
                    var value = Assert.IsType<string>(pi.GetValue(actual));
                    Assert.Equal("Ok", value);
                },
                pi =>
                {
                    Assert.Equal("Value", pi.Name);
                    var value = Assert.IsType<Version>(pi.GetValue(actual));
                    Assert.Equal(ok, value);
                });
        }
    }
}
