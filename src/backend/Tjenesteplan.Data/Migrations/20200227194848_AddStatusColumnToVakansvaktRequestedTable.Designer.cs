﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tjenesteplan.Data.Contexts;

namespace Tjenesteplan.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200227194848_AddStatusColumnToVakansvaktRequestedTable")]
    partial class AddStatusColumnToVakansvaktRequestedTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Tjenesteplan.Data.Features.Invitations.InvitationEntity", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("Role");

                    b.HasKey("Guid");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.Notifications.NotificationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<bool>("IsRead");

                    b.Property<string>("Title");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfWeeks");

                    b.Property<DateTime>("StartDate");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tjenesteplaner");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanUkedagEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Dagsplan");

                    b.Property<DateTime>("Date");

                    b.Property<int>("TjenesteplanId");

                    b.Property<int>("TjenesteplanUkeId");

                    b.HasKey("Id");

                    b.HasIndex("TjenesteplanId", "TjenesteplanUkeId");

                    b.ToTable("TjenesteplanUkedager");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanUkeEntity", b =>
                {
                    b.Property<int>("TjenesteplanId");

                    b.Property<int>("TjenesteplanUkeId");

                    b.Property<int?>("UserId");

                    b.HasKey("TjenesteplanId", "TjenesteplanUkeId");

                    b.HasIndex("UserId");

                    b.ToTable("TjenesteplanUker");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.TjenesteplanChanges.TjenesteplanChangeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ChangeDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<int>("Dagsplan");

                    b.Property<DateTime>("Date");

                    b.Property<int>("TjenesteplanId");

                    b.Property<int>("UserId");

                    b.Property<int?>("VaktChangeRequestId");

                    b.HasKey("Id");

                    b.HasIndex("TjenesteplanId");

                    b.HasIndex("UserId");

                    b.HasIndex("VaktChangeRequestId");

                    b.ToTable("TjenesteplanChanges");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.Vakansvakter.VakansvaktRequestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CoveredByLegeId");

                    b.Property<int>("Dagsplan");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Message");

                    b.Property<int>("OriginalLegeId");

                    b.Property<DateTime>("RegisteredDate");

                    b.Property<int>("Status");

                    b.Property<int>("TjenesteplanId");

                    b.HasKey("Id");

                    b.HasIndex("CoveredByLegeId");

                    b.HasIndex("OriginalLegeId");

                    b.HasIndex("TjenesteplanId");

                    b.ToTable("VakansvaktRequests");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.VaktChangeRequests.VaktChangeAlternativeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int>("VaktChangeRequestReplyId");

                    b.HasKey("Id");

                    b.HasIndex("VaktChangeRequestReplyId");

                    b.ToTable("VaktChangeAlternatives");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.VaktChangeRequests.VaktChangeRequestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Dagsplan");

                    b.Property<DateTime>("Date");

                    b.Property<DateTime>("RequestRegisteredDate");

                    b.Property<int>("Status");

                    b.Property<int>("TjenesteplanId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TjenesteplanId");

                    b.HasIndex("UserId");

                    b.ToTable("VaktChangeRequests");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.VaktChangeRequests.VaktChangeRequestReplyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("NumberOfRemindersSent");

                    b.Property<int>("Status");

                    b.Property<int>("UserId");

                    b.Property<int>("VaktChangeRequestId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VaktChangeRequestId");

                    b.ToTable("VaktChangeRequestReplies");
                });

            modelBuilder.Entity("WebApi.Features.Users.Data.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("ResetPasswordToken");

                    b.Property<int>("Role");

                    b.Property<int?>("TjenesteplanId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("TjenesteplanId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.Notifications.NotificationEntity", b =>
                {
                    b.HasOne("WebApi.Features.Users.Data.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanEntity", b =>
                {
                    b.HasOne("WebApi.Features.Users.Data.UserEntity", "User")
                        .WithMany("AdminTjenesteplaner")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanUkedagEntity", b =>
                {
                    b.HasOne("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanUkeEntity", "TjenesteplanUke")
                        .WithMany("Days")
                        .HasForeignKey("TjenesteplanId", "TjenesteplanUkeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanUkeEntity", b =>
                {
                    b.HasOne("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanEntity", "Tjenesteplan")
                        .WithMany("Weeks")
                        .HasForeignKey("TjenesteplanId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Features.Users.Data.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.TjenesteplanChanges.TjenesteplanChangeEntity", b =>
                {
                    b.HasOne("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanEntity", "Tjenesteplan")
                        .WithMany()
                        .HasForeignKey("TjenesteplanId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Features.Users.Data.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Tjenesteplan.Data.Features.VaktChangeRequests.VaktChangeRequestEntity", "VaktChangeRequest")
                        .WithMany()
                        .HasForeignKey("VaktChangeRequestId");
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.Vakansvakter.VakansvaktRequestEntity", b =>
                {
                    b.HasOne("WebApi.Features.Users.Data.UserEntity", "CoveredByLege")
                        .WithMany()
                        .HasForeignKey("CoveredByLegeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WebApi.Features.Users.Data.UserEntity", "OriginalLege")
                        .WithMany()
                        .HasForeignKey("OriginalLegeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanEntity", "Tjenesteplan")
                        .WithMany("VakansvaktRequests")
                        .HasForeignKey("TjenesteplanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.VaktChangeRequests.VaktChangeAlternativeEntity", b =>
                {
                    b.HasOne("Tjenesteplan.Data.Features.VaktChangeRequests.VaktChangeRequestReplyEntity", "VaktChangeRequestReply")
                        .WithMany("VaktChangeRequestAlternatives")
                        .HasForeignKey("VaktChangeRequestReplyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.VaktChangeRequests.VaktChangeRequestEntity", b =>
                {
                    b.HasOne("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanEntity", "Tjenesteplan")
                        .WithMany("VaktChangeRequests")
                        .HasForeignKey("TjenesteplanId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Features.Users.Data.UserEntity", "User")
                        .WithMany("VaktChangeRequests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Tjenesteplan.Data.Features.VaktChangeRequests.VaktChangeRequestReplyEntity", b =>
                {
                    b.HasOne("WebApi.Features.Users.Data.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Tjenesteplan.Data.Features.VaktChangeRequests.VaktChangeRequestEntity", "VaktChangeRequest")
                        .WithMany("VaktChangeRequestsReplies")
                        .HasForeignKey("VaktChangeRequestId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("WebApi.Features.Users.Data.UserEntity", b =>
                {
                    b.HasOne("Tjenesteplan.Data.Features.Tjenesteplan.Data.TjenesteplanEntity", "Tjenesteplan")
                        .WithMany("Leger")
                        .HasForeignKey("TjenesteplanId");
                });
#pragma warning restore 612, 618
        }
    }
}
