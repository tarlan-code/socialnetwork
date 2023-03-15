namespace Zust.Application.ViewModels
{
    public class EventCardVM
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public bool IsIdenUserEvent { get; set; }
        public bool IsAttend { get; set; }
    }
}
