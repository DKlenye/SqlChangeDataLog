webix.protoUI({
    name: 'view.userstable',
    $init: function(cfg) {

        var me = this,
            bind = function(fn) {
                return webix.bind(me[fn], me);
            };

        webix.extend(this.defaults, {
            rows: [
                {
                    view: 'datatable',
                    id: 'table.users',
                    resizeColumn: true,
                    columns: [
                        { id: "Login", header: [app.i18n.UserTable.Login, { content: "textFilter"}], sort: "string", fillspace: 1 },
                        { id: "Name", header: [app.i18n.UserTable.Name, { content: "textFilter"}], sort: "string", fillspace: 2 },
                        { id: "Phone", header: [app.i18n.UserTable.Phone, { content: "textFilter"}], sort: "string", fillspace: 1 },
                        {
                            id: "Email",
                            header: [app.i18n.UserTable.Email, { content: "textFilter"}],
                            sort: "string",
                            fillspace: 2,
                            template: function (o) {
                                var email = o["Email"];
                                if (!email) return "";
                                return '<a href="mailto:' + email + '">' + email + '</a>';
                            }
                        }
                    ],
                    on: {
                        onBeforeLoad: bind("_onBeforeLoad"),
                        onAfterLoad: bind("_onAfterLoad"),
                        onBeforeFilter: bind("_onBeforeFilter")
                    },
                    select: 'row'
                }
            ]
        });
    },

    _onBeforeFilter: function() {
        $$("table.users").scrollTo(0, 0);
    },
    _onBeforeLoad: function() {
        var grid = $$("table.users");
        grid.disable();
        grid.showOverlay(app.i18n.Loading+" <span class='webix_icon fa-spinner fa-spin'></span>");
    },
    _onAfterLoad: function() {
        var grid = $$("table.users");

        grid.hideOverlay();
        grid.enable();
    },

    load: function(params) {

        var _params = webix.copy(params);

        $$('table.users').clearAll();

        $$('table.users').define("url", {
            $proxy: true,
            source: app.getUrl("SelectLogUsers"),
            params: _params,
            load: function(view, callback, params) {
                params = webix.extend(params || {}, this.params || {}, true);
                webix.ajax().bind(view).post(this.source, params, callback);
            }
        });
    }

}, webix.ui.layout);