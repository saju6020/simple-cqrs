namespace Platform.Infrastructure.Core.Accessors
{
    using System;
    using System.Threading;

    public class CorrelationIdAccessor : ICorrelationIdAccessor
    {
        private static readonly AsyncLocal<Guid> CurrentCorrelationId = new AsyncLocal<Guid>();

        private static readonly AsyncLocal<Guid> ScopeCorrelationId = new AsyncLocal<Guid>();

        public Guid Id
        {
            get
            {
                return CurrentCorrelationId.Value;
            }

            set
            {
                CurrentCorrelationId.Value = value;
            }
        }

        public Guid ScopeId
        {
            get
            {
                return ScopeCorrelationId.Value;
            }

            set
            {
                ScopeCorrelationId.Value = value;
            }
        }
    }
}
