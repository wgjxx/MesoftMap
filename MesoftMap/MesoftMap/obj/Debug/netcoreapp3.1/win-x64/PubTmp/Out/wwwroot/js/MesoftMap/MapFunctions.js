var vm = new Vue({
    el: "#myapp",
    data: {
        showLoading: true,
        msg: "Hello World",
        msg2: "<b>Hello World<b>",
        PleaseClickMe: "点击我",
        mapInfoes: [],
        httpHost: '',
        regionTipInfo: {},
        map: {},
        path: [],
        mapProperty: {
            center: new qq.maps.LatLng(34.59251962614407, 114.59014890482649),
            zoom: 10,
            east_north: new qq.maps.LatLng(34.419910, 114.207490),   //东北角
            west_south: new qq.maps.LatLng(34.419910, 114.207490)
        },
        polygons: [],

        isAddingRegion: false,
        isEditingRegion: false,
        isEditingRegionColor: false,
        regionName: '',
        regionFillColor: '',
        regionStrokeColor: '',
        cuRregionPath: { points: [] },
        selectedRegion: {},
        regions: [],
        polygonProperty: {
            defaultFillColor: new qq.maps.Color(38, 145, 234, 0.2),
            selectedFillColor: new qq.maps.Color(38, 145, 234, 0.5),
            defaultStokeColor: new qq.maps.Color(38, 145, 234, 1.0)
        },
        LeftSideVisible: true,
        mapInfoCount: 0,

        latlngInfos: [],   //显示的坐标信息
        mapDisplayOption: {
            isDisplayRegions: false,
            displayLatLng: false,
            isDisplayAllMarkers: true,
            isSetBoundary: true,
            canSetRegionColor: false,
            position_TopOfMapClassDisplayOption: '460px'
        },
        markerClassVisible: [true, true, true, true, true, true]   //对应的类别标注是否显示,markerClassVisible[0]对应mapClassID=1
    },
    mounted() {
        this.switchLeftSide();
        this.init();
    },
    methods: {
        init() {
            this.httpHost = window.location.protocol + '//' + window.location.host;
            this.initMapInfoes();
            var map = this.map;
            var that = this;
            var mapM = document.getElementById("mapContainer");


            //添加监听事件
            qq.maps.event.addListener(map, 'click', function (e) {
                that.onClickMap(e);
                //console.log('onClickMap',e)
            }, true);
            qq.maps.event.addListener(map, 'keypress', function (e) {

                console.log('map keypress:', e)
            }, true);
            qq.maps.event.addListener(map, 'mousemove', function (event) {
                var latLng = event.latLng,
                    lat = latLng.getLat().toFixed(5),
                    lng = latLng.getLng().toFixed(5);
                document.getElementById("latLng").innerHTML = lat + ', ' + lng;
            });
            qq.maps.event.addListener(map, 'zoom_changed', function () {
                var zoomLevel = map.getZoom();

                that.mapProperty.zoom = zoomLevel;

            });

        },
        setDisplayAllMarkers(isDisplay) {
            this.mapDisplayOption.isDisplayAllMarkers = isDisplay;
            this.onDisplayAllMapClass();
        },
        switchLeftSide() {
            this.LeftSideVisible = !this.LeftSideVisible;
            if (this.LeftSideVisible) {
                $('#mesoft-footer').show();
                $('#mesoft-header').show();
            } else {
                $('#mesoft-footer').hide();
                $('#mesoft-header').hide();
            }

        },
        setMapCenter(center) {
            var center = this.mapProperty.center;
            this.map.setCenter(center);
            console.log('center map:', center);
        },
        initMapInfoes() {
            var that = this;
            this.map = new qq.maps.Map(document.getElementById('mapContainer'), {
                center: this.mapProperty.center,
                zoom: this.mapProperty.zoom,
                draggableCursor: "crosshair",
                draggingCursor: "pointer",
            });
            this.regionTipInfo = new qq.maps.InfoWindow({
                map: this.map,
                content: "",
                position: this.mapProperty.center,
                zIndex: 10,
                visible: this.mapDisplayOption.displayLatLng
            });

            $.ajax({
                type: "get",
                url: that.httpHost + '/api/Map/GetMapDisplayOption',  //先获取地图显示的属性
                //data: { providerID: that.providerID },
                contentType: "application/json; charset=utf-8",
                dataType: "json",//表示后台返回的数据是json对象
                success: function (data) {

                    that.mapDisplayOption = data;
                    //that.markerClassVisible.forEach((item, index) => that.markerClassVisible[index] = data.isDisplayAllMarkers)
                    for (var i = 0; i < that.markerClassVisible.length; i++) {
                        that.markerClassVisible[i] = data.isDisplayAllMarkers;
                    }
                    that.drawMapInfoesFromServer();
                },
                error: function (error) {
                    console.log('error:', error);
                    alert(error);
                }
            });

        },
        //从服务器下载标注、区域等数据并显示出来
        drawMapInfoesFromServer() {
            var that = this;
            $.ajax({
                type: "get",
                url: that.httpHost + '/api/Map/GetAllMapinfoes',
                //data: { providerID: that.providerID },
                contentType: "application/json; charset=utf-8",
                dataType: "json",//表示后台返回的数据是json对象
                success: function (data) {

                    if (!data || data.codeId < 0) return;
                    var items = data.data;
                    console.log('all mapinfoes:', data);
                    that.mapInfoes = items;
                    console.log('all mapinfoes:', that.mapInfoes);
                    that.mapProperty.east_north = new qq.maps.LatLng(items[0].lat, items[0].lon);   //东北角
                    that.mapProperty.west_south = new qq.maps.LatLng(items[0].lat, items[0].lon);   //西南角

                    var chunkCount = parseInt(items.length > 40 ? 20 : items.length / 2);  //将dom操作分块执行，提升用户交互体验
                    var chunksize = parseInt(items.length / chunkCount);
                    while (chunksize * chunkCount < items.length) {
                        chunkCount += 1;
                    }
                    for (var chunk = 0; chunk < chunkCount; chunk++) {
                        var curSize = (chunk + 1) * chunksize;

                        setTimeout(that.drawChunk(items, chunk * chunksize, curSize)
                            , 254);
                    }

                    var zoom = 10;
                    that.map.setZoom(zoom);

                    that.setBoundary(that.mapProperty.east_north, that.mapProperty.west_south);


                    that.initRegions();
                    setTimeout(function () {

                        var index = items.length % 2 == 0 ? items.length / 2 : (items.length - 1) / 2;
                        var selItem = items[index];
                        if (selItem.lat <= 0.1) return;
                        var center = new qq.maps.LatLng(selItem.lat, selItem.lon);

                        console.log('success to set center:', center);
                        //that.setMapCenter(center);
                    }, 254);

                    console.log('that.showLoading:', that.showLoading)
                },
                error: function (error) {
                    console.log('error:', error);
                    that.showLoading = false;
                }
            });
        },
        //显示一块标注
        drawChunk(items, start, end) {
            var curSize = curSize;

            var that = this;

            for (var i = start; i < end; i++) {
                if (i >= items.length) {

                    return;
                } else if (i == items.length - 1) {
                    that.showLoading = false;   //显示最后一个标注时
                }
                var item = items[i];
                this.drawMapInfo(item, false);
                if (item.lat <= 0.1) break;

                if (item.lat > this.mapProperty.east_north.lat) {
                    this.mapProperty.east_north.lat = item.lat;
                }
                if (item.lon > this.mapProperty.east_north.lng) {
                    this.mapProperty.east_north.lng = item.lon;
                }
                if (item.lat < this.mapProperty.west_south.lat) {
                    this.mapProperty.west_south.lat = item.lat;
                }
                if (item.lon < this.mapProperty.west_south.lng) {
                    this.mapProperty.west_south.lng = item.lon;
                }
            }
            //需要稍微扩大一下范围，这样可以把边缘的点显示完整
            this.mapProperty.east_north.lat += 0.01;
            this.mapProperty.east_north.lng += 0.01;
            this.mapProperty.west_south.lat -= 0.01;
            this.mapProperty.west_south.lng -= 0.01;

            //var randIndex = parseInt(Math.random() * (end - start) + start, 10);
            //if (randIndex >= items.length) randIndex = items.length - 1;
            if (end >= items.length - 1) start = 0;
            var index = (end + start) % 2 == 0 ? (end + start) / 2 : (end + start - 1) / 2;
            var selItem = items[index];
            selItem.klat = (this.mapProperty.east_north.lat + this.mapProperty.west_south.lat) / 2;
            selItem.klng = (this.mapProperty.east_north.lng + this.mapProperty.west_south.lng) / 2;
            console.log('selitem this.mapProperty.east_north:', this.mapProperty.east_north)
            console.log('selitem this.mapProperty.west_south:', this.mapProperty.west_south)
            var center = new qq.maps.LatLng(selItem.lat, selItem.lon);

            if (selItem.lat <= 0.2) return;
            that.setMapCenter(center);
            console.log('mapinfo count:', this.mapInfoCount);
        },
        drawMapInfo(item, infoVisible = false) {
            var lat = item.lat;
            var lng = item.lon;
            if (lat <= 0.2) return;

            this.mapInfoCount += 1;
            var fillColor = new qq.maps.Color(20, 169, 30, 0.2);

            var center = new qq.maps.LatLng(lat, lng);
            this.drawCustomOverLay(item);

            //var circle = !item.circle ? new qq.maps.Circle({
            //    map: this.map,
            //    visible: this.markerClassVisible[item.mapClassID - 1],
            //    radius: 300,
            //    fillColor: fillColor,
            //    strokeWeight: 3
            //}) : item.circle;
            //circle.setCenter(center);

            var marker = !item.marker ? new qq.maps.Marker({
                //设置Marker的位置坐标
                //position: center,
                //设置显示Marker的地图
                //map: this.map,
                title: item.name,
                visible: this.markerClassVisible[item.mapClassID - 1]
            }) : item.marker;
            marker.setPosition(center);

            //闪烁效果
            if (item.isTwinkle) {
                var anchor = new qq.maps.Point(6, 6),
                    size = new qq.maps.Size(24, 24),
                    origin = new qq.maps.Point(0, 0),
                    icon = new qq.maps.MarkerImage('/images/Twinkle/twinkle.gif', size, origin, anchor);

                marker = new qq.maps.Marker({
                    icon: icon,
                    //map: this.map,
                    title: item.name,
                    visible: this.markerClassVisible[item.mapClassID - 1],
                    position: center
                });
            }
            marker.setMap(this.map);
            //添加信息窗口



            if (!item.marker) {
                item.marker = marker;

                //item.circle = circle;
                var that = this;
                qq.maps.event.addListener(marker, 'click', function () {
                    var info = new qq.maps.InfoWindow({
                        map: that.map,
                        position: center,
                        visible: true
                    });
                    info.setContent(that.getInfoContent(item));
                    info.setPosition(center);
                    item.info = info;
                    $('#justFocus')[0].focus();
                });

            }
        },

        setBoundary(east_north, west_south) {
            if (!this.mapDisplayOption.isSetBoundary) return;

            east_north.lng = (east_north.lng) + 0.3;
            east_north.lat = east_north.lat + 0.3;

            west_south.lng = west_south.lng - 0.3;
            west_south.lat = west_south.lat - 0.3;

            var latlngBounds = new qq.maps.LatLngBounds(west_south, east_north);
            this.map.setBoundary(latlngBounds);
            console.log('success to set Bounds:', latlngBounds)
        },
        drawCustomOverLay(item) {   //在元素位置设置遮盖物，默认是一张关联图片

            if (!item.isHaveImage) return;   //没有图片或者不需要显示

            var position = new qq.maps.LatLng(item.lat, item.lon);
            var innerHtml = OverlayObjectFactory.getImgHTML("/images/" + item.mapClassID + "/" + item.id + ".jpg", 50, 50, 'opacityImage');

            var latlng = position;
            var overlay = item.overlay ? item.overlay : new CustomOverlay(latlng, innerHtml);
            overlay.setMap(this.map);
            if (!item.overlay) {
                item.overlay = overlay;
            }
            if (!this.markerClassVisible[item.mapClassID - 1]) {
                overlay.setMap(null);
            }
        },
        onDisplayAllMapClass() {
            console.log('this.isDisplayAllMarkers', this.mapDisplayOption.isDisplayAllMarkers)
            for (var i = 0; i < this.markerClassVisible.length; i++) {
                this.markerClassVisible[i] = this.mapDisplayOption.isDisplayAllMarkers;
            }
            var items = this.mapInfoes;
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                visible = this.mapDisplayOption.isDisplayAllMarkers;
                if (item.marker)
                    item.marker.setVisible(visible);
                //item.circle.setVisible(visible);
                this.drawCustomOverLay(item);
                if (!visible && item.info) { item.info.close(); }
            }
        },
        displayMapClass(e) { //设置是否显示某类别标注
            if (e.target.value > 0)
                this.mapDisplayOption.isDisplayAllMarkers = this.markerClassVisible.filter(item => item == true).length == this.markerClassVisible.length;


            if (this.mapDisplayOption.isDisplayAllMarkers || e.target.value == 0) {
                this.onDisplayAllMapClass();
                return;
            }

            var mapClassID = e.target.value;

            var items = this.mapInfoes.filter(item => item.mapClassID == mapClassID);
            console.log('displayMapClass:', items);
            var visible = false;
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                visible = this.markerClassVisible[mapClassID - 1];
                item.marker.setVisible(visible);
                //item.circle.setVisible(visible);
                this.drawCustomOverLay(item);
                if (!visible && item.info) { item.info.close(); }
            }
        },
        getInfoContent(item) {
            var content = item.region + ' <br />' + item.name + '<br /> 地址：' + item.address + ' <br />备注：' + item.memo;
            switch (item.mapClassID) {
                case 1:
                    //重点监管单位清单  显示县区   企业名称  地址   行业类别  纳入名单原因
                    content = item.region + ' <br />' + item.name +
                        '<br /> 地址：' + (item.address ? item.address : '') +
                        ' <br />行业类别：' + item.industry
                        + ' <br />纳入原因：' + (item.monitorMemo ? item.monitorMemo : '');
                    break;
                case 2:
                    //涉重金属企业清单   显示企业名称  县区   重点行业类型   主要原料   根据环评计算的最大排放量   新增量   减排量   合计
                    content = item.region + ' <br />' + item.name + ' <br />行业类别：' + item.industry +
                        ' <br />主要原料：' + item.mainMaterial
                        + ' <br />最大排放：' + item.maxDischarged +
                        ' <br />新增量：' + (item.addDischarged ? item.addDischarged : '') +
                        ' <br />减排量：' + (item.subDischarged ? item.subDischarged : '')
                        + ' <br />合计：' + item.sumDischarged;
                    break;
                case 3:
                    //危险废物经营范围清单显示    县区   企业名称   地址   危险废物经营类别和代码   经营规模   经营方式
                    content = item.region + ' <br />' + item.name +
                        ' <br />地址：' + (item.address ? item.address : '') +
                        ' <br />经营类别和代码：' + item.dangerNo
                        + ' <br />经营规模：' + (item.dangerSum ? item.dangerSum : '') +
                        ' <br />经营方式：' + item.dangerKind;
                    break;
                case 4:
                    //开封市污染地块名录及开发利用清单  表格中的都需要显示
                    content = (item.region ? item.region : '') +
                        ' <br />' + item.name +
                        ' <br />地址：' + (item.address ? item.address : '')
                        + ' <br />行业类别：' + item.industry +
                        ' <br />纳入原因：' + (item.monitorMemo ? item.monitorMemo : '');
                    break;
                case 5:
                    //开封市疑似污染地块清单显示   县区   地块名称   地址   行业类别   地块情况   地块创建时间
                    content = item.region + ' <br />' + item.name +
                        ' <br />地址：' + (item.address ? item.address : '')
                        + ' <br />行业类别：' + item.industry + ' <br />地块情况：' + item.mainMaterial
                        + ' <br />地块创建时间：' + item.maxDischarged;
                    break;
                case 6:
                    //
                    break;
                default:
                    break;
            }
            return content;
        },


        //初始化显示轮廓
        initRegions() {
            var that = this;
            $.ajax({
                type: "get",
                url: this.httpHost + '/api/Map/GetAllRegions',
                //data: { providerID: that.providerID },
                contentType: "application/json; charset=utf-8",
                dataType: "json",//表示后台返回的数据是json对象
                success: function (data) {

                    if (!data || data.codeId < 0) return;
                    var items = data.data;
                    console.log('success get map regions:', items);
                    for (var i = 0; i < items.length; i++) {
                        var region = items[i];
                        if (region.points && region.points.length == 0) continue;
                        region.id = region.points[0].regionID;  //保存轮廓的ID号，用于以后的编辑使用

                        var points = [];
                        for (var j = 0; j < region.points.length; j++) {
                            points.push(new qq.maps.LatLng(region.points[j].lat, region.points[j].lng));
                        };
                        region.points = points;

                        that.drawRegion(region);

                        that.regions.push(region);
                        console.log('success get map region:', region);
                    }
                    console.log('success init retions:', that.regions);
                },
                error: function (error) {
                    console.log('error:', error)
                }
            });
        },
        displayRegionsChange(e) {
            this.onDisplayRegions(this.mapDisplayOption.isDisplayRegions);
        },
        onDisplayRegions(isDisplay) {
            this.regions.forEach(item => item.polygon.setVisible(isDisplay));
        },

        addRegions() {
            this.isAddingRegion = true;

            if (!this.mapDisplayOption.isDisplayRegions) {
                this.mapDisplayOption.isDisplayRegions = true;
                this.onDisplayRegions(true);
            }
            alert('提醒：开始设置区域，请依序点击要设置的区域轮廓边缘，完成后请保存！');

            this.cuRregionPath.polyline = new qq.maps.Polyline({
                map: this.map,
                strokeDashStyle: 'dash',
                editable: true
            });
            $('#justFocus')[0].focus();
        },
        onEditRegion() {
            this.isEditingRegion = true;

            region = this.selectedRegion;
            this.regionName = region.regionName;
            if (!this.mapDisplayOption.isDisplayRegions) {
                this.mapDisplayOption.isDisplayRegions = true;
                this.onDisplayRegions(true);
            }

            if (!this.cuRregionPath.polyline) {
                this.cuRregionPath.polyline = new qq.maps.Polyline({
                    map: this.map,
                    strokeDashStyle: 'dash',
                    editable: true
                });
            }
            this.cuRregionPath.points = region.polygon.path.elems.filter(item => item.lat > 0);

            this.cuRregionPath.polyline.setPath(this.cuRregionPath.points);   //将选中的区域显示轮廓
            region.polygon.setPath([]);   //先将选中的区域隐藏
            $('#justFocus')[0].focus();
        },

        drawRegion(region) {
            var strokeColor = region.strokeColor ? this.getQQColor(region.strokeColor) : this.polygonProperty.defaultStokeColor;
            var points = region.points;
            var polygon = !region.polygon ? new qq.maps.Polygon({
                map: this.map,
                strokeColor: strokeColor,
                strokeDashStyle: 'dash',
                regionID: region.id,        //通过这个值记录regionID的值
                title: region.regionName
            }) : region.polygon;

            if (!region.polygon) region.polygon = polygon;
            polygon.setPath(points);
            polygon.setVisible(this.mapDisplayOption.isDisplayRegions);
            var fillColor = region.fillColor ? this.getQQColor(region.fillColor) : this.polygonProperty.defaultFillColor;
            polygon.setFillColor(fillColor);
            polygon.origFillColor = fillColor;            

            if (!region.polygon) {
                region.polygon = polygon;
            }
            {//添加监听事件
                
                var that = this;
                qq.maps.event.addListener(polygon, 'click', function (e) {
                    if (that.isAddingRegion || that.isEditingRegion || that.isEditingRegionColor) return;  //如果正在添加或修改区域，则没有选中操作

                    var selectedPolygon = e.target;

                    that.onSelectRegion(selectedPolygon.regionID);
                });
                qq.maps.event.addListener(polygon, 'mouseover', function (e) {
                    return;  //屏蔽此功能
                    if (that.isAddingRegion || that.isEditingRegion || !that.mapDisplayOption.displayLatLng) return;
                    var position = new qq.maps.LatLng(e.latLng.lat, e.latLng.lng);
                    that.regionTipInfo.open();

                    that.regionTipInfo.setContent(region.regionName);
                });
                qq.maps.event.addListener(polygon, 'mouseout', function (e) {
                    return;  //屏蔽此功能
                    that.regionTipInfo.close();
                });
                qq.maps.event.addListener(polygon, 'mousemove', function (e) {

                    if (that.isAddingRegion || that.isEditingRegion) return;  //如果正在添加或修改区域，则没有选中操作
                    var latLng = e.latLng,
                        lat = latLng.getLat().toFixed(5),
                        lng = latLng.getLng().toFixed(5);
                    document.getElementById("latLng").innerHTML = region.regionName + ' : ' + lat + ', ' + lng;
                    return;  //屏蔽此功能
                    var position = new qq.maps.LatLng(e.latLng.lat, e.latLng.lng);
                    that.regionTipInfo.setPosition(position);

                });
                return points;
            }
        },
        onSelectRegion(regionID) {
            $("#justFocus")[0].focus();
            if (this.selectedRegion.id) {//已经选择了
                this.selectedRegion.polygon.setFillColor(this.selectedRegion.polygon.origFillColor);
                if (regionID === this.selectedRegion.id) {  //点击的就是选中的区域，需要去掉选择
                    this.selectedRegion = {};
                    return;
                }
            }
            {
                this.selectedRegion = this.regions.find(i => i.id === regionID);
                this.selectedRegion.points = this.selectedRegion.polygon.path.elems.filter(item => item.lat > 0);

                var polygon = this.selectedRegion.polygon;
                this.regionFillColor = new qq.maps.Color(polygon.fillColor.red, polygon.fillColor.green, polygon.fillColor.blue, polygon.fillColor.alpha);
                this.regionStrokeColor = new qq.maps.Color(polygon.strokeColor.red, polygon.strokeColor.green, polygon.strokeColor.blue, polygon.strokeColor.alpha);

                polygon.origFillColor = this.regionFillColor;
                polygon.origStrokeColor = this.regionStrokeColor;
                var alpha = parseFloat(this.regionFillColor.alpha) + 0.3;
                
                var selectedColor = new qq.maps.Color(this.regionFillColor.red, this.regionFillColor.green, this.regionFillColor.blue, alpha)
                polygon.setFillColor(selectedColor /*this.polygonProperty.selectedFillColor*/);  //设置选中颜色
               
            }
        },
        endSetRegion() {
            if (this.isEditingRegion) {   //如果是编辑状态，则需要恢复显示区域轮廓
                if (this.selectedRegion.polygon) {
                    this.selectedRegion.polygon.setPath(this.selectedRegion.points);
                    this.onSelectRegion(this.selectedRegion.id);
                }
                this.isEditingRegion = false;
            }
            this.isAddingRegion = false;
            this.cuRregionPath.points = [];  //已经保存，将工作空间的点集清空
            this.regionName = '';
            //this.cuRregionPath.polygon.setPath(this.cuRregionPath.points);
            this.cuRregionPath.polyline.setPath([]);

        },
        saveRegion(e) {

            if (this.regionName === '') {
                alert('请输入一个区域名称！');
                $("#nameInput")[0].focus()
                return;
            }
            if (!this.cuRregionPath.polyline.path || this.cuRregionPath.polyline.path.elems.length < 3) {
                alert('请至少选择三个点！');
                $("#nameInput")[0].focus()
                return;
            }

            var that = this;
            var points = this.cuRregionPath.polyline.path.elems.filter(item => item.lat > 0); // [];

            var region = {
                regionName: this.regionName,
                points: points
            }
            console.log('saveRegion(e) region:', region);


            var httpHost = this.httpHost;
            var regionID = this.selectedRegion.id ? this.selectedRegion.id : 0;

            var data = { RegionID: regionID, RegionName: region.regionName, Points: region.points }
            var jsonData = JSON.stringify(data);
            console.log('data:', jsonData);
            $.ajax({
                type: "post",
                url: httpHost + '/api/Map/AddRegionWithPoints',
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",//表示后台返回的数据是json对象
                success: function (data) {
                    console.log('success save region. return data:', data);
                    if (!data || data.codeId < 0) {
                        alert(data.errMessage);
                        return;
                    }
                    region.id = data.codeId;
                    that.drawRegion(region);

                    if (that.isAddingRegion) {
                        that.regions.push(region);
                    }
                    else { //编辑状态

                        for (var i = 0; i < that.regions.length; i++) {
                            if (that.regions[i].id == region.id) {
                                that.regions[i] = region;

                                break;
                            }
                        }
                        that.onSelectRegion(that.selectedRegion.id)
                    }
                    that.endSetRegion();  //成功保存，则结束当前设置区域工作

                },
                error: function (error) {

                    alert(error);

                }
            });
        },

        onEditRegionColor() {
            this.isEditingRegionColor = true;

            var rgba = 'rgba(' + this.regionFillColor.red + ',' +
                this.regionFillColor.green + ',' + this.regionFillColor.blue + ',' +
                this.regionFillColor.alpha + ')';
            
            $('#fillColor').ClassyColor({                
                color: rgba,
                colorSpace: 'rgba',
                labels: true,
                displayColor: 'css',                
            });
           
            console.log('rgba colorpicker fill:', $('#fillColor').text())
            console.log('rgba fillcolor:', rgba);
            var strokergba = 'rgba(' + this.regionStrokeColor.red + ',' +
                this.regionStrokeColor.green + ',' + this.regionStrokeColor.blue + ',' +
                this.regionStrokeColor.alpha + ')';
            $('#strokeColor').ClassyColor({
                color: strokergba,
                colorSpace: 'rgba',
                labels: true,
                displayColor: 'css',                
            });
            console.log('rgba strokecolor:', strokergba);
        },
        //pickColor是个字符串，形如： rgba(6, 42, 68, 0.17)
        getQQColor(pickColor) {
            console.log('getQQColor(pickColor) ', pickColor);
            pickColor = pickColor.replace('rgba(', '').replace(')', '');
            var rgba = pickColor.split(',');

            var color = new qq.maps.Color(parseInt(rgba[0]), parseInt(rgba[1]), parseInt(rgba[2]), rgba[3]);
            console.log('getQQColor(pickColor) color:', color);
            return color;
        },
        saveRegionColor() {
            var that = this;
            var pickColor = $('#fillColor').text();
            var fillColor = this.getQQColor(pickColor);
            console.log('get pickColor', fillColor);
            this.selectedRegion.polygon.setFillColor(fillColor);
            this.selectedRegion.polygon.origFillColor = fillColor;
            this.selectedRegion.polygon.setStrokeColor(this.getQQColor($('#strokeColor').text()))
            var data = { regionID: this.selectedRegion.id, FillColor: pickColor, StrokeColor: $('#strokeColor').text() };
            $.ajax({
                type: "post",
                url: that.httpHost + '/api/Map/UpdateRegionColor',
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",//表示后台返回的数据是json对象
                success: function (data) {
                    console.log('success save region. return data:', data);
                    if (!data || data.codeId < 0) {
                        alert(data.errMessage);
                        return;
                    }
                    that.isEditingRegionColor = false;
                    that.onSelectRegion(that.selectedRegion.id);
                },
                error: function (error) {

                    alert(error);

                }
            });


        },
        cancelSetRegionColor() {
            this.isEditingRegionColor = false;
        },
        updateRegionPoints(regionID, data_points) {

            var httpHost = this.httpHost;
            var data = data_points;
            console.log('JSON.stringify(data)', data);
            console.log('JSON.stringify(data)', JSON.stringify(data));
            $.ajax({
                type: "post",
                url: httpHost + '/api/Map/AddRegionPoints?regionID=' + regionID,
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",//表示后台返回的数据是json对象
                success: function (data) {
                    if (data.codeId <= 0) alert(data.errMessage);
                }
            })
            return;

            var chunk = 15;
            var size = data_points.length / chunk + 1;
            for (var i = 0; i < size; i++) {
                var start = i * chunk;
                if (start >= data_points.length) break;
                var end = (i + 1) * chunk;
                if (end >= data_points.length) end = data_points.length;
                var curPoints = data_points.slice(start, end);
                var data = curPoints;

                $.ajax({
                    type: "post",
                    url: httpHost + '/api/Map/AddRegionPoints?regionID=' + regionID,
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",//表示后台返回的数据是json对象
                    success: function (data) {

                    }
                })

            }

        },
        onDeleteRegion() {
            if (this.selectedRegion.id && confirm('确实要删除选中的区域吗？')) {
                this.deleteRegion(this.selectedRegion.id);
                return;
            }
        },

        deleteRegion(id) {
            var that = this;
            $.ajax({
                type: "get",
                url: this.httpHost + '/api/Map/DeleteRegion',
                data: { id: id },
                contentType: "application/json; charset=utf-8",
                dataType: "json",//表示后台返回的数据是json对象
                success: function (data) {

                    if (data.codeId < 0) {
                        alert(data.errMessage);
                        return;
                    }
                    that.regions = that.regions.filter(item => item.id !== id);
                    that.selectedRegion.polygon.setPath({});
                    that.selectedRegion = {};
                },
                error: function (error) {
                    console.log('error:', error)
                }
            });
        },


        displayLatLng(e) {
            this.latlngInfos = this.latlngInfos.filter(item => item.visible);
            console.log('displayLatLng:', this.latlngInfos)
            var map = this.mapDisplayOption.displayLatLng ? this.map : null;
            for (var i = 0; i < this.latlngInfos.length; i++) {
                var infoWin = this.latlngInfos[i];
                infoWin.setMap(map);
            }

        },
        onClickMap(e) {

            var latlng = e.latLng;
            if (this.mapDisplayOption.displayLatLng && !(this.isAddingRegion || this.isEditingRegion)) {  //点击显示坐标
                var infoWin = new qq.maps.InfoWindow({
                    map: this.map,
                    content: '纬度:' + latlng.lat + '<br/> 经度:' + latlng.lng,
                    position: latlng
                });
                infoWin.open();
                //infoWin.setContent('纬度:' + latlng.lat + '<br/> 经度:' + latlng.lng);
                //infoWin.setPosition(latlng);
                this.latlngInfos.push(infoWin);
                console.log('onClickMap:', infoWin);
            }


            if (this.isAddingRegion || this.isEditingRegion) {   //处于添加或修改区域状态，点击添加新坐标点
                var point = latlng;
                if (!this.cuRregionPath.polyline) {
                    this.cuRregionPath.polyline = new qq.maps.Polyline({
                        path: this.cuRregionPath.points,
                        map: this.map,
                        strokeDashStyle: 'dash',
                        editable: true
                    });
                }
                this.cuRregionPath.polyline.path.elems.push(point);
                this.cuRregionPath.polyline.setPath(this.cuRregionPath.polyline.path.elems);
            }
        },
        onKeyPress(e) {
            if (this.LeftSideVisible) return;  //只在全屏时有效

            e.target.value = -1;
            var value = -1;
            switch (e.key) {
                case 'A':
                case 'a':
                    value = 0;
                    break;

                case '1':
                    value = 1;
                    break;
                case '2':
                    value = 2;
                    break;
                case '3':
                    value = 3;
                    break;
                case '4':
                    value = 4;
                    break;
                case '5':
                    value = 5;
                    break;
                case '6':
                    value = 6;
                    break;
            }
            if (value >= 0) {
                this.mapDisplayOption.isDisplayAllMarkers = false;
                e.target.value = 0;
                this.displayMapClass(e);
                e.target.value = value;
                this.mapDisplayOption.isDisplayAllMarkers = (value === 0);
                if (value > 0) {
                    this.markerClassVisible[value - 1] = true;
                }
                this.displayMapClass(e);
            }
            console.log(e)
        },
        onKeyDown(e) {
            //this.regionFillColor = $('#fillColor').text();
            console.log('success to fillColor:', e);
            switch (e.key) {
                case 'Delete':
                    this.onDeleteRegion();
                    return;
                case 'Escape':
                    this.endSetRegion();
                    if (this.selectedRegion.id) {
                        this.onSelectRegion(this.selectedRegion.id);
                    }
                    return;
            }

            if ((this.isAddingRegion || this.isEditingRegion) && e.ctrlKey) {
                switch (e.key) {
                    case 'z':
                        //this.cuRregionPath.points = this.cuRregionPath.polyline.path.elems;
                        //this.cuRregionPath.points.pop();
                        this.cuRregionPath.points = [];
                        for (var i = 0; i < this.cuRregionPath.polyline.path.elems.length - 1; i++) {
                            var latlng = this.cuRregionPath.polyline.path.elems[i];
                            var point = new qq.maps.LatLng(latlng.lat, latlng.lng)
                            this.cuRregionPath.points.push(point);
                        }
                        this.cuRregionPath.polyline.setPath(this.cuRregionPath.points);
                        break;
                    case 's':

                        break;
                }

            }
        }
    },
});