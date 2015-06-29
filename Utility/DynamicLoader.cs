using System;
using System.Diagnostics;

namespace Utility.DynamiCloader
{
    public interface ILoadable
    {
        void Execute();
    }
    public class Implementation1 : ILoadable
    {
        public void Execute()
        {
            // code here
            Debug.WriteLine("Implementation 1 executed");
        }
    }
    public class Implementation2 : ILoadable
    {
        public void Execute()
        {
            // code here
            Debug.WriteLine("Implementation 2 executed");
        }
    }
    public class LocalBroker
    {
        public static ILoadable CreateInstance( string loadableClass )
        {
            ILoadable loadable;
            Type type;
            type = Type.GetType(loadableClass);
            // Can only load classes in the same project (predefined set of functions)
            loadable = (ILoadable) System.Activator.CreateInstance(type);
            return loadable;
        }
    }
    public class Callee
    {
        public void Main()
        {
            LocalBroker.CreateInstance( "Utility.DynamiCloader.Implementation1" ).Execute();
            LocalBroker.CreateInstance( "Utility.DynamiCloader.Implementation2" ).Execute();
        }
    }
    public class RemoteBroker
    {
        public static ILoadable CreateInstance( string assemblyName, string assemblyPath, string loadableClass )
        {
            System.Reflection.Assembly assembly = null;
            ILoadable loadable = null;
            // when located locally or in Gac use this
            if (!String.IsNullOrEmpty(assemblyName))
            {
                assembly = System.Reflection.Assembly.Load(assemblyName);
            }
            // when loading from path use this
            if (!String.IsNullOrEmpty(assemblyPath))
            {
                assembly = System.Reflection.Assembly.LoadFile(assemblyPath);
            }
            if ( assembly != null )
            {
                loadable = (ILoadable) assembly.CreateInstance(loadableClass);
            }
            return loadable;
        }
    }
    public class DynamicCallee
    {
        public void Main()
        {
            RemoteBroker.CreateInstance("Utility", null, "Utility.DynamiCloader.Implementation1" ).Execute();
            RemoteBroker.CreateInstance(null, @"C:\Dev\Test\TryOut\Utility\bin\Debug\Utility.dll", "Utility.DynamiCloader.Implementation2" ).Execute();
        }
    }
}