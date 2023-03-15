namespace Zust.Application.ViewModels
{
    public class HeaderVM
    {
        public List<NotificationVM> Notifications { get; set; }
        public List<RequestVM> Requests { get; set; }
        public string ProfilImage { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
    }
}
