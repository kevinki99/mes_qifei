using iMES.Entity.MappingConfiguration;
using iMES.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iMES.Entity.MappingConfiguration
{
    public class Base_MaterialDetailTreeMapConfig : EntityMappingConfiguration<Base_MaterialDetailTree>
    {
        public override void Map(EntityTypeBuilder<Base_MaterialDetailTree>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

