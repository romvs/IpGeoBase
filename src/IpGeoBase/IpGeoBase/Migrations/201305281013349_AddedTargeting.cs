namespace IpGeoBase.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedTargeting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "RuleBases",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TargetId = c.Guid(nullable: false),
                        Kind = c.Int(nullable: false),
                        LocationId = c.Int(),
                        AreaId = c.Int(),
                        Country = c.String(maxLength: 2),
                        RegionId = c.Int(),
                        RuleType = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Targets", t => t.TargetId, cascadeDelete: false)
                .ForeignKey("Locations", t => t.LocationId, cascadeDelete: false)
                .ForeignKey("Regions", t => t.RegionId, cascadeDelete: false)
                .ForeignKey("Areas", t => t.AreaId, cascadeDelete: false)
                .Index(t => t.TargetId)
                .Index(t => t.LocationId)
                .Index(t => t.RegionId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "Targets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("RuleBases", new[] { "AreaId" });
            DropIndex("RuleBases", new[] { "RegionId" });
            DropIndex("RuleBases", new[] { "LocationId" });
            DropIndex("RuleBases", new[] { "TargetId" });
            DropForeignKey("RuleBases", "AreaId", "Areas");
            DropForeignKey("RuleBases", "RegionId", "Regions");
            DropForeignKey("RuleBases", "LocationId", "Locations");
            DropForeignKey("RuleBases", "TargetId", "Targets");
            DropTable("Targets");
            DropTable("RuleBases");
        }
    }
}
