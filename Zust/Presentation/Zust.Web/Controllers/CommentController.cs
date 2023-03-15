using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;
using Zust.Persistence.Services;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]

    public class CommentController : Controller
    {
        IToastNotification _notf;
        IPathService _path;
        ICommentService _commentService;
        IPostService _postService;
        IPrivacySettingService _privacySettingService;
        IFriendService _friendService;
        IMapper _mapper;
        INotificationService _notificationService;
        UserManager<AppUser> _userManager;

        public CommentController(IToastNotification notf, UserManager<AppUser> userManager, IPathService path, ICommentService commentService, IPostService postService, IFriendService friendService, IPrivacySettingService privacySettingService, IMapper mapper, INotificationService notificationService)
        {
            _notf = notf;
            _userManager = userManager;
            _path = path;
            _commentService = commentService;
            _postService = postService;
            _friendService = friendService;
            _privacySettingService = privacySettingService;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentVM comment)
        {
            if(comment is null) return Content("False");
            IFormFile? image = comment.Image;

            if(image is not null)
            {
                string result = image.CheckValidate(5, "image");

                if(result.Length > 0)
                {
                    ModelState.AddModelError("Image", result);
                }

            }

            if(image is null && comment.Text is null)
                ModelState.AddModelError("Comment", "Comment is required");
            
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    if(item.Value.Errors.Count > 0)
                        _notf.AddWarningToastMessage(item.Value.Errors.FirstOrDefault().ErrorMessage, new ToastrOptions
                        {
                            Title = "Comment"
                        });
                }
                return Content("False");

            }

            AppUser user = await _userManager.GetUserAsync(User);

            Comment newComment = new();

            newComment.AppUser = user;

            if (comment.Text is not null)
            {
                if (comment.Text[0] == '@')
                {
                    string endText = comment.Text.Substring(comment.Text.LastIndexOf("~")+1);
                    if (String.IsNullOrEmpty(endText))
                    {
                        if(image is null)
                        {
                            _notf.AddWarningToastMessage("Comment not empty", new ToastrOptions
                            {
                                Title = "Comment"
                            });
                            return Content("False");
                        }
                    }
                    Comment? repliedComment = await _commentService.GetComment(comment.CommentId);
                    if (repliedComment is null) return BadRequest();
                    newComment.Replied = repliedComment;
                    newComment.Content = endText;
                }
                else
                {
                    newComment.Content = comment.Text;
                }
            }


            if(image is not null)
                newComment.Image = image.SaveFile(Path.Combine(_path.UsersFolder, user.UserName, "commentmedias"));

            Post post = await _postService.GetPost(comment.PostId);

            if (post is not null)
            {
                PrivacySetting privacy = await _privacySettingService.GetPrivacySettingAsync(post.AppUserId);
                if (privacy is not null)
                {
                    if (privacy.WhoCanSeeYourProfile is null)
                    {
                        _notf.AddInfoToastMessage("This user privacy only no one");
                        return Content("False");

                    }
                    else if (privacy.WhoCanSeeYourProfile == false)
                    {
                        bool result = await _friendService.IsFriend(user.Id, post.AppUserId);
                        if (!result)
                        {
                            _notf.AddInfoToastMessage("You are not friend this user");
                            return Content("False");
                        }
                    }
                }
            }
            newComment.Post = post;

            await _commentService.AddComment(newComment);

            AddedCommentVM addedCommentVM = _mapper.Map<AddedCommentVM>(newComment);

            if (user.Id != post.AppUserId)
                await _notificationService.AddNotificationAsync(new Notification
                {
                    ReceiverId = post.AppUserId,
                    Sender = user,
                    Title = "Add Comment In Your Post"
                });

            return Json(addedCommentVM);
        }

        [HttpPost]
        public async Task<IActionResult> GetReplyes(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            List<Comment> comments = await _commentService.GetReplyComments(id);
            List<AddedCommentVM> replycomments = new();
            comments.ForEach(c => replycomments.Add(_mapper.Map<AddedCommentVM>(c)));

            return Json(replycomments);
        }

        [HttpPost]
        public async Task<IActionResult> GetMoreComments(int postid, int commentcount)
        {
            List<Comment> comments = await _commentService.GetCommentsAsync(postid, commentcount);
            List<AddedCommentVM> addedComments = new();

            comments.ForEach(c => addedComments.Add(_mapper.Map<AddedCommentVM>(c)));
            return Json(addedComments);
        }
    }
}
