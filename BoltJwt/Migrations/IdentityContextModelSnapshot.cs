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
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BoltJwt.Domain.Model.DefinedAuthorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("def_authorizations","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("groups","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.GroupRole", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("RoleId");

                    b.HasKey("GroupId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("group_role","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("roles","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.RoleAuthorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DefAuthorizationId");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("DefAuthorizationId", "RoleId")
                        .IsUnique();

                    b.ToTable("role_authorizations","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Disabled")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("ForgotPasswordAuthCode");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<bool>("Root")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Surname");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("users","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.UserActivationCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<long>("Timestamp");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("user_activation_codes","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.UserAuthorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DefAuthorizationId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("DefAuthorizationId", "UserId")
                        .IsUnique();

                    b.ToTable("user_authorizations","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.UserGroup", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("UserId");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("user_group","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.UserRole", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("user_role","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Infrastructure.AppConfigurations.Configuration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<string>("EndpointFqdn");

                    b.Property<string>("SmtpEmail");

                    b.Property<string>("SmtpHostName");

                    b.Property<string>("SmtpPassword");

                    b.Property<int>("SmtpPort");

                    b.Property<string>("SmtpUserName");

                    b.HasKey("Id");

                    b.ToTable("configuration","IdentityContext");
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.GroupRole", b =>
                {
                    b.HasOne("BoltJwt.Domain.Model.Group", "Group")
                        .WithMany("GroupRoles")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BoltJwt.Domain.Model.Role", "Role")
                        .WithMany("GroupRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.RoleAuthorization", b =>
                {
                    b.HasOne("BoltJwt.Domain.Model.DefinedAuthorization")
                        .WithMany("RolesAuthorizations")
                        .HasForeignKey("DefAuthorizationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BoltJwt.Domain.Model.Role")
                        .WithMany("Authorizations")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.UserActivationCode", b =>
                {
                    b.HasOne("BoltJwt.Domain.Model.User", "User")
                        .WithOne("ActivationCode")
                        .HasForeignKey("BoltJwt.Domain.Model.UserActivationCode", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.UserAuthorization", b =>
                {
                    b.HasOne("BoltJwt.Domain.Model.DefinedAuthorization")
                        .WithMany("UserAuthorizations")
                        .HasForeignKey("DefAuthorizationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BoltJwt.Domain.Model.User")
                        .WithMany("Authorizations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.UserGroup", b =>
                {
                    b.HasOne("BoltJwt.Domain.Model.Group", "Group")
                        .WithMany("UserGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BoltJwt.Domain.Model.User", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BoltJwt.Domain.Model.UserRole", b =>
                {
                    b.HasOne("BoltJwt.Domain.Model.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BoltJwt.Domain.Model.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
