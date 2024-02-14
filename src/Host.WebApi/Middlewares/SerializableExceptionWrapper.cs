namespace Platform.Infrastructure.Host.WebApi.Middlewares
{
    using System;

    internal class SerializableExceptionWrapper
    {
        public string Message { get; }

        public string StackTrace { get; }

        public int HResult { get; set; }

        public string TargetSite { get; }

        public string HelpLink { get; set; }

        public SerializableExceptionWrapper(Exception e)
        {
            this.StackTrace = e.StackTrace;
            this.Message = e.Message;
            this.HelpLink = e.HelpLink;
            this.HResult = e.HResult;
            this.TargetSite = $"Method : {e.TargetSite?.Name} Class: {e.TargetSite?.DeclaringType?.AssemblyQualifiedName}";
        }
    }
}
