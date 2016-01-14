webix.protoUI({
    name: 'view.logdetails',
    $init: function(config) {
        webix.extend(this.defaults, {
            disabled:true,
            rows: [
                {
                    id:"table.logdetails",
                    view: 'datatable',
                    resizeColumn: true,
                    columns: [
                        { id: "Column", header: "Column", fillspace: 0.5 },
                        { id: "OldValue", header: "OldValue", fillspace: 1 },
                        { id: "NewValue", header: "NewValue", fillspace: 1 }
                    ]
                }
            ]
        });
    },

    load: function (data) {

        if (data) {
            this.enable();
            this.parseXml(data);
        } else {
            this.clear();
            this.disable();
        }
    },

    parseXml:function(data) {
        var parseFn = this["parse_" + data.changeType];
        var dataObject = parseFn.call(this, data.description);
        var table = $$('table.logdetails');
        table.clearAll();
        table.parse(dataObject);
    },
    
    xmlToObject:function(xml){
        var doc = webix.DataDriver.xml.toObject(xml);
        var tag = doc.childNodes[0];
        var a = tag.attributes;

        var rezult = {};
        if (a && a.length) {
            for (var i = 0; i < a.length; i++) {
                rezult[a[i].name] = a[i].value;
            }
        }
        return rezult;
    },

    parse_I: function(xml) {
        var obj = this.xmlToObject(xml);
        var rezult = [];
        for (var i in obj) {
            rezult.push({
                Column:i,
                NewValue:obj[i]
            });
        }
        return rezult;
    },

    parse_U: function (xml) {
        var splitIndex = xml.indexOf('>') + 1;
        var _new = this.xmlToObject(xml.substring(0, splitIndex));
        var _old = this.xmlToObject(xml.substring(splitIndex, xml.length));

        var rezult = [];
        var map = {};

        for (var i in _old) {
            map[i] = _old[i];
        }

        for (var i in _new) {
            if (!map[i]) map[i] = _new[i];
        }

        for (var i in map) {
            rezult.push({
                Column: i,
                OldValue: webix.isUndefined(_old[i]) ? 'NULL' : _old[i],
                NewValue: webix.isUndefined(_new[i]) ? 'NULL' : _new[i],
                $css: _old[i] == _new[i] ? "" : { "background-color": "papayawhip" }
            });
        }

        return rezult;
    },

    parse_D:function(xml) {
        var obj = this.xmlToObject(xml);
        var rezult = [];
        for (var i in obj) {
            rezult.push({
                Column: i,
                OldValue: obj[i]
            });
        }
        return rezult;
    },
    
    clear:function() {
        $$('table.logdetails').clearAll();
    }

}, webix.ui.layout);