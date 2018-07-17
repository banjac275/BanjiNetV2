$(document).ready(function () {
    var received;
    var WId = "./RavenService.asmx/retWorkerFromIdR";
    var CId = "./RavenService.asmx/retCompanyFromIdR";
    var poss = " is new company that has joined our network!";
    var poss2 = " is new person that has joined our network!";
    var poss3 = " has updated profile!";
    var poss4 = " has added as employer ";
    var poss5 = " is friends with ";
    var poss6 = " is no longer friends with ";
    var count = 0;

    AjaxServiceAllChanges(function (data) {
        var xmldoc = $.parseXML(data),
            $xml = $(xmldoc),
            $title = $xml.find("Changes");
        //var parsed = JSON.parse($title);
        received = $title;
        console.log($title);

        for (var i = $title.length-1; i >= 0; i--)
        {
            count++;
            var t2 = false;
            
            var types = $title[i].children[0].innerHTML;
            var times;
            if (types === poss || types === poss2 || types === poss3) {
                times = $title[i].children[5].innerHTML;
                t2 = true;
            }
            else {
                t2 = false;
                times = $title[i].children[7].innerHTML;
            }
            console.log($title[i].children[0].innerHTML);
          
            if (t2 !== true)
            {
                let not = '<hr/><div class="row"><div class="info col-lg-7">' + $title[i].children[2].innerHTML
                    + types + $title[i].children[5].innerHTML + '</div><div class="when col-lg-5">' + times + '</div></div><hr/>';
                $("#contents").append(not);
            }

            if (t2 === true)
            {
                let not = '<hr/><div class="row"><div class="info col-lg-7">' + $title[i].children[2].innerHTML
                    + types + '</div><div class="when col-lg-5">' + times + '</div></div><hr/>';
                $("#contents").append(not);
            }
                
                        
        }

    });

    

    function getLength(arr) {
        return Object.keys(arr).length;
    }

    function AjaxServiceAllChanges(fn) {

        $.ajax({
            url: "./RavenService.asmx/retAllChanges",
            dataType: "text",
            type: "POST",
            error: function (err) {
                alert("Error", err);
            },
            success: function (data) {
                fn(data);
            }
        });
    }

});