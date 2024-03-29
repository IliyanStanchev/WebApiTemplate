using Serilog;
using Serilog.Core;
using ILogger = Serilog.ILogger;

namespace WebApiTemplate.Web.Adapters
{
    public static class SerilogAdapter
	{
		private static Logger CreateLogger(IConfiguration configuration, string serviceName)
        {
            return new LoggerConfiguration()
                   .ReadFrom.Configuration(configuration)
                   .Enrich.WithProperty("Source", serviceName)
                   .Enrich.FromLogContext()
                   .MaskSensitiveData()
                   .CreateLogger();
        }

		public static ILogger CreateLogger(IConfiguration configuration, IServiceCollection services, string serviceName, string serviceVersion)
		{
			var logger = CreateLogger(configuration, serviceName);

            Log.Information("Configuring web host ({ServiceName} version {ServiceVersion})...",
                            serviceName,
                            serviceVersion);

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger));
            return logger;
        }

        public static LoggerConfiguration MaskSensitiveData(this LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration;
        }
    }
}
