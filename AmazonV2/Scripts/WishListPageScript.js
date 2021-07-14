function removeItem(btn) {
    var row = btn.parentElement.parentElement;
    var rowParent = row.parentElement;
    var bookId = parseInt(row.getElementsByClassName("bookId")[0].innerHTML);

    callServer('../Ajax/UpdateWishListAjax.php', 'remove=' + bookId.toString(), () => {
        rowParent.removeChild(row);
    });
}