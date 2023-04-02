using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ImpactSpace.Core.Projects;

public class TagManager : DomainService
{
    private readonly ITagRepository _tagRepository;
    private readonly IRepository<ProjectTag> _projectTagRepository;
    
    public TagManager(
        ITagRepository tagRepository,
        IRepository<ProjectTag> projectTagRepository)
    {
        _tagRepository = tagRepository;
        _projectTagRepository = projectTagRepository;
    }
    
    public async Task<Tag> CreateAsync(
        [NotNull] string name)
    {
        Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            TagConstants.MaxNameLength
        );
        
        var existingTag = await _tagRepository.FindByNameAsync(name);
        
        if (existingTag != null)
        {
            throw new TagAlreadyExistsException(name);
        }
        
        return new Tag(
            GuidGenerator.Create(),
            name
        );
    }
    
    public async Task ChangeNameAsync(
        [NotNull] Tag tag,
        [NotNull] string newName)
    {
        Check.NotNull(tag, nameof(tag));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));
        
        var existingTag = await _tagRepository.FindByNameAsync(newName);
        if (existingTag != null && existingTag.Id != tag.Id)
        {
            throw new TagAlreadyExistsException(newName);
        }
        
        tag.ChangeName(newName);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var tag = await _tagRepository.GetAsync(id);

        await _projectTagRepository.DeleteAsync(pt => pt.TagId == tag.Id);
        
        await _tagRepository.DeleteAsync(tag);
    }
}