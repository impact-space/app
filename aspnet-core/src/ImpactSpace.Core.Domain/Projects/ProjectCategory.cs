using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects
{
    public class ProjectCategory : Entity<Guid>, IMultiTenant, IHasConcurrencyStamp
    {
        public string Name { get; private set; }
        
        public virtual ICollection<Project> Projects { get; private set; }
        
        public Guid? TenantId { get; set; }
        
        public string ConcurrencyStamp { get; set; }

        protected ProjectCategory()
        {
            // This constructor is for deserialization / ORM purposes
        }

        internal ProjectCategory(Guid id, [NotNull] string name)
            : base(id)
        {
            SetName(name);

            Projects = new Collection<Project>();
        }

        internal ProjectCategory ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }

        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProjectCategoryConsts.MaxNameLength
            );
        }
    }
}