namespace Blog.ORM.Models
{
    public class BaseEntity
    {        

        public bool IsPublished { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set;}

        public DateTime? PublishedOn { get; set; }
    }
}
