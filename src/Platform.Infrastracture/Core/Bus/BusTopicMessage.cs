namespace Platform.Infrastructure.Core.Bus
{
    /// <summary>
    ///   <para>Bust tompic message abstraction.</para>
    /// </summary>
    /// <seealso cref="Platform.Infrastructure.Core.Bus.Message" />
    /// <seealso cref="Platform.Infrastructure.Core.Bus.IBusTopicMessage" />
    public abstract class BusTopicMessage : Message, IBusTopicMessage
    {
        public string TopicName { get; set; }
    }
}
