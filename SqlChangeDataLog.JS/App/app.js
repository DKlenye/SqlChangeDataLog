app = {};

webix.ready(function() {
    webix.ui({
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
                        collapsed: true,
                        data: [
                            {
                                id: "viewscroll.tablesettings",
                                icon: "table",
                                value: "Data log settings"
                            },
                            {
                                id: "viewscroll.logview",
                                icon: "search",
                                value: "View log data"
                            }
                        ],
                        on: {
                            onItemClick: function(id) {
                                $$(id).show(false, false);
                            }
                        }
                    },
                    {
                        id: 'viewscroll',
                        cells: [
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
                            }
                        ]
                    }
                ]
            }
        ]
    });

    $$('toolbar').restoreState();
    $$('sidebar').select('viewscroll.tablesettings');
});

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
    $$('logtable').load(logCfg);
};

var onTableSelect = function(table) {
    $$("table_editor").load(app.connection, table);
};

var onLogTableSelect = function (data) {
    $$("logdetails").load(data);
};