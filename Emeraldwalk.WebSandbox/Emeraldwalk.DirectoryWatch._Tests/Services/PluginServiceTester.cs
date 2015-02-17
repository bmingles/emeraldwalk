using ClaySharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emeraldwalk.DirectoryWatch._Tests.Services
{
    public interface IPlugin
    {
        int SomeMethod(string name);
    }

    public interface IAnotherPlugin
    {
        int SomeMethod(string name);
    }

    public class AnotherPlugin: IAnotherPlugin
    {
        public int SomeMethod(string name)
        {
            return 5;
        }
    }

    public class DynamicPlugin: DynamicObject {
        public int SomeMethod(string name)
        {
            return 4;
        }
    }

    [TestClass]
    public class PluginServiceTester
    {
        [TestMethod]
        public void ClayTest()
        {
            dynamic New = new ClayFactory();
            
            var somePlugin = New.Person(new AnotherPlugin());

            IPlugin plugin = (IPlugin)somePlugin;

            int count = plugin.SomeMethod("john");
        }

        [TestMethod]
        public void DynamicTest()
        {
            dynamic plugin = new DynamicPlugin();
            plugin.SomeMethod("john");

            var x = new
            {
                SomeMethod = new Func<string, int>(name => 4)
            };


        }
    }
}
