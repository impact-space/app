using System;
using Shouldly;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Guids;
using NSubstitute;
using Volo.Abp.Data;
using Xunit;

namespace ImpactSpace.Core.Challenges
{
    public sealed class ChallengeDataSeedContributorTests : CoreDomainTestBase
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IChallengeRepository _challengeRepository;
        private readonly ILogger<ChallengeDataSeedContributor> _logger;

        public ChallengeDataSeedContributorTests()
        {
            _guidGenerator = GetRequiredService<IGuidGenerator>();
            _challengeRepository = GetRequiredService<IChallengeRepository>();
            _logger = Substitute.For<ILogger<ChallengeDataSeedContributor>>();
        }

        [Fact]
        public async Task Should_Seed_Challenges()
        {
            // Arrange
            var dataSeedContext = new DataSeedContext();
            var contributor = new ChallengeDataSeedContributor(_challengeRepository, _guidGenerator, _logger);

            // Act
            await contributor.SeedAsync(dataSeedContext);

            // Assert
            var challenges = await _challengeRepository.GetListAsync();
            
            challenges.ShouldContain(c => c.Name == "Environmental");
            challenges.ShouldContain(c => c.Name == "Social");
            challenges.ShouldContain(c => c.Name == "Economic");
            challenges.ShouldContain(c => c.Name == "Political");
            // ...other challenge names

            foreach (var challenge in challenges)
            {
                challenge.Id.ShouldNotBe(Guid.Empty);
            }
        }
    }
}