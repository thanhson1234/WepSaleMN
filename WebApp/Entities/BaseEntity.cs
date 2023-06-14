namespace WebApp.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public int? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public int? UpdatedUid { get; set; }
        public int? DeletedBy { get; set; }
        public int? Status { get; set; }
        public int? Deleted { get; set; }
    }
}
