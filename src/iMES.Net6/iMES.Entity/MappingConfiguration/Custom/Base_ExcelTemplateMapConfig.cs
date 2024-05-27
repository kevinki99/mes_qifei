using iMES.Entity.MappingConfiguration;
using iMES.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iMES.Entity.MappingConfiguration
{
    public class Base_ExcelTemplateMapConfig : EntityMappingConfiguration<Base_ExcelTemplate>
    {
        public override void Map(EntityTypeBuilder<Base_ExcelTemplate>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

