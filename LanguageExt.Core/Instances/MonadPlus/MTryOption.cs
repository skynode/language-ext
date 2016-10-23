﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt.TypeClasses;
using System.Diagnostics.Contracts;
using static LanguageExt.Prelude;

namespace LanguageExt.Instances
{
    public struct MTryOption<A> :
        Optional<TryOption<A>, A>,
        MonadPlus<TryOption<A>, A>,
        Foldable<TryOption<A>, A>,
        BiFoldable<TryOption<A>, Unit, A>
    {
        static TryOption<A> none = TryOption(() => Option<A>.None);

        public MB Bind<MONADB, MB, B>(TryOption<A> ma, Func<A, MB> f) where MONADB : struct, Monad<MB, B>
        {
            var mr = ma.Try();
            if (mr.IsFaulted) default(MONADB).Fail(mr.Exception);
            if (mr.Value.IsNone) default(MONADB).Fail(default(A));
            return f(mr.Value.Value);
        }

        [Pure]
        public TryOption<A> Fail(object err) => 
            none;

        [Pure]
        public TryOption<A> Fail(Exception err = null) => 
            none;

        [Pure]
        public TryOption<A> Plus(TryOption<A> ma, TryOption<A> mb) => TryOption(() =>
        {
            var res = ma.Try();
            if (!res.IsFaulted && res.Value.IsSome) return res.Value;
            return mb.Run();
        });

        [Pure]
        public TryOption<A> Return(IEnumerable<A> xs) =>        // TODO: We need a lazy Return
            TryOption(() =>
            {
                var x = xs.Take(1).ToArray();
                return x.Length == 0
                    ? None
                    : Optional(x[0]);
            });

        [Pure]
        public TryOption<A> Return(A x, params A[] xs) =>       // TODO: We need a lazy Return
            TryOption(() => x);

        [Pure]
        public TryOption<A> Zero() => 
            none;

        [Pure]
        public bool IsNone(TryOption<A> opt) =>
            opt.Match(
                Some: __ => false, 
                None: () => true, 
                Fail: ex => true);

        [Pure]
        public bool IsSome(TryOption<A> opt) =>
            opt.Match(
                Some: __ => true,
                None: () => false,
                Fail: ex => false);

        [Pure]
        public bool IsUnsafe(TryOption<A> opt) =>
            false;

        [Pure]
        public B Match<B>(TryOption<A> opt, Func<A, B> Some, Func<B> None)
        {
            var res = opt.Try();
            if (res.IsFaulted || res.Value.IsNone)
                return None();
            else
                return Some(res.Value.Value);
        }

        public Unit Match(TryOption<A> opt, Action<A> Some, Action None)
        {
            var res = opt.Try();
            if (res.IsFaulted || res.Value.IsNone) None(); else Some(res.Value.Value);
            return unit;
        }

        [Pure]
        public B MatchUnsafe<B>(TryOption<A> opt, Func<A, B> Some, Func<B> None)
        {
            var res = opt.Try();
            if (res.IsFaulted || res.Value.IsNone)
                return None();
            else
                return Some(res.Value.Value);
        }

        [Pure]
        public S Fold<S>(TryOption<A> ma, S state, Func<S, A, S> f)
        {
            var res = ma.Try();
            if (res.IsFaulted || res.Value.IsNone) return state;
            return f(state, res.Value.Value);
        }

        [Pure]
        public S FoldBack<S>(TryOption<A> ma, S state, Func<S, A, S> f)
        {
            var res = ma.Try();
            if (res.IsFaulted || res.Value.IsNone) return state;
            return f(state, res.Value.Value);
        }

        [Pure]
        public S BiFold<S>(TryOption<A> ma, S state, Func<S, Unit, S> fa, Func<S, A, S> fb)
        {
            var res = ma.Try();
            if (res.IsFaulted || res.Value.IsNone)
                return fa(state, unit);
            else
                return fb(state, res.Value.Value);
        }

        [Pure]
        public S BiFoldBack<S>(TryOption<A> ma, S state, Func<S, Unit, S> fa, Func<S, A, S> fb)
        {
            var res = ma.Try();
            if (res.IsFaulted || res.Value.IsNone)
                return fa(state, unit);
            else
                return fb(state, res.Value.Value);
        }

        [Pure]
        public int Count(TryOption<A> ma) =>
            ma.Try().Value.IsSome
                ? 1
                : 0;
    }
}