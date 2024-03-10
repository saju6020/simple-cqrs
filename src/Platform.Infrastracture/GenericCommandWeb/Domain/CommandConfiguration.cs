namespace GenericCommandWeb.Domain
{
        public class CommandConfiguration
        {
            public string TenantId { get; set; }
            public string VerticalId { get; set; }
            public List<CommandRoles> CommandRoles { get; set; }
        }
}
