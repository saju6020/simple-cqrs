using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.ORM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogContentsLenght : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("450d388f-4463-40a1-8206-70f2d3daffb7"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CreatedOn", "Description", "IsPublished", "PublishedOn", "Title", "UpdatedOn" },
                values: new object[] { new Guid("7da3afc5-a93e-4fff-88d1-651fad287d1b"), null, null, false, null, "Default", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("7da3afc5-a93e-4fff-88d1-651fad287d1b"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CreatedOn", "Description", "IsPublished", "PublishedOn", "Title", "UpdatedOn" },
                values: new object[] { new Guid("450d388f-4463-40a1-8206-70f2d3daffb7"), null, null, false, null, "Default", null });
        }
    }
}
