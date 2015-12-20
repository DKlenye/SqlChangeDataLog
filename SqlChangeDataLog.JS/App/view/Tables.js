webix.protoUI({
    name: 'view.tables',
    $init: function(config) {

        var icons = config.icons || {};
        var me = this,
            bind = function (fn) {
            return webix.bind(me[fn], me);
        };

        webix.extend(this.defaults, {
            rows: [{
                    view: "datatable",
                    id: "table.tables",
                    select: "row",
                    columns: [
                        {
                            id: 'Operations',
                            header: [
                                "Operations",
                                {
                                    content: "selectFilter",
                                    options: [{ id: "All", value: "All" }, { id: "Logging", value: "Logging" }, { id: "Not Logging", value: "Not Logging" }],
                                    compare: function(value, filter) {
                                        if (filter == "All") return true;
                                        return filter == "Logging" ? !!value : !value;
                                    }
                                }
                            ],
                            width: 130,
                            template: function(obj) {

                                if (!obj.Operations) return "";

                                var buttonTpl = webix.template("<span class='webix_icon #icon#' style='cursor:pointer'></span>");
                                var operations = obj.Operations;

                                return operations.map(function(o) {
                                    return buttonTpl({ icon: icons[o] || 'fa-question' });
                                }).join("");

                            }
                        },
                        { id: 'Name', fillspace: true, header: ["Table", { content: "textFilter" }], sort: "string" }
                    ],
                    on: {
                        onBeforeLoad : bind("_onBeforeLoad"),
                        onAfterLoad : bind("_onAfterLoad"),
                        onSelectChange:bind("_onSelectChange")
                    }
                }
            ]
        });
        },
   _onBeforeLoad:function() {
       var grid = $$("table.tables");
        
        grid.disable();
        grid.clearAll();
        grid.showOverlay("Loading <span class='webix_icon fa-spinner fa-spin'></span>");
    },
    _onAfterLoad: function () {
        var grid = $$("table.tables");
        
        grid.filterByAll();
        grid.hideOverlay();
        grid.enable();
    },
    
    _onSelectChange:function() {
        var grid = $$("table.tables");
        var item = grid.getSelectedItem();
        var table = '';
        if (item) {
            table = item.Name;
        }
        this.callEvent("tableSelect", [table]);
    },

    load: function(params) {
        $$("table.tables").load("post->/Handlers/SelectTables.ashx", null, params);
    }
}, webix.ui.layout);


/*
{
                                                98,
                                                id: 'tables',
                                                select: "row",
                                                on: {
                                                    onSelectChange: function () {
                                                        var selection = this.getSelectedItem();

                                                        if (!selection) {
                                                            var grid = $$('columnstable');
                                                            grid.clearAll();
                                                            return;
                                                        }

                                                        webix.ajax().post('/Handlers/SelectTableDetails.ashx', { server: 'db2', database: 'nsd2', tableName: selection.Name }, function (data) {

                                                            var a = []
                                                            data = JSON.parse(data);
                                                            console.log(data);

                                                            data.Columns.forEach(function (col) {
                                                                a.push({ ColumnName: col });
                                                            });

                                                            var grid = $$('columnstable');
                                                            grid.clearAll();
                                                            grid.parse(a);

                                                        });
                                                    }
                                                },
                                                columns: [
                                                    {
                                                        id: 'Operations',
                                                        header: [
                                                            "Operations",
                                                            {
                                                                content: "selectFilter",
                                                                options: [{ id: "All", value: "All" }, { id: "Logging", value: "Logging" }, { id: "Not Logging", value: "Not Logging"}],
                                                                compare: function (value, filter) {
                                                                    if (filter == "All") return true;
                                                                    return filter == "Logging" ? !!value : !value;
                                                                }
                                                            }
                                                        ],
                                                        width: 130,
                                                        template: function (obj) {

                                                            if (!obj.Operations) return "";

                                                            var buttonTpl = webix.template("<span class='webix_icon #icon#' style='cursor:pointer'></span>");
                                                            var operations = obj.Operations;

                                                            return operations.map(function (o) {
                                                                return buttonTpl({ icon: icons[o] || 'fa-question' });
                                                            }).join("");

                                                        }
                                                    },
                                                    { id: "Name", fillspace: true, header: ["Table", { content: "textFilter"}], sort: "string" }
                                                ]
                                            }
                                            */