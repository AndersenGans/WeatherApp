﻿var city;
$(document).ready(function() {
    var list = document.getElementById("ListOfCities");
    list.selectedIndex = -1;

    $('#ListOfCities').change(function() {
        var list = document.getElementById("ListOfCities");
        city = list.options[list.selectedIndex].value;
        var result = encodeURIComponent(city);
        console.log(result);
        $('#weathers').empty();
        $('#weathers').load("/Home/ShowDailyWeatherPartial?search=" + result);
    });
    if (document.getElementById("cityName") !== null)
    city = document.getElementById("cityName").value;
    //$('#btn-find').click(function() {
    //    city = document.getElementById("city").value;
    //    var addToList = encodeURIComponent(document.getElementById("chck-add").checked);
    //    var result = encodeURIComponent(city);
    //    $.get("/Home/ShowDailyWeather?search=" + result + "&addToMainList=" + addToList);
    //});
});



function day_btn() {
    var result = encodeURIComponent(city);
    $('#results').load("/Home/ShowDailyWeatherPartial?search=" + result);
}

function three_day_btn() {
    var result = encodeURIComponent(city);
    $('#results').load("/Home/ShowThreeDaysWeather?search=" + result);
}

function week_btn() {
    var result = encodeURIComponent(city);
    $('#results').load("/Home/ShowWeekWeather?search=" + result);
}

//function del_city() {
//    var list = document.getElementById("ListOfCities");
//    if (list.options[list.selectedIndex] !== undefined) {
//        city = list.options[list.selectedIndex].value;
//        var result = encodeURIComponent(city);
//        $.get("/Home/DeleteCity?cityName=" + result);
//    }
//}