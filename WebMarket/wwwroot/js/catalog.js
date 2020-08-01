function onClickedCard(clicked_id) {
}

function addOnHoverDiv(hovered_id) {
    let card_id = "#card_expand_" + hovered_id;
}
function onHoveredCard(hovered_id) {
}
function onHoveredCardOut(hovered_id) {
}

function onHoveredButton(hovered_id) {
    let btn_id = "#card_addtocart_button_" + hovered_id;
    if ($(btn_id).text() == "+") {
        $(btn_id).text("Buy");
    }
    if ($(btn_id).text() == "Bought") {
        $(btn_id).text("Sell");
    }
}
function onHoveredButtonOut(hovered_id) {
    let btn_id = "#card_addtocart_button_" + hovered_id;
    if ($(btn_id).text() == "Buy") {
        $(btn_id).text("+");
    }
    if ($(btn_id).text() == "Sell") {
        $(btn_id).text("Bought");
    }
}

function addTag() {
    let node = document.createElement("SPAN");
    node.className += "badge badge-pill badge-primary tag";
    node.style.marginLeft = "12px";
    node.id = $("#productType_add").val();
    let text_node = document.createTextNode($("#productType_add").val());
    node.appendChild(text_node);

    //! adding cross for deleting tag
    let cross_node = document.createElement("I");
    cross_node.className += "fas fa-times tag-cross";
    cross_node.style.paddingLeft = "4px";
    node.appendChild(cross_node);

    if (document.getElementById(node.id) == null) {
        document.getElementById("tags").appendChild(node);
    }

    //! binding removing of a tag on click
    $(cross_node).on('click', function () {
        $(this).parent().remove();
    });
}

function addFindTag() {
    let node = document.createElement("SPAN");
    node.className += "badge badge-pill badge-primary tag";
    node.style.marginLeft = "12px";
    node.id = $("#findTag").val();
    let text_node = document.createTextNode($("#findTag").val());
    node.appendChild(text_node);

    //! adding cross for deleting tag
    let cross_node = document.createElement("I");
    cross_node.className += "fas fa-times tag-cross";
    cross_node.style.paddingLeft = "4px";
    node.appendChild(cross_node);

    if (document.getElementById(node.id) == null) {
        document.getElementById("findTags").appendChild(node);
    }

    //! binding removing of a tag on click
    $(cross_node).on('click', function () {
        $(this).parent().remove();
    });
}

function removeFindTag(el) {
    var element = el;
    $(element).parent().remove();
}

function countChars(obj) {
    var maxLength = obj.maxLength;
    var strLength = obj.value.length;
    var charRemain = (maxLength - strLength);

    if (charRemain < 0) {
        document.getElementById('descriptionCharCounter_add').innerHTML = '<span style="color:red;">You have exceeded the limit of ' + maxLengthZ + ' characters</span>';
    }
    else {
        document.getElementById('descriptionCharCounter_add').innerHTML = charRemain + ' characters remaining';
    }
}

function sortProducts() {

}

$("#submitProductButton").on("click", function (e) {
    var tags_array = [];

    $('#tags > span').each(function () {
        tags_array.push($(this).text());
    });
    var prod_name = $('#productName_add').val();
    var prod_zip = $('#productZipFile').val();

    $.ajax({
        type: "post",
        dataType: "json",
        url: "/Catalog/AddProduct",
        traditional: true,
        data: {
            productName: prod_name,
            tags: tags_array,
            productZipFile: prod_zip
        },
        success: function (data) {
            console.log(data.message);
        },
    });
});

$("#applyShowProductsButton").on("click", function (e) {
    //e.preventDefault();

    var find_tags_array = [];

    $('#findTags > span').each(function () {
        find_tags_array.push($(this).text());
    })

    $.ajax({
        type: "post",
        dataType: "json",
        url: "/Catalog/SubmitTags",
        traditional: true,
        data: {
            findTags: find_tags_array,
        },
        success: function (data) {
            condole.log(data.message);
        },
    });
});