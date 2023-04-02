using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace ImpactSpace.Core.Challenges;

public class ChallengeDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IChallengeRepository _challengeRepository; 
    private readonly IGuidGenerator _guidGenerator;
    private readonly ILogger<ChallengeDataSeedContributor> _logger;

    public ChallengeDataSeedContributor(
        IChallengeRepository challengeRepository, 
        IGuidGenerator guidGenerator, 
        ILogger<ChallengeDataSeedContributor> logger)
    {
        _challengeRepository = challengeRepository;
        _guidGenerator = guidGenerator;
        _logger = logger;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        _logger.LogInformation("Seeding Challenges...");
        
        var challenges = new HashSet<string>(){
            "Environmental",
            "Social",
            "Economic",
            "Political",
            "Health",
            "Education",
            "Technology",
            "Ethics",
            "Human Rights",
            "Equality",
            "Justice",
            "Cultural",
            "Animal Rights",
            "Child Welfare",
            "Climate Change",
            "Community Development",
            "Conservation",
            "Consumer Protection",
            "Criminal Justice",
            "Disability Rights",
            "Disaster Response",
            "Drug Policy",
            "Energy",
            "Food Security",
            "Gender Equality",
            "Gun Control",
            "Housing",
            "Immigration",
            "Indigenous Rights",
            "Labor Rights",
            "Mental Health",
            "Poverty",
            "Privacy",
            "Racial Justice",
            "Refugee Rights",
            "Religious Freedom",
            "Reproductive Rights",
            "Scientific Research",
            "Substance Abuse",
            "Veterans Issues",
            "Voting Rights",
            "Water Rights",
            "LGBTQIA+",
            "Other"
        };
        
        foreach (var challenge in challenges)
        {
            if (await _challengeRepository.FindAsync(x => x.Name == challenge) == null)
            {
                await _challengeRepository.InsertAsync(new Challenge(_guidGenerator.Create(), challenge));
            }
        }
    }
}