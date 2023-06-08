using System.Reflection;
using Xunit.Sdk;

namespace Example.IntegrationTests.TestContext;

public class DirtyTestAttribute : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        base.Before(methodUnderTest);
        IntegrationTestContext.Current ??= new IntegrationTestContext();
    }
    
    public override void After(MethodInfo methodUnderTest)
    {
        IntegrationTestContext.Current!.StateChanged = true;
        base.After(methodUnderTest);
    }
}
