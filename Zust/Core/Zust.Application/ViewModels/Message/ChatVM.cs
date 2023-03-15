namespace Zust.Application.ViewModels
{
    public class ChatVM
    {
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string ProfilImage { get; set; }
        public List<MessageVM> Messages { get; set; }
    }
}
