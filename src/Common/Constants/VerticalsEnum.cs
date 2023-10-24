namespace Platform.Infrastructure.Common.Constants
{
    using System.ComponentModel;

    /// <summary>Shohoz verticals.</summary>
    public enum ShohozVerticalsEnum
    {
        /// <summary>The default</summary>
        [Description("00000000-0000-0000-0000-000000000000")]
        DEFAULT,

        /// <summary>The food</summary>
        [Description("3799ab80-cf73-4dbf-90b5-6ac97c0db835")]
        FOOD,

        /// <summary>The ride</summary>
        [Description("cf05efc8-d972-42cf-acde-d702cbbebc95")]
        RIDE,

        /// <summary>The ticket</summary>
        [Description("897b0e7d-b32a-4594-be28-fae39d2f0f8b")]
        TICKET,

        /// <summary>The truck</summary>
        [Description("023a3032-2e20-4068-a936-d3b743bbb328")]
        TRUCK,

        /// <summary>The telemedicine</summary>
        [Description("c1776ad6-6ce3-4349-bdd3-c6729e08e1e5")]
        TELEMEDICINE,

        /// <summary>The vehicle rental</summary>
        [Description("814f184e-4bcd-47dc-a8dd-f5a53d5781dd")]
        VEHICLE_RENTAL,
    }
}
