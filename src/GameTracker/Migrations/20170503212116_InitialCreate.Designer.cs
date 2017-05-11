using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GameTracker.Data;

namespace GameTracker.Migrations
{
    [DbContext(typeof(GameDbContext))]
    [Migration("20170503212116_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GameTracker.Models.Game", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GameImagesID");

                    b.Property<string>("Name");

                    b.Property<DateTime>("Original_release_date");

                    b.Property<int>("PlatformID");

                    b.HasKey("ID");

                    b.HasIndex("GameImagesID");

                    b.HasIndex("PlatformID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("GameTracker.Models.GameImage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Icon_url");

                    b.Property<string>("Medium_url");

                    b.Property<string>("Screen_url");

                    b.Property<string>("Small_url");

                    b.Property<string>("Super_url");

                    b.Property<string>("Thumb_url");

                    b.Property<string>("Tiny_url");

                    b.HasKey("ID");

                    b.ToTable("GameImage");
                });

            modelBuilder.Entity("GameTracker.Models.Platform", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("abbreviation");

                    b.Property<string>("api_detail_url");

                    b.Property<string>("site_detail_url");

                    b.HasKey("ID");

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("GameTracker.Models.Game", b =>
                {
                    b.HasOne("GameTracker.Models.GameImage", "GameImages")
                        .WithMany()
                        .HasForeignKey("GameImagesID");

                    b.HasOne("GameTracker.Models.Platform", "Platform")
                        .WithMany("Games")
                        .HasForeignKey("PlatformID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
