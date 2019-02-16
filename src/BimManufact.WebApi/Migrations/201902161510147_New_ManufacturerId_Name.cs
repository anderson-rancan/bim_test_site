namespace BimManufact.WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_ManufacturerId_Name : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Manufacturers");
            AddColumn("dbo.Manufacturers", "ManufacturerId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Manufacturers", "ManufacturerId");
            DropColumn("dbo.Manufacturers", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Manufacturers", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Manufacturers");
            DropColumn("dbo.Manufacturers", "ManufacturerId");
            AddPrimaryKey("dbo.Manufacturers", "Id");
        }
    }
}
