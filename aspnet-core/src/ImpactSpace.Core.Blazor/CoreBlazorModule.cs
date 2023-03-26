using System.IO;
using System.Security.Cryptography.X509Certificates;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ImpactSpace.Core.Blazor.Menus;
using ImpactSpace.Core.EntityFrameworkCore;
using ImpactSpace.Core.Localization;
using ImpactSpace.Core.MultiTenancy;
using Microsoft.Extensions.Logging;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme;
using Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Blazor.Server;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.SettingManagement.Blazor.Server;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Blazor.Server;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace ImpactSpace.Core.Blazor;

[DependsOn(
    typeof(CoreApplicationModule),
    typeof(CoreEntityFrameworkCoreModule),
    typeof(CoreHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreComponentsServerLeptonXLiteThemeModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpIdentityBlazorServerModule),
    typeof(AbpTenantManagementBlazorServerModule),
    typeof(AbpSettingManagementBlazorServerModule)
   )]
public class CoreBlazorModule : AbpModule
{
    private readonly ILogger<CoreBlazorModule> _logger;

    public CoreBlazorModule(ILogger<CoreBlazorModule> logger)
    {
        _logger = logger;
    }
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(CoreResource),
                typeof(CoreDomainModule).Assembly,
                typeof(CoreDomainSharedModule).Assembly,
                typeof(CoreApplicationModule).Assembly,
                typeof(CoreApplicationContractsModule).Assembly,
                typeof(CoreBlazorModule).Assembly
            );
        });
        
        PreConfigureCertificatesForOpenIddict(context);

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("Core");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
    }

    private void PreConfigureCertificatesForOpenIddict(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        _logger.LogInformation("Environment: " + hostingEnvironment.EnvironmentName);
        PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
        {
            if (hostingEnvironment.IsProduction())
            {
                //https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            }
        });

        
        PreConfigure<OpenIddictServerBuilder>(options =>
        {
            if (hostingEnvironment.IsProduction())
            {
                options.AddEncryptionCertificate(LoadCertificate("7785791AA3CCC380C5D9CE9F94BB72E28234F04F"));
                options.AddSigningCertificate(LoadCertificate("7C6478219A691D940D25745280A29DC2FAC40BA8"));
            }
        });
        
        
    }

    private X509Certificate2 LoadCertificate(string thumbprint)
    {
        var bytes = File.ReadAllBytes($"/var/ssl/private/{thumbprint}.p12");
        return new X509Certificate2(bytes);
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureAuthentication(context);
        ConfigureUrls(configuration);
        ConfigureBundles();
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureSwaggerServices(context.Services);
        ConfigureAutoApiControllers();
        ConfigureBlazorise(context);
        ConfigureRouter(context);
        ConfigureMenu(context);
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            // MVC UI
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );

            //BLAZOR UI
            options.StyleBundles.Configure(
                BlazorLeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/blazor-global-styles.css");
                    //You can remove the following line if you don't use Blazor CSS isolation for components
                    bundle.AddFiles("/ImpactSpace.Core.Blazor.styles.css");
                }
            );
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<CoreDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ImpactSpace.Core.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<CoreDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ImpactSpace.Core.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<CoreApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ImpactSpace.Core.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<CoreApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ImpactSpace.Core.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<CoreBlazorModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Core API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    private void ConfigureBlazorise(ServiceConfigurationContext context)
    {
        context.Services
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons();
    }

    private void ConfigureMenu(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CoreMenuContributor());
        });
    }

    private void ConfigureRouter(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(CoreBlazorModule).Assembly;
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(CoreApplicationModule).Assembly);
        });
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CoreBlazorModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder();

        app.UseAbpRequestLocalization();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API");
        });
        app.UseConfiguredEndpoints();
    }
}
