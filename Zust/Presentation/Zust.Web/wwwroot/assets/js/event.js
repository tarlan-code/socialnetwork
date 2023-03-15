function GetEvents(skip,q) {
    var url = "/Events/GetEvents?skip=" + skip + "&q=" + q;
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {

            $.each(result, function (index, value) {

                $("#events").append(
                    `<div class="col-lg-3 col-md-6">
                <div class="single-events-card">
                    <a href="#">
                        <img  src="/assets/zust/users/${value.username}/events/${value.image}" class="mx-auto d-block" style="witdh:auto;height:150px;" alt="image">
                    </a>
                    <div class="events-content">
                        <h3>
                            <a href="#">${value.name}</a>
                        </h3>
                        <p>${value.location}</p>
    
                        <div class="events-footer d-flex justify-content-between align-items-center">
                            <a href="/Events/${value.isIdenUserEvent ? "Delete" : value.isAttend ?  "RemoveAttend" : "Attend"}/${value.id}" class="btn btn-sm p-1 ${value.isIdenUserEvent ? "btn-danger" : "btn-primary"} change-attend">${value.isIdenUserEvent ? "Delete" : value.isAttend ? "Remove Attend" : "Attend"}</a>
                            <span>${value.eventDate.substring(0, 16).replace("T", "  ")}</span>
                        </div>
                    </div>
                </div>
            </div>`)
            });
        },
        error: function (response) {
            alert(response.responseText);
        },
    })
}


GetEvents(0,"")

$(document).on("click",".load-more-events",function(){
    debugger
    var skip = $("#events").children().length;
    GetEvents(skip,"");
})


$(document).on("click","#search-event",function(e){ 
    e.preventDefault();
    var q = $("#search-event-input").val();
    if(q=='undefined'){
        q="";   
    }
    $("#events").html("");
    $("#load-more-event").remove()
    GetEvents(0,q);
 })

