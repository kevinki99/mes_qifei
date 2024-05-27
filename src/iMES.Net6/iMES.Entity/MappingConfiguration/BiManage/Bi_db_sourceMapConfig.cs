using iMES.Entity.MappingConfiguration;
using iMES.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iMES.Entity.MappingConfiguration
{
    public class Bi_db_sourceMapConfig : EntityMappingConfiguration<Bi_db_source>
    {
        public override void Map(EntityTypeBuilder<Bi_db_source>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

