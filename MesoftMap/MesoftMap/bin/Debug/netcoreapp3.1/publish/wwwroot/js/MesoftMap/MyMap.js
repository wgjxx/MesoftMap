
function MapProperty() {
    var path1 = [
        new qq.maps.LatLng(39.999216, 116.305389),
        new qq.maps.LatLng(39.972516, 116.299725),
        new qq.maps.LatLng(39.974489, 116.326675)
    ];
    var path2 = [
        new qq.maps.LatLng(40.003950, 116.285648),
        new qq.maps.LatLng(40.006185, 116.304016),
        new qq.maps.LatLng(39.984881, 116.337147),
        new qq.maps.LatLng(39.970937, 116.313457),
        new qq.maps.LatLng(39.980146, 116.278954),
        new qq.maps.LatLng(40.003950, 116.285648)
    ];

    var fillColor = new qq.maps.Color(220, 38, 38, 0.2);//  Color(220, 38, 38, 0.2);
    this.getPath1 = function () {
        return path1;
    } 
    this.getPath2 = function () {
        return path2;
    }
    this.getFillColor = function () {
        return fillColor;
    }
}

var mapProperty = new MapProperty();




