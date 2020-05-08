using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mediador3
{
    internal static class FN
    {
        public static Func<bool, bool, bool> AndAgregator {
            get { 
                return (acc, val) => acc && val; 
            }
        }
        public static Func<Assembly, bool> IsNotSystemAssembly
        {
            get
            {
                return a => !a.FullName.StartsWith("System", System.StringComparison.InvariantCulture);
            }
        }
        public static Func<Type, bool> IsNotCompilerGenerated
        {
            get
            {
                return t => !t.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute), true);
            }
        }
        public static Func<Type, bool> IsConcreteClass
        {
            get
            {
                return t => t.IsClass && !t.IsAbstract;
            }
        }
        public static Func<Type, bool> IsNotNull
        {
            get
            {
                return t => t != null;
            }
        }
        public static Func<Type, bool> AND(Func<Type, bool> f1, Func<Type, bool> f2)
        {
            return t => f1(t) && f2(t);
        }
        public static Func<Type, bool> OR(Func<Type, bool> f1, Func<Type, bool> f2)
        {
            return t => f1(t) || f2(t);
        }
        public static Func<Type, bool> ClassImplements(Type interfaceType)
        {
            return t => interfaceType.IsAssignableFrom(t);
        }
        public static T Instanciate<T>(Type from) { return (T)Activator.CreateInstance(from); }
    }
}