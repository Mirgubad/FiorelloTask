
jQuery(document).ready(function ($) {

    $(document).on("click", '#addToCard', function () {
        var id = $(this).data('id')
        $.ajax({
            method: "POST",
            url: "/basket/add",
            data: {
                id: id
            },
            success: function (result) {
                console.log(result)
            }
        })
    })


    $(document).on("click", '#deleteButton', function () {
        var id = $(this).data('id')

        $.ajax({
            method: "POST",
            url: "/basket/delete",
            data: {
                id: id
            },
            success: function (result) {
                $(`.basket-product[id=${id}]`).remove();


            }
        })
    })


    $(document).on("click", '.quantity-control-less', function () {
        var id = $(this).data('id')
        var value = $(`#input-${id}`).val();
       
        $.ajax({
            method: "POST",
            url: "/basket/less",
            data: {
                id: id
            },
            success: function (result) {
               
              
            }
        })
    })

    $(document).on("click", '.quantity-control-more', function () {
        var id = $(this).data('id')

        $.ajax({
            method: "POST",
            url: "/basket/more",
            data: {
                id: id
            },
            success: function (result) {

               
            }
        })
    })



})

