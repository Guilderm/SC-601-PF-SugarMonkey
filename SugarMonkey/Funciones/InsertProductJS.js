
$("#ProductID").change(function () {

    if ($("#ProductID").val() == "0") {
        $("#Name").val("");
        $("#Description").val("");
        $("#CategoryID").val("");
        $("#UnitPrice").val("");
        $("#ImagePath").val("");
        $("#ThumbnailPath").val("");
        $("#percentageOff").val("");
    }
    else {
        $.ajax({
            type: "POST",
            url: '/Product/ConsultarProductAJAX',
            data:
            {
                Productid: $("#ProductID").val(),
                name: $("#Name").val(data),
                description: $("#Description").val(data),
                category: $("#CategoryID").val(data),
                price: $("#UnitPrice").val(data),
                image: $("#ImagePath").val(data),
                thumbnail: $("#ThumbnailPath").val(data),
                discount: $("#percentageOff").val(data)
            },
            dataType: 'json',
            success: function (data) {
                $("#Name").val(data.name);
                $("#Description").val(data.description);
                $("#CategoryID").val(data.category);
                $("#UnitPrice").val(data.price);
                $("#ImagePath").val(data.image);
                $("#ThumbnailPath").val(data.thumbnail);
                $("#percentageOff").val(data.discount);
            },
            error: function (data) {
                alert('mal');
            }
        });
    }


});