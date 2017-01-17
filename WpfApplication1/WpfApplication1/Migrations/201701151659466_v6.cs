namespace WpfApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alerts",
                c => new
                    {
                        AlertID = c.Int(nullable: false, identity: true),
                        AlertName = c.String(nullable: false, maxLength: 1000),
                        Enable = c.Boolean(nullable: false),
                        Anouncement = c.Boolean(nullable: false),
                        Popup = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AlertID);
            
            CreateTable(
                "dbo.AlertWebs",
                c => new
                    {
                        WebID = c.Int(nullable: false, identity: true),
                        AlertID = c.Int(nullable: false),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.WebID)
                .ForeignKey("dbo.Alerts", t => t.AlertID, cascadeDelete: true)
                .Index(t => t.AlertID);
            
            CreateTable(
                "dbo.Details",
                c => new
                    {
                        Det_ID = c.Long(nullable: false, identity: true),
                        SessionID = c.Long(nullable: false),
                        PluginID = c.Int(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        KeyData = c.String(maxLength: 1000, fixedLength: true),
                        TextData = c.String(),
                        BinData = c.String(),
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
                        IP_in_is_v4 = c.Boolean(nullable: false),
                        IP_out = c.String(nullable: false, maxLength: 16, unicode: false),
                        IP_out_is_v4 = c.Boolean(nullable: false),
                        MAC_in = c.String(nullable: false, maxLength: 32),
                        MAC_out = c.String(nullable: false, maxLength: 32),
                        Started = c.DateTime(nullable: false),
                        Ended = c.DateTime(nullable: false),
                        Port_in = c.Int(),
                        Port_out = c.Int(),
                        State = c.Int(nullable: false),
                        IsSSL = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SessionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Details", "SessionID", "dbo.Sessions");
            DropForeignKey("dbo.AlertWebs", "AlertID", "dbo.Alerts");
            DropIndex("dbo.Details", new[] { "SessionID" });
            DropIndex("dbo.AlertWebs", new[] { "AlertID" });
            DropTable("dbo.Sessions");
            DropTable("dbo.Details");
            DropTable("dbo.AlertWebs");
            DropTable("dbo.Alerts");
        }
    }
}
