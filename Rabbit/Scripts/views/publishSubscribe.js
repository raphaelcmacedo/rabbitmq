$(document).ready(function () {

    var messages = [];
    //setInterval(fetch, 2000);
       
    //Add
    $("#btnAdd").click(function (e) {
        var text = $("#text").val();
        var exchange = $("#exchangeSend").val();

        $.ajax({
            url: "PublishSubscribe/Add",
            type: 'POST',
            data: { text: text, exchange: exchange },
            success: function (object) {
                clearForm();
                if (object.Success) {
                    
                } else {
                    alert(object.Message);
                }

            }
        });
    });

    function listen(e) {
        var action = '';
        var $listen = e.target;

        if ($listen.checked) {
            action = 'Listen';
        }
        else {
            action = 'Unlisten';
        }

        var consumerNumber = e.target.id;
        consumerNumber = consumerNumber.slice(-1);
        var exchange = $("#exchangeListener" + consumerNumber).val();

        if (exchange === '') {
            alert('Please inform a exchange.');
            return;
        }

        $.ajax({
            url: "PublishSubscribe/" + action,
            type: 'POST',
            data: { exchange: exchange },
            error: function (e) {
                alert(e);

            }

        });
    }

    //Listen
    $("#listen1").click(listen);
    $("#listen2").click(listen);
    $("#listen3").click(listen);


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