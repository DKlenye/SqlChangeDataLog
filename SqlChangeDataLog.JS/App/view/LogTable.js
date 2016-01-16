webix.protoUI({
    name: 'view.logtable',
    $init: function(cfg) {

        var dateToString = webix.Date.dateToStr("%d.%m.%Y %H:%i:%s");
        var me = this,
            bind = function(fn) {
                return webix.bind(me[fn], me);
            };

        webix.extend(this.defaults, {
            rows: [ 
                {
                    view: 'datatable',
                    id: 'table.log',
                    resizeColumn:true,
                    columns: [
                        { id: "idChangeLog", header: [app.i18n.LogTable.idChangeLog, {content: "serverFilter"}], width: 80, sort: 'server' },
                        { id: "date", header: [app.i18n.LogTable.date, { content: "serverFilter"}], width: 150, sort: 'server' },
                        { id: "user", header: [app.i18n.LogTable.user, { content: "serverFilter"}], width: 260 },
                        {
                            id: "changeType",
                            header: [
                                app.i18n.LogTable.changeType, {
                                    content: "serverSelectFilter",
                                    options: [{ id: "", value: app.i18n.All }, { id: "I", value: app.i18n.Insert }, { id: "U", value: app.i18n.Update }, { id: "D", value: app.i18n.Delete}]
                                }
                            ],
                            width: 125,
                            template: function (obj) {
                                var o = {
                                    I: app.i18n.Insert,
                                    U: app.i18n.Update,
                                    D: app.i18n.Delete
                                };
                                return o[obj.changeType];
                            }
                        },
                        { id: "table", header: [app.i18n.LogTable.table, { content: "serverFilter"}], fillspace: true },
                        { id: "idString", header: [app.i18n.LogTable.idString, { content: "serverFilter"}], width: 120 }
                    ],
                    dataThrottle: 500,
                    scheme: {
                        $init: function(obj) {
                            obj.date = dateToString(new Date(obj.date));
                        }
                    },
                    on: {
                        onBeforeLoad: bind("_onBeforeLoad"),
                        onAfterLoad: bind("_onAfterLoad"),
                        onSelectChange: bind("_onSelectChange")
                    },
                    select: 'row'//,
                    //pager: 'pagerA'
                },
               /* {
                    paddingY: 7,
                    rows: [
                        {
                            view: "pager",
                            id: "pagerA",
                            template: "{common.first()} {common.prev()} {common.pages()} {common.next()} {common.last()} " + app.i18n.LogTable.page + " {common.page()} " + app.i18n.LogTable.from + " #limit#",
                            size: 50,
                            group: 5
                        }
                    ]
                }*/
            ]
        });
    },


    _onBeforeLoad: function() {
        var grid = $$("table.log");

        grid.disable();
        grid.showOverlay("Loading <span class='webix_icon fa-spinner fa-spin'></span>");
    },
    _onAfterLoad: function() {
        var grid = $$("table.log");

        grid.hideOverlay();
        grid.enable();
    },

    _onSelectChange: function() {
        var grid = $$("table.log");
        var item = grid.getSelectedItem();
        this.callEvent("onSelectChange", [item]);
    },

    load: function(params) {

        var _params = webix.copy(params);
        _params.count = 50;

        $$('table.log').clearAll();

        $$('table.log').define("url", {
                $proxy: true,
                source: "/Handlers/SelectChangeLog.ashx",
                params: _params,
                load: function(view, callback, params) {
                    params = webix.extend(params || {}, this.params || {}, true);
                    webix.ajax().bind(view).post(this.source, params, callback);
                }
        });
    }

}, webix.ui.layout);