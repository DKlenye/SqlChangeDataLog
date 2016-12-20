webix.protoUI({
    name: 'view.table_list',
    $init: function(config) {

        var icons = config.icons || {};
        var me = this,
            bind = function(fn) {
                return webix.bind(me[fn], me);
            };

        webix.extend(this.defaults, {
            rows: [
                {
                    view: "datatable",
                    navigation: true,
                    id: "table.table_list",
                    select: "row",
                    resizeColumn: true,
                    columns: [
                        {
                            id: 'Operations',
                            header: [
                                app.i18n.TableList.Operations,
                                {
                                    content: "selectFilter",
                                    options: [{ id: "All", value: app.i18n.All }, { id: "Logging", value: app.i18n.TableList.Logging }, { id: "Not Logging", value: app.i18n.TableList.NotLogging }],
                                    compare: function(value, filter) {
                                        if (filter == "All") return true;
                                        return filter == "Logging" ? !!value : !value;
                                    }
                                }
                            ],
                            width: 150,
                            template: function(obj) {

                                if (!obj.Operations) return "";

                                var buttonTpl = webix.template("<span class='webix_icon #icon#' style='cursor:pointer'></span>");
                                var operations = obj.Operations;

                                return operations.map(function(o) {
                                    return buttonTpl({ icon: icons[o] || 'fa-question' });
                                }).join("");

                            }
                        },
                        { id: 'Name', fillspace: true, header: [app.i18n.TableList.Tables, { content: "textFilter" }], sort: "string" },
                        { id: 'Description', fillspace: true, header: [app.i18n.Description, { content: "textFilter" }], sort: "string" }
                    ],
                    scheme: {
                        $init: function (obj) {
                            var table = app.DescriptionMap[obj.Name];
                            if (table) {
                                obj.Description = table.Description;
                            }
                        }
                    },
                    on: {
                        onBeforeLoad: bind("_onBeforeLoad"),
                        onAfterLoad: bind("_onAfterLoad"),
                        onSelectChange: bind("_onSelectChange"),
                        onBeforeFilter: bind("_onBeforeFilter")
                    }
                }
            ]
        });
    },

    _onBeforeFilter: function() {
        $$("table.table_list").scrollTo(0, 0);
    },

    _onBeforeLoad: function() {
        var grid = $$("table.table_list");

        grid.disable();
        grid.clearAll();
        grid.showOverlay(app.i18n.Loading+" <span class='webix_icon fa-spinner fa-spin'></span>");
    },
    _onAfterLoad: function() {
        var grid = $$("table.table_list");

        grid.filterByAll();
        grid.hideOverlay();
        grid.enable();
    },

    _onSelectChange: function() {
        var grid = $$("table.table_list");
        var item = grid.getSelectedItem();
        var table = '';
        if (item) {
            table = item.Name;
        }
        this.callEvent("tableSelect", [table]);
    },

    load: function(params) {
        $$("table.table_list").load("post->" + app.getUrl("SelectTableList"), null, params);
    },

    refreshItem: function(item) {

        var grid = $$("table.table_list");
        var dataset = grid.data;

        var record = dataset.find(function(obj) {
            return obj.Name == item.TableName;
        }, true);

        var operations = [];
        ["Insert", "Update", "Delete"].forEach(function(o) {
            if (item[o]) {
                operations.push(o.toLowerCase());
            }
        });

        if (operations.length == 0) {
            operations = null;
        }

        dataset.updateItem(record.id, { Operations: operations });
    }

}, webix.ui.layout);