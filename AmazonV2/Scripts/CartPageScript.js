var spnCartTotal;
var cellNumberOfItems;
var cellTotalPrice;
function setup() {
    spnCartTotal = document.getElementById("spnCartTotal");
    cellNumberOfItems = document.getElementById("cellNumberOfItems");
    cellTotalPrice = document.getElementById("cellTotalPrice");
}
function updateItem(inp) {
    if (parseInt(inp.value) <= 0) { return false; }
    var row = inp.parentElement.parentElement;
    var bookId = parseInt(row.getElementsByClassName("bookId")[0].innerHTML);
    var cellSubTotal = row.getElementsByClassName("cellSubTotal")[0];
    var cellError = row.getElementsByClassName("error-cell")[0];

    callServer('../Ajax/UpdateCartAjax.php', 'bookId=' + bookId.toString() + '&quantity=' + inp.value, (txt) => {
        var info = JSON.parse(txt);
        spnCartTotal.innerHTML = info.cartTotal + " JOD";
        cellTotalPrice.innerHTML = info.cartTotal.toFixed(2) + " JOD";
        cellSubTotal.innerHTML = info.itemTotal.toFixed(2);
        cellNumberOfItems.innerHTML = info.numberOfItems;
        if (cellError !== null && cellError !== undefined) {
            row.removeChild(cellError);
        }
    });
}
function removeItem(btn) {
    var row = btn.parentElement.parentElement;
    var rowParent = row.parentElement;
    var bookId = parseInt(row.getElementsByClassName("bookId")[0].innerHTML);

    callServer('../Ajax/UpdateCartAjax.php', 'bookId=' + bookId.toString() + '&quantity=0', (txt) => {
        var info = JSON.parse(txt);
        rowParent.removeChild(row);
        spnCartTotal.innerHTML = info.cartTotal + " JOD";
        cellTotalPrice.innerHTML = info.cartTotal.toFixed(2) + " JOD";
        cellNumberOfItems.innerHTML = info.numberOfItems;
    });
}
function ccheckout() {
    window.location = "CheckoutPage.php";
}