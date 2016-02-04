app = {
    getUrl: function (handler) {
        return "Handlers/" + handler + ".ashx";
    }
};

webix.ready(function () {

    app.i18n.setLocale();
    app.skin.setSkin()
        .then(function() {
            return webix.require("../Content/app.css");
        })
       .then(function () {
           webix.delay(startUI, this, [], 300);
       });


});


var startUI = function () {
    
    webix.ui({
        id: "RootView",
        rows: [
            {
                view: 'view.toolbar',
                id: 'toolbar',
                on: {
                    'activate': onToolbarActivate
                }
            },
            {
                cols: [
                    {
                        id: "sidebar",
                        view: "sidebar",
                        tooltip: {
                            template: "<span class='webix_strong'>#value#</span>"
                        },
                        collapsed: true,
                        data: [
                            {
                                id: "viewscroll.logview",
                                icon: "search",
                                value: app.i18n.LogView
                            },
                            {
                                id: "viewscroll.tablesettings",
                                icon: "bars",
                                value: app.i18n.TableSettings
                            },
                            {
                                id:"viewscroll.users",
                                icon:"user",
                                value: app.i18n.UserTable.Users
                            }
                            
                        ],
                        on: {
                            onItemClick: function (id) {
                                $$(id).show(false, false);
                            }
                        }
                    },
                    {
                        id: 'viewscroll',
                        cells: [
                            {
                                id: 'viewscroll.logview',
                                cols: [
                                    {
                                        view: 'view.logtable',
                                        id: 'logtable',
                                        gravity: 1.5,
                                        on: {
                                            'onSelectChange': onLogTableSelect
                                        }
                                    },
                                    { view: 'resizer' },
                                    {
                                        view: 'view.logdetails',
                                        id: 'logdetails'
                                    }
                                ]
                            },
                            {
                                id: 'viewscroll.tablesettings',
                                cols: [
                                    {
                                        view: 'view.table_list',
                                        id: 'table_list',
                                        icons: app.settings.icons,
                                        on: {
                                            'tableSelect': onTableSelect
                                        }
                                    },
                                    { view: 'resizer' },
                                    {
                                        view: 'view.table_editor', id: 'table_editor', icons: app.settings.icons,
                                        on: {
                                            'onTableChange': function (data) {
                                                $$('table_list').refreshItem(data);
                                            }
                                        }
                                    }
                                ]
                            },
                            {
                                id: "viewscroll.users",
                                cols: [
                                    {
                                        view: "view.userstable",
                                        id: "usertable"
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        ]
    });

    $$('toolbar').restoreState();
    $$('sidebar').select('viewscroll.logview');
    $$('RootView').resize();

}


var onToolbarActivate = function (logCfg) {
    
    if (!logCfg) {
        app.connection = null;
        var grid = $$("table.table_list");
        grid.disable();
        grid.clearAll();
        $$('table.log').clearAll();
        return;
    }

    app.connection = logCfg;
    $$('table_list').load(logCfg);
    $$('usertable').load(logCfg);
    $$('logtable').clearFilter();
    $$('logtable').load(logCfg);
    $$('table.log').markSorting("idChangeLog", "desc");
};

var onTableSelect = function(table) {
    $$("table_editor").load(app.connection, table);
};

var onLogTableSelect = function (data) {
    $$("logdetails").load(data);
};