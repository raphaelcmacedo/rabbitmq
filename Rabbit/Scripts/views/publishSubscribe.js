$(document).ready(function () {

    var messages1 = [];
    var messages2 = [];
    var messages3 = [];
    setInterval(callFetch, 2000);
       
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

        var consumerNumber = e.target.id;
        consumerNumber = consumerNumber.slice(-1);
        var exchange = $("#exchangeListener" + consumerNumber).val();
        
        if ($listen.checked) {
            action = 'Listen';
        }
        else {
            action = 'Unlisten';
        }

        if (exchange === '') {
            alert('Please inform a exchange.');
            e.target.checked = false;
            return;
        }

        $.ajax({
            url: "PublishSubscribe/" + action,
            type: 'POST',
            data: { exchange: exchange, threadNumber: consumerNumber },
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
    function callFetch() {
        fetch("1");
        fetch("2");
        fetch("3");
    }


   function fetch (consumerNumber) {
       var messages = [];
       if (consumerNumber === "1") {
           messages = messages1;
       } else if (consumerNumber === "2") {
           messages = messages2;
       } else if (consumerNumber === "3") {
           messages = messages3;
       }

       if (messages.length > 0) {
           var message = messages[0];
           var queue = $("#queue" + consumerNumber).val();

           if (queue.length > 0) {
               queue += "\r\n";
           }
           
           queue += message;
           $("#queue" + consumerNumber).val(queue);
           messages.splice(0, 1);
       } else {
           $.ajax({
               url: "PublishSubscribe/Fetch",
               type: 'POST',
               data: {"threadNumber":consumerNumber},
               success: function (object) {
                   if (object.Success) {
                       if (object.data != null && object.data.length > 0) {
                           messages = object.data.split('\r\n');

                           if (consumerNumber === "1") {
                               messages1 = messages;
                           } else if (consumerNumber === "2") {
                               messages2 = messages;
                           } else if (consumerNumber === "3") {
                               messages3 = messages;
                           }

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