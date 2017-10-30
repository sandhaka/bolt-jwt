using BoltJwt.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoltJwt.Infrastructure.Context
{
    public class IdentityContext : DbContext
    {
        private const string DefaultSchema = nameof(IdentityContext);

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<DefinedAuthorization> Authorizations { get; set; }

        public DbSet<Role> Roles { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        /// <summary>
        /// Configure model mapping
        /// Note: Many-to-many relationships at the moment needs a table entity.
        /// Waiting for a better solution, issue tracker: https://github.com/aspnet/EntityFrameworkCore/issues/1368
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DefinedAuthorization>(ConfigureAuth);
            modelBuilder.Entity<UserAuthorization>(ConfigureUserAuth);
            modelBuilder.Entity<RoleAuthorization>(ConfigureRoleAuth);

            /**
             * Configure a many-to-many relationship with users and roles.
             * A user can have more than one roles. A role can be assigned to many user.
             */
            modelBuilder.Entity<User>(ConfigureUsers);
            modelBuilder.Entity<Role>(ConfigureRoles);

            modelBuilder.Entity<UserRole>()
                .HasKey(t => new {t.RoleId, t.UserId});

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pt => pt.RoleId);

            /**
             * Configure a many-to-many relationship with users and groups.
             * A user can be a member of more than one group. A group can contains many users.
             */
            modelBuilder.Entity<Group>(ConfigureGroups);

            modelBuilder.Entity<UserGroup>()
                .HasKey(t => new {t.GroupId, t.UserId});

            modelBuilder.Entity<UserGroup>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserGroups)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(pt => pt.Group)
                .WithMany(p => p.UserGroups)
                .HasForeignKey(pt => pt.GroupId);

            /**
             * Configure a many-to-many relationship with roles and groups.
             * A group can have more than one roles. A role can be assigned to many groups.
             */
            modelBuilder.Entity<GroupRole>()
                .HasKey(t => new {t.GroupId, t.RoleId});

            modelBuilder.Entity<GroupRole>()
                .HasOne(pt => pt.Group)
                .WithMany(p => p.GroupRoles)
                .HasForeignKey(pt => pt.GroupId);

            modelBuilder.Entity<GroupRole>()
                .HasOne(pt => pt.Role)
                .WithMany(p => p.GroupRoles)
                .HasForeignKey(pt => pt.RoleId);
        }

        private void ConfigureUserAuth(EntityTypeBuilder<UserAuthorization> userAuthConfig)
        {
            userAuthConfig.ToTable("user_authorizations", DefaultSchema);
            userAuthConfig.HasKey(a => a.Id);
            userAuthConfig.Property<string>("AuthorizationName").IsRequired();
        }

        private void ConfigureRoleAuth(EntityTypeBuilder<RoleAuthorization> roleAuthConfig)
        {
            roleAuthConfig.ToTable("role_authorizations", DefaultSchema);
            roleAuthConfig.HasKey(a => a.Id);
            roleAuthConfig.Property<string>("AuthorizationName").IsRequired();
        }

        private void ConfigureAuth(EntityTypeBuilder<DefinedAuthorization> authConfig)
        {
            authConfig.ToTable("def_authorizations", DefaultSchema);
            authConfig.HasKey(a => a.Id);
            authConfig.Property<string>("Name").IsRequired();
        }

        private void ConfigureGroups(EntityTypeBuilder<Group> groupConfig)
        {
            groupConfig.ToTable("groups", DefaultSchema);
            groupConfig.HasKey(g => g.Id);
            groupConfig.Property<string>("Description").IsRequired(false);
        }

        private void ConfigureRoles(EntityTypeBuilder<Role> roleConfig)
        {
            roleConfig.ToTable("roles", DefaultSchema);
            roleConfig.HasKey(r => r.Id);
            roleConfig.Property<string>("Description").IsRequired(false);

            roleConfig.HasMany(i => i.Authorizations)
                .WithOne()
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureUsers(EntityTypeBuilder<User> usersConfig)
        {
            usersConfig.ToTable("users", DefaultSchema);
            usersConfig.HasKey(u => u.Id);
            usersConfig.Property<string>("Name").IsRequired();
            usersConfig.Property<string>("Surname").IsRequired(false);
            usersConfig.Property<string>("UserName").IsRequired();
            usersConfig.Property<string>("Password").IsRequired();
            usersConfig.Property<string>("Email").IsRequired(false);
            usersConfig.Property<bool>("Admin").IsRequired();
            usersConfig.Property<bool>("Root").IsRequired();

            usersConfig.HasMany(i => i.Authorizations)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}