using iMES.Entity.MappingConfiguration;
using iMES.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iMES.Entity.MappingConfiguration
{
    public class Production_AssembleWorkOrderMapConfig : EntityMappingConfiguration<Production_AssembleWorkOrder>
    {
        public override void Map(EntityTypeBuilder<Production_AssembleWorkOrder>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

