﻿using System.Collections.Generic;

namespace Xpand.Extensions.Linq{
    public static partial class LinqExtensions{
        public static IEnumerable<TSource> YieldItem<TSource>(this TSource source){
            yield return source;
        }
    }
}