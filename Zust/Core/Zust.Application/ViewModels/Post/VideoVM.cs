namespace Zust.Application.ViewModels
{
    public class VideoVM
    {
        public string Media { get; set; }
        public int PostId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public DateTime Date { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int SharedCount { get; set; }
        public string ProfilImage { get; set; }
    }
}
