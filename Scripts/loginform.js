$(document).ready(function() {
    $("#signinbtn").click(function (e) {

        e.preventDefault();

        var email = $("#emailsignin").val();
        var password = $("#passwordsignin").val();
        var checkbox = $("#remember").val();

        //pravimo data string
        //var dataString = 'email1='+email+'&password1='+password+'&check='+checkbox;

        if (email == ''||password == '')
        {
            alert("Please fill all fields!");
        }
        else {
            $.ajax({
                type: "POST",
                url: "./php/login.php",
                data: {
                    'email': email,
                    'password': password,
                    'remember': checkbox
                },
                success: [function (data) {
                    alert(data);
                }]
            });
        }

        return false;
    });
});
