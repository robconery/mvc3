var vidpubSearch = {
    resetResults: function (controller) {
        $(".search-results").empty();
    },
    getResults: function (query, controller) {
        $.getJSON("/" + controller, { query: query }, function (data) {
            var results = { controller: controller, items: data };
            $("#productionTemplate").tmpl(results).appendTo("#" + controller + "-search-results");
        });
    },
    getCustomerResults: function (query) {
        $.getJSON("/customers", { query: query }, function (data) {
            var results = { items: data };
            $("#customerTemplate").tmpl(results).appendTo("#customers-search-results");
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
            vidpubSearch.getCustomerResults(val);
        } else {
            $(".search-results").first().html("Need to enter a value to search for");
        }
        return false;
    });
});