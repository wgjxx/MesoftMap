function CustomOverlayObject(position, innerHtml) {
    this.innerHtml = innerHtml;
    this.position = position;

    getImgHTML=function(imgSrc, width, height, sClass) {
        var html = "<img src='" + imgSrc + "' class='" + sClass + "' width='" + width + "' height='" + height + "' />";
        return html;
    }
    return this;
}
var OverlayObjectFactory = CustomOverlayObject(null, null);