namespace Zust.Application.ViewModels
{
    public class ProfileVM
    {
        public ProfileInformationVM? profileInformationVM { get; set;}
        public ProfileImageVM? profileImageVM { get; set; }
        public BannerImageVM? bannerImageVM { get; set; }
        public PrivacyVM? privacyVM { get; set; }
        public FriendCheckVM? friendCheckVM { get; set; }
    }
}
