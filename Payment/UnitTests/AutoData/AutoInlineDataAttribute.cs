using AutoFixture.Xunit2;
using Xunit;

namespace UnitTests.AutoData
{
    public class AutoInlineDataAttribute : CompositeDataAttribute
    {
        public AutoInlineDataAttribute(params object[] values)
           : base(new InlineDataAttribute(values), new AutoNSubstituteDataAttribute())
        {
        }
    }
}
