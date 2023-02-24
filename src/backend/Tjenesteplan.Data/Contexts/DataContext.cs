using Microsoft.EntityFrameworkCore;
using Tjenesteplan.Data.Features.Avdelinger;
using Tjenesteplan.Data.Features.Invitations;
using Tjenesteplan.Data.Features.Notifications;
using Tjenesteplan.Data.Features.Sykehus;
using Tjenesteplan.Data.Features.Tjenesteplan.Data;
using Tjenesteplan.Data.Features.TjenesteplanChanges;
using Tjenesteplan.Data.Features.Vakansvakter;
using Tjenesteplan.Data.Features.VaktChangeRequests;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<InvitationEntity> Invitations { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<SykehusEntity> Sykehus { get; set; }
        public DbSet<AvdelingEntity> Avdelinger { get; set; }
        public DbSet<TjenesteplanEntity> Tjenesteplaner { get; set; }
        public DbSet<TjenesteplanChangeEntity> TjenesteplanChanges { get; set; }
        public DbSet<VaktChangeRequestEntity> VaktChangeRequests { get; set; }

        public DbSet<VakansvaktRequestEntity> VakansvaktRequests { get; set; }

        public DbSet<VaktChangeRequestReplyEntity> VaktChangeRequestReplies { get; set; }
        public DbSet<VaktChangeAlternativeEntity> VaktChangeAlternatives { get; set; }

		public DbSet<NotificationEntity> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvitationEntity>()
                .ToTable("Invitations")
                .HasKey(k => k.Guid);

            modelBuilder.Entity<UserEntity>()
                .ToTable("Users");

            modelBuilder.Entity<SykehusEntity>()
                .ToTable("Sykehus");

            modelBuilder.Entity<AvdelingEntity>()
                .ToTable("Avdelinger");

            modelBuilder.Entity<UserAvdelingEntity>()
                .ToTable("UserAvdelinger")
                .HasKey(ua => new { ua.UserId, ua.AvdelingId });

            modelBuilder.Entity<TjenesteplanEntity>()
                .ToTable("Tjenesteplaner");

            modelBuilder.Entity<UserTjenesteplanEntity>()
                .ToTable("UserTjenesteplan")
                .HasKey(v => new { v.UserId, v.TjenesteplanId });

            modelBuilder.Entity<TjenesteplanChangeEntity>()
                .ToTable("TjenesteplanChanges");

            modelBuilder.Entity<TjenesteplanUkeEntity>()
                .ToTable("TjenesteplanUker")
                .HasKey(v => new { v.TjenesteplanId, v.TjenesteplanUkeId });

            modelBuilder.Entity<TjenesteplanUkedagEntity>()
                .ToTable("TjenesteplanUkedager");

            modelBuilder.Entity<VaktChangeRequestEntity>()
                .ToTable("VaktChangeRequests");

            modelBuilder.Entity<VaktChangeRequestReplyEntity>()
                .ToTable("VaktChangeRequestReplies");

            modelBuilder.Entity<VaktChangeAlternativeEntity>()
                .ToTable("VaktChangeAlternatives");

            modelBuilder.Entity<VakansvaktRequestEntity>()
                .ToTable("VakansvaktRequests");

            modelBuilder.Entity<NotificationEntity>()
		        .ToTable("Notifications");

            modelBuilder.Entity<InvitationEntity>().Property(i => i.Email).IsRequired();

            modelBuilder.Entity<InvitationEntity>()
                .HasOne(i => i.Avdeling)
                .WithMany()
                .HasForeignKey(i => i.AvdelingId);

            modelBuilder.Entity<AvdelingEntity>()
                .HasOne(a => a.Sykehus)
                .WithMany(s => s.Avdelinger)
                .HasForeignKey(t => t.SykehusId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AvdelingEntity>()
                .HasOne(a => a.Listefører)
                .WithMany()
                .HasForeignKey(t => t.ListeforerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

             modelBuilder.Entity<UserAvdelingEntity>()
                .HasOne(a => a.User)
                .WithMany(t => t.Avdelinger)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

             modelBuilder.Entity<UserAvdelingEntity>()
                .HasOne(a => a.Avdeling)
                .WithMany(a => a.Users)
                .HasForeignKey(t => t.AvdelingId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<TjenesteplanChangeEntity>()
                .Property(p => p.ChangeDate).HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<TjenesteplanEntity>()
                .HasOne(t => t.Avdeling)
                .WithMany(a => a.Tjenesteplaner)
                .HasForeignKey(t => t.AvdelingId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<UserTjenesteplanEntity>()
                .HasOne(ut => ut.Tjenesteplan)
                .WithMany(t => t.Leger)
                .HasForeignKey(ut => ut.TjenesteplanId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<UserTjenesteplanEntity>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.Tjenesteplaner)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<TjenesteplanChangeEntity>()
                .HasOne(t => t.Tjenesteplan)
                .WithMany()
                .HasForeignKey(t => t.TjenesteplanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TjenesteplanChangeEntity>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TjenesteplanChangeEntity>()
                .HasOne(t => t.VaktChangeRequest)
                .WithMany()
                .HasForeignKey(t => t.VaktChangeRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<TjenesteplanUkeEntity>()
                .HasOne(v => v.Tjenesteplan)
                .WithMany(t => t.Weeks)
                .HasForeignKey(t => t.TjenesteplanId);

            modelBuilder.Entity<TjenesteplanUkeEntity>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TjenesteplanUkedagEntity>()
                .HasOne(v => v.TjenesteplanUke)
                .WithMany(t => t.Days)
                .HasForeignKey(t => new {t.TjenesteplanId, t.TjenesteplanUkeId});

            modelBuilder.Entity<VaktChangeRequestEntity>()
                .HasOne(v => v.Tjenesteplan)
                .WithMany(t => t.VaktChangeRequests)
                .HasForeignKey(v => v.TjenesteplanId);

            modelBuilder.Entity<VaktChangeRequestEntity>()
                .HasOne(v => v.User)
                .WithMany(t => t.VaktChangeRequests)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VaktChangeRequestEntity>()
                .HasOne(v => v.VaktChangeChosenAlternative)
                .WithMany()
                .HasForeignKey(v => v.VaktChangeChosenAlternativeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<VaktChangeRequestReplyEntity>()
                .HasOne(v => v.VaktChangeRequest)
                .WithMany(t => t.VaktChangeRequestsReplies)
                .HasForeignKey(v => v.VaktChangeRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VaktChangeRequestReplyEntity>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VaktChangeAlternativeEntity>()
                .HasOne(v => v.VaktChangeRequestReply)
                .WithMany(t => t.VaktChangeRequestAlternatives)
                .HasForeignKey(v => v.VaktChangeRequestReplyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VakansvaktRequestEntity>()
                .HasOne(v => v.OriginalLege)
                .WithMany()
                .HasForeignKey(v => v.OriginalLegeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VakansvaktRequestEntity>()
                .HasOne(v => v.CoveredByLege)
                .WithMany()
                .HasForeignKey(v => v.CoveredByLegeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VakansvaktRequestEntity>()
                .HasOne(v => v.Tjenesteplan)
                .WithMany(t => t.VakansvaktRequests)
                .HasForeignKey(v => v.TjenesteplanId);

            modelBuilder.Entity<NotificationEntity>()
		        .HasOne(n => n.User)
		        .WithMany()
		        .HasForeignKey(n => n.UserId)
		        .OnDelete(DeleteBehavior.Cascade);
        }
    }

}