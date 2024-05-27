using iMES.Entity.MappingConfiguration;
using iMES.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iMES.Entity.MappingConfiguration
{
    public class Bi_db_dimMapConfig : EntityMappingConfiguration<Bi_db_dim>
    {
        public override void Map(EntityTypeBuilder<Bi_db_dim>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

