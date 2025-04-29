using FluentMigrator;

namespace MyDub.Auth.Migrations;

[Migration(20231508, TransactionBehavior.None)]
public class InitSchema : Migration {
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsInt64().PrimaryKey("user_pk").Identity()
            .WithColumn("login").AsString().NotNullable().Unique()
            .WithColumn("password_hash").AsString().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("users");
    }
}