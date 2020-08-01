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
    node.id = $("#productType").val();
    let text_node = document.createTextNode($("#productType").val());
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

function removeTag(el) {
    var element = el;
    $(element).parent().remove();
}
