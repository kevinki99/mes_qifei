using iMES.Entity.MappingConfiguration;
using iMES.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iMES.Entity.MappingConfiguration
{
    public class Bi_db_ybpMapConfig : EntityMappingConfiguration<Bi_db_ybp>
    {
        public override void Map(EntityTypeBuilder<Bi_db_ybp>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

