using FluentValidation;

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.StaticFiles;

using Uni.Database;
using Uni.Models.Forms;
using Uni.Transformers;
using Uni.Validators;

using WebMarkupMin.AspNetCore7;

// Configure services
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHsts(options =>
{
    //options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromSeconds(31536000);
});

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin()));

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);

        options.LoginPath = "/Account/Login";
    });

builder.Services.AddDbContext<UniContext>();

builder.Services.AddRazorPages(options =>
    options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer())));

builder.Services.AddWebMarkupMin(options => options.DisablePoweredByHttpHeaders = true)
    .AddHtmlMinification();

// BookingFormModel validation
builder.Services.AddScoped<IValidator<BookingFormModel>, BookingFormModelValidator>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    app.UseExceptionHandler("/exception");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStatusCodePagesWithReExecute("/error/{0}");

#region Static files

FileExtensionContentTypeProvider mimeProvider = new();

if (app.Environment.IsDevelopment())
{
    mimeProvider.Mappings[".less"] = "text/prs.less";
    mimeProvider.Mappings[".ts"] = "text/prs.typescript";
}

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = mimeProvider
});

#endregion

app.UseWebMarkupMin();

app.UseRouting();

app.UseCors();

#region Security headers

if (!app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers.TryAdd("Content-Security-Policy",
            "default-src 'self'; frame-ancestors 'self'; frame-src 'self' https://yandex.ru/");
        context.Response.Headers.TryAdd("X-Frame-Options", "SAMEORIGIN");
        context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
        context.Response.Headers.TryAdd("Referrer-Policy", "no-referrer-when-downgrade");
        await next();
    });
}

#endregion

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();
