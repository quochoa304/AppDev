function addItem(bookId, quantity = 1) {
    //var url = new URL(`/Cart/AddItem?bookId=${bookId}&quantity=${quantity}`,)
    fetch(`/Cart/AddItem?bookId=${bookId}&quantity=${quantity}`, {
        method: 'GET',
    }).then((res) => {
        if (res.ok) {
            console.log(`/Cart/AddItem?bookId=${bookId}&quantity=${quantity} is ok`)
        }
    })
}