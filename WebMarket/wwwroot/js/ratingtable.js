// pos must be r (right) or l (left)
function setStarsWidth(pos, index, final_width) {
    let bar_id = "#" + pos + "bar-" + index;
    $(bar_id).css("width", final_width)
}