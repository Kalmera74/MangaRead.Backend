using MangaRead.Application;
using MangaRead.Infrastructure;
using Serilog;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

            builder.Services.AddApplication().AddInfrastructure(builder.Configuration, builder.Environment);

            builder.Services.Configure<FileUploadOptions>(builder.Configuration.GetSection("FileUpload"));

            builder.Services.Configure<BucketOptions>(builder.Configuration.GetSection("BucketDetails"));



        }


        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

        builder.Host.UseSerilog();

        var app = builder.Build();


        if (Environment.GetEnvironmentVariable("SHOULD_SEED") == "true")
        {
            app.Services.Seed();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Mangaluck API");
                opt.RoutePrefix = string.Empty;
                opt.DocumentTitle = "Mangaluck API";
            });
            app.UseDeveloperExceptionPage();
        }

        app.UseSerilogRequestLogging();
        app.MapControllers();

        RegisterListeningInterfaces(app);

        app.Run();

    }
    static void RegisterListeningInterfaces(WebApplication app)
    {
        var url = app.Configuration.GetValue<string>("HostSettings:Url");

        if (!string.IsNullOrEmpty(url))
        {
            app.Urls.Add(url);
        }
    }
}
