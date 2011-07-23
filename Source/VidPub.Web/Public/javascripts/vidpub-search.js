var vidpubSearch = {
    resetResults: function (controller) {
        $(".search-results").empty();
    },
    getResults: function (query, controller) {
        $.getJSON("/"+controller, { query: query }, function (data) {
            var results = { controller: controller, items: data };
            $("#searchTemplate").tmpl(results).appendTo("#"+controller+"-search-results");
        });
    }
};
jQuery(function () {
    $("#searchForm").submit(function () {
        var val = $("#search").val();
        vidpubSearch.resetResults();
        if (val.length > 0) {
            vidpubSearch.getResults(val, "productions");
            vidpubSearch.getResults(val, "episodes");
            vidpubSearch.getResults(val, "customers");
        } else {
            $(".search-results").first().html("Need to enter a value to search for");
        }
        return false;
    });
});