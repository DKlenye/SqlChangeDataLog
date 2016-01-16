webix.protoUI({
    name: 'view.toolbar',
    defaultLogTable: "ChangeLog",
    stateName:'toolbar',
    minHeight: 68,
    maxHeight: 99,
    inserting: false,
    $init: function () {

        var me = this,
            bind = function (fn) {
                return webix.bind(me[fn], me);
            };

        webix.extend(this.defaults, {
            height: me.minHeight,
            cols: [
                {
                    id: 'button.add',
                    view: 'icon',
                    icon: 'plus-circle',
                    tooltip: 'Add database',
                    click: bind("toggleAdd")
                },
                {
                    id: 'layout.add',
                    hidden: true,
                    cols: [{
                        width: 250,
                        id:'layout.add.text',
                        rows: [
                                {
                                    id: 'text.server',
                                    label: 'server',
                                    dataIndex: 'server',
                                    view: 'text',
                                    height: 30,
                                    nextFocus: 'text.database',
                                    on: { 'onKeyPress': bind("onTextKeyPress") }
                                },
                                {
                                    id: 'text.database',
                                    dataIndex: 'database',
                                    label: 'database',
                                    view: 'text',
                                    height: 30,
                                    nextFocus: 'text.logtable',
                                    on: { 'onKeyPress': bind("onTextKeyPress") }
                                },
                                {
                                    id: 'text.logtable',
                                    dataIndex:'logtable',
                                    label: 'log table',
                                    view: 'text',
                                    value:this.defaultLogTable,
                                    height: 30,
                                    on: { 'onKeyPress': bind("onTextKeyPress") }
                                }
                            ]
                    }, {
                        width: 100,
                        rows: [
                            {},
                            { view: 'button', type: 'icon', height: 30, label: 'Cancel', icon: 'ban', click: bind("toggleAdd") },
                            { view: 'button', type: 'icon', height: 30, label: 'Ok', icon: 'check', click: bind("confirmAdd") }
                        ]
                    }]
                }]
        });

    },
   
    onTextKeyPress: function (key, event) {
    
        switch (key) {
        //ESC         
        case 27:
            {
                this.toggleAdd();
                return false;
            }
        //ENTER
        case 13:
            {
                var id = event.srcElement.parentNode.parentNode.getAttribute("view_id");
                var field = $$(id);

                if (field && field.config.nextFocus) {
                    var focusField = $$(field.config.nextFocus);
                    focusField.focus();
                    focusField.getInputNode().select();
                    return false;
                }

                this.confirmAdd();
                return false;
            }
        }
    },

    toggleAdd: function () {
        var isInserting = this.inserting;
        var height = isInserting ? this.minHeight : this.maxHeight;

        $$('layout.add')[isInserting ? "hide" : "show"]();
        $$('button.add')[isInserting ? "show" : "hide"]();

        this.eachButtons(function(b) {
            b[isInserting ?"enable":"disable"]();
        });

        if (!isInserting) {
            var server = $$("text.server");
            server.focus();
            server.getInputNode().select();
        } else {
            this.resetAddLayout();
        }

        this.define('height', height);
        this.resize();

        this.inserting = !this.inserting;
    },

    resetAddLayout:function() {
        this.eachAddFields(function(text) {
            text.setValue('');
        });
        $$('text.logtable').setValue(this.defaultLogTable);
    },

    eachAddFields: function (fn, scope) {
        $$('layout.add.text').getChildViews().forEach(fn, scope || this);
    },

    eachButtons:function(fn) {
        this.getChildViews().forEach(function(button) {
            if (button.name == "button") {
                fn(button);
            }
        });
    },

    getTargetActivationButton:function() {
        var rezult, me = this;
        this.getChildViews().every(function(button) {
            if (button.name == "button" && me.activeButton != button) {
                rezult = button;
                return false;
            }
            return true;
        });
        return rezult;
    },

    getAddConfig:function() {
        var cfg = {};
        this.eachAddFields(function(field) {
            cfg[field.config.dataIndex] = field.getValue();
        });
        return cfg;
    },
    
    
    confirmAdd:function() {
        var me = this;
        var addCfg = me.getAddConfig();

        me.disable();
        me.showOverlay("Connect <span class='webix_icon fa-spinner fa-spin'></span>");

        webix.ajax().post('/Handlers/CheckConnection.ashx', addCfg, function (response) {

            me.enable();
            me.hideOverlay();

            response = JSON.parse(response);

            var errorMessage = "";
            if (!response.Server) {
                errorMessage = "Bad Server name";
            }
            if (!errorMessage && !response.Database) {
                errorMessage = "Bad Database name";
            }
            if (!errorMessage && response.BadTableName) {
                errorMessage = "Bad Log Table name";
            };

            if (errorMessage) {
                webix.alert({
                    title: 'Error',
                    type: 'alert-error',
                    text: errorMessage
                });
                return;
            };

            var addCfg = me.getAddConfig();
            addCfg.warning = !response.LogTable;

            me.toggleAdd();
            var button = me.addButton(addCfg);
            var $button = $$(button);
            me.activate($button);
            if ($button.config.logCfg.warning) {
                me.createLogTable($button);
            }

            me.saveState();    
        });
    },

    addButton: function (cfg) {
        var insertIndex = this.index($$("button.add"));
        var view = this.addView(this.createButton(cfg), insertIndex);
        return view;
    },

    createButton: function (cfg) {
        return {
            icon: 'database',
            autowidth: true,
            view: "button",
            type: "htmlbutton",
            css:'webix_segment_N',
            icon: "database",
            logCfg: cfg,
            label: this.buildButtonLabel(cfg),
            click: webix.bind(this.onButtonClick,this)
        };
    },

    buildButtonLabel:function(cfg) {
        var template = webix.template(
            '</span> <span class="webix_tab_close webix_icon fa-times"></span>' +
                ' <span class="webix_icon fa-icon fa-database" style="font-size:18px;"></span>' +
                    '<span> #server# #database# <br/>' +
                        (cfg.warning == true ? '<span class="webix_icon fa-icon fa-exclamation-triangle" style="font-size:18px; color: #ffd21a !important;">' : "") +
                        '</span>#logtable# </span> ');
        return template(cfg);
    },

    activate: function(button) {
        if (this.activeButton) {
            webix.html.removeCss(this.activeButton.getNode(), "webix_selected");
        }
        if (button) {
            this.activeButton = button;
            webix.html.addCss(button.getNode(), "webix_selected");
            this.callEvent("activate", [button.config.logCfg]);
        } else {
            this.activeButton = null;
            this.callEvent("activate", []);
        }
    },

    onButtonClick: function (id,e) {

        var button = $$(id);
        var target = e.target || e.srcElement;

        switch (target.className) {
        
            case "webix_tab_close webix_icon fa-times":
            {
                if (this.activeButton == button) {
                    var targetButton = this.getTargetActivationButton();
                    this.activate(targetButton);    
                }
                this.removeView(button);
                this.saveState();
                break;
            }

            case "webix_icon fa-icon fa-exclamation-triangle":
            {
                this.createLogTable(button);
                break;
            }

            default:
            {
                this.activate(button);
                this.saveState();  
            }
        }
        
    },
    
    restoreState: function () {
        var state = this.loadState();
        var me = this;
        if (state) {
            state.forEach(function (cfg) {

                var active = cfg.active || false;
                delete cfg.active;

                var button = me.addButton(cfg);
                if (active) {
                    me.activate($$(button));
                }
            });
        }
    },

    getState: function () {
        var state = [];
        var me = this;

        this.eachButtons(function(b) {
            var cfg = b.config.logCfg;
            if (me.activeButton && me.activeButton == b) {
                cfg.active = true;
            }
            state.push(cfg);
        });

        return JSON.stringify(state);
    },

    saveState: function () {
        webix.storage.local.put(this.stateName, this.getState());
    },

    loadState: function () {
        var state = webix.storage.local.get(this.stateName);
        if (state) return JSON.parse(state);
    },

    createLogTable: function (button) {
        var me = this;
        var cfg = button.config.logCfg;

        var message = "Table " + cfg.logtable + " not found in database " + cfg.database + ". Create table '" + cfg.logtable + "'?";
        webix.confirm({
            title: 'Not found table ' + cfg.logtable,
            type:"confirm-warning",
            text: message,
            callback : function(ok) {
                if (ok) {
                    webix.ajax().post('/Handlers/CreateTable.ashx', cfg, function() {
                        button.config.logCfg.warning = false;
                        button.define("label", me.buildButtonLabel(button.config.logCfg));
                        button.refresh();
                    });
                }
            }
        });
    }
    
}, webix.ui.toolbar, webix.OverlayBox);