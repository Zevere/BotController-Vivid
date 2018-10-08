using Framework;

namespace Ops.IntegrationTests.Shared
{
    public class TestCollectionOrderer : TestCollectionOrdererBase
    {
        private static readonly string[] Collections =
        {
            "user registration",
            "user registration errors",
        };

        public TestCollectionOrderer()
            : base(Collections) { }
    }
}
