notifier = {

    success: function (message) {
        $("#notifier").attr("class", "success");
        $("#notifier").html(message);
        this.goAway();
    },
    error: function (message) {
        $("#notifier").attr("class", "error");
        $("#notifier").html(message);
        this.goAway();
    },
    warn: function (message) {
        $("#notifier").attr("class", "notice");
        $("#notifier").html(message);
        this.goAway();
    },
    notify: function (response) {
        if (response.success) {
            notifier.success(response.message);
        } else {
            notifier.error(response.message);
        }
    },
    goAway: function () {
        $("#notifier").delay(3000).fadeOut();
    }
}
