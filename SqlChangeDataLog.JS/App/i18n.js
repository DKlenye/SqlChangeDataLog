app.i18n = {
    stateName: "locale",
    defaultLocale: "ru-RU",
    "en-US": {
        Ok: "Ok",
        Cancel: "Cancel",
        Save:"Save",
        TableSettings: "Data log settings",
        LogView: "View log data",
        Insert: "Insert",
        Delete: "Delete",
        Update: "Update",
        All:"All",
        Toolbar: {
            LabelWidth: 80,
            Server: "server",
            Database: "database",
            LogTable: "log table",
            BadServer: "Bad Server name",
            BadDatabase: "Bad Database name",
            BadTable: "Bad Log Table name",
            NotFoundTitle: "Not Found table #logtable#",
            NotFoundMessage: "Table #logtable# not found in database #database#. Create table '#logtable#'?"
        },
        LogTable: {
            idChangeLog: "Id",
            date: "Date",
            user: "UserName",
            changeType: "ChangeType",
            table: "TableName",
            idString: "IdString",
            page:"Page",
            from:"from"
        },
        TableEditor: {
            Columns: "Columns" ,
            TriggerText: "Trigger Text",
            ColumnName: "Column Name"
        },
        LogDetails: {
          Column: "Column",
          OldValue: "Old Value",
          NewValue:"New Value"
        },
        TableList: {
            Operations: "Operations",
            Tables: "Tables",
            Logging:"Logging",
            NotLogging:"Not Logging"
        }
    },
    "ru-RU": {
        Ok: "Ок",
        Cancel: "Отмена",
        Save: "Сохранить",
        TableSettings: "Настройка логгирования",
        LogView: "Просмотр данных",
        Insert: "Добавление",
        Delete: "Удаление",
        Update: "Изменение",
        All: "Все",
        Toolbar: {
            LabelWidth:110,
            Server: "Сервер",
            Database: "БД",
            LogTable: "Таблица лога",
            BadServer: "Не найден сервер, либо у вас нет доступа",
            BadDatabase: "Не найдена база данных, либо у вас нет доступа",
            BadTable: "Таблица существует в базе данных и не является таблицей для ведения лога",
            NotFoundTitle: "Не найдена таблица #logtable#",
            NotFoundMessage: "Таблица #logtable# не существует в базе данных #database#. Создать таблицу '#logtable#'?"
        },
        LogTable: {
            idChangeLog: "Код",
            date: "Дата изменения",
            user: "Пользователь",
            changeType: "Тип изменения",
            table: "Таблица",
            idString: "Ключ записи",
            page:"Страница",
            from:"из"
        },
        TableEditor: {
          Columns:"Столбцы таблицы",
          TriggerText:"Текст триггера",
          ColumnName:"Имя столбца"
      },
      LogDetails: {
          Column: "Столбец",
          OldValue: "Старое значение",
          NewValue: "Новое значение"
      },
      TableList: {
            Operations: "Операции",
            Tables: "Таблицы",
            Logging:"Логгируются",
            NotLogging:"Не логгируются"
        }
    },
    setLocale: function (locale) {
        
        if (locale) {
            this.saveState(locale);
            location.reload();
        }

        locale = this.loadState() || this.defaultLocale;
        webix.extend(this, this[locale], true);
        this.locale = locale;
    },
    
    saveState: function (locale) {
        webix.storage.local.put(this.stateName, locale);
    },

    loadState: function () {
        var state = webix.storage.local.get(this.stateName);
        return state;
    }
};