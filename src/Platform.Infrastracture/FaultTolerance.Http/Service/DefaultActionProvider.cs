namespace Platform.Infrastructure.FaultTolerance.Http.Service
{
    using System;
    using System.Net.Http;
    using Polly;
    using Serilog;
    using Platform.Infrastructure.CustomException;

    /// <summary>
    /// This class is responsible for providing default actions for onBreak,onReset,onTry
    /// </summary>
    public class DefaultActionProvider
    {
        /// <summary>
        /// Gets the on break action.
        /// </summary>
        /// <returns> returns action. </returns>
        public static Action<DelegateResult<HttpResponseMessage>, TimeSpan> GetOnBreakAction()
        {
            // NullLogger<DefaultActionProvider>.Instance.LogInformation();
            return (exception, timeSpan) =>
            {
                throw new BaseException(exception.Exception.Message);
            };
        }

        /// <summary>
        /// Gets the on reset action.
        /// </summary>
        /// <returns> returns Action.</returns>
        public static Action GetOnResetAction()
        {
            return () => { };
        }

        /// <summary>
        /// Gets the on try action.
        /// </summary>
        /// <returns> returns Action.</returns>
        public static Action<DelegateResult<HttpResponseMessage>, TimeSpan> GetOnTryAction()
        {
            return (exception, timeSpan) =>
            {
                Log.Logger.Debug(exception.Exception.Message);
                Log.Logger.Debug("Retrying Again");
            };
        }
    }
}
