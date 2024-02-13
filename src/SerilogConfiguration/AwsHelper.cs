namespace Platform.Infrastructure.LogConfiguration.SerilogConfiguration
{
    using System;
    using Amazon;
    using Amazon.CloudWatchLogs;
    using Amazon.Runtime.CredentialManagement;
    using Serilog.Sinks.AwsCloudWatch;
    using Platform.Infrastructure.LogConfiguration.Abstraction;

    public static class AwsHelper
    {
        public static CloudWatchSinkOptions GetCloudWatchSinkOptions(
            string logGroupName,
            int batchSize,
            int queueSize,
            LogLevel minimumLogLevel)
        {
            return new CloudWatchSinkOptions()
            {
                LogGroupName = logGroupName,
                MinimumLogEventLevel = SeriLogLevelProvider.GetLogLevel(minimumLogLevel),
                BatchSizeLimit = batchSize,
                QueueSizeLimit = queueSize,
                Period = TimeSpan.FromSeconds(10),
                CreateLogGroup = true,
                LogStreamNameProvider = new DefaultLogStreamProvider(),
                RetryAttempts = 3,
                TextFormatter = new AwsCustomTextFormatter(),
            };
        }

        public static AmazonCloudWatchLogsClient GetAwsCloudWatchClient(string awsRegion)
        {
            return new AmazonCloudWatchLogsClient(RegionEndpoint.GetBySystemName(awsRegion));
        }

        public static void RegisterProfile(LogSettings logSettings)
        {
            var options = new CredentialProfileOptions { AccessKey = logSettings.AwsAccessKey, SecretKey = logSettings.AwsSecretKey };
            var profile = new CredentialProfile("default", options);
            profile.Region = RegionEndpoint.GetBySystemName(logSettings.AwsRegion);
            var netSDKFile = new SharedCredentialsFile();
            netSDKFile.RegisterProfile(profile);
        }
    }
}
