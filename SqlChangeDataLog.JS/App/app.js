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
                        view: 'view.tables',
                        id: 'tables',
                        icons: app.settings.icons,
                        on:{
                            'tableSelect':onTableSelect
                        }
                    },
                    { view: 'resizer' },
                    {
                        view: 'view.table_editor',
                        id: 'table_editor',
                        icons: app.settings.icons
                    }
                ]
            }
        ]
    });

    $$('toolbar').restoreState();

});

var onToolbarActivate = function (logCfg) {
    app.connection = logCfg;
    $$('tables').load(logCfg);
};

var onTableSelect = function (table) {
    $$("table_editor").load(app.connection, table);
}
