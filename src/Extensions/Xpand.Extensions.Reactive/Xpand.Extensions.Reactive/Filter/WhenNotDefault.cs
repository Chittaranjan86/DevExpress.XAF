﻿using System;
using System.Reactive.Linq;
using Xpand.Extensions.Reactive.Transform;

namespace Xpand.Extensions.Reactive.Filter{
    public static partial class Filter{
        public static IObservable<TSource> WhenNotDefault<TSource>(this IObservable<TSource> source,Func<TSource,object> predicate) =>source
	        .SelectMany(source1 => predicate(source1).ReturnObservable().WhenNotDefault().To(source1));

        public static IObservable<TSource> WhenNotDefault<TSource>(this IObservable<TSource> source) => source
	        .Select(_ => (object) _).Where(o => o != null)
	        .Select(o => (TSource) o)
	        .Where(_ => !_.Equals(default(TSource)));
    }
}