using iMES.Entity.MappingConfiguration;
using iMES.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iMES.Entity.MappingConfiguration
{
    public class Sys_DeptTreeMapConfig : EntityMappingConfiguration<Sys_DeptTree>
    {
        public override void Map(EntityTypeBuilder<Sys_DeptTree>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

