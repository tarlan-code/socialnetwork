using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]

    public class PostController : Controller
    {
        IToastNotification _notf;
        IPathService _pathService;
        IPostService _postService;
        IPrivacySettingService _privacySetting;
        IFriendService _friendService;
        IUserService _userService;
        IReactionService _reactionService;
        ICommentService _commentService;
        INotificationService _notificationService;
        IMapper _mapper;
        UserManager<AppUser> _userManager;
        public PostController(IToastNotification notf, UserManager<AppUser> userManager, IPathService pathService, IPostService postService, IFriendService friendService, IUserService userService, IMapper mapper, IReactionService reactionService, IPrivacySettingService privacySetting, ICommentService commentService, INotificationService notificationService)
        {
            _notf = notf;
            _userManager = userManager;
            _pathService = pathService;
            _postService = postService;
            _friendService = friendService;
            _userService = userService;
            _mapper = mapper;
            _reactionService = reactionService;
            _privacySetting = privacySetting;
            _commentService = commentService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> CreatePost(PostVM post)
        {
            if(post is null) return RedirectToAction("Index","Home");

            List<IFormFile>? images = post.Images;
            List<IFormFile>? videos = post.Videos;
            string result = String.Empty;
            if (images is not null)
            {
                foreach (IFormFile? item in images)
                {
                    result = item?.CheckValidate(10, "image") ?? "";
                    if(result.Length > 0)
                    {
                        ModelState.AddModelError("Images",result);
                        break;
                    }
                }
            }


            if (videos is not null)
            {
                foreach (IFormFile? item in videos)
                {
                    result = item?.CheckValidate(10, "video/mp4") ?? "";
                    if (result.Length > 0)
                    {
                        ModelState.AddModelError("Videos", result);
                        break;
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    if (item.Value.Errors.Count > 0)
                        _notf.AddErrorToastMessage(item.Value.Errors.FirstOrDefault()?.ErrorMessage, new ToastrOptions
                        {
                            Title = item.Key
                        });
                }
                return RedirectToAction("Index", "Home");
            }

            AppUser user = await _userManager.GetUserAsync(User);
            Post newPost = new()
            {
                AppUser = user,
                Content = post.Text,
            };

            List<PostMedia> medias = new();
            if(images is not null)
            {
                foreach (IFormFile img in images)
                {
                    medias.Add(new PostMedia { Media = img.SaveFile(Path.Combine(_pathService.UsersFolder, user.UserName, "medias")), Post = newPost});
                }
            }

            if (videos is not null)
            {
                foreach (IFormFile video in videos)
                {
                    medias.Add(new PostMedia { Media = video.SaveFile(Path.Combine(_pathService.UsersFolder, user.UserName, "medias")), Post = newPost });
                }
            }
            if(images is not null || videos is not null)
                newPost.PostMedias = medias;

            if(post.Tags is not null)
            {
                List<PostTag> tags = new();
                List<string> friendsIds = _friendService.GetFriendsList(user.Id);
                foreach (string item in post.Tags)
                {
                    if (friendsIds.Contains(item))
                        tags.Add(new PostTag
                        {
                            AppUserId = item,
                            Post = newPost
                        });
                }
                newPost.PostTags = tags;
            }


            await _postService.AddPostAsync(newPost);


            return RedirectToAction("Index","Profiles",new {username = user.UserName });
        }

        [HttpPost]
        public async Task<IActionResult> GetTagFriends()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            List<string> friends = _friendService.GetFriendsList(user.Id);
            return Json(_userService.GetUsersWithUsername(friends));
        }


        [HttpPost]
        public async Task<IActionResult> GetUserPosts(string? username,int skip)
        {
            AppUser user = new();
            AppUser IdenUser = await _userManager.GetUserAsync(User);
            if (String.IsNullOrEmpty(username)) user = IdenUser;
            else user = await _userManager.FindByNameAsync(username);
            if (user is null) return NotFound();
            List<Post> posts = await _postService.GetUserPosts(user.Id,skip);

            List<ShowedPostVM> postsVM = await ConvertPosts(posts,user,IdenUser);
            
            return Json(postsVM);
        }



        [HttpPost]
        public async Task<IActionResult> AddReaction(int postId,string reaction)
        {
            Reaction reac = await _reactionService.GetReaction(reaction);
            if(reac is null)
            {
                _notf.AddWarningToastMessage("Reaction not found");
                return Content("False");
            }
            Post post = await _postService.GetPost(postId);
            AppUser user = await _userManager.GetUserAsync(User);
            PrivacySetting privacy = await _privacySetting.GetPrivacySettingAsync(post.AppUserId);
            if(privacy is not null)
            {
                if(privacy.WhoCanSeeYourProfile is null)
                {
                    _notf.AddWarningToastMessage("Not added reaction");
                    return Content("False");
                }
                else if(privacy.WhoCanSeeYourProfile == false)
                {
                    bool isFriend = await _friendService.IsFriend(user.Id, post.AppUserId);
                    if (!isFriend)
                    {
                        _notf.AddWarningToastMessage("You are not friend this post user");
                        return Content("False");
                    }
                }
            }

            PostReaction postReaction = await _reactionService.GetPostReaction(user.Id, postId);
            if(postReaction is not null)
            {
                if(postReaction.Reaction.IconName == reac.IconName)
                {
                    await _reactionService.DeletePostReaction(postReaction);
                    return Content("Deleted");
                }
                else
                {
                    postReaction.Reaction = reac;
                    await _postService.UpdatePost(post);
                    return Content("True");
                }
            }

            List<PostReaction> postReactions = new();
            postReactions.Add(new PostReaction
            {
                Post = post,
                Reaction = reac,
                AppUser = user
            });
            post.PostReactions = postReactions;

            bool result = await _postService.UpdatePost(post);
            if (!result) return Content("False");

            if(user.Id != post.AppUserId)
                await _notificationService.AddNotificationAsync(new Notification
                {
                    ReceiverId = post.AppUserId,
                    Sender = user,
                    Title = $"Add Reaction Your Post"
                });
            return Content("True");
        }



        public async Task<IActionResult> GetNewsFeedPosts(int skip=0)
        {
            AppUser user = await _userManager.GetUserAsync(User);

            
            List<string> friendlist = _friendService.GetFriendsList(user.Id);
            friendlist.Add(user.Id);
            List<Post> posts = new();
            if (friendlist is not null && friendlist.Count > 0)
                posts = await _postService.GetNewsFeedPosts(friendlist,skip);
            else return Json("");

            List<ShowedPostVM> postsVM = await ConvertPosts(posts,user,user);
            return Json(postsVM);

        }


        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Post post = await _postService.GetPost((int)id);
            AppUser user = await _userManager.GetUserAsync(User);
            if (user.Id != post.AppUserId) return BadRequest();
            if(post is not null)
                await _postService.DeletePost(post);
            return RedirectToAction("Index","Profiles",new { username = post.AppUser.UserName });
        }




        async Task<List<ShowedPostVM>> ConvertPosts(List<Post> posts,AppUser user,AppUser IdenUser)
        {

            List<ShowedPostVM> postsVM = new();
            if (posts.Count > 0 && posts is not null)
            {
                foreach (Post post in posts)
                {
                    ShowedPostVM showedPost = _mapper.Map<ShowedPostVM>(post);
                    PostReaction reaction = await _reactionService.GetPostReaction(IdenUser.Id, post.Id);
                    if (reaction is not null)
                        showedPost.Reaction = reaction.Reaction.IconName;

                    List<Comment> postComments = await _commentService.GetCommentsAsync(post.Id);
                    if (postComments is not null)
                    {
                        List<AddedCommentVM> comments = new();
                        foreach (var comment in postComments)
                        {
                            comments.Add(_mapper.Map<AddedCommentVM>(comment));
                        }
                        showedPost.Comments = comments;
                    }
                    showedPost.CommentsCount = Convert.ToUInt32(await _commentService.GetCommentCount(post.Id));
                    showedPost.IdenUserUsername = IdenUser.UserName;
                    showedPost.IdenUserProfilImage = IdenUser.ProfilImage;
                    postsVM.Add(showedPost);

                }

            }
            return postsVM;
        }




    }
}
