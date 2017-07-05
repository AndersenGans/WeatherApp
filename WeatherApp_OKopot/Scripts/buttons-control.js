var city;
$(document).ready(function () {
    var list = document.getElementById("ListOfCities");
    list.selectedIndex = -1;
    $('#ListOfCities').change(function () {
        var list = document.getElementById("ListOfCities");
        city = list.options[list.selectedIndex].value;
        var result = encodeURIComponent(city);
        $('#results').load("/Home/ShowDailyWeather?search=" + result);
    });
    $('#btn-find').click(function() {
        city = document.getElementById("city").value;
        var result = encodeURIComponent(city);
        $('#results').load("/Home/ShowDailyWeather?search=" + result);
    });
});

function day_btn() {
    var result = encodeURIComponent(city);
    $('#results').load("/Home/ShowDailyWeather?search=" + result);
}

function three_day_btn() {
    var result = encodeURIComponent(city);
    $('#results').load("/Home/ShowThreeDaysWeather?search=" + result);
};

function week_btn() {
    var result = encodeURIComponent(city);
    $('#results').load("/Home/ShowWeekWeather?search=" + result);
};