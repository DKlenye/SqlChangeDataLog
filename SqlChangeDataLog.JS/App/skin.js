app.skin = {
    stateName: "skin",
    defaultSkin: 'flat',
    url:'../Content/skins/#skin#.css',
    setSkin: function (skin) {

        if (skin) {
            this.saveState(skin);
            location.reload();
        }

        skin = this.loadState() || this.defaultSkin;
        this.currentSkin = skin;
        webix.skin.set(skin);
        return webix.require(webix.template(this.url)({ skin: skin }));
    },
    
    getSkins: function () {
        var skinList = [];
        for (var name in webix.skin) {
            if (name.substring(0, 1) != '$' && typeof webix.skin[name] == "object") {
                skinList.push({id:name, skin: name });
            }
        }
        return skinList;
    },

    saveState: function (skin) {
        webix.storage.local.put(this.stateName, skin);
    },

    loadState: function () {
        var state = webix.storage.local.get(this.stateName);
        return state;
    }
};