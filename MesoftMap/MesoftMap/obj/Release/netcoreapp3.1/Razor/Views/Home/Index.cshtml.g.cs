#pragma checksum "E:\GitHub\MesoftMap\MesoftMap\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "844a35b9035b04b907249a84219e9816082f6e1c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\GitHub\MesoftMap\MesoftMap\Views\_ViewImports.cshtml"
using MesoftMap;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\GitHub\MesoftMap\MesoftMap\Views\_ViewImports.cshtml"
using MesoftMap.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"844a35b9035b04b907249a84219e9816082f6e1c", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"87ea3767442cde31ec2a82071412ec54a1750428", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/MesoftMap/MyMap.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/Vue/jquery-2.2.2.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "E:\GitHub\MesoftMap\MesoftMap\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "地图应用";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<style type=""text/css"">
    * {
        margin: 0px;
        padding: 0px;
    }

    body, button, input, select, textarea {
        font: 12px/16px Verdana, Helvetica, Arial, sans-serif;
    }

    #info {
        /*width: 603px;*/
        padding-top: 3px;
        overflow: hidden;
    }

    .btn {
        /*width: 190px;*/
    }

    #container {
        min-width: 600px;
        min-height: 767px;
    }
    [v-cloak] {
        display: none;
    }
</style>
<script charset=""utf-8"" src=""https://map.qq.com/api/js?v=2.exp&key=K2XBZ-OHEK6-RP2S5-M575M-EXDQV-6LBJZ""></script>
");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "844a35b9035b04b907249a84219e9816082f6e1c4524", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "844a35b9035b04b907249a84219e9816082f6e1c5567", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n<script src=\"/js/Vue/vue.min.js\"></script>\r\n\r\n<div id=\"app\" style=\"float:left;width:10%;\">\r\n");
            WriteLiteral(@"    <!-- <input type=""button"" v-bind:value=""msg""> -->
    <p><input type=""button"" :value=""PleaseClickMe"" v-on:click=""clickMe""></p>
    <p><input type=""button"" v-on:click=""setPositions"" :value=""'设置区域'""></p>
    
</div>

<div class=""text-center""  style=""float:left;width:90%;"">
");
            WriteLiteral(@"    <div>
        <div id=""container""></div>        
    </div>
</div>

<script>
    
    var vm = new Vue({
        el: ""#app"",
        data: {
            msg: ""Hello World"",
            msg2: ""<b>Hello World<b>"",
            PleaseClickMe: ""点击我"",
            mapInfoes:[],

            map: {},
            path: [],
            mapProperty: {
                center: new qq.maps.LatLng(39.984110, 116.307490),
                zoom:13
            },
            polygons:[]
            
        },
        mounted() {
            this.init();
        },
        methods: {
            init() {
                
                this.initMapInfoes();
                var polygon = new qq.maps.Polygon({
                    path: mapProperty.getPath1(),
                    strokeColor: 'rgba(220,38,38,0.2)',
                    strokeWeight: 5,
                    fillColor: mapProperty.getFillColor(),
                    map: this.map
                });
                this.polyg");
            WriteLiteral(@"ons.push(polygon);
                //setMap
                var mapM = document.getElementById(""mapM"");
                qq.maps.event.addDomListener(mapM, ""click"", function () {
                    polygon.setVisible(true);
                    if (polygon.getMap()) {
                        polygon.setMap(null);
                    } else {
                        polygon.setMap(map);
                    }
                });
                //setCursor
                var curF = true;
                var cursor = document.getElementById(""cursor"");
                qq.maps.event.addDomListener(cursor, ""click"", function () {
                    polygon.setMap(map);
                    polygon.setVisible(true);
                    if (curF) {
                        curF = false;
                        polygon.setCursor(""default"");
                    } else {
                        curF = true;
                        polygon.setCursor(""pointer"");
                    }
                ");
            WriteLiteral(@"});
                //setFillColor
                var fillCF = true;
                var fillC = document.getElementById(""fillC"");
                qq.maps.event.addDomListener(fillC, ""click"", function () {
                    polygon.setMap(map);
                    polygon.setVisible(true);
                    if (fillCF) {
                        fillCF = false;
                        polygon.setFillColor(""#0f0"");
                    } else {
                        fillCF = true;
                        polygon.setFillColor(""#111"");
                    }
                });
                //setPath
                var pathF = true;
                var path = document.getElementById(""path"");
                qq.maps.event.addDomListener(path, ""click"", function () {
                    polygon.setMap(map);
                    polygon.setVisible(true);
                    if (pathF) {
                        pathF = false;
                        polygon.setPath(path2);
             ");
            WriteLiteral(@"       } else {
                        pathF = true;
                        polygon.setPath(path1);
                    }
                });
                //setStrokeColor
                var strokeCF = true;
                var strokeC = document.getElementById(""strokeC"");
                qq.maps.event.addDomListener(strokeC, ""click"", function () {
                    polygon.setMap(map);
                    polygon.setVisible(true);
                    if (strokeCF) {
                        strokeCF = false;
                        polygon.setStrokeColor(""#1c29d8"");
                    } else {
                        strokeCF = true;
                        polygon.setStrokeColor(""#000"");
                    }
                });
                //setStrokeDashStyle
                var dashF = true;
                var dash = document.getElementById(""dash"");
                qq.maps.event.addDomListener(dash, ""click"", function () {
                    polygon.setMap(map);
     ");
            WriteLiteral(@"               polygon.setVisible(true);
                    if (dashF) {
                        dashF = false;
                        polygon.setStrokeDashStyle(""dash"");
                    } else {
                        dashF = true;
                        polygon.setStrokeDashStyle(""solid"");
                    }
                });
                //setStrokeWeight
                var strokeWeightF = true;
                var strokeWeight = document.getElementById(""strokeWeight"");
                qq.maps.event.addDomListener(strokeWeight, ""click"", function () {
                    polygon.setMap(map);
                    polygon.setVisible(true);
                    if (strokeWeightF) {
                        strokeWeightF = false;
                        polygon.setStrokeWeight(10);
                    } else {
                        strokeWeightF = true;
                        polygon.setStrokeWeight(5);
                    }
                });

                var visib");
            WriteLiteral(@"leTF = true;

            },
            initMapCenter(center) {
                this.map.setCenter(center);
                console.log('center map:', center);
            },
            initMapInfoes() {
                var that = this;
                this.mapProperty.center = new qq.maps.LatLng(39.984110, 116.307490);
                var map = new qq.maps.Map(document.getElementById('container'), {
                    center: this.mapProperty.center,
                    zoom: this.mapProperty.zoom
                });
                this.map = map;
                var httpHost = window.location.protocol + '//' + window.location.host;
                $.ajax({
                    type: ""get"",
                    url: httpHost + '/api/Map/GetAllMapinfoes',
                    //data: { providerID: that.providerID },
                    contentType: ""application/json; charset=utf-8"",
                    dataType: ""json"",//表示后台返回的数据是json对象
                    success: function (data) {   ");
            WriteLiteral(@"                     
                        console.log('success get map infoes:', data);
                        if (!data || data.codeId<0) return;
                        var items = data.data;

                        that.mapInfoes = items;
                        var centerItem = items[items.length / 2];
                        var center = new qq.maps.LatLng(centerItem.lat, centerItem.lon);
                        that.initMapCenter(center);
                        for (var i = 0; i < items.length; i++) {
                            var item = items[i];
                            that.drawMapInfo(item);
                        }
                        console.log('get map infoes secceed!', that.mapInfoes);
                    },
                    error: function (error) {
                        console.log('error:', error)
                    }
                });
            },

            drawMapInfo(item) {
                var fillColor = new qq.maps.Color(20, 169, 30, ");
            WriteLiteral(@"0.2);
                //for (var i = 0; i < this.workers.length; i++) {
                var lat = item.lat;
                var lng = item.lon;
                var center = new qq.maps.LatLng(lat, lng);
                var circle = !item.circle ? new qq.maps.Circle({
                    map: this.map,
                    //center: center,
                    radius: 600,
                    fillColor: fillColor,
                    strokeWeight: 5
                }) : item.circle;
                circle.setCenter(center);

                var marker = !item.marker ? new qq.maps.Marker({
                    //设置Marker的位置坐标
                    //position: center,
                    //设置显示Marker的地图
                    map: this.map,
                    title: item.name
                }) : item.marker;
                marker.setPosition(center);
                //添加信息窗口
                var info = !item.info ? new qq.maps.InfoWindow({
                    map: this.map,
                  ");
            WriteLiteral(@"  position: center,
                    visible: true
                }) : item.info;
                info.setContent(item.name + '<br /> 地址：' + item.address + ' <br />备注：' + item.monitorMemo);
                info.setPosition(center);

                if (!item.marker) {
                    item.marker = marker;
                    item.info = info;
                    item.circle = circle;
                    qq.maps.event.addListener(marker, 'click', function () {
                        info.open(); //单击时打开提示信息                        
                    });
                }
            },
            clickMe() {
                alert('hello mesoft!')
                this.msg = ""Hello Mesoft"";
            },
            setPositions() {
                
                var path = [
                    new qq.maps.LatLng(39.999216 + 0.01, 116.305389 + 0.01),
                    new qq.maps.LatLng(39.972516 + 0.01, 116.299725 + 0.01),
                    new qq.maps.LatLng(39.974489");
            WriteLiteral(@" + 0.01, 116.326675 + 0.01)
                ];
                var fillColor  = new qq.maps.Color(20, 169, 30, 0.2);
                var polygon = new qq.maps.Polygon({
                    path: path,
                    strokeColor: '#00ff00',
                    strokeWeight: 5,
                    fillColor: fillColor,
                    map: this.map
                });
            }
            
        },
    });

</script>
");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
