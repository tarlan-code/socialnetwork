function getPosts() {
    debugger
    var url = "/Post/GetNewsFeedPosts"
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            $.each(data, function (index, item) {
                $(".news-feed-area").append(

                    `
                    <div class="news-feed news-feed-post">
                                        <div class="post-header d-flex justify-content-between align-items-center">
                                            <div class="image">
                                                <a href="/Profiles?username=${item.username}"><img src='/assets/zust/users/${item.profilImage == "default.png" ? "other/default.png" : item.username + "/" + item.profilImage}' class="rounded-circle" alt="image"></a>
                                            </div>
                                            <div class="info ms-3">
                                                <span class="name"><a href="/Profiles?username=${item.username}">${item.fullname}</a></span>
                                                <span class="small-text"><a href="#">${item.date.substring(0, 10)}</a></span>
                                            </div>
                                            <div class="dropdown">
                                                <button class="dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="flaticon-menu"></i></button>
                                                <ul class="dropdown-menu">
                                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-edit"></i> Edit Post</a></li>
                                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-private"></i> Hide Post</a></li>
                                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-trash"></i> Delete Post</a></li>
                                                </ul>
                                            </div>
                                        </div>

                                        <div class="post-body">
                                            <p>${item.text ?? ""}</p>
                                            
                                            <div class="post-image">
                                                <div id="${"car" + item.postId}" class="carousel slide" data-bs-interval="false" data-bs-ride="carousel">
                                                  <div class="carousel-inner">
                                                
                                                 
                                                  </div>
                                                  <button class="carousel-control-prev" type="button" data-bs-target="#${"car" + item.postId}" data-bs-slide="prev">
                                                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                                    <span class="visually-hidden">Previous</span>
                                                  </button>
                                                  <button class="carousel-control-next" type="button" data-bs-target="#${"car" + item.postId}" data-bs-slide="next">
                                                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                                    <span class="visually-hidden">Next</span>
                                                  </button>
                                                </div>  
                                            </div>
                                            <ul class="post-meta-wrap d-flex justify-content-between align-items-center">
                                                <li class="post-react" value="${item.postId}">
                                                <span class="currentreact">
                                                ${item.reaction == "Like" ? `<a href="#" class="react" value="Like"><img src="/assets/images/react/react-1.png" alt="Like"></a>` :
                        item.reaction == "Heart" ? `<a href="#" class="react" value="Heart"><img src="/assets/images/react/react-2.png" alt="Like"></a>` :
                            item.reaction == "Grinning" ? `<a href="#" class="react" value="Grinning"><img src="/assets/images/react/react-3.png" alt="Like"></a>` :
                                item.reaction == "Astonished" ? `<a href="#" class="react" value="Astonished"><img src="/assets/images/react/react-4.png" alt="Like"></a>` :
                                    item.reaction == "Crying" ? `<a href="#" class="react" value="Crying"><img src="/assets/images/react/react-5.png" alt="Like"></a>` :
                                        item.reaction == "Enraged" ? `<a href="#" class="react" value="Enraged"><img src="/assets/images/react/react-6.png" alt="Like"></a>` :
                                            `<a  href="#" value="Like" class="react"><i class="flaticon-like"></i></a>`

                    }
                                                </span>    
                                                <span>Like</span>
                                                
                                                <small class="likecount">${item.likeCount} </small>

                                                    <ul class="react-list">
                                                        <li>
                                                            <a href="#" class="react" value="Like"><img src="/assets/images/react/react-1.png" alt="Like"></a>
                                                        </li>
                                                        <li>
                                                            <a href="#" class="react" value="Heart"><img src="/assets/images/react/react-2.png" alt="Like"></a>
                                                        </li>
                                                        <li>
                                                            <a href="#" class="react" value="Grinning"><img src="/assets/images/react/react-3.png" alt="Like"></a>
                                                        </li>
                                                        <li>
                                                            <a href="#" class="react" value="Astonished"><img src="/assets/images/react/react-4.png" alt="Like"></a>
                                                        </li>
                                                        <li>
                                                            <a href="#" class="react" value="Crying"><img src="/assets/images/react/react-5.png" alt="Like"></a>
                                                        </li>
                                                        <li>
                                                            <a href="#" class="react" value="Enraged"><img src="/assets/images/react/react-6.png" alt="Like"></a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="post-comment">
                                                    <a href="#"><i class="flaticon-comment"></i><span>Comment</span> <span class="number">${item.commentsCount} </span></a>
                                                </li>
                                                <li class="post-share">
                                                    <a href="#"><i class="flaticon-share"></i><span>Share</span> <span class="number">24 </span></a>
                                                </li>
                                            </ul>
                                            <div class="post-comment-list" id="comment${item.postId}">
                                            
                                                
                                                
                                            </div>
                                            <div class="more-comments">
                                                    <a class="morecomments" data-postid="${item.postId}">More Comments+</a>
                                            </div>
                                            <form class="post-footer commentform" method="post" action="/Comment/AddComment" enctype="multipart/form-data">
                                                <div class="footer-image">
                                                    <a href="#"><img src="/assets/zust/users/${item.idenUserProfilImage == "default.png" ? "other/default.png" : item.idenUserUsername + "/" + item.idenUserProfilImage}" class="rounded-circle" alt="image"></a>
                                                </div>
                                                <div class="form-group">
                                                    <textarea name="Text" class="form-control" placeholder="Write a comment..."></textarea>
                                                    <input name="Image" id="commentimage${item.postId}" type="file" style="display:none;visibility:hidden">
                                                    <input name="PostId" type="number" value="${item.postId}" style="display:none;visibility:hidden">
                                                    <input name="CommentId" type="number" class="comment-id" style="display:none;visibility:hidden">
                                                    <label for="commentimage${item.postId}" class="me-4"><i class="flaticon-photo-camera"></i></label>
                                                    <label for="submit${item.postId}"><i class="fa-regular fa-paper-plane"></i></label>
                                                    <button id="submit${item.postId}" class="addcomment" type="submit" style="display:none;visibility:hidden"></button>
                                                </div>
                                            </form>
                                        </div>
                                    </div>
                    `
                )
                var html = ""
                console.log("media" + item.postMediasName.length)
                debugger
                if (item.postMediasName.length == 0) {
                    $("#car" + item.postId).remove();
                }
                else {

                    var videos = []
                    var isactive = true;
                    $.each(item.postMediasName, function (key, value) {
                        if (value.slice(-3) == "mp4") {
                            var videoId = "video" + value.slice(0, 5)
                            html += `<div class="carousel-item ${isactive ? "active" : ""}">
                            <div id="${videoId}" class="d-block w-100 player"></div>
                            </div>`
                            isactive = false
                            videos.push(value)
                        }
                        else {
                            html += `<div class="carousel-item ${isactive ? "active" : ""}">
                                <img src="/assets/zust/users/${item.username}/medias/${value}" class="d-block w-100" alt="postmedia">
                                </div>`
                            isactive = false
                        }
                    })
                    $(".carousel-inner").last().append(html)

                    for (let index = 0; index < videos.length; index++) {
                        var player = new Clappr.Player({ source: "/assets/zust/users/" + item.username + "/medias/" + videos[index], parentId: "#video" + videos[index].slice(0, 5) });
                    }

                }

                html = ""
                $.each(item.comments, function (index, value) {
                    html +=
                        `<div class="comment-list">
                    <div class="comment-image">
                        <a href="/Profiles?username=${value.username}"><img src="/assets/zust/users/${value.profilimage == "default.png" ? "other/default.png" : value.username + "/" + value.profilimage}" class="rounded-circle" alt="image"></a>
                    </div>
                    <div class="comment-info">
                        <h3>
                            <a href="/Profiles?username=${value.username}">${value.fullname}</a>
                        </h3>
                        <span>${value.date.substring(0, 16).replace("T", "  ")}</span>
                        <img src="/assets/zust/users/${value.username}/commentmedias/${value.image}" class="w-25" style=" ${value.image == null ? "display:none;visibility:hidden" : "display:block"}">
                        <p>${value.content ?? ""}</p>
                        <ul class="comment-react">
                            <li><a href="#" class="like">Like(${value.likeCount})</a></li>
                            <li><a class="reply" id="${value.id}"  value="${value.username}">Reply</a></li>
                            <li><a class="replycomments" value="${value.id}">Show reply comments</a></li>
                            <div id="reply-comment-list${value.id}"></div>
                        </ul>
                    </div>
                </div>`
                })

                $("#comment" + item.postId).prepend(html)
            })


        },
        error: function (e) {
            alert(e);
        },
    });
}


// CallFunction
getPosts();





$(document).on("click", ".react", function (e) {
    e.preventDefault();
    var react = $(this).attr("value");
    var postId = $(this).parents(".post-react").attr("value");
    url = `/Post/AddReaction?postId=${postId}&reaction=${react}`
    var html = $(this).parent().html()
    var currentreaction = $(this).parents(".post-react").children(".currentreact")
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {
            if (result == "True") {

                currentreaction.html(html.trim())
            }
            else if (result == "False" || result == "Deleted") {
                currentreaction.html(`<a href="#" value="Like" class="react"><i class="flaticon-like"></i></a>`)
            }
        },
        error: function (response) {
            alert(response.responseText);
        },
    })
})






$(document).on("submit", ".commentform", function (e) {
    e.preventDefault();

    var formData = new FormData($(this)[0]);
    var isreply = $(this).find("textarea").val()[0] == "@"

    var elem = $(this).parents(".post-body").children(".post-comment-list");

    $(this).trigger("reset");
    $.ajax({
        url: $(this).attr('action'),
        type: $(this).attr('method'),
        data: formData,
        async: false,
        success: function (data) {
            console.log(data)
            debugger
            if (isreply) {
                elem = $("#reply-comment-list" + data.repliedId)
            }
            elem.prepend(
                `<div class="comment-list ms-1 mt-2" >
                    <div class="comment-image">
                        <a href="/Profiles?username=${data.username}"><img src="/assets/zust/users/${data.profilimage == "default.png" ? "other/default.png" : data.username + "/" + data.profilimage}" class="rounded-circle" alt="image"></a>
                    </div>
                    <div class="comment-info">
                        <h3>
                            <a href="/Profiles?username=${data.username}">${data.fullname}</a>
                        </h3>
                        <span>${data.date.substring(0, 16).replace("T", "  ")}</span>
                        <img src="/assets/zust/users/${data.username}/commentmedias/${data.image}" class="w-25" style=" ${data.image == null ? "display:none;visibility:hidden" : "display:block"}">
                        <p>${data.content ?? ""}</p>
                        <ul class="comment-react">
                            <li><a class="like">Like(${data.likeCount})</a></li>
                            <li><a class="reply" id="${data.id}" value="${data.username}">Reply</a></li>
                            <li><a class="replycomments" value="${data.id}">Show reply comments</a></li>
                            <div id="reply-comment-list${data.id}"></div>
                        </ul>
                    </div>
                </div>`
            )


        },
        error: function () {
            alert('error');
        },
        cache: false,
        contentType: false,
        processData: false
    });
    return false;
});





$(document).on("click", ".reply", function () {
    var username = $(this).attr("value");
    var id = $(this).attr("id");
    var form = $(this).parents(".post-body").children("form")
    form.find("textarea").val("@" + username + "~")
    form.find(".comment-id").val(id)
})


$(document).on("click", ".replycomments", function () {
    var commentId = $(this).attr("value")
    var url = "/Comment/GetReplyes/" + commentId
    var commentlist = $(this).parent().parent().children("#reply-comment-list" + $(this).attr("value"))
    $(this).addClass("d-none");
    console.log(commentlist)
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {
            console.log(result);
            $.each(result, (index, value) => {
                commentlist.append(`
                
                <div class="comment-list ms-1 mt-2 id="single-comment${value.id} ">
                    <div class="comment-image">
                        <a href="/Profiles?username=${value.username}"><img src="/assets/zust/users/${value.profilimage == "default.png" ? "other/default.png" : value.username + "/" + value.profilimage}" class="rounded-circle" alt="image"></a>
                    </div>
                    <div class="comment-info">
                        <h3>
                            <a href="/Profiles?username=${value.username}">${value.fullname}</a>
                        </h3>
                        <span>${value.date.substring(0, 16).replace("T", "  ")}</span>
                        <img src="/assets/zust/users/${value.username}/commentmedias/${value.image}" class="w-25" style=" ${value.image == null ? "display:none;visibility:hidden" : "display:block"}">
                        <p>${value.content ?? ""}</p>
                        <ul class="comment-react">
                            <li><a class="like">Like(${value.likeCount})</a></li>
                            <li><a class="reply" id="${value.id}" value="${value.username}">Reply</a></li>
                            <li><a class="replycomments" value="${value.id}">Show reply comments</a></li>
                            <div id="reply-comment-list${value.id}"></div>
                        </ul>
                    </div>
                </div>

                `)
            })

        },
        error: function (response) {
            alert(response.responseText);
        },
    })
})