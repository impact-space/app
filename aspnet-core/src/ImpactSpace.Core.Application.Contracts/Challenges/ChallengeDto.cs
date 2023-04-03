using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace ImpactSpace.Core.Challenges;

public class ChallengeDto : EntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }
    public string ConcurrencyStamp { get; set; }
}