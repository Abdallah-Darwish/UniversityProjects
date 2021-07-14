function setup() {
    bookId = parseInt(document.getElementById("bookId").value);
    btnAddToCart = document.getElementById("btnAddToCart");
    btnRemoveFromCart = document.getElementById("btnRemoveFromCart");
    btnAddToWishList = document.getElementById("btnAddToWishList");
    btnRemoveFromWishList = document.getElementById("btnRemoveFromWishList");
    spnCartTotal = document.getElementById("spnCartTotal");
}
function addToCart() {
    callServer('../Ajax/UpdateCartAjax.php', 'bookId=' + bookId.toString() + '&quantity=1', (txt) => {
        var info = JSON.parse(txt);
        spnCartTotal.innerHTML = info.cartTotal + " JOD";
        btnAddToCart.hidden = true;
        btnRemoveFromCart.hidden = false;
    });
    return true;
}
function removeFromCart() {
    callServer('../Ajax/UpdateCartAjax.php', 'bookId=' + bookId.toString() + '&quantity=0', (txt) => {
        var info = JSON.parse(txt);
        spnCartTotal.innerHTML = info.cartTotal + " JOD";
        btnAddToCart.hidden = false;
        btnRemoveFromCart.hidden = true;
    });
    return true;
}
function addToWishList() {
    callServer('../Ajax/UpdateWishListAjax.php', 'add=' + bookId.toString(), (x) => {
        btnAddToWishList.hidden = true;
        btnRemoveFromWishList.hidden = false;
    });
}
function removeFromWishList() {
    callServer('../Ajax/UpdateWishListAjax.php', 'remove=' + bookId.toString(), (x) => {
        btnAddToWishList.hidden = false;
        btnRemoveFromWishList.hidden = true;
    });
}