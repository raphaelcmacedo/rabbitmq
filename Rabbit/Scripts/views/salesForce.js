$(document).ready(function () {

    var messages = [];
    setInterval(fetch, 2000);
    
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
                    var quoteName = "The opportunity " + object.data + " has been integrated successfuly.";
                    var queue = $("#queue").val();

                    if (queue.length > 0) {
                        queue += "\r\n";
                    }

                    queue += quoteName;
                    $("#queue").val(queue);
                } else {
                    alert(object.Message);
                }

            }
        });
    });

});