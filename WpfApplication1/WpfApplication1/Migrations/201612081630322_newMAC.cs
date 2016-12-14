namespace WpfApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMAC : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sessions", "MAC_in", c => c.String(nullable: false, maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sessions", "MAC_in", c => c.String());
        }
    }
}
