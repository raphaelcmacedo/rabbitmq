$(document).ready(function () {

    
    //Fetch 1 Message
    $("#btnIntegrate").click(function (e) {
        var durable = $("#durable").is(":checked");
        var queue = $("#queueName").val();

        $.ajax({
            url: "SalesForce/IntegrateOneMessage",
            type: 'POST',
            data: { queue: queue, durable:durable },
            success: function (object) {
                if (object.Success) {
                    var message = "The opportunity " + object.data + " has been integrated successfuly.";
                    alert(message);

                } else {
                    alert(object.Message);
                }
            }
        });
    });

    $("#btnFindSalesForce").click(function (e) {
        $.ajax({
            url: "SalesForce/FindAllSalesForce",
            type: 'POST',
            data: { },
            success: function (object) {
                if (object.Success) {
                    var opportunities = object.data;
                    var texto = "";

                    for (var i = 0; i < opportunities.length; i++) {
                        var name = opportunities[i].Name;
                        if (texto.indexOf(name) < 0) {
                            texto += name + "\r\n";
                        }
                    }

                    $("#opportunities").val(texto);
                } else {
                    alert(object.Message);
                }
            }
        });
    });

});