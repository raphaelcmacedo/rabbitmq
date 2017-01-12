$(document).ready(function () {

    var messages = [];
    setInterval(fetch, 2000);
       
    //Add
    $("#btnAdd").click(function (e) {
        $.ajax({
            url: "Rabbit/Add",
            type: 'POST',
            data: { text: $("#text").val() },
            success: function (object) {
                clearForm();
                if (object.Success) {
                    alert("Added to queue successfully");
                } else {
                    alert(object.Message);
                }

            }
        });
    });

    //Listen
    $("#listen").click(function (e) {
        var action = '';

        if ($("#listen").is(":checked")) {
            action = 'Listen';
        }
        else {
            action = 'Unlisten';
        }


        $.ajax({
            url: "Rabbit/" + action,
            type: 'POST',
            data: { },
            success: function (object) {
                if (!object.Success) {
                    alert(object.Message);
                }
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

    function Listen() {
        $.ajax({
            url: "Rabbit/Listen",
            type: 'POST',
            data: {}
        });
    }
});