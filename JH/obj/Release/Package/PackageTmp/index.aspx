<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="JH.index" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <script src="/shuiguo/js/jquery.min.js"></script>
    <script src="/Scripts/ajax.js"></script>
    <script src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    
    <script>
        wx.config({
            debug: false,
            appId: '<%=appId%>',
    timestamp: '<%=timestamp%>',
      nonceStr: '<%=nonceStr%>',
      signature: '<%=signature%>',
      jsApiList: [
        'getLocation',
        'openLocation'
      ]
  });
  wx.ready(function () {
      wx.getLocation({
          type: 'wgs84', // 默认为wgs84的gps坐标，如果要返回直接给openLocation用的火星坐标，可传入'gcj02'
          success: function (res) {
              var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
              var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。 
              $.get("Handler.ashx", { p: "getCity", lat: latitude, lon: longitude, r: Math.random() }, function (data) {
                  var lat = latitude;
                  var lon = longitude;  
                  location.href = "MapPage?city=" + data.info + "&lat=" + lat + "&lon=" + lon; 
              });
          },
          cancel: function (res) {
              alert('用户拒绝授权获取地理位置,请打开定位后方可使用！');
          }, error: function (res) {
              alert("定位失败！");
          }
      });
  });
</script>
</body>
</html>
