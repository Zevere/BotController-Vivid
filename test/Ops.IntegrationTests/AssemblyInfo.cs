using Framework;
using Ops.IntegrationTests.Shared;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

[assembly: TestCollectionOrderer(
    "Ops.IntegrationTests.Shared." + nameof(TestCollectionOrderer),
    "Ops.IntegrationTests"
)]

[assembly: TestCaseOrderer(TestConstants.TestCaseOrderer, TestConstants.AssemblyName)]
