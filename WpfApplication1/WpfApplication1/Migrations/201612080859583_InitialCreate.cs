namespace WpfApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Details",
                c => new
                    {
                        Det_ID = c.Long(nullable: false, identity: true),
                        SessionID = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        KeyData = c.String(maxLength: 1000, fixedLength: true),
                        TextData = c.String(),
                        BinData = c.Binary(),
                    })
                .PrimaryKey(t => t.Det_ID)
                .ForeignKey("dbo.Sessions", t => t.SessionID)
                .Index(t => t.SessionID);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        SessionID = c.Long(nullable: false, identity: true),
                        IP_in = c.String(nullable: false, maxLength: 16, unicode: false),
                        IP_in_bin = c.Binary(nullable: false, maxLength: 16, fixedLength: true),
                        IP_out = c.String(nullable: false, maxLength: 16, unicode: false),
                        IP_out_bin = c.Binary(nullable: false, maxLength: 16, fixedLength: true),
                        MAC_in = c.String(),
                        Started = c.DateTime(),
                        Ended = c.DateTime(),
                        Port_in = c.Int(),
                        Port_out = c.Int(),
                        IsSSL = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SessionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Details", "SessionID", "dbo.Sessions");
            DropIndex("dbo.Details", new[] { "SessionID" });
            DropTable("dbo.Sessions");
            DropTable("dbo.Details");
        }
    }
}
