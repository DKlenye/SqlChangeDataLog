webix.protoUI({
    name: 'view.logtable',
    $init: function(cfg) {

        var dateToString = webix.Date.dateToStr("%d.%m.%Y %H:%i:%s");
        var me = this,
            bind = function (fn) {
                return webix.bind(me[fn], me);
            };

        webix.extend(this.defaults, {
            rows: [
                {
                    view: 'datatable',
                    id: 'table.log',
                    columns: [
                        { id: "idChangeLog", header: "Id", width: 80 },
                        { id: "_date", header: "Date", width: 150 },
                        { id: "user", header: "UserName", width: 260 },
                        { id: "changeType", header: "ChangeType", width: 100 },
                        { id: "table", header: "TableName", fillspace: true },
                        { id: "idString", header: "IdString", width: 100 }
                    ],
                    data: {
                        url: 'post->/Handlers/SelectChangeLog.ashx'  
                    },
                    scheme: {
                        $init: function(obj) {
                            obj._date = dateToString(new Date(obj.date));
                        }
                    },
                    datafetch:50,
                    datathrottle: 500,
                    loadahead:100,
                    on: {
                        onBeforeLoad: bind("_onBeforeLoad"),
                        onAfterLoad: bind("_onAfterLoad"),
                        onSelectChange: bind("_onSelectChange")
                    },
                    select: 'row',
                    pager: 'pagerA'
                },
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
                }
            ]
        });
    },


    _onBeforeLoad: function() {
        var grid = $$("table.log");

        grid.disable();
        grid.clearAll();
        grid.showOverlay("Loading <span class='webix_icon fa-spinner fa-spin'></span>");
    },
    _onAfterLoad: function() {
        var grid = $$("table.log");

        grid.filterByAll();
        grid.hideOverlay();
        grid.enable();
    },

    _onSelectChange: function() {
        var grid = $$("table.log");
        var item = grid.getSelectedItem();
        this.callEvent("onSelectChange", [item]);
    },

    load: function (params) {
        $$("table.log").load("post->/Handlers/SelectChangeLog.ashx", null, params);
    }

}, webix.ui.layout);