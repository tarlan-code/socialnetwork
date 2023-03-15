



$(".search").keyup(function(){
    var id = this.id;

    var q = $("#"+id).val();
    const searheduser = $("#users-"+id)
   debugger
    url = "/Home/GetSearchedUsers?text=" + q
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
           
            searheduser.html("")
            $.each(data, function (index, item) {

                div = `<div class="contact-item p-2">
        <a href="/Profiles?username=${item.username}"><img src="/assets/zust/users/${item.profilImage == "default.png" ? "other/default.png" : item.username + "/" + item.profilImage}" class="rounded-circle" alt="image" style="height:7vh"></a>
        <span class="name"><a href="Profiles?username=${item.username}">${item.fullname}</a></span>
    </div>`

                searheduser.append(div)

            })

            console.log(data);
        },
        error: function (e) {
            alert(e);
        },
    })
});


$('#countries').on('change', function () {
    $("#cities").html("<option selected='1' value=''>City</option>");
    var selectval = $('#countries').find(":selected").val();
    if (selectval != null && selectval != "") {

        $.ajax({
            type: "POST",
            url: "Settings/GetCities/" + selectval,
            success: function (result) {
                if (result != null) {
                    $.each(result, function (index, item) {
                        $("#cities").append(`<option value='${item.id}'>${item.name}</option>`)
                    })
                }
            },
            error: function (response) {
                alert(response.responseText);
            },
        })
    }
});



$(document).on("click", "#addfriend", function (e) {
    e.preventDefault();
    url = this.href
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {
            if (result == "True") {
                $("#addfriend").text("Request sent")
                $("#addfriend").attr("href", "/Requests/RemoveFriend?" + url.split("?")[1]);
                $("#addfriend").attr("id", "removefriend");
            }
        },
        error: function (response) {
            alert(response.responseText);
        },
    })
})



$(document).on("click", "#removefriend", function (e) {
    e.preventDefault();
    url = this.href
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {
            if (result == "True") {
                if ($("#friendbutton").children().length > 1) {
                    $("#friendbutton").children().first().remove()
                }

                $("#removefriend").text("Add Friend")
                $("#removefriend").attr("href", "/Requests/AddFriend?" + url.split("?")[1]);
                $("#removefriend").attr("id", "addfriend");
            }
        },
        error: function (response) {
            alert(response);
        },
    })
})


$(document).on("click", ".addrequest", function (e) {
    e.preventDefault();
    url = this.href
    var username = url.split("=")[1];
    $.ajax({
        type: "POST",
        url: url,
        success: function () {
            $("#" + username).remove()
        },
        error: function (response) {
            alert(response);
        },
    })
})



$(document).on("click", ".removerequest", function (e) {
    e.preventDefault();
    url = this.href
    var username = url.split("=")[1];
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {
            if (result == "True") {
                $("#" + username).remove()
            }
        },
        error: function (response) {
            alert(response);
        },
    })
})







$("#tagbutton").on("click", function (e) {
    e.preventDefault();
    var tagusers = $("#tagadd")
    check = true;
    if (tagusers.css("visibility") == "hidden") {
        if(check){
            getUsers()
            check = false;}
        tagusers.css("visibility", "visible");
    } else {
        tagusers.css("visibility", "hidden");
    }

})


function getUsers(){
    $.ajax({
        type: "POST",
        url: "/Post/GetTagFriends",
        success: function (data) {
            var html = "";
            $.each(data, function (key, value) {
             html+=`<option value="${value}">${key}</option>`
             
            })
            $("#tagfriends").html(html)
    
        },
        error: function (e) {
            alert(e);
        },
    });
}





$(document).on("click",".morecomments",function(){
    var postid = $(this).attr("data-postid");
    var commentcount = $("#comment"+postid+">.comment-list").length;
    var commentcount = parseInt(commentcount);
    $.ajax({
        type: "POST",
        url: "/Comment/GetMoreComments?postid="+postid+"&commentcount="+commentcount,
        success: function (data) {
            $.each(data,(index,value)=>{
                $("#comment"+postid).append(`<div class="comment-list">
                <div class="comment-image">
                    <a href="/Profiles?username=${value.username}"><img src="/assets/zust/users/${value.profilimage == "default.png" ? "other/default.png" : value.username + "/" + value.profilimage}" class="rounded-circle" alt="image"></a>
                </div>
                <div class="comment-info">
                    <h3>
                        <a href="/Profiles?username=${value.username}">${value.fullname}</a>
                    </h3>
                    <span>${value.date.substring(0,16).replace("T","  ")}</span>
                
                    ${value.image==null ? "" :`<img src="/assets/zust/users/${value.username}/commentmedias/${value.image}" class="w-25" style=" ${value.image==null ? "display:none;visibility:hidden": "display:block"}">`} 
                    <p>${value.content ?? ""}</p>
                    <ul class="comment-react">
                        <li><a href="#" class="like">Like(${value.likeCount})</a></li>
                        <li><a class="reply" id="${value.id}"  value="${value.username}">Reply</a></li>
                        <li><a class="replycomments" value="${value.id}">Show reply comments</a></li>
                        <div id="reply-comment-list${value.id}"></div>
                    </ul>
                </div>
            </div>`)
            })
           
        },
        error: function (e) {
            alert(e);
        },
    });
})

function GetContacts(q=null){
    var url;
    q==null ? url = "/Messages/GetContacts" : url = "/Messages/GetContacts?q="+q;
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {
           $.each(result,(index,value)=>{
            debugger
            $(".contact-body").append(`
            <div class="contact-item" id="${value.username}">
                <a href="#"><img  src="/assets/zust/users/${value.profilImage == "default.png" ? "other/default.png" : value.username + "/" + value.profilImage}" class="rounded-circle" alt="image"></a>
                <span class="name"><a href="/Messages?username=${value.username}" class="user-contact">${value.name}</a></span>
                <span class="status-offline status"></span>
            </div>
            `)
           })
        },
        error: function (response) {
            alert(response.responseText);
        },
    })
}

GetContacts();



$(document).on("click","#find-contact",function(e){
    e.preventDefault();
    var q = $("#search-contact").val();
    $(".contact-body").empty();
    GetContacts(q);
})





$(document).on("click",".user-contact",function(e){
    e.preventDefault();
    $(".live-chat-body").css("display","block")
    $(".live-chat-body").css("visibility","visible")

    var url = $(this).attr("href");
    var currentController = window.location.pathname
    if(currentController != "/Messages"){
        window.location.href = "/Messages"
    }
    var objDiv = $(".chat-content");
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {
            var chatEl = $(".live-chat-info")
            chatEl.attr("data-username",result.username)
            chatEl.html(`
            <a href="/Profiles?username=${result.username}"><img  src="/assets/zust/users/${result.profilImage == "default.png" ? "other/default.png" : result.username + "/" + result.profilImage}" class="rounded-circle" alt="image"></a>
            <h3>
                <a href="/Profiles?username=${result.username}">${result.fullname}</a>
            </h3>
            `)
            $("#delete-message").attr("href","/Messages/DeleteMessages?username="+result.username)
            var messags = ""
            $.each(result.messages,(index,value)=>{
                var date = value.date.split("T")
                messags +=`
                <div class="chat-item ">

                <div class="chat ${result.username != value.username ? "chat-left" : ""}">
                    <div class="chat-avatar">
                        <a class="d-inline-block">
                            <img  src="/assets/zust/users/${value.profilImage == "default.png" ? "other/default.png" : value.username + "/" + value.profilImage}" width="50" height="50" class="rounded-circle" alt="image">
                        </a>
                    </div>

                    <div class="chat-body">
                        <div class="chat-message">
                        ${value.media == null ? "" : "<img src='/assets/zust/users/"+value.username+"/messages/"+value.media+"' class='w-50'>"} 
                            <p>${value.content ?? ""}</p>
                            <span class="time d-block">${date[0]+"  "+date[1].substring(0,5)}</span>
                        </div>
                    </div>
                </div>
                `
            })

            objDiv.html(messags);
            objDiv.animate({
                scrollTop: objDiv.get(0).scrollHeight
            }, 1000);
        },
        error: function (response) {
            alert(response.responseText);
        }
    })
})



$(document).on("submit","#send-message-form",function(e){
    e.preventDefault();
    var form = $(this)
    var data = new FormData($(this)[0]);
    var username = $(".live-chat-info").attr("data-username");
    var objDiv = $(".chat-content");
    
    data.append("username",username);
    $.ajax({
        url: $(this).attr('action'),
        type: $(this).attr('method'),
        data:data,  
        async:true,
        success: function (result) {
            console.log(result)
           if(result != "False"){
            var date = result.date.split("T")
            objDiv.append(

                `
                <div class="chat-item ">

                <div class="chat ${result.username != username ? "chat-left" : ""}">
                    <div class="chat-avatar">
                        <a class="d-inline-block">
                            <img  src="/assets/zust/users/${result.profilImage == "default.png" ? "other/default.png" : result.username + "/" + result.profilImage}" width="50" height="50" class="rounded-circle" alt="image">
                        </a>
                    </div>

                    <div class="chat-body">
                        <div class="chat-message">
                        ${result.media == null ? "" : "<img src='/assets/zust/users/"+result.username+"/messages/"+result.media+"' class='w-50'>"} 
                            <p>${result.content ?? ""}</p>
                            <span class="time d-block">${date[0]+"  "+date[1].substring(0,5)}</span>
                        </div>
                    </div>
                </div>
                `
            );
           
            objDiv.animate({
                scrollTop: objDiv.get(0).scrollHeight
            }, 1000);
            form[0].reset();
           }
        },
        error: function (response) {
            alert(response.responseText);
        },
        cache: false,
        contentType: false,
        processData: false
    })
})




$(document).on("click",".load-more-notification",function(e){
    e.preventDefault();
    var skip = $(".notf").length;
debugger
    var url = "/Notifications/GetNotifications?skip="+skip;
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {
            $.each(result,(index,value)=>{
                var date = value.date.split("T")
                $(".all-notifications-body").append(

                    `
                    <div class="item d-flex justify-content-between align-items-center notf">
                        <div class="figure">
                                    <img src="/assets/zust/users/${value.profilImage == "default.png" ? "other/default.png" : value.username + "/" + value.profilImage}" class="rounded-circle" width="70" alt="image">
                        </div>
                        <div class="text">
                            <h4><a href="/Profiles?username=${value.username}">${value.fullname}</a></h4>
                            <span>${value.title}</span>
                            <span class="main-color">${value.date} Ago</span>
                        </div>
                        <div class="icon">
                            <a class="delete-notf" href="/Notifications/Delete/${value.id}"><i class="flaticon-x-mark"></i></a>
                        </div>
                    </div> 
                            
                    `
                )


               
            })
        },
        error: function (response) {
            alert(response.responseText);
        },
    })
})




$(document).on("click","#loadrequest",function(e){
    e.preventDefault();
    var requests = $(".request")
    var skip = requests.length;
    var url = "/Requests/GetRequests?skip="+skip;
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {
            $.each(result,(index,value)=>{
                requests.append(
    
    ` <div class="col-lg-3 col-sm-6" id="${value.username}">
    <div class="single-friends-card">
        <div class="friends-image">
            <a href="#">
                <img src="/assets/zust/users/${value.bannerImage == "defaultbanner.png" ? "other/defaultbanner.png" : value.username + "/" + value.bannerImage}" alt="image">
            </a>
            <div class="icon">
                <a href="/Profiles?username=${value.username}"><i class="flaticon-user"></i></a>
            </div>
        </div>
        <div class="friends-content">
            <div class="friends-info d-flex justify-content-between align-items-center">
                <a href="/Profiles?username=${value.username}">
                    <img src="/assets/zust/users/${value.profilImage == "default.png" ? "other/default.png" : value.username + "/" + value.profilImage}" alt="image" style="max-width:100px">
                </a>
                <div class="text ms-3">
                    <h3><a href="#">${value.fullname}</a></h3>
                </div>
            </div>
            <ul class="statistics text-start">
            
                <li>
                    <a href="#">
                        <span class="item-number">${value.friendsCount}</span>
                        <span class="item-text">Friends</span>
                    </a>
                </li>
            </ul>
            <div class="button-group d-flex justify-content-between align-items-center" >
                <div class="add-friend-btn">
                    <a class="btn btn-light btn-sm addrequest" href="/Requests/Confirm?username=${value.username}">Add Friend</a>
                </div>
                <div class="send-message-btn">
                    <a href="/Requests/RemoveFriend?username=${value.username}" class="btn btn-light btn-sm removerequest">Delete</a>
                </div>
            </div>
        </div>
    </div>

</div>`
    
    
                )

            })
        },
        error: function (response) {
            alert(response.responseText);
        }
    })

})



$(document).on("submit",".video-form",function (e) {
    e.preventDefault();
    var formData = new FormData($(this)[0]);
    $(this).trigger("reset");
    $.ajax({
        url: $(this).attr('action'),
        type: $(this).attr('method'),
        data: formData,
        async: false,
        success: function (data) {
            if(data != "False"){
                $.notify("Comment added","success");
            }
        },
        error: function(){
            alert('error');
        },
        cache: false,
        contentType: false,
        processData: false
    });
    return false;
});



$(document).on("click",".delete-request",function(e){
    e.preventDefault();
    debugger
    var url = $(this).attr("href");
    var removedElem = $(this).parents(".request-item")
    $.ajax({
        type: "GET",
        url: url,
        success: function (result) {
            removedElem.remove();
            $("#request-count").html(parseInt($("#request-count").html())-1)
        },
        error: function (response) {
            alert(response.responseText);
        },
    })
})


$(document).on("click",".confirm-request",function(e){
    e.preventDefault();
    debugger
    var url = $(this).attr("href");
    var removedElem = $(this).parents(".request-item")
    $.ajax({
        type: "GET",
        url: url,
        success: function (result) {
            removedElem.remove();
            $("#request-count").html(parseInt($("#request-count").html())-1)
        },
        error: function (response) {
            alert(response.responseText);
        },
    })
})


$(document).on("click","#read-notfs",function(e){
    e.preventDefault();
    var url = $(this).attr("href");
    $.ajax({
        type: "GET",
        url: url,
        success: function (result) {
            $("#notf-count").html(0)
        },
        error: function (response) {
            alert(response.responseText);
        }
    })

})




$(document).on("click",".delete-notf",function(e){
    e.preventDefault();
    var url = $(this).attr("href");
    var removedElem = $(this).parents(".notf")
    $.ajax({
        type: "GET",
        url: url,
        success: function (result) {
            removedElem.remove();
        },
        error: function (response) {
            alert(response.responseText);
        },
    })
})



