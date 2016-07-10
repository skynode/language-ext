﻿using Xunit;
using LanguageExt.TypeClasses;
using LanguageExt.Instances;
using static LanguageExt.TypeClass;

namespace LanguageExtTests
{
    public class TypeClassORD
    {
        [Fact]
        public void IntOrd()
        {
            Assert.True(lessThan<TInteger, int>(10, 20));
            Assert.False(lessThan<TInteger, int>(20, 10));
            Assert.False(lessThan<TInteger, int>(10, 10));

            Assert.True(greaterThan<TInteger, int>(20, 10));
            Assert.False(greaterThan<TInteger, int>(10, 20));
            Assert.False(greaterThan<TInteger, int>(10, 10));

            Assert.True(lessOrEq<TInteger, int>(10, 20));
            Assert.False(lessOrEq<TInteger, int>(20, 10));
            Assert.True(lessOrEq<TInteger, int>(10, 10));

            Assert.True(greaterOrEq<TInteger, int>(20, 10));
            Assert.False(greaterOrEq<TInteger, int>(10, 20));
            Assert.True(greaterOrEq<TInteger, int>(10, 10));
        }

        [Fact]
        public void DoubleOrd()
        {
            Assert.True(lessThan<TDouble, double>(10, 20));
            Assert.False(lessThan<TDouble, double>(20, 10));
            Assert.False(lessThan<TDouble, double>(10, 10));

            Assert.True(greaterThan<TDouble, double>(20, 10));
            Assert.False(greaterThan<TDouble, double>(10, 20));
            Assert.False(greaterThan<TDouble, double>(10, 10));

            Assert.True(lessOrEq<TDouble, double>(10, 20));
            Assert.False(lessOrEq<TDouble, double>(20, 10));
            Assert.True(lessOrEq<TDouble, double>(10, 10));

            Assert.True(greaterOrEq<TDouble, double>(20, 10));
            Assert.False(greaterOrEq<TDouble, double>(10, 20));
            Assert.True(greaterOrEq<TDouble, double>(10, 10));
        }

        [Fact]
        public void StringOrd()
        {
            Assert.True(lessThan<TString, string>("aaa", "bbb"));
            Assert.False(lessThan<TString, string>("bbb", "aaa"));
            Assert.False(lessThan<TString, string>("aaa", "aaa"));

            Assert.True(greaterThan<TString, string>("bbb", "aaa"));
            Assert.False(greaterThan<TString, string>("aaa", "bbb"));
            Assert.False(greaterThan<TString, string>("aaa", "aaa"));

            Assert.True(lessOrEq<TString, string>("aaa", "bbb"));
            Assert.False(lessOrEq<TString, string>("bbb", "aaa"));
            Assert.True(lessOrEq<TString, string>("aaa", "aaa"));

            Assert.True(greaterOrEq<TString, string>("bbb", "aaa"));
            Assert.False(greaterOrEq<TString, string>("aaa", "bbb"));
            Assert.True(greaterOrEq<TString, string>("aaa", "aaa"));
        }
    }
}
