$(document).ready(function () {

    Listen();
    setInterval(fetch, 5000);
   
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
        $.ajax({
            url: "Rabbit/Fetch",
            type: 'POST',
            data: { },
            success: function (object) {
                if (object.Success) {
                    $("#queue").val(object.data);
                } else {
                    alert(object.Message);
                }

            }
        });
    };

    function clearForm() {
        $("#text").val('');
        $("#queue").val('');
    }

    function Listen() {
        $.ajax({
            url: "Rabbit/Listen",
            type: 'POST',
            data: {}
        });
    }
});