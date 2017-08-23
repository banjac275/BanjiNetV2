$(document).ready(function() {
    $("#signinbtn").click(function (e) {

        e.preventDefault();

        var Email = $("#emailsignin").val();
        var Password = $("#passwordsignin").val();
        var Checkbox = $("#remember").val();

        //pravimo data string
        //var dataString = {'Email': Email,'Password': Password,'Checkbox': Checkbox};

        if (Email == ''||Password == '')
        {
            alert("Please fill all fields!");
        }
        else {
            $.ajax({
                type: "POST",
                url: "./MongoService.asmx/returnWorkerFromEmail",
                data: { 'mail': Email },
                success: [function (data) {
                    alert(data);
                }],
                error: function (result) {
                    alert(result);
                }
            });
        }

        return false;
    });
});
