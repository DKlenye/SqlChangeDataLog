webix.protoUI({
    name: 'view.table_editor',
    operations:[
        { id: "Insert", value: "<span class='webix_icon " + app.settings.icons["insert"] + "'></span><span style='padding-left: 4px'>Insert</span>" },
        { id: "Update", value: "<span class='webix_icon " + app.settings.icons['update'] + "'></span><span style='padding-left: 4px'>Update</span>" },
        { id: "Delete", value: "<span class='webix_icon " + app.settings.icons['delete'] + "'></span><span style='padding-left: 4px'>Delete</span>" }
    ],
    $init: function (config) {
        var icons = config.icons || {};
        var me = this;
        var bind = function(fn) {
            return webix.bind(me[fn], me);
        };
        
        webix.extend(this.defaults, {
            disabled: true,
            rows: [
                {
                    view: 'toolbar',
                    cols: [
                        {
                            view: "segmented",
                            id:"segmented.operation",
                            optionWidth: 120,
                            value: "none",
                            options: [
                                
                            ],
                            on: {
                                onChange: bind("onOperationChange")
                            }
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
                                                { id: "Checked", header: { content: "masterCheckbox" }, template: "{common.checkbox()}", width: 40 },
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
            var params = webix.copy(connection);
            params["TableName"] = table;
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

        webix.ajax().post('/Handlers/SelectTable.ashx', params, function (data) {
            me.fillData(data);
        });
    },

    clear: function () {
        this.setSegmentedTableName();
        this.clearView();
        delete this.data;
        
        $$("segmented.operation")._settings.options = [];
        $$("segmented.operation").refresh();
    },

    clearView:function() {
        $$("table.columns").clearAll();
        $$("text.trigger").setValue("");
    },

    fillData: function (data) {

        data = JSON.parse(data);
        this.data = data;

        this.setSegmentedTableName(data.Name);
        this.setOptions();

    },

    setOptions:function() {

        var data = this.data;
        var optionValue;
        var segment = $$('segmented.operation');
        var options = segment._settings.options;

        this.eachOperations(function(operation) {
            var trigger = data[operation.id];
            if (!trigger) {
                //todo выключить опцию
            } else {
                options.push(operation);
                if (!optionValue) {
                    optionValue = operation.id;
                }
            }
        });

        segment.refresh();
        segment.setValue(optionValue || "none");
        this.onOperationChange(optionValue);
    },
    
    fillColumns:function(operation) {

        var data = this.data;
        var trigger = data[operation];
        var rezult = [];

        var isKey = function(column) {
            return data.KeyColumns.indexOf(column) != -1;
        };

        var isChecked = function(column) {
            if (!trigger || !trigger.Columns) return false;
            return trigger.Columns.indexOf(column) != -1;
        };
        
        data.Columns.forEach(function(column) {
            rezult.push({
                ColumnName:column,
                IsKey:isKey(column),
                Checked:isChecked(column)
            });
        });

        $$("table.columns").parse(rezult);

    },

    fillTriggerText:function(operation) {
        var trigger = this.data[operation];
        var triggerText = "";
        if (trigger && trigger.TriggerText) {
            triggerText = trigger.TriggerText;
        }
        $$('text.trigger').setValue(triggerText);
    },

    setSegmentedTableName: function (tableName) {
        var columnOption = $$('segmented.trigger')._settings.options[0];
        columnOption.value = "<span class='webix_icon fa-columns'></span><span style='padding-left: 4px'>Columns " + (tableName || "") + "</span>";
        $$('segmented.trigger').refresh();
    },

    onOperationChange: function (operation) {
        this.clearView();
        this.fillColumns(operation);
        this.fillTriggerText(operation);
    },

    eachOperations: function (fn) {
        this.operations.forEach(fn);
    }


}, webix.ui.layout);