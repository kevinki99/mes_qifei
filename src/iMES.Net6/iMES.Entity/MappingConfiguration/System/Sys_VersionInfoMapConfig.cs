using iMES.Entity.MappingConfiguration;
using iMES.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iMES.Entity.MappingConfiguration
{
    public class Sys_VersionInfoMapConfig : EntityMappingConfiguration<Sys_VersionInfo>
    {
        public override void Map(EntityTypeBuilder<Sys_VersionInfo>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

