namespace WpfApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Details", "BinData", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Details", "BinData", c => c.Binary());
        }
    }
}
