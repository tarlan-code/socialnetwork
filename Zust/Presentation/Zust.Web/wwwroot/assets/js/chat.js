

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").withAutomaticReconnect().build();

async function start() {
    try {
        await connection.start();
        console.log("connectedddddd");
    } catch (err) {
        console.log("Baglanmaga calisir");
        setTimeout(() => start(), 2000);
    }
}

start();




connection.on("receiveMessage", (result,username) => {

    var currentusername = $(".live-chat-info").attr("data-username")
    if(currentusername == username){
    var chatarea = $("#chat-area");
    var date = result.date.split("T")
    chatarea.append( `
    <div class="chat-item ">

    <div class="chat">
        <div class="chat-avatar">
            <a class="d-inline-block">
                <img  src="/assets/zust/users/${result.profilImage == "default.png" ? "other/default.png" : result.username + "/" + result.profilImage}" width="50" height="50" class="rounded-circle" alt="image">
            </a>
        </div>

        <div class="chat-body">
            <div class="chat-message">
            ${result.media == null ? "" : "<img src='/assets/zust/users/"+username+"/messages/"+result.media+"' class='w-50'>"} 
                <p>${result.content ?? ""}</p>
                <span class="time d-block">${date[0]+"  "+date[1].substring(0,5)}</span>
            </div>
        </div>
    </div>
    `)
    chatarea.animate({
        scrollTop: chatarea.get(0).scrollHeight
    }, 1000);
}
})


connection.on("isonline",(result)=>{
    $.each(result, function (key, value) { 
        debugger
        $(".contact-body").find("#"+key).find(".status").attr("class","status-online status");
         
    });
})


connection.on("isoffline",(username)=>{
        $(".contact-body").find("#"+username).find(".status").attr("class","status-offline status");
})
