namespace WpfApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveShit : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Sessions", "IP_in_bin");
            DropColumn("dbo.Sessions", "IP_out_bin");
            DropColumn("dbo.Sessions", "IsSSL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sessions", "IsSSL", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sessions", "IP_out_bin", c => c.Binary(nullable: false, maxLength: 16, fixedLength: true));
            AddColumn("dbo.Sessions", "IP_in_bin", c => c.Binary(nullable: false, maxLength: 16, fixedLength: true));
        }
    }
}
