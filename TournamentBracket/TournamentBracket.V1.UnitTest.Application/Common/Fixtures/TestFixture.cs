namespace TournamentBracket.V1.UnitTest.Application.Common.Fixtures
{
    public class TestFixture : IDisposable
    {
        public Guid TournamentTestID { get; private set; }
        public TestFixture()
        {
            TournamentTestID = Guid.NewGuid();
        }

        public void Dispose()
        {

        }
    }
}
