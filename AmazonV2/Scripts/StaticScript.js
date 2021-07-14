function logout() {
    window.location.href = "Logout.php";
}

function callServer(pageUrl, params, oncomplete, async = true) {
    var httpc = new XMLHttpRequest();
    httpc.open("GET", pageUrl + "?" + params, async);

    httpc.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    httpc.setRequestHeader("Content-Length", params.length);

    httpc.onreadystatechange = () => {
        if (httpc.readyState == 4 && httpc.status == 200) { // complete and no errors
            oncomplete(httpc.responseText);
        }
    };
    //alert(pageUrl + "?" + params);
    httpc.send();
    //httpc.send(params);
}
