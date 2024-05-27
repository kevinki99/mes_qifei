using iMES.Entity.MappingConfiguration;
using iMES.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iMES.Entity.MappingConfiguration
{
    public class Bi_db_setMapConfig : EntityMappingConfiguration<Bi_db_set>
    {
        public override void Map(EntityTypeBuilder<Bi_db_set>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

