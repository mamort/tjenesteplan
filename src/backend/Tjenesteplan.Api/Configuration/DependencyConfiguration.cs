using Microsoft.Extensions.DependencyInjection;
using Tjenesteplan.Api.Features.MinTjenesteplan.GetMineTjenesteplaner;
using Tjenesteplan.Api.Features.MinTjenesteplan.GetMinTjenesteplan;
using Tjenesteplan.Api.Features.Tjenesteplaner.AddLege;
using Tjenesteplan.Api.Features.Tjenesteplaner.Changes;
using Tjenesteplan.Api.Features.Tjenesteplaner.ChangeTjenesteplanDateForLege;
using Tjenesteplan.Api.Features.Tjenesteplaner.CreateTjenesteplan;
using Tjenesteplan.Api.Features.Tjenesteplaner.EditTjenesteplan;
using Tjenesteplan.Api.Features.Tjenesteplaner.GetTjenesteplan;
using Tjenesteplan.Api.Features.Tjenesteplaner.GetWeeklyTjenesteplan;
using Tjenesteplan.Api.Features.Tjenesteplaner.RemoveLege;
using Tjenesteplan.Api.Features.UserInvitations.RegisterInvitation;
using Tjenesteplan.Api.Features.Users.AddUserToAvdeling;
using Tjenesteplan.Api.Features.Users.AutheticatedUser;
using Tjenesteplan.Api.Features.Users.DeleteUser;
using Tjenesteplan.Api.Features.Users.NewPassword;
using Tjenesteplan.Api.Features.Users.RegisterUser;
using Tjenesteplan.Api.Features.Users.RemoveLegeFromAvdeling;
using Tjenesteplan.Api.Features.Users.ResetPassword;
using Tjenesteplan.Api.Features.Users.UpdateUser;
using Tjenesteplan.Api.Features.Users.ViewUser;
using Tjenesteplan.Api.Features.VakansvaktRequests.AcceptVakansvaktRequest;
using Tjenesteplan.Api.Features.VakansvaktRequests.AddVakansvaktRequest;
using Tjenesteplan.Api.Features.VakansvaktRequests.ApproveVakansvaktRequest;
using Tjenesteplan.Api.Features.VakansvaktRequests.GetAvailableVakansvaktRequests;
using Tjenesteplan.Api.Features.VakansvaktRequests.GetVakansvaktRequest;
using Tjenesteplan.Api.Features.VakansvaktRequests.GetVakansvaktRequests;
using Tjenesteplan.Api.Features.VakansvaktRequests.RejectVakansvaktRequest;
using Tjenesteplan.Api.Features.VakansvaktRequests.UndoVakansvaktRequest;
using Tjenesteplan.Api.Features.VaktChangeRequests.AcceptVaktChange;
using Tjenesteplan.Api.Features.VaktChangeRequests.AddVaktChangeRequest;
using Tjenesteplan.Api.Features.VaktChangeRequests.AllRequests;
using Tjenesteplan.Api.Features.VaktChangeRequests.ReceivedRequests;
using Tjenesteplan.Api.Features.VaktChangeRequests.SentRequests;
using Tjenesteplan.Api.Features.VaktChangeRequests.UndoVaktChangeRequest;
using Tjenesteplan.Api.Features.VaktChangeRequests.VaktChangeSuggestions;
using Tjenesteplan.Api.Services.Email;
using Tjenesteplan.Api.Services.Events;
using Tjenesteplan.Api.Services.JwtToken;
using Tjenesteplan.Api.Services.PasswordHash;
using Tjenesteplan.Data.Features.Avdelinger;
using Tjenesteplan.Data.Features.Invitations;
using Tjenesteplan.Data.Features.Notifications;
using Tjenesteplan.Data.Features.Sykehus;
using Tjenesteplan.Data.Features.Tjenesteplan;
using Tjenesteplan.Data.Features.TjenesteplanChanges;
using Tjenesteplan.Data.Features.Users;
using Tjenesteplan.Data.Features.Users.Data;
using Tjenesteplan.Data.Features.Vakansvakter;
using Tjenesteplan.Data.Features.VaktChangeRequests;
using Tjenesteplan.Domain.Api;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Framework;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Domain.Services.Holiday;
using Tjenesteplan.Events;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Api.Configuration
{
    public static class DependencyConfiguration
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHashService, PasswordHashService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IPersistentQueueFactory, AzureQueueFactory>();

            services.AddScoped<AuthenticateUserAction, AuthenticateUserAction>();
            services.AddScoped<RegisterUserAction, RegisterUserAction>();
            services.AddScoped<UpdateUserAction, UpdateUserAction>();
            services.AddScoped<DeleteUserAction, DeleteUserAction>();
            services.AddScoped<GetAllUsersAction, GetAllUsersAction>();
            services.AddScoped<GetLegerAction, GetLegerAction>();
            services.AddScoped<GetUserAction, GetUserAction>();
            services.AddScoped<GetTjenesteplanAction, GetTjenesteplanAction>();
            services.AddScoped<CreateTjenesteplanAction, CreateTjenesteplanAction>();
            services.AddScoped<EditTjenesteplanAction, EditTjenesteplanAction>();
            services.AddScoped<AddLegeToTjenesteplanAction, AddLegeToTjenesteplanAction>();
            services.AddScoped<RemoveLegeFromTjenesteplanAction, RemoveLegeFromTjenesteplanAction>();
            services.AddScoped<GetMinTjenesteplanAction, GetMinTjenesteplanAction>();
            services.AddScoped<GetMineTjenesteplanerAction, GetMineTjenesteplanerAction>();
            services.AddScoped<GetAllRequestsAction, GetAllRequestsAction>();
            services.AddScoped<AddVaktChangeRequestAction, AddVaktChangeRequestAction>();
            services.AddScoped<GetSentRequestsAction, GetSentRequestsAction>();
            services.AddScoped<GetVaktChangeRequestsReceivedAction, GetVaktChangeRequestsReceivedAction>();
            services.AddScoped<AddVaktChangeSuggestionsAction, AddVaktChangeSuggestionsAction>();
            services.AddScoped<AcceptVaktChangeAction, AcceptVaktChangeAction>();
            services.AddScoped<ResetPasswordAction, ResetPasswordAction>();
            services.AddScoped<NewPasswordAction, NewPasswordAction>();
            services.AddScoped<GetWeeklyTjenesteplanAction, GetWeeklyTjenesteplanAction>();
            services.AddScoped<ChangeTjenesteplanDateForLegeAction, ChangeTjenesteplanDateForLegeAction>();
            services.AddScoped<AddVakansvaktRequestAction, AddVakansvaktRequestAction>();
            services.AddScoped<GetVakansvaktRequestsAction, GetVakansvaktRequestsAction>();
            services.AddScoped<GetVakansvaktRequestAction, GetVakansvaktRequestAction>();
            services.AddScoped<ApproveVakansvaktRequestAction, ApproveVakansvaktRequestAction>();
            services.AddScoped<RejectVakansvaktRequestAction, RejectVakansvaktRequestAction>();
            services.AddScoped<AcceptVakansvaktRequestAction, AcceptVakansvaktRequestAction>();
            services.AddScoped<UndoVakansvaktRequestAction, UndoVakansvaktRequestAction>();
            services.AddScoped<GetAvailableVakansvaktRequestsAction, GetAvailableVakansvaktRequestsAction>();
            services.AddScoped<RegisterInvitationAction, RegisterInvitationAction>();
            services.AddScoped<RemoveLegeFromAvdelingAction, RemoveLegeFromAvdelingAction>();
            services.AddScoped<AddUserToAvdelingAction, AddUserToAvdelingAction>();
            services.AddScoped<TjenesteplanChangesAction, TjenesteplanChangesAction>();
            services.AddScoped<UndoVaktChangeRequestAction, UndoVaktChangeRequestAction>();
            
            services.AddScoped<IUserRepository, CachedUserRepository>();
            services.AddScoped<INonCachedUserRepository, UserRepository>();

            services.AddScoped<ITjenesteplanRepository, TjenesteplanRepository>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<IVaktChangeRequestRepository, VaktChangeRequestRepository>();
            services.AddScoped<IVaktChangeRequestRepliesRepository, VaktChangeRequestRepliesRepository>();
            services.AddScoped<ITjenesteplanChangesRepository, TjenesteplanChangesRepository>();

            services.AddScoped<INotificationRepository, CachedNotificationsRepository>();
            services.AddScoped<INonCachedNotificationsRepository, NotificationRepository>();

            services.AddScoped<IVakansvaktRequestRepository, VakansvaktRequestRepository>();
            services.AddScoped<ISykehusRepository, SykehusRepository>();
            services.AddScoped<IAvdelingRepository, AvdelingRepository>();
        }
    }
}
 