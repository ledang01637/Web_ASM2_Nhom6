$(document).ready(function () {
    $('#btnDelete').click(function (e) {
        e.preventDefault();
        var productId = $(this).attr('rel-p');
        var isDelete = true;
        var button = $(this);
        $.ajax({
            url: 'User/Cart/DeleteProduct',
            type: 'POST',
            data: { id: productId, isDelete: isDelete },
            success: function (response) {
                if (response.success) {
                    var row = button.closest('tr');
                    row.remove();
                    calculateTotal();
                }
            }
        });
    });
    //Reset
    $('.btn-rfresh-qty').click(function (e) {
        e.preventDefault();
        $('.txt-qty').val(1);
        calculateTotal();
    });
    //ChangeTotal
    $('.txt-qty').change(function () {
        calculateTotal();
    });
    //Total
    function calculateTotal() {
        var totalPrice = 0;
        $('.txt-qty').each(function (index) {
            var quantity = parseInt($(this).val());
            var price = parseFloat($('.price').eq(index).text().replace(' Đ', '').replace(/,/g, ''));
            var total = quantity * price;
            $('.total').eq(index).text(total.toLocaleString() + ' Đ');
            totalPrice += total;
        });
        $('.total-price').text(totalPrice.toLocaleString() + ' Đ');
    }
    calculateTotal();
});