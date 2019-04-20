using Lingsearcher.Entity;
using Lingsearcher.App_Start.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Data.Entity;
using Hangfire;
using Lingsearcher.Services;
using Lingsearcher.Filters;

[assembly: OwinStartup(typeof(ByteBank.Forum.Startup))]

namespace ByteBank.Forum
{
    public class Startup
    {
        private string ConnectionString = "Server=lingsearcher.database.windows.net;Initial Catalog=lingsearcher;MultipleActiveResultSets=true;User ID=gobatoj;Password=wjfo$nfD";

        public void Configuration(IAppBuilder builder)
        {
            //Configuracao hangfire
            GlobalConfiguration.Configuration.UseSqlServerStorage(ConnectionString);

            builder.UseHangfireDashboard();
            builder.UseHangfireServer();

            //this call placement is important
            //var options = new DashboardOptions
            //{
                //Authorization = new[] { new AuthorizationFilter() }
            //};

            /*
             * Com connection string no Web Config
                builder.CreatePerOwinContext<DbContext>(() =>
                        new IdentityDbContext<UserApplication>("DefaultConnection"));
            */
            builder.CreatePerOwinContext<DbContext>(() =>
                new IdentityDbContext<UserApplication>(ConnectionString));
            
            /*
            builder.CreatePerOwinContext<DbContext>(() =>
                new DbContextIdentity<UserApplication>());
            */
            builder.CreatePerOwinContext<IUserStore<UserApplication>>(
                (opcoes, contextoOwin) =>
                {
                    var dbContext = contextoOwin.Get<DbContext>();
                    return new UserStore<UserApplication>(dbContext);
                });

            builder.CreatePerOwinContext<UserManager<UserApplication>>(
                (opcoes, contextoOwin) =>
                {
                    var userStore = contextoOwin.Get<IUserStore<UserApplication>>();
                    var userManager = new UserManager<UserApplication>(userStore);

                    var userValidator = new UserValidator<UserApplication>(userManager)
                    {
                        RequireUniqueEmail = true
                    };

                    userManager.UserValidator = userValidator;

                    // Utilização da classe pronta de PasswordValidator que implementa IIdentityValidator
                    userManager.PasswordValidator = new PasswordValidator()
                    {
                        RequiredLength = 8,
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireUppercase = true,
                        RequireNonLetterOrDigit = true
                    };

                    userManager.EmailService = new MailService();

                    // Criação de um dataprotectionprovider para ser o do token??
                    var dataProtectionProvider = opcoes.DataProtectionProvider;
                    var dataProtectionProviderCreated = dataProtectionProvider.Create("Lingsearcher");

                    userManager.UserTokenProvider = new DataProtectorTokenProvider<UserApplication>(dataProtectionProviderCreated);

                    userManager.MaxFailedAccessAttemptsBeforeLockout = 3;
                    userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    userManager.UserLockoutEnabledByDefault = true;


                    return userManager;
                });

            builder.CreatePerOwinContext<SignInManager<UserApplication, string>>(
                (opcoes, contextoOwin) =>
                {
                    var userManager = contextoOwin.Get<UserManager<UserApplication>>();

                    var SignInManager = new SignInManager<UserApplication, string>
                    (
                        userManager,
                        contextoOwin.Authentication
                    );

                    return SignInManager;
                });

            builder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName = "UserApplicationCookie",
                ExpireTimeSpan = TimeSpan.FromMinutes(60)
            });

            //Agendamento do job de atualizacao de precos diario
            RecurringJob.AddOrUpdate(() => new ProductUpdateService().UpdatePrices(), Cron.Daily);
        }
    }
}