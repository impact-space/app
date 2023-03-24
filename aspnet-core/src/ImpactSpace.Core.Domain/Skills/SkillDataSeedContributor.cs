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
        
        // Define the Skill Groups and their Skills
        var skillGroupData = new Dictionary<string, string[]>
        {
            ["Activism"] = new[] {"Activist", "Community Engagement", "Canvassing", "Campaign Experience"},
            ["Communication"] = new[] {"Verbal Communication", "Written Communication", "Public Speaking", "Presentation Skills"},
            ["Consulting"] = new[] {"Management Consulting", "IT Consulting", "Strategy Consulting", "Financial Consulting"},
            ["Creative"] = new[] {"Creative Design", "Graphic Design", "Creative Writing", "Editor", "Writer"},
            ["Crisis Management"] = new[] {"Crisis Management", "Emergency Response", "Disaster Relief"},
            ["Customer Service"] = new[] {"Customer Service", "Customer Relations", "Conflict Resolution"},
            ["Design"] = new[] {"User Experience (UX) Design", "User Interface (UI) Design", "Information Architecture", "Design Thinking"},
            ["Education"] = new[] {"Education/Teaching", "Curriculum Development", "Instructional Design"},
            ["Entrepreneurship"] = new[] {"Entrepreneurship", "Business Planning", "Start-up Experience", "Venture Capital"},
            ["Event Planning"] = new[] {"Event Planning", "Logistics", "Conference Coordination"},
            ["Finance"] = new[] {"Finance", "Accounting", "Budgeting", "Financial Analysis"},
            ["Fundraising"] = new[] {"Fundraising", "Community Fundraising", "Donor Relations", "Grant Writing"},
            ["Health and Medicine"] = new[] {"Medicine", "Nursing", "Public Health", "Medical Research"},
            ["Health and Wellness"] = new[] {"Health and Wellness", "Nutrition", "Fitness", "Yoga", "Meditation"},
            ["Hospitality"] = new[] {"Hospitality", "Event Coordination", "Food and Beverage Management", "Hotel Management"},
            ["Human Resources"] = new[] {"Human Resources", "Talent Acquisition", "Employee Relations", "Benefits Administration"},
            ["Journalism"] = new[] {"Journalism", "Investigative Reporting", "Fact-Checking", "News Writing"},
            ["Language"] = new[] {"Language Fluency", "Translation", "Interpretation", "Localization"},
            ["Leadership"] = new[] {"Leadership", "Team Management", "Coaching and Mentoring", "Performance Management"},
            ["Legal"] = new[] {"Legal", "Compliance", "Contract Negotiation", "Intellectual Property"},
            ["Logistics and Supply Chain"] = new[] {"Logistics", "Supply Chain Management", "Inventory Management", "Procurement"},
            ["Management"] = new[] {"Project Management", "Product Management", "Non-Profit Management", "Executive Leadership - Non-Profit", "Executive Leadership - Private Sector"},
            ["Marketing"] = new[] {"Marketing", "Brand Management", "Market Research", "Advertising"},
            ["Non-Profit"] = new[] {"Non-Profit Experience", "Non-Profit Management", "Volunteer Coordinating", "Volunteer Organizing", "Organization Building"},
            ["Operations"] = new[] {"Operations Management", "Process Improvement", "Lean Six Sigma", "Quality Control"},
            ["Politics"] = new[] {"Political Operative", "Elected Official", "Poll-Worker", "Public Relations"},
            ["Project Coordination"] = new[] {"Project Coordination", "Scheduling", "Resource Allocation", "Risk Management"},
            ["Research"] = new[] {"Researcher", "Data Analysis", "Survey Design", "Quantitative Analysis"},
            ["Sales"] = new [] {"Sales", "Business Development", "Account Management", "Relationship Management"},
            ["Science"] = new[] {"Scientific Research", "Lab Techniques", "Experiment Design", "Data Analysis"},
            ["Strategy"] = new[] {"Strategic Planning", "Business Strategy", "Competitive Analysis", "Market Analysis"},
            ["Teaching and Training"] = new[] {"Teaching", "Training", "Curriculum Development", "Instructional Design"},
            ["Writing and Editing"] = new[] {"Writing", "Editing", "Copywriting", "Proofreading"},
            ["Programming Languages"] = new[] {"C", "C++", "C#", "Clojure", "Elixir", "Go", "Haskell", "Java", "JavaScript", "Objective-C", "PHP", "Python", "Ruby", "Rust", "Swift"},
            ["Web Development"] = new[] {"HTML", "CSS", "JavaScript", "jQuery", "React", "Angular", "Vue.js", "Ember.js", "Node.js", "Express.js", "PHP", "Ruby on Rails", "Django", "Drupal", "WordPress", "Web Design", "Web Scraping"},
            ["Mobile Development"] = new[] {"iOS", "Android", "React Native", "Flutter", "Expo", "Droid"},
            ["Database Technologies"] = new[] {"MySQL", "Oracle12c", "MongoDB", "Neo4j", "Graph Database", "SQLite"},
            ["Cloud Technologies"] = new[] {"AWS", "Microsoft Azure", "Google Cloud Platform", "OpenStack", "Cloud Foundry", "Heroku", "Docker", "Kubernetes"},
            ["Backend Technologies"] = new[] {".NET", "Back-End", "API", "Phoenix"},
            ["Data Science and Analytics"] = new[] {"Data Science", "Machine Learning", "Data Analyst", "Data Migration", "Pollster", "Hadoop", "Spark", "Hive", "Pig", "NoSQL Databases", "Data Warehousing"},
            ["Other Technical Skills"] = new[] {"Computer Hardware", "Windows", "Tech Support", "Web Infrastructure", "Block-Chain Tech", "Social Media Management", "Information Architecture", "E-Mail", "Hacker"},
            ["Front-End Technologies"] = new[] {"HTML", "CSS", "JavaScript", "jQuery", "React", "Angular", "Vue.js", "Ember.js"},
            ["DevOps"] = new[] {"Continuous Integration", "Continuous Deployment", "Jenkins", "Git", "Ansible", "Chef", "Puppet"},
            ["Cybersecurity"] = new[] {"Network Security", "Identity and Access Management", "Application Security", "Cryptography", "Penetration Testing"},
            ["UI/UX Design"] = new[] {"User Research", "Wireframing", "Prototyping", "Interaction Design", "Visual Design"},
            ["Project Management"] = new[] {"Agile Methodology", "Scrum", "Kanban", "Waterfall Methodology", "Jira", "Trello"},
            ["Quality Assurance"] = new[] {"Manual Testing", "Automated Testing", "Performance Testing", "Security Testing"},
            ["Big Data"] = new[] {"Hadoop", "Spark", "Hive", "Pig", "NoSQL Databases", "Data Warehousing"},
            ["Embedded Systems"] = new[] {"Embedded C", "Microcontroller Programming", "RTOS", "Hardware Design", "PCB Design"},
            ["Game Development"] = new[] {"Unity", "Unreal Engine", "Game Design", "Game Mechanics", "Game AI"},
            ["Blockchain and Cryptocurrency"] = new[] {"Ethereum", "Bitcoin", "Smart Contracts", "Solidity", "Cryptocurrency Trading", "Blockchain Development"},
            ["Internet of Things (IoT)"] = new[] {"IoT Sensors", "IoT Communication Protocols", "IoT Security", "IoT Analytics", "IoT Platforms"},
            ["Natural Language Processing (NLP)"] = new[] {"Sentiment Analysis", "Text Classification", "Named Entity Recognition", "Topic Modeling", "Speech Recognition"},
            ["Virtual and Augmented Reality (VR/AR)"] = new[] {"Unity", "Unreal Engine", "3D Modeling", "VR/AR Development", "Interaction Design"},
            ["Serverless Computing"] = new[] {"AWS Lambda", "Azure Functions", "Google Cloud Functions", "Serverless Framework", "FaaS"},
            ["Data Visualization"] = new[] {"Tableau", "D3.js", "Plotly", "ggplot", "Infographics Design"},
            ["Business Intelligence (BI)"] = new[] {"Data Warehousing", "ETL", "OLAP", "Data Mining", "Business Analytics"},
            ["Marketing Automation"] = new[] {"CRM", "Email Marketing", "Social Media Marketing", "Content Marketing", "Marketing Analytics"},
            ["Sales and Customer Relationship Management (CRM)"] = new[] {"Sales Automation", "Lead Generation", "Customer Support", "Customer Retention", "CRM Platforms"},
            ["Technical Writing and Documentation"] = new[] {"Technical Writing", "User Documentation", "API Documentation", "Technical Editing", "Writing Style Guides"}
        };

        // Retrieve all existing skill groups and skills
        var existingSkillGroups = await _skillGroupRepository.GetListAsync();
        var existingSkills = await _skillRepository.GetListAsync();
        
        // Map the existing skill groups and skills to dictionaries for easy lookup
        var existingSkillGroupNames = existingSkillGroups.ToDictionary(sg => sg.Name, sg => sg);
        var existingSkillNames = existingSkills.ToDictionary(s => s.Name, s => s);
        
        // Add any missing skill groups and skills according to the skillGroupData dictionary
        foreach (var (skillGroupName, skills) in skillGroupData)
        {
            // Check if the Skill Group already exists, otherwise create it
            if (!existingSkillGroupNames.TryGetValue(skillGroupName, out var existingSkillGroup))
            {
                existingSkillGroup = new SkillGroup(_guidGenerator.Create(), skillGroupName);
                await _skillGroupRepository.InsertAsync(existingSkillGroup, autoSave: true);
                existingSkillGroupNames.Add(skillGroupName, existingSkillGroup);
            }

            // Check if the Skills already exist, otherwise create them
            foreach (var skillName in skills)
            {
                if (!existingSkillNames.ContainsKey(skillName))
                {
                    var newSkill = new Skill(_guidGenerator.Create(), skillName, existingSkillGroup.Id);
                    await _skillRepository.InsertAsync(newSkill, autoSave: true);
                    existingSkillNames.Add(skillName, newSkill);
                }
            }
        }
    }
}