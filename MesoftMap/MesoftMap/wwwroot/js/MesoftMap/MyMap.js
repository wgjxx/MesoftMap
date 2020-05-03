
function MapUtility() {
    //判断点是否在区域内。由点沿Y轴向上做射线，与多边形交点为偶数时，则不在区域内
    InOrOutPolygon = function (region, point) {
        var nvert = region.points.length;
        var testx = parseFloat(point.lat), testy = parseFloat(point.lng);
        //  int i, j, crossings = 0;
        var crossings = 0;
        for (var i = 0, j = nvert - 1; i < nvert; j = i++) {
            // 点在两个x之间 且以点垂直y轴向上做射线
            var x1 = parseFloat(region.points[i].lat), x2 = parseFloat(region.points[j].lat);
            var y1 = parseFloat(region.points[i].lng), y2 = parseFloat(region.points[j].lng);
            var k = (y2 - y1) / (x2 - x1);
            if ((((x1 < testx) && (x2 >= testx))
                || ((x1 >= testx) && (x2 < testx)))
                && (testy < k * (testx - x1) + y1))
                crossings++;
        }
        console.log('InOrOutPolygon point:', testx)
        console.log('InOrOutPolygon points:', region.points)
        console.log('InOrOutPolygon crossings:', crossings)
        return (crossings % 2 != 0);
    }
    return this;
}

var mapUtility = MapUtility();




