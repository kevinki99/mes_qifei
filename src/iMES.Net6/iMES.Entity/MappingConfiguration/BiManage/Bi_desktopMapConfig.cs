using iMES.Entity.MappingConfiguration;
using iMES.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iMES.Entity.MappingConfiguration
{
    public class Bi_desktopMapConfig : EntityMappingConfiguration<Bi_desktop>
    {
        public override void Map(EntityTypeBuilder<Bi_desktop>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

