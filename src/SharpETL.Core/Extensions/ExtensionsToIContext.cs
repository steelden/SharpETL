using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Utility;

namespace SharpETL.Extensions
{
    public static class ExtensionsToIContext
    {
        public static T Get<T>(this IContext context, string name, T defaultValue = default(T)) { return (T)context.Get(name, defaultValue); }
        public static T GetOrDefault<T>(this IContext context, string name, T defaultValue = default(T)) { return (T)context.GetOrDefault(name, defaultValue); }
        public static void Set<T>(this IContext context, string name, T value) { context.Set(name, value); }
    }
}
