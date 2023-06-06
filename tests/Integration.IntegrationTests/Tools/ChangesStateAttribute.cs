using System.Reflection;
using Xunit.Sdk;

namespace Integration.IntegrationTests.Tools;

public class ChangesStateAttribute : BeforeAfterTestAttribute
{
    public override void After(MethodInfo methodUnderTest)
    {
        IntegrationTestContext.Current.StateChanged = true;
        base.After(methodUnderTest);
    }
}
