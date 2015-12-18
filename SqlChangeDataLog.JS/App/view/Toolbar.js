webix.protoUI({
    name: 'app.toolbar',
    $init: function() {

        var me = this,
            bind = function(fn) {
                return webix.bind(me[fn], me);
            };

        webix.extend(this.defaults, {
            height:66,
            cols: [
                {
                    id: 'button.add',
                    view: 'icon',
                    icon: 'plus-circle',
                    tooltip: 'Add database',
                    on: {
                        onItemClick: bind("onAddButtonClick")
                    }
                    //click: '$$("tbar")._addButtonClick()'
                }
            ]
        });

    },
    
   /* defaults: {
        id: 'toolbar',
        height: 66,
        cols: [
            {
                id: 'button.add',
                view: 'icon',
                icon: 'plus-circle',
                tooltip: 'Add database',
                click: '$$("tbar")._addButtonClick()'
            },  
            {
                id: 'addtext',
                width: 250,
                hidden: true,
                rows: [
                    {
                        id: 'servertext',
                        label: 'server',
                        view: 'text',
                        height: 30,
                        nextFocus:'databasetext',
                        on: {
                           'onKeyPress': function(key, event) {
                                switch (key) {
                                    //ESC 
                                case 27:
                                {
                                    $$('tbar')._cancelConfirm();
                                    return false;
                                }
                                //ENTER
                                case 13:
                                {
                                    $$('databasetext').focus();
                                    $$('databasetext').getInputNode().select();
                                    return false;
                                }
                                }
                            }
                        }

                    },
                    {
                        id: 'databasetext',
                        label: 'database',
                        view: 'text',
                        height: 30,
                        on: {
                            'onKeyPress': function(key, event) {
                                switch (key) {
                                    //ESC  
                                case 27:
                                {
                                    $$('tbar')._cancelConfirm();
                                    return false;
                                }
                                //ENTER
                                case 13:
                                {
                                    $$('logtabletext').focus();
                                    $$('logtabletext').getInputNode().select();
                                    return false;
                                }
                                }
                            }
                        }
                    },
                    {
                        id: 'logtabletext',
                        label: 'log_table',
                        view: 'text',
                        height: 30,
                        value: 'ChangeLog',
                        on: {
                            'onKeyPress': function(key, event) {
                                switch (key) {
                                    //ESC   
                                case 27:
                                {
                                    $$('tbar')._cancelConfirm();
                                    return false;
                                }
                                //ENTER
                                case 13:
                                {
                                    $$('tbar')._confirmDatabase();
                                    return false;
                                }
                                }
                            }
                        }
                    }
                ]
            },
            {
                id: 'addconfirm',
                width: 100,
                hidden: true,
                rows: [
                    {},
                    { view: 'button', type: 'icon', height: 30, label: 'Cancel', icon: 'ban', click: '$$("tbar")._cancelConfirm()' },
                    { view: 'button', type: 'icon', height: 30, label: 'Ok', icon: 'check', click: '$$("tbar")._confirmDatabase()' }
                ]
            }
        ]
    },*/


    onTextKeyPress: function(key, event) {
        console.log(this, arguments);
    },
    
    onAddButtonClick:function() {
        this.define('height', 99);
        this.resize();
    },


    _confirmDatabase: function() {

        var textData = {};

        $$('addtext').getChildViews().forEach(function(textInput) {
            textData[textInput.config.label] = textInput.getValue();
        });

        this._cancelConfirm();

        $$('tbar')._addButton(textData);

    },

    _cancelConfirm: function() {
        $$("addbutton").show();
        $$("addtext").hide();
        $$("addconfirm").hide();

        this.define('height', 66);
        this.resize();

    },

    _addButtonClick: function() {
        $$("addbutton").hide();
        $$("addtext").show();
        $$("addconfirm").show();
        this.define('height', 99);
        this.resize();


        $$('servertext').focus();
        $$('servertext').getInputNode().select();
    },

    _addButton: function(cfg) {
        var insertIndex = this.index($$("addbutton"));
        var view = this.addView(this._createButton(cfg), insertIndex);

        this.save_state();

        this._connect($$(view));
    },

    _createButton: function(cfg) {
        var template = webix.template('<span class="webix_icon fa-icon fa-database"></span> #server# #database# <br/>#log_table#');
        return {
            icon: 'database',
            autowidth: true,
            view: "button",
            type: "htmlbutton",
            icon: "database",
            server: cfg.server,
            database: cfg.database,
            log_table: cfg.log_table,
            label: template(cfg),
            on: {
                'onItemClick': this.onButtonClick
            }

        };
    },

    _connect: function(button) {

        if (this.connection) {
            webix.html.removeCss(this.connection.getNode(), "connect");
        }

        this.connection = button;
        webix.html.addCss(button.getNode(), "connect");
        LoadTables(button.config.server, button.config.database);
    },

    onButtonClick: function(id) {
        var button = $$(id);
        $$("tbar")._connect(button);
    },

    restore_state: function() {
        var state = this.load_state();
        var me = this;
        if (state) {
            state.forEach(function(cfg) {
                var insertIndex = me.index($$("addbutton"));
                view = me.addView(me._createButton(cfg), insertIndex);
            });
        }
    },

    get_State: function() {
        var state = [];
        this.getChildViews().forEach(function(v) {
            if (v.name == "button") {
                state.push({
                    server: v.config.server,
                    database: v.config.database,
                    log_table: v.config.log_table
                });
            }
        });
        return JSON.stringify(state);
    },

    save_state: function() {
        webix.storage.local.put("tbarState", this.get_State());
    },

    load_state: function() {
        var state = webix.storage.local.get("tbarState");
        if (state) return JSON.parse(state);
    }


}, webix.ui.toolbar);