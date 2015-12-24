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
                        { id: "idChangeLog", header: ["Id", {content: "serverFilter"}], width: 80, sort: 'server' },
                        { id: "date", header: ["Date",{content: "serverFilter"}], width: 150, sort: 'server' },
                        { id: "user", header: ["UserName", { content: "serverFilter"}], width: 260},
                        {
                            id: "changeType",
                            header: [
                                "ChangeType", {
                                    content: "serverSelectFilter",
                                    options: [{ id: "", value: "All" }, { id: "I", value: "Insert" }, { id: "U", value: "Update" }, { id: "D", value: "Delete"}]
                                }
                            ],
                            width: 100,
                            template: function (obj) {
                                var o = {
                                    I: 'Insert',
                                    U: 'Update',
                                    D: 'Delete'
                                };
                                return o[obj.changeType];
                            }
                        },
                        { id: "table", header: ["TableName", { content: "serverFilter"}], fillspace: true },
                        { id: "idString", header: ["IdString", { content: "serverFilter" }], width: 100 }
                    ],
                    dataThrottle: 500,
                    scheme: {
                        $init: function(obj) {
                            obj.date = dateToString(new Date(obj.date));
                        }
                    },
                    on: {
                        //onBeforeLoad: bind("_onBeforeLoad"),
                       // onAfterLoad: bind("_onAfterLoad"),
                        onSelectChange: bind("_onSelectChange")
                    },
                    select: 'row'//,
                    //pager: 'pagerA'
                }/*,
                {
                    paddingY: 7,
                    rows: [
                        {
                            view: "pager",
                            id: "pagerA",
                            template: "{common.first()} {common.prev()} {common.pages()} {common.next()} {common.last()} Page {common.page()} from #limit#",
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

        //grid.filterByAll();
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