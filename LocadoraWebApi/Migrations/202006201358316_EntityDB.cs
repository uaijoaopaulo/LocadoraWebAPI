namespace LocadoraWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class EntityDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_ClienteCF",
                c => new
                {
                    idCliente = c.Int(nullable: false, identity: true),
                    nomeCliente = c.String(),
                    CPF = c.String(),
                    clienteAtivo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.idCliente);

            CreateTable(
                "dbo.tb_LocacaoCF",
                c => new
                {
                    idLocacao = c.Int(nullable: false, identity: true),
                    dataLocacao = c.DateTime(nullable: false),
                    dataDevolucao = c.DateTime(),
                    locacaoAtiva = c.Boolean(nullable: false),
                    idCliente = c.Int(nullable: false),
                    idFilme = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.idLocacao)
                .ForeignKey("dbo.tb_ClienteCF", t => t.idCliente, cascadeDelete: true)
                .ForeignKey("dbo.tb_FilmeCF", t => t.idFilme, cascadeDelete: true)
                .Index(t => t.idCliente)
                .Index(t => t.idFilme);

            CreateTable(
                "dbo.tb_FilmeCF",
                c => new
                {
                    idFilme = c.Int(nullable: false, identity: true),
                    nomeFilme = c.String(),
                    filmeAtivo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.idFilme);

        }

        public override void Down()
        {
            DropForeignKey("dbo.tb_LocacaoCF", "idFilme", "dbo.tb_FilmeCF");
            DropForeignKey("dbo.tb_LocacaoCF", "idCliente", "dbo.tb_ClienteCF");
            DropIndex("dbo.tb_LocacaoCF", new[] { "idFilme" });
            DropIndex("dbo.tb_LocacaoCF", new[] { "idCliente" });
            DropTable("dbo.tb_FilmeCF");
            DropTable("dbo.tb_LocacaoCF");
            DropTable("dbo.tb_ClienteCF");
        }
    }
}
