namespace Zust.Domain.Entities
{
    public class BaseEntity:IBaseEntity
    {
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
