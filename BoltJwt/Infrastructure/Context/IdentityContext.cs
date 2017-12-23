using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoltJwt.Infrastructure.Context
{
    public class IdentityContext : DbContext, IUnitOfWork
    {
        private const string DefaultSchema = nameof(IdentityContext);

        // Mapped sets:

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<DefinedAuthorization> Authorizations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserActivationCode> UserActivationCodes { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        /// <summary>
        /// Save the current context
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Save result</returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Configure model mapping
        /// Note: Many-to-many relationships at the moment needs a table entity.
        /// Waiting for a better solution, issue tracker: https://github.com/aspnet/EntityFrameworkCore/issues/1368
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /**
             * Configure entity tables
             */

            modelBuilder.Entity<DefinedAuthorization>(ConfigureAuth);
            modelBuilder.Entity<UserAuthorization>(ConfigureUserAuth);
            modelBuilder.Entity<UserActivationCode>(ConfigureUserActivationCodes);
            modelBuilder.Entity<RoleAuthorization>(ConfigureRoleAuth);
            modelBuilder.Entity<Group>(ConfigureGroups);
            modelBuilder.Entity<User>(ConfigureUsers);
            modelBuilder.Entity<Role>(ConfigureRoles);

            /**
             * Configure join tables
             */

            /**
             * Configure a many-to-many relationship with users and roles.
             * A user can have more than one roles. A role can be assigned to many user.
             */
            modelBuilder.Entity<UserRole>().ToTable("user_role", DefaultSchema);
            modelBuilder.Entity<UserRole>()
                .HasKey(t => new {t.RoleId, t.UserId});

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pt => pt.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pt => pt.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            /**
             * Configure a many-to-many relationship with users and groups.
             * A user can be a member of more than one group. A group can contains many users.
             */
            modelBuilder.Entity<UserGroup>().ToTable("user_group", DefaultSchema);
            modelBuilder.Entity<UserGroup>()
                .HasKey(t => new {t.GroupId, t.UserId});

            modelBuilder.Entity<UserGroup>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserGroups)
                .HasForeignKey(pt => pt.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroup>()
                .HasOne(pt => pt.Group)
                .WithMany(p => p.UserGroups)
                .HasForeignKey(pt => pt.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            /**
             * Configure a many-to-many relationship with roles and groups.
             * A group can have more than one roles. A role can be assigned to many groups.
             */
            modelBuilder.Entity<GroupRole>().ToTable("group_role", DefaultSchema);
            modelBuilder.Entity<GroupRole>()
                .HasKey(t => new {t.GroupId, t.RoleId});

            modelBuilder.Entity<GroupRole>()
                .HasOne(pt => pt.Group)
                .WithMany(p => p.GroupRoles)
                .HasForeignKey(pt => pt.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupRole>()
                .HasOne(pt => pt.Role)
                .WithMany(p => p.GroupRoles)
                .HasForeignKey(pt => pt.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureUserAuth(EntityTypeBuilder<UserAuthorization> userAuthConfig)
        {
            userAuthConfig.ToTable("user_authorizations", DefaultSchema);
            userAuthConfig.HasKey(a => a.Id);

            userAuthConfig.HasIndex(i => new {i.DefAuthorizationId, i.UserId}).IsUnique();
        }

        private void ConfigureUserActivationCodes(EntityTypeBuilder<UserActivationCode> userActivationCodeConfig)
        {
            userActivationCodeConfig.ToTable("user_activation_codes", DefaultSchema);
            userActivationCodeConfig.HasKey(a => a.Id);
            userActivationCodeConfig.Property<int>("UserId").IsRequired();
            userActivationCodeConfig.Property<long>("Timestamp").IsRequired();
            userActivationCodeConfig.Property<string>("Code").IsRequired();
        }

        private void ConfigureRoleAuth(EntityTypeBuilder<RoleAuthorization> roleAuthConfig)
        {
            roleAuthConfig.ToTable("role_authorizations", DefaultSchema);
            roleAuthConfig.HasKey(a => a.Id);

            roleAuthConfig.HasIndex(i => new {i.DefAuthorizationId, i.RoleId}).IsUnique();
        }

        private void ConfigureAuth(EntityTypeBuilder<DefinedAuthorization> authConfig)
        {
            authConfig.ToTable("def_authorizations", DefaultSchema);
            authConfig.HasKey(a => a.Id);
            authConfig.Property<string>("Name").IsRequired();

            authConfig.HasIndex(a => a.Name).IsUnique();

            authConfig.HasMany(i => i.RolesAuthorizations)
                .WithOne()
                .HasForeignKey(p => p.DefAuthorizationId)
                .OnDelete(DeleteBehavior.Restrict);

            authConfig.HasMany(i => i.UserAuthorizations)
                .WithOne()
                .HasForeignKey(p => p.DefAuthorizationId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureGroups(EntityTypeBuilder<Group> groupConfig)
        {
            groupConfig.ToTable("groups", DefaultSchema);
            groupConfig.HasKey(g => g.Id);
            groupConfig.Property<string>("Description").IsRequired();
        }

        private void ConfigureRoles(EntityTypeBuilder<Role> roleConfig)
        {
            roleConfig.ToTable("roles", DefaultSchema);
            roleConfig.HasKey(r => r.Id);
            roleConfig.Property<string>("Description").IsRequired();

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
            usersConfig.Property<string>("Email").IsRequired();
            usersConfig.Property<bool>("Root").HasDefaultValue(false);
            usersConfig.Property<bool>("Disabled").HasDefaultValue(true);

            usersConfig.HasIndex(u => u.UserName).IsUnique();
            usersConfig.HasIndex(u => u.Email).IsUnique();

            usersConfig.HasMany(i => i.Authorizations)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}