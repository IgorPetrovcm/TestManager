namespace TestManager;

using System.Text;
using TestManager.Attributes;
using System.Reflection;


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

    public void GetTests()
    {
        StringBuilder result = new StringBuilder();
        
        foreach (Type testClass in _testClasses)
        {
            result.Append((testClass.Name + "\n\t"));

            IEnumerable<MethodInfo> methods = testClass.GetMethods()
                .Where(x => x.GetCustomAttributes(typeof(MethodTestingAttribute), false).Length > 0);

            foreach (MethodInfo method in methods)
            {
                method.Invoke(null);
            }
        }
    }
}