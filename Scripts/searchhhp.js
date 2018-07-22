let received = [];
let shown = [];
let perm1 = 1, perm2 = 2, perm3 = 3, perm4 = 4, perm5 = 5;
let j = 1, m = 1;

//db
let basee = localStorage.getItem("dbres");
let input = document.querySelector("#srcinput");

let suggest = document.querySelector("#livesearch");
suggest.style.display = "none";
let timeoutID = null;

let mongoUrl = './MongoService.asmx/searchAll';
let ravenUrl = './RavenService.asmx/searchAll';

$("#livesearch").on('click', '.searchcont', function () {
    $(this).css("color", "#0000CC");
    suggest.style.display = "none";
    console.log($(this)[0].children[0].childNodes);
    if ($(this)[0].children[0].childNodes.length > 1)
        input.value = $(this)[0].children[0].childNodes[0].innerHTML + $(this)[0].children[0].childNodes[1].data;
    else
        input.value = $(this)[0].children[0].innerHTML;
    console.log(shown);

    insertIntoTables(shown, $(this)[0].children[0].innerHTML);
});

//kako se kuca tekst tako se i pretraga vrsi
$("#srcinput").keyup(function (e) {
    clearTimeout(timeoutID);
    timeoutID = setTimeout(findMember.bind(undefined, true, e.target.value), 500);
});

//sta se desava kad se klikne dugme go za pregtragu
$("#srcbttn").click(function (e) {

    e.preventDefault();

    $("#listingC").empty();
    $("#listingW").empty();

    var search = $("#srcinput").val();
    suggest.style.display = "none";

    findMember(false, search);

});

$('#listingC').on('click', '.view', function () {
    for (var i = 0; i < received.length; i++) {
        if (received[i].Email === $(this).closest("tr")[0].children[2].innerHTML) {
            if (basee === "raven")
                localStorage.setItem("companyViewR", JSON.stringify(received[i].Id));
            else
                localStorage.setItem("companyView", JSON.stringify(received[i].Id));
        }
    }
    window.location.assign("./companyInfo.aspx");

});

$('#listingW').on('click', '.view', function () {
    for (var i = 0; i < received.length; i++) {
        if (received[i].Email === $(this).closest("tr")[0].children[3].innerHTML) {
            console.log($(this).closest("tr")[0].children[3].innerHTML);
            console.log(basee);
            if (basee === "raven")
                localStorage.setItem("workerViewR", received[i].Id);
            else
                localStorage.setItem("workerView", received[i].Id);
        }
    }
    window.location.assign("./fellowworker.aspx");

});

input.addEventListener("click", (e) => {
    e.stopPropagation();
    if (input.value === "" && suggest.childElementCount === 0) suggest.style.display = "none";
    else suggest.style.display = "block";

});

(document.body || document.documentElement).addEventListener("click", (event) => {
    if (!suggest.contains(event.target))
        suggest.style.display = "none";

});

function findMember(propose, str) {
    console.log('search: ' + str);
    received = [];
    suggest.style.display = "block";
    $("#livesearch").empty();
    let all = true;
    let prep1 = [];
    let searchUrl = null;

    if (basee === "raven" || basee === null) searchUrl = ravenUrl;
    else searchUrl = mongoUrl;

    if ($('#ffirst').not(':checked').length) {
        perm1 = 0;
    }
    else {
        perm1 = 1;
        all = false;
    }

    if ($('#llast').not(':checked').length) {
        perm2 = 0;
    }
    else {
        perm2 = 2;
        all = false;
        console.log(perm2);
    }

    if ($('#mmail').not(':checked').length) {
        perm3 = 0;
    }
    else {
        perm3 = 3;
        all = false;
    }

    if ($('#ccomp').not(':checked').length) {
        perm4 = 0;;
    }
    else {
        perm4 = 4;
        all = false;
    }

    if ($('#skiill').not(':checked').length) {
        perm5 = 0;
    }
    else {
        perm5 = 5;
        all = false;
    }

    if (all === true) {
        prep1 = [1, 2, 3, 4, 5];
    }
    else {
        prep1 = [perm1, perm2, perm3, perm4, perm5];
    }

    getAjaxResponse(JSON.stringify(prep1), str, searchUrl, (data) => {
        let xmldoc = $.parseXML(data),
            $xml = $(xmldoc),
            $title = $xml.find("string");
        let par = JSON.parse($title.text());

        let tmp = [];

        par.forEach((el, ind) => {
            if (el[0] === "[") {
                tmp.push({ 'recv': JSON.parse(el), 'ind': ind });
            }

        });
        console.log(tmp);
        if (tmp.length === 0) alert("Nothing found!");
        else {
            let suggested = [];
            received = [];

            if (str === "") {
                received = tmp;
                received = eliminateDuplicates(received, 0);
                console.log(received);
            } else {
                tmp.forEach((el) => {
                    switch (el.ind) {
                        case 0:
                            el.recv.forEach((elTmp) => {
                                received.push(elTmp);
                                suggested.push({
                                    'Id': elTmp.Id,
                                    'val': elTmp.FirstName,
                                    'collection': 'worker'
                                });
                            });
                            break;
                        case 1:
                            el.recv.forEach((elTmp) => {
                                received.push(elTmp);
                                suggested.push({
                                    'Id': elTmp.Id,
                                    'val': elTmp.LastName,
                                    'collection': 'worker'
                                });
                            });
                            break;
                        case 2:
                            el.recv.forEach((elTmp) => {
                                received.push(elTmp);
                                suggested.push({
                                    'Id': elTmp.Id,
                                    'val': elTmp.Email,
                                    'collection': 'worker'
                                });
                            });
                            break;
                        case 3:
                            el.recv.forEach((elTmp) => {
                                received.push(elTmp);
                                suggested.push({
                                    'Id': elTmp.Id,
                                    'val': elTmp.Email,
                                    'collection': 'company'
                                });
                            });
                            break;
                        case 4:
                            el.recv.forEach((elTmp) => {
                                received.push(elTmp);
                                suggested.push({
                                    'Id': elTmp.Id,
                                    'val': elTmp.CompanyName,
                                    'collection': 'company'
                                });
                            });
                            break;
                        case 5:
                            el.recv.forEach((elTmp) => {
                                let matchedSkill = [];
                                if (Array.isArray(elTmp.Skills)) {
                                    elTmp.Skills.forEach(s => {
                                        if (s.search(new RegExp(str, "i")) !== -1) matchedSkill.push(s);
                                    });
                                } else matchedSkill = elTmp.Skills;
                                received.push(elTmp);
                                suggested.push({
                                    'Id': elTmp.Id,
                                    'val': matchedSkill,
                                    'collection': 'worker'
                                });
                            });
                            break;
                    }
                });

                if (str !== "") {

                    suggested.forEach((el) => {
                        if (!Array.isArray(el.val)) el.val = addSpan(el.val, str);
                        else {
                            el.val.forEach(t => {
                                t = addSpan(t, str);
                            });
                        }
                    });
                }
                //sklanjanje duplikata
                suggested = eliminateDuplicates(suggested, 0);
                received = eliminateDuplicates(received, 0);

                console.log(suggested);
                console.log(received);

                shown = suggested;
            }          

            if (propose === true) {
                let prepHtml = [];
                suggested.forEach(el => {
                    if (!Array.isArray(el.val)) prepHtml.push('<div class="searchcont"><p class="searchcont--val">' + el.val + '</p><p class="searchcont--collection">' + el.collection + '</p></div>');
                    else {
                        el.val.forEach(t => {
                            prepHtml.push('<div class="searchcont"><p class="searchcont--val">' + t + '</p><p class="searchcont--collection">' + el.collection + '</p></div>');
                        });
                    }
                });

                prepHtml = eliminateDuplicates(prepHtml, 1);

                console.log(prepHtml);

                if (prepHtml.length !== 0) {
                    prepHtml.forEach(el => {
                        $('#livesearch').append(el);
                    });
                }
            } else {
                console.log(propose);
                insertIntoTables(received);
            }
        }

    });
}

function insertIntoTables(rec, str = null) {
    $("#listingW").empty();
    $("#listingC").empty();
    let elId = [];

    if (str !== null) {

        rec.forEach((el) => {
            if (!Array.isArray(el.val)) {
                if (str === el.val) {
                    elId.push(el.Id);
                    elId.push(el.collection);
                }
            } else {
                el.val.forEach(t => {
                    if (str === t) {
                        elId.push(el.Id);
                        elId.push(el.collection);
                    }
                });
            }
        });

        if (received.length !== 0 && elId.length !== 0) {
            j = 1, m = 1;
            received.forEach(el => {
                if (el.Id === elId[0]) {
                    putInTable(el, elId[1]);
                }
            });
        }
    } else {
        j = 1, m = 1;
        received.forEach(el => {
            if (el.hasOwnProperty("FirstName"))
                putInTable(el, "worker");
            else
                putInTable(el, "company");
        });
    }    
}

function putInTable(element, collection) {
    if (collection === "worker") {
        $("#listW").css("display", "block");
        let mail = element.Email;
        let first = element.FirstName;
        let last = element.LastName;
        let company = element.CompanyName;
        let skill = null;
        if (element.Skills !== null) {
            skill = element.Skills.join();
        }
        let table = '<tr><th scope= "row">' + j + '</th>'
            + '<td>' + first + '</td>'
            + '<td>' + last + '</td>'
            + '<td>' + mail + '</td>'
            + '<td>' + skill + '</td>'
            + '<td>' + company + '</td>'
            + '<td><button type="button" class="view btn btn-default">View</button></td></tr >';
        $("#listingW").append(table);
        j++;
    } else {
        $("#listC").css("display", "block");
        let company = element.CompanyName;
        let mail = element.Email;
        let type = element.Type;
        let loc = element.Location;
        let table = '<tr><th scope= "row">' + m + '</th>'
            + '<td>' + company + '</td>'
            + '<td>' + mail + '</td>'
            + '<td>' + type + '</td>'
            + '<td>' + loc + '</td>'
            + '<td> <button type="button" class="view btn btn-default">View</button></td></tr>';
        $("#listingC").append(table);
        m++;
    }        
}

function eliminateDuplicates(arr, cond) {
    let type1, type2;
    let tmpNoDoubles = [];
    let i = 0;
    while (i < arr.length) {
        let tmpEl = arr[i];
        tmpNoDoubles.push(tmpEl);
        arr.forEach((el) => {
            if (cond === 0) { type1 = el.Id; type2 = tmpEl.Id; }
            else { type1 = el; type2 = tmpEl; }
            if (type1 !== type2) tmpNoDoubles.push(el);
        });
        arr = tmpNoDoubles;
        tmpNoDoubles = [];
        i++;
    }
    return arr;
}

function addSpan(val, str) {
    let stringValue = val;
    let stringValueCopy = stringValue;
    let match;

    match = new RegExp(str, "i").exec(stringValueCopy)
    stringValueCopy = stringValueCopy.replace(match, "");
    stringValue = stringValue.replace(match, `<span class="found">temporary</span>`);

    stringValue = stringValue.replace(/temporary/g, str);
    let pos = String(val).search(new RegExp(str, "i"));
    if (pos !== -1) val = stringValue;
    return val;
}

function getAjaxResponse(arr, sstring, base, fn) {

    $.ajax({
        url: base,
        dataType: "text",
        type: "POST",
        data: { 'word': sstring, 'check': arr },
        error: function (err) {
            alert("Error", err);
        },
        success: function (data) {
            fn(data);
        }
    });
}