namespace Zust.Application.ViewModels
{
    public class ShowedPostVM
    {
        public int PostId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string IdenUserProfilImage { get; set; }
        public string IdenUserUsername { get; set; }
        public string ProfilImage { get; set; }
        public DateTime Date { get; set; }
        public int ShareCount { get; set; }

        public string? Text { get; set; }
        public List<string>? PostMediasName { get; set; }
        public List<string>? PostTagsUsernames { get; set; }
        public List<AddedCommentVM>? Comments { get; set; }
        public uint LikeCount { get; set; }
        public uint CommentsCount { get; set; }
        public string? Reaction { get; set; }

    }
}
