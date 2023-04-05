using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace ImpactSpace.Core.Skills;

public class SkillDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<SkillGroup, Guid> _skillGroupRepository;
    private readonly IRepository<Skill, Guid> _skillRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ILogger<SkillDataSeedContributor> _logger;

    public SkillDataSeedContributor(
        IRepository<SkillGroup, Guid> skillGroupRepository, 
        IRepository<Skill, Guid> skillRepository, 
        IGuidGenerator guidGenerator, 
        ILogger<SkillDataSeedContributor> logger)
    {
        _skillGroupRepository = skillGroupRepository;
        _skillRepository = skillRepository;
        _guidGenerator = guidGenerator;
        _logger = logger;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        _logger.LogInformation("Seeding database with default skills");
        
        // Retrieve all existing skill groups and skills
        var existingSkillGroups = await _skillGroupRepository.CountAsync();
        
        if (existingSkillGroups > 0)
        {
            _logger.LogInformation("Database already contains seeded skills");
            return;
        }
        
        // Define the Skill Groups and their Skills
        var skillGroupData = GetSkillGroupData();
        
        // Lists to store new skill groups and skills to insert
        var newSkillGroups = new List<SkillGroup>();
        var newSkills = new List<Skill>();
        
        // Add any missing skill groups and skills according to the skillGroupData dictionary
        foreach (var (skillGroupName, skills) in skillGroupData)
        {
            var skillGroup = new SkillGroup(_guidGenerator.Create(), skillGroupName);
            newSkillGroups.Add(skillGroup);

            foreach (var skillName in skills)
            {
                var newSkill = new Skill(_guidGenerator.Create(), skillName, skillGroup.Id);
                newSkills.Add(newSkill);
            }
        }
        
        // Perform bulk inserts for new skill groups and skills
        if (newSkillGroups.Count > 0)
        {
            await _skillGroupRepository.InsertManyAsync(newSkillGroups);
        }
        if (newSkills.Count > 0)
        {
            await _skillRepository.InsertManyAsync(newSkills);
        }
    }
    
    private Dictionary<string, HashSet<string>> GetSkillGroupData()
{
    return new Dictionary<string, HashSet<string>>
    {
        ["Tech Skills"] = new()
        {
            ".NET", "Angular", "API", "App Development", "AWS", "Back-End", "Block-Chain Tech", "C", "C#",
            "C++", "Clojure", "Computer Hardware", "CSS", "Data Analyst", "Data Migration", "Data Science",
            "Dev Ops", "Django", "Docker", "Droid", "Drupal", "E-Mail", "Elixir", "Ember", "Expo",
            "Front-End", "Full-Stack", "Go", "Graph Database", "Hacker", "Haskell", "Heroku", "Information Architecture",
            "iOS", "Java", "JavaScript", "jQuery", "Kuberneti", "Machine Learning", "Mobile Dev", "Mongo DB",
            "My SQL", "Neo4j", "Network Admin", "Node.JS", "Objective-C", "Oracle12c", "Phoenix", "PHP",
            "Pollster", "Python", "Rails", "React", "Ruby", "Rust", "Social Media Management", "SQLite",
            "Swift", "Tech Support", "Web Apps", "Web Design", "Web Development", "Web Infrastructure", "Web Scraping",
            "Windows", "Wordpress"
        },
        ["Non-Tech Skills"] = new()
        {
            "Activist", "Business Development", "Campaign Experience", "Canvassing", "Community Engagement",
            "Community Fundraising", "Community Organizing", "Creative Design", "Creative Writing", "Crisis Management",
            "Customer Service", "Education/Teaching", "Elected Official", "Event Planning", "Fundraising", "Graphic Design",
            "Human Resources", "Journalist", "Legal", "Logistics", "Magical Powers", "Marketing", "Non-Profit Experience",
            "Non-Profit Management", "Phone-Banking", "Political Operative", "Poll-Worker", "Poverty/Homelessness",
            "Product Management", "Project Management", "Public Relations", "Researcher", "Social Media Expert",
            "Volunteer Coordinating", "Volunteer Organizing", "Well Connected", "Editor", "Writer",
            "Executive Leadership - Non-Profit", "Executive Leadership - Private Sector", "Organization Building"
        }
    };
}

}