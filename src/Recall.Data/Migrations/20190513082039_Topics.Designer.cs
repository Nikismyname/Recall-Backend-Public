﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Recall.Data;

namespace Recall.Data.Migrations
{
    [DbContext(typeof(RecallDbContext))]
    [Migration("20190513082039_Topics")]
    partial class Topics
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Recall.Data.Models.Core.Directory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfSubdirectories");

                    b.Property<int>("NumberOfVideos");

                    b.Property<int>("Order");

                    b.Property<int?>("ParentDirectoryId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ParentDirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Directories");
                });

            modelBuilder.Entity("Recall.Data.Models.Core.ExtensionAddedVideo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ExtensionAddedVideos");
                });

            modelBuilder.Entity("Recall.Data.Models.Core.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BackgroundColor");

                    b.Property<string>("BorderColor");

                    b.Property<int>("BorderThickness");

                    b.Property<string>("Content");

                    b.Property<int>("Formatting");

                    b.Property<int>("Level");

                    b.Property<int>("Order");

                    b.Property<int?>("ParentNoteId");

                    b.Property<int?>("SeekTo");

                    b.Property<string>("TextColor");

                    b.Property<int>("Type");

                    b.Property<int>("VideoId");

                    b.HasKey("Id");

                    b.HasIndex("ParentNoteId");

                    b.HasIndex("VideoId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Recall.Data.Models.Core.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("HashedPassword");

                    b.Property<string>("LastName");

                    b.Property<string>("Role");

                    b.Property<int?>("RootDirectoryId");

                    b.Property<string>("Salt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("RootDirectoryId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Recall.Data.Models.Core.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("DirectoryId");

                    b.Property<bool>("IsLocal");

                    b.Property<bool>("IsVimeo");

                    b.Property<bool>("IsYouTube");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<int?>("SeekTo");

                    b.Property<string>("Url");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("Recall.Data.Models.Meta.Connection", b =>
                {
                    b.Property<int>("VideoOneId");

                    b.Property<int>("VideoTwoId");

                    b.Property<string>("Name");

                    b.Property<int>("Strength");

                    b.Property<int>("Type");

                    b.HasKey("VideoOneId", "VideoTwoId");

                    b.HasIndex("VideoTwoId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("Recall.Data.Models.Meta.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Approved");

                    b.Property<string>("CriteriaForBelonging");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentTopicId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ParentTopicId");

                    b.HasIndex("UserId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Recall.Data.Models.Meta.VideoTopic", b =>
                {
                    b.Property<int>("VideoId");

                    b.Property<int>("TopicId");

                    b.Property<int>("Adherence");

                    b.HasKey("VideoId", "TopicId");

                    b.HasIndex("TopicId");

                    b.ToTable("VideosTopics");
                });

            modelBuilder.Entity("Recall.Data.Models.Core.Directory", b =>
                {
                    b.HasOne("Recall.Data.Models.Core.Directory", "ParentDirectory")
                        .WithMany("Subdirectories")
                        .HasForeignKey("ParentDirectoryId");

                    b.HasOne("Recall.Data.Models.Core.User", "User")
                        .WithMany("Directories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Recall.Data.Models.Core.ExtensionAddedVideo", b =>
                {
                    b.HasOne("Recall.Data.Models.Core.User", "User")
                        .WithMany("ExtensionAddedVideos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Recall.Data.Models.Core.Note", b =>
                {
                    b.HasOne("Recall.Data.Models.Core.Note", "ParentNote")
                        .WithMany("ChildNotes")
                        .HasForeignKey("ParentNoteId");

                    b.HasOne("Recall.Data.Models.Core.Video", "Video")
                        .WithMany("Notes")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Recall.Data.Models.Core.User", b =>
                {
                    b.HasOne("Recall.Data.Models.Core.Directory", "RootDirectory")
                        .WithOne("RootUser")
                        .HasForeignKey("Recall.Data.Models.Core.User", "RootDirectoryId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Recall.Data.Models.Core.Video", b =>
                {
                    b.HasOne("Recall.Data.Models.Core.Directory", "Directiry")
                        .WithMany("Videos")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Recall.Data.Models.Core.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Recall.Data.Models.Meta.Connection", b =>
                {
                    b.HasOne("Recall.Data.Models.Core.Video", "VideoOne")
                        .WithMany("ConnectedTo")
                        .HasForeignKey("VideoOneId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Recall.Data.Models.Core.Video", "VideoTwo")
                        .WithMany("ConnectedFrom")
                        .HasForeignKey("VideoTwoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Recall.Data.Models.Meta.Topic", b =>
                {
                    b.HasOne("Recall.Data.Models.Meta.Topic", "ParentTopic")
                        .WithMany("Subtopics")
                        .HasForeignKey("ParentTopicId");

                    b.HasOne("Recall.Data.Models.Core.User", "User")
                        .WithMany("MyTopics")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Recall.Data.Models.Meta.VideoTopic", b =>
                {
                    b.HasOne("Recall.Data.Models.Meta.Topic", "Topic")
                        .WithMany("Videos")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Recall.Data.Models.Core.Video", "Video")
                        .WithMany("Topics")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
