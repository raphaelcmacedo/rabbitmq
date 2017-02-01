$(document).ready(function () {

    var messages = [];
    setInterval(fetch, 2000);
       
    //Add
    $("#btnAdd").click(function (e) {
        var text = $("#text").val();
        var durable = $("#durable").is(":checked");

        $.ajax({
            url: "Rabbit/Add",
            type: 'POST',
            data: { text: text, durable:durable  },
            success: function (object) {
                clearForm();
                if (object.Success) {
                    //alert("Added to queue successfully");
                } else {
                    alert(object.Message);
                }

            }
        });
    });

    //Fetch 1 Message
    $("#btnFetch").click(function (e) {
        var durable = $("#durable").is(":checked");

        $.ajax({
            url: "Rabbit/FetchOneMessage",
            type: 'POST',
            data: { simulateError: false, simulateRejection: false, durable:durable },
            success: function (object) {
                if (object.Success) {
                    var message = object.data;
                    var queue = $("#queue").val();

                    if (queue.length > 0) {
                        queue += "\r\n";
                    }

                    queue += message;
                    $("#queue").val(queue);
                } else {
                    alert(object.Message);
                }

            }
        });
    });

    //Fetch error
    $("#btnError").click(function (e) {
        var durable = $("#durable").is(":checked");

        $.ajax({
            url: "Rabbit/FetchOneMessage",
            type: 'POST',
            data: { simulateError: true, simulateRejection: false, durable: durable },
            success: function (object) {
                if (object.Success) {
                    
                } else {
                    alert(object.Message);
                }

            }
        });
    });

    //Reject
    $("#btnReject").click(function (e) {
        var durable = $("#durable").is(":checked");

        $.ajax({
            url: "Rabbit/FetchOneMessage",
            type: 'POST',
            data: { simulateError: false, simulateRejection: true, durable: durable },
            success: function (object) {
                if (object.Success) {

                } else {
                    alert(object.Message);
                }

            }
        });
    });

    //Listen
    $("#listen").click(function (e) {
        var action = '';
        var durable = $("#durable").is(":checked");

        if ($("#listen").is(":checked")) {
            action = 'Listen';
        }
        else {
            action = 'Unlisten';
        }


        $.ajax({
            url: "Rabbit/" + action,
            type: 'POST',
            data: {durable, durable}, 
            error:function(e){
                alert(e);

            }
        
        });
    });


    //Fetch
   function fetch () {
       if (messages.length > 0){
           var message = messages[0];
           var queue = $("#queue").val();

           if (queue.length > 0) {
               queue += "\r\n";
           }
           
           queue += message;
           $("#queue").val(queue);
           messages.splice(0, 1);
       } else {
           $.ajax({
               url: "Rabbit/Fetch",
               type: 'POST',
               data: {},
               success: function (object) {
                   if (object.Success) {
                       if (object.data.length > 0) {
                           messages = object.data.split('\r\n');
                           fetch();
                       }
                   } else {
                       alert(object.Message);
                   }

               }
           });
       }


    };

    function clearForm() {
        $("#text").val('');
    }

});