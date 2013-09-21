namespace CartoSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlowTypeNumeric : DbMigration
    {
        public override void Up()
        {
            Sql("update Flows set FlowType = 1 where FlowType = 'Fichier'");
            Sql("update Flows set FlowType = 2 where FlowType = 'Base à base'");
            Sql("update Flows set FlowType = 3 where FlowType = 'Service'");
            AlterColumn("dbo.Flows", "FlowType", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Flows", "FlowType", c => c.String());
        }
    }
}
