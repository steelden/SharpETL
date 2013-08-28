using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SharpETL.Utility
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Assembly> GetAssemblies(bool skipSystem)
        {
            IEnumerable<Assembly> result = AppDomain.CurrentDomain.GetAssemblies();
            if (skipSystem) {
                result = result.Where(x => !x.FullName.StartsWith("System.") && !x.FullName.StartsWith("System,"));
            }
            return result;
        }

        public static IEnumerable<Type> GetTypesImplementingInterfaces(IEnumerable<Assembly> assemblies, IEnumerable<Type> interfaces)
        {
            if (assemblies == null) throw new ArgumentNullException("assembly");
            if (interfaces == null) throw new ArgumentNullException("interfaces");
            if (!interfaces.All(x => x.IsInterface)) throw new ArgumentException("Not all types in 'interfaces' parameter are actual interfaces.");

            return GetAllAssemblyTypes(assemblies).Where(x => x.IsClass && !x.IsAbstract && x.FindInterfaces(CheckInterfaces, interfaces).Length > 0);
        }

        private static IEnumerable<Type> GetAllAssemblyTypes(IEnumerable<Assembly> assemblies)
        {
            var result = Enumerable.Empty<Type>();
            foreach (var assembly in assemblies) {
                result = result.Concat(GetAssemblyTypes(assembly));
            }
            return result;
        }

        private static IEnumerable<Type> GetAssemblyTypes(Assembly assembly)
        {
            try {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex) {
                return ex.Types.Where(x => x != null);
            }
        }

        private static bool CheckInterfaces(Type current, object param)
        {
            return ((IEnumerable<Type>)param).Any(x => x.IsAssignableFrom(current));
        }
    }
}
