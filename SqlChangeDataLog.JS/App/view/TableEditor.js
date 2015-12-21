webix.protoUI({
    name: 'view.table_editor',
    $init: function (config) {
        var icons = config.icons || {};

        webix.extend(this.defaults, {
            disabled: true,
            rows: [
                {
                    view: 'toolbar',
                    cols: [
                        {
                            view: "segmented",
                            optionWidth: 120,
                            value: 4,
                            options: [
                                { id: "operation.insert", value: "<span class='webix_icon " + icons["insert"] + "'></span><span style='padding-left: 4px'>Insert</span>" },
                                { id: "operation.update", value: "<span class='webix_icon " + icons['update'] + "'></span><span style='padding-left: 4px'>Update</span>" },
                                { id: "operation.delete", value: "<span class='webix_icon " + icons['delete'] + "'></span><span style='padding-left: 4px'>Delete</span>" }
                            ]
                        }
                    ]
                },
                {
                    rows: [
                        {
                            view: 'segmented',
                            id: 'segmented.trigger',
                            multiview: true,
                            value: 'layout.columns',
                            options: [
                                { value: "<span class='webix_icon fa-columns'></span><span style='padding-left: 4px'>Columns</span>", id: 'layout.columns' },
                                { value: "<span class='webix_icon fa-file-text-o'></span><span style='padding-left: 4px'>TriggerText</span>", id: 'layout.triggertext' }
                            ]
                        },
                        {
                            cells: [
                                {
                                    id: 'layout.columns',
                                    rows: [{
                                            view: 'datatable',
                                            id: 'table.columns',
                                            columns: [
                                                { id: "checked", header: { content: "masterCheckbox" }, template: "{common.checkbox()}", width: 40 },
                                                {
                                                    id: "ColumnName",
                                                    fillspace: true,
                                                    header: "Column Name",
                                                    template: function (obj) {
                                                        if (!obj.IsKey) return obj.ColumnName;
                                                        return "<span class='webix_icon fa-key' style='color:gold'></span> "+obj.ColumnName;
                                                    }
                                                }
                                            ]
                                        },
                                        {
                                            height: 45,
                                            cols: [
                                                { view: "button", type: "iconButton", label: "Save", icon: "floppy-o", width: 95 },
                                                {}
                                            ]
                                        }
                                    ]
                                },
                                {
                                    id: "layout.triggertext",
                                    rows: [
                                        {
                                            view: 'textarea',
                                            id:'text.trigger'
                                        },
                                        {
                                            height: 45,
                                            cols: [
                                                { view: "button", type: "iconButton", label: "Save", icon: "floppy-o", width: 95 },
                                                {}
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        });
    },

    load: function (connection, table) {
        if (table) {
            this.enable();
            var params = connection;
            connection["TableName"] = table;
            this._loadTable(params);
        } else {
            this.clear();
            this.disable();
        }
    },

    _loadTable: function (params) {

        var me = this;
        var table = $$("table.columns");

        me.clear();

        webix.ajax().post('/Handlers/SelectTableDetails.ashx', params, function (data) {
            me.fillData(data);
        });
    },


    clear: function () {
        this.setSegmentedTableName();
        $$("table.columns").clearAll();
    },

    fillData: function (data) {

        var a = []
        data = JSON.parse(data);

        console.log(data);

        this.setSegmentedTableName(data.Name);
        
        data.Columns.forEach(function (col) {

            var column = { ColumnName: col };
            if (data.KeyColumns.indexOf(col) != -1) {
                column.IsKey = true;
            }

            a.push(column);
        });

        $$("table.columns").parse(a);

        this.setTrigger(data, "Insert");


    },

    setSegmentedTableName: function (tableName) {
        var columnOption = $$('segmented.trigger')._settings.options[0];
        columnOption.value = "<span class='webix_icon fa-columns'></span><span style='padding-left: 4px'>Columns " + (tableName || "") + "</span>";
        $$('segmented.trigger').refresh();
    },
    
    setTrigger:function(data, operation) {
        var trigger = data[operation];
        $$('text.trigger').setValue(trigger.TriggerText);
    }


}, webix.ui.layout);