﻿using System;
using System.Collections.Generic;

namespace Xpand.Source.Extensions.Linq{
    internal static partial class Extensions{
        public static int FindIndex<T>(this IList<T> list, Predicate<T> predicate) {
            for(int i = 0; i < list.Count; i++)
                if(predicate(list[i]))
                    return i;
            return -1;
        }

    }
}