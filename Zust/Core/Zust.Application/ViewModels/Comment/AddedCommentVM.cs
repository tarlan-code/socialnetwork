namespace Zust.Application.ViewModels
{
    public class AddedCommentVM
    {
        public int Id { get; set; }
        public int? RepliedId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Profilimage { get; set; }
        public int LikeCount { get; set; }
        public DateTime Date { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
    }
}
