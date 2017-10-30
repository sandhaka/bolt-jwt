﻿// <auto-generated />
using BoltJwt.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BoltJwt.Migrations
{
    [DbContext(typeof(IdentityContext))]
    partial class IdentityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BoltJwt.Model.DefinedAuthorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("def_authorizations","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Model.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("groups","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Model.GroupRole", b =>
                {
                    b.Property<int?>("GroupId");

                    b.Property<int?>("RoleId");

                    b.HasKey("GroupId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("GroupRole");
                });

            modelBuilder.Entity("BoltJwt.Model.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("roles","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Model.RoleAuthorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorizationName")
                        .IsRequired();

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("role_authorizations","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Admin");

                    b.Property<string>("Email");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<bool>("Root");

                    b.Property<string>("Surname");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("users","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Model.UserAuthorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorizationName")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("user_authorizations","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Model.UserGroup", b =>
                {
                    b.Property<int?>("GroupId");

                    b.Property<int?>("UserId");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroup");
                });

            modelBuilder.Entity("BoltJwt.Model.UserRole", b =>
                {
                    b.Property<int?>("RoleId");

                    b.Property<int?>("UserId");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("BoltJwt.Model.GroupRole", b =>
                {
                    b.HasOne("BoltJwt.Model.Group", "Group")
                        .WithMany("GroupRoles")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BoltJwt.Model.Role", "Role")
                        .WithMany("GroupRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BoltJwt.Model.RoleAuthorization", b =>
                {
                    b.HasOne("BoltJwt.Model.Role")
                        .WithMany("Authorizations")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BoltJwt.Model.UserAuthorization", b =>
                {
                    b.HasOne("BoltJwt.Model.User")
                        .WithMany("Authorizations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BoltJwt.Model.UserGroup", b =>
                {
                    b.HasOne("BoltJwt.Model.Group", "Group")
                        .WithMany("UserGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BoltJwt.Model.User", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BoltJwt.Model.UserRole", b =>
                {
                    b.HasOne("BoltJwt.Model.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BoltJwt.Model.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
