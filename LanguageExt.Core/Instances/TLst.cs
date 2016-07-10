﻿using System;
using System.Diagnostics.Contracts;
using LanguageExt.TypeClasses;

namespace LanguageExt.Instances
{
    public struct TLst<A> : Monoid<Lst<A>>, Difference<Lst<A>>
    {
        [Pure]
        public Lst<A> Append(Lst<A> x, Lst<A> y) =>
            x.Append(y);

        [Pure]
        public Lst<A> Difference(Lst<A> x, Lst<A> y) =>
            x.Difference(y);

        [Pure]
        public Lst<A> Empty() =>
            List.empty<A>();
    }
}
