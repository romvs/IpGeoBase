namespace IpGeoBase.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AreaId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Areas", t => t.AreaId, cascadeDelete: true)
                .Index(t => t.AreaId);
            
            CreateTable(
                "Locations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RegionId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 250),
                        Latitude = c.Decimal(nullable: false, precision: 10, scale: 6),
                        Longitude = c.Decimal(nullable: false, precision: 10, scale: 6),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Regions", t => t.RegionId, cascadeDelete: true)
                .Index(t => t.RegionId);
            
            CreateTable(
                "Ranges",
                c => new
                    {
                        Start = c.Long(nullable: false),
                        End = c.Long(nullable: false),
                        Country = c.String(nullable: false, maxLength: 2),
                        Description = c.String(nullable: false, maxLength: 33),
                        LocationId = c.Int(),
                    })
                .PrimaryKey(t => new { t.Start, t.End })
                .ForeignKey("Locations", t => t.LocationId)
                .Index(t => t.LocationId);
            
        }
        
        public override void Down()
        {
            DropIndex("Ranges", new[] { "LocationId" });
            DropIndex("Locations", new[] { "RegionId" });
            DropIndex("Regions", new[] { "AreaId" });
            DropForeignKey("Ranges", "LocationId", "Locations");
            DropForeignKey("Locations", "RegionId", "Regions");
            DropForeignKey("Regions", "AreaId", "Areas");
            DropTable("Ranges");
            DropTable("Locations");
            DropTable("Regions");
            DropTable("Areas");
        }
    }
}
