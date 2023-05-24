namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SongsTables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SongName = c.String(nullable: false, maxLength: 50),
                        Artists = c.String(nullable: false, maxLength: 200),
                        AlbumName = c.String(nullable: false, maxLength: 50),
                        Year = c.Int(nullable: false),
                        Genre = c.String(nullable: false),
                        Lyrics = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SongsTables");
        }
    }
}
