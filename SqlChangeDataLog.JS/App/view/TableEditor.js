webix.protoUI({
    name: 'view.table_editor',
   
    $init: function (config) {
        var icons = config.icons || {};
        var me = this;
        var bind = function(fn) {
            return webix.bind(me[fn], me);
        };
        
        this.operations = [
            { id: "Insert", value: "<span class='webix_icon " + app.settings.icons["insert"] + "'></span><span style='padding-left: 4px'>"+app.i18n.Insert+"</span>" },
            { id: "Update", value: "<span class='webix_icon " + app.settings.icons['update'] + "'></span><span style='padding-left: 4px'>" + app.i18n.Update + "</span>" },
            { id: "Delete", value: "<span class='webix_icon " + app.settings.icons['delete'] + "'></span><span style='padding-left: 4px'>" + app.i18n.Delete + "</span>" }
        ],

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
                            value: "None",
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
                                { value: "<span class='webix_icon fa-columns'></span><span style='padding-left: 4px'>" + app.i18n.TableEditor.Columns + "</span>", id: 'layout.columns' },
                                { value: "<span class='webix_icon fa-file-text-o'></span><span style='padding-left: 4px'>" + app.i18n.TableEditor.TriggerText + "</span>", id: 'layout.triggertext' }
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
                                                { id: "Checked", header: { content: "masterCheckbox", css: "center" }, css:"center", template: "{common.checkbox()}", width: 40 },
                                                {
                                                    id: "ColumnName",
                                                    fillspace: true,
                                                    header: app.i18n.TableEditor.ColumnName,
                                                    template: function (obj) {
                                                        if (!obj.IsKey) return obj.ColumnName;
                                                        return "<span class='webix_icon fa-key' style='color:gold'></span> "+obj.ColumnName;
                                                    }
                                                }
                                            ],
                                            on: {
                                                'onCheck': bind("_onCheck")
                                            }
                                        },
                                        { view: 'resizer' },
                                        {
                                            
                                            height: 150,
                                            rows: [
                                                


                                                { type: "header", template: app.i18n.TableEditor.ExtendedLogic},
                                                {
                                                    view: 'textarea',
                                                    id: 'text.extended'

                                                }
                                            ]
                                        },
                                        {
                                            height: 45,
                                            cols: [
                                                { view: "button", type: "iconButton", label: app.i18n.Save , icon: "floppy-o", width:115 ,on: { onItemClick: bind("saveTable")} },
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
            this.params = webix.copy(connection);
            this.params["TableName"] = table;
            this._loadTable();
        } else {
            this.clear();
            this.disable();
        }
    },

    _loadTable: function () {

        var me = this;
        var table = $$("table.columns");

        me.clear();

        webix.ajax().post('/Handlers/SelectTable.ashx', this.params, function (data) {
            me.fillData(data);
        });
    },

    _getOptions : function() {
        var segmented = $$("segmented.operation");
        var settings = segmented._settings || segmented.s;
        return settings.options;
    },

    _setOptions:function(options) {
        var segmented = $$("segmented.operation");
        var settings = segmented._settings || segmented.s;
        settings.options = options;
    },
    _refreshOptions:function() {
        $$("segmented.operation").refresh();
    },

    clear: function () {
        this.setSegmentedTableName();
        this.clearView();
        delete this.data;

        this._setOptions([]);
        this._refreshOptions();
    },

    clearView:function() {
        $$("table.columns").clearAll();
        $$("text.trigger").setValue("");
        $$("text.extended").setValue("");
    },

    fillData: function (data) {
        data = JSON.parse(data);
        this.data = data;

        this.records = {
            Insert: this.buildOperationRecord("Insert"),
            Update: this.buildOperationRecord("Update"),
            Delete: this.buildOperationRecord("Delete")
        };

        this.setSegmentedTableName(data.TableName);
        this.setOptions();
    },

    buildOperationRecord:function(operation) {
        var data = this.data;
        var trigger = data[operation];
        var rezult = [];

        var isKey = function (column) {
            return data.KeyColumns.indexOf(column) != -1;
        };

        var isChecked = function (column) {
            if (!trigger || !trigger.Columns) return false;
            return trigger.Columns.indexOf(column) != -1;
        };

        data.Columns.forEach(function (column) {
            rezult.push({
                ColumnName: column,
                IsKey: isKey(column),
                Checked: isChecked(column)
            });
        });

        return rezult;
    },

    setOptions:function() {

        var data = this.data;
        var optionValue;
        var options = [];
        var segment = $$('segmented.operation');

        this.eachOperations(function(operation) {
            var trigger = data[operation.id];
            if (!trigger) {
                options.push(operation); //todo выключить опцию
            } else {
                options.push(operation);
                if (!optionValue) {
                    optionValue = operation.id;
                }
            }
        });

        this._setOptions(options);
        this._refreshOptions();

        optionValue = optionValue || "Insert";
        segment.setValue(optionValue);
        this.onOperationChange(optionValue, "None");
    },
    
    fillTriggerText:function(operation) {
        var trigger = this.data[operation];
        var triggerText = "";
        if (trigger && trigger.TriggerText) {
            triggerText = trigger.TriggerText;
        }
        $$('text.trigger').setValue(triggerText);
    },

    fillExtendedLogic: function(operation) {
        var trigger = this.data[operation];
        var text = "";
        if (trigger && trigger.ExtendedLogic) {
            text = trigger.ExtendedLogic;
        }
        $$('text.extended').setValue(text);
    },

    setSegmentedTableName: function (tableName) {

        var segmented = $$('segmented.trigger'),
            settings = segmented._settings || segmented.s;

        var columnOption = settings.options[0];
        columnOption.value = "<span class='webix_icon fa-columns'></span><span style='padding-left: 4px'>"+app.i18n.TableEditor.Columns+" "+ (tableName || "") + "</span>";
        $$('segmented.trigger').refresh();
    },

    onOperationChange: function (operation, oldOperation) {

        if (oldOperation != "None") {
            var trigger = this.data[oldOperation];
            if(trigger) trigger.ExtendedLogic = $$('text.extended').getValue();
        }
        this.clearView();
        $$("table.columns").parse(this.records[operation]);
        this.fillTriggerText(operation);
        this.fillExtendedLogic(operation);
    },

    eachOperations: function (fn) {
        this.operations.forEach(fn);
    },

    _onCheck: function () {
        var record = this.operations[$$('segmented.operation').getValue()];
    },

    saveTable: function () {

        var trigger = this.data[$$('segmented.operation').getValue()];
        if (trigger) {trigger.ExtendedLogic = $$('text.extended').getValue();}
        
        var data = this.data;
        var me = this;

        ["Insert", "Update", "Delete"].forEach(function(e) {
            var record = me.records[e];
            var columns = [];
            record.forEach(function(_column) {
                if (_column.Checked) {
                    columns.push(_column.ColumnName);
                }
            });
            if (columns.length == 0) {
                delete data[e];
            } else {
                if (!data[e]) {
                    data[e] = {
                        Operation:e,
                        TableName: data.TableName
                    };
                }
                data[e].Columns = columns;
            }
        });

        this.saveData();

    },

    saveData: function () {

        this.disable();
        var data = this.data;
        var me = this;
        var params = webix.copy(this.params);
        params.Table = data;
        webix.ajax().post('/Handlers/SaveTable.ashx', params, webix.bind(me.onSave, me));
    },

    onSave: function (data) {
        var dto = JSON.parse(data);
        this.callEvent("onTableChange", [dto]);
        
        this.fillData(data);
        this.enable();
    }


}, webix.ui.layout);