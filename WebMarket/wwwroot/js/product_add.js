$(document).ready(function () {
    $('.custom-file-input').on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
    });
});

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
    //var prod_name = $('#productName_add').val();
    //var prod_zip = $('#productZipFile').val();

    $.ajax({
        type: "post",
        dataType: "json",
        url: "/Product/AddTags",
        traditional: true,
        data: {
            //productName: prod_name,
            //productType: prod_type,
            tags: tags_array,
            //productZipFile: prod_zip
        },
        success: function (data) {
            console.log(data.message);
        },
    });
});