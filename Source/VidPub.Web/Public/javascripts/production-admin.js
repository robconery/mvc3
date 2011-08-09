
Episode = Backbone.Model.extend({
    defaults : {
        Title: "New Episode",
        Description : "Lorem Ipsum",
        ReleasedOn : "12/31/2011"
    },
    initialize: function () {
        this.bind("error", this.notifyCollectionError);
        this.bind("change", this.notifyCollectionChange);
    },
    idAttribute: "ID",
    url: function () {
        return this.isNew() ? "/api/episodes/create" : "/api/episodes/edit/" + this.get("ID");
    }
});
Episodes = Backbone.Collection.extend({
    model: Episode,
    url: function () {
        return "/api/episodes/?pid=" + this.ProductionID;
    }
});
Production = Backbone.Model.extend({
    defaults: {
        Title: "New Production",
        Author: "Joe Blow",
        Price: "25.00",
        Description: "Lorem Ipsum",
        ReleasedOn: "12/31/2011"
    },
    initialize: function () {
        this.bind("error", this.notifyCollectionError);
        this.bind("change", this.notifyCollectionChange);
    },
    idAttribute: "ID",
    url: function () {
        return this.isNew() ? "/api/productions/create" : "/api/productions/edit/" + this.get("ID");
    },
    validate: function (atts) {
        if ("Title" in atts & !atts.Title) {
            return "Title is required";
        }
    },
    notifyCollectionError: function (model, error) {
        this.collection.trigger("itemError", error);
    },
    notifyCollectionChange: function () {
        this.collection.trigger("itemChanged", this);
    }

});
Productions = Backbone.Collection.extend({
    model : Production,
    url : "/api/productions"
});
productions = new Productions();

ListView = Backbone.View.extend({
    tagName: "ol",
    initialize: function () {
        _.bindAll(this, 'render');
        this.template = $("#listTemplate");
        this.collection.bind("itemSaved", this.render);
    },
    events: {
        "click .production-link": "showForm",
        "click #new-production": "showCreate"
    },
    showCreate: function () {
        app.navigate("create", true);
        return false;
    },
    showForm: function (evt) {
        //get the ID that was clicked
        var id = $(evt.currentTarget).data('production-id');
        //navigate
        app.navigate("edit/" + id, true);

        return false;
    },
    render: function () {
        var data = { items: this.collection.toJSON() };
        var html = this.template.tmpl(data);
        $(this.el).html(html);
        return this;
    }

});
FormView = Backbone.View.extend({

    initialize: function () {
        _.bindAll(this, "render");
        this.template = $("#productionFormTemplate");
    },
    events: {

        "change input": "updateModel",
        "submit #productionForm": "save"
    },
    save: function () {
        this.model.save(
            this.model.attributes,
            {
                success: function (model, response) {
                    model.collection.trigger("itemSaved", model);
                },
                error: function (model, response) {
                    model.trigger("itemError", "There was a problem saving " + model.get("Title"));
                }
            }
        );
        return false;
    },
    updateModel: function (evt) {
        var field = $(evt.currentTarget);
        var data = {};
        var key = field.attr('ID');
        var val = field.val();
        data[key] = val;
        if (!this.model.set(data)) {
            //reset the form field
            field.val(this.model.get(key));
        }
    },
    render: function () {
        var html = this.template.tmpl(this.model.toJSON());
        $(this.el).html(html);
        this.$(".datepicker").datepicker();
        return this;
    }
});

NotifierView = Backbone.View.extend({
    initialize: function () {
        this.template = $("#notifierTemplate");
        this.className = "success";
        this.message = "Success";
        _.bindAll(this, "render", "notifySave", "notifyError");
        //use the globals - no need to depend on a single collection
        productions.bind("itemSaved", this.notifySave);
        productions.bind("itemError", this.notifyError);
    },
    events: {
        "click": "goAway"
    },
    goAway: function () {
        $(this.el).delay(3000).fadeOut();
    },
    notifySave: function (model) {
        this.message = model.get("Title") + " saved";
        this.render();
        this.goAway();
    },
    notifyError: function (message) {
        this.message = message;
        this.className = "error";
        this.render();
        this.goAway();
   },
    render: function () {
        var html = this.template.tmpl({ message: this.message, className: this.className });
        $(this.el).show();
        $(this.el).html(html);
        return this;
    }
});
EpisodeFormView = Backbone.View.extend({
    initialize: function () {
        this.template = $("#episodeFormTemplate");
    },
    events: {
        "submit #episodeForm": "save",
        "change input": "updateModel"
    },
    save: function (evt) {
        this.model.save(
            this.model.attributes,
            {
                success: function (model, response) {
                    alert(model.get("Title") + " saved");
                },
                error: function (model, response) {
                    alert("Problems... " + response.responseText);
                }
            }
        );
        return false;
    },
    updateModel: function (evt) {
        var field = $(evt.currentTarget);
        var data = {};
        var key = field.attr('ID');
        var val = field.val();
        data[key] = val;
        if (!this.model.set(data)) {
            //reset the form field
            field.val(this.model.get(key));
        }
    },
    render: function () {
        var html = this.template.tmpl(this.model.toJSON());
        $(this.el).html(html);
        return this;
    }
});
EpisodeListView = Backbone.View.extend({
    initialize: function () {
        this.template = $("#episodeListTemplate");
        _.bindAll(this, "render");
    },
    events: {
        "click .episode-link": "editEpisode",
        "click #new-episode" : "newEpisode"
    },
    editEpisode: function (evt) {
        var id = $(evt.currentTarget).data('episode-id');
        var model = this.collection.get(id);
        episodeForm = new EpisodeFormView({ model: model, el: "#episodeForm" });
        episodeForm.render();
        return false;
    },
    newEpisode : function(){
        episodeForm = new EpisodeFormView({ model: new Episode(), el: "#episodeForm" });
        episodeForm.render();
        return false;

    },
    render: function () {
        var data = { items: this.collection.toJSON() };
        var html = this.template.tmpl(data);
        $(this.el).show();
        $(this.el).html(html);
        return this;
    }
});

var ProductionAdmin = Backbone.Router.extend({

    initialize: function () {
        listView = new ListView({ collection: productions, el: "#production-list" });
        formView = new FormView({ el: "#production-form" });
        notifierView = new NotifierView({ el: "#notifications" });
    },
    routes: {
        "": "index",
        "edit/:id": "edit",
        "create": "create"
    },
    index: function () {
        listView.render();
    },
    edit: function (id) {
        listView.render();
        $(notifierView.el).empty();
        $(formView.el).empty();
        //grab the model from the productions
        var model = productions.get(id);
        formView.model = model;
        formView.render();

        //grab the episodes
        episodes = new Episodes();
        episodes.ProductionID = id;
        episodes.fetch({
            success: function (model, response) {
                episodeList = new EpisodeListView({ collection: episodes, el: "#episode-list" });
                episodeList.render();
            }
        });
    },
    create: function () {
        var model = new Production();
        listView.render();
        $(notifierView.el).empty();
        $(formView.el).empty();
        $("#episode-list").empty();
        $("#episodeForm").empty();
        formView.model = model;
        formView.render();

    }

});

jQuery(function () {

    productions.fetch({
        success: function () {
            //create the router
            window.app = new ProductionAdmin();
            Backbone.history.start();
        },
        error: function () {
            //display a nice error page
        }
    });

});
