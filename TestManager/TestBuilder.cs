namespace TestManager;

using System.Text;
using TestManager.Attributes;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;
using System.Diagnostics;


public class TestBuilder
{
    private List<Type>? _testClasses;
    
    public IEnumerable<Type> TestClasses {get {return _testClasses;} }

    public TestBuilder(Type testClass)
    {
        _testClasses = new List<Type>()
        {
            testClass
        };
    }

    public string GetTests()
    {
        StringBuilder result = new StringBuilder();
        
        foreach (Type testClass in _testClasses)
        {
            result.Append((testClass.Name + "\n\t"));

            IEnumerable<MethodInfo> methods = testClass.GetMethods()
                .Where(x => x.GetCustomAttributes(typeof(MethodTestingAttribute), false).Length > 0);

            object classObject = Activator.CreateInstance(testClass);

            foreach (MethodInfo method in methods)
            {
                Stopwatch diagnostic = new Stopwatch();
                
                diagnostic.Start();
                
                method.Invoke(classObject, null);

                diagnostic.Stop();

                result.Append(method.Name + "\t" + diagnostic.Elapsed.TotalSeconds + "\n\t");
            }
        }

        return result.ToString();
    }
}