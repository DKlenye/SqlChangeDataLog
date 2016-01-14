// Шаги алгоритма ECMA-262, 5-е издание, 15.4.4.19
// Ссылка (en): http://es5.github.com/#x15.4.4.19
// Ссылка (ru): http://es5.javascript.ru/x15.4.html#x15.4.4.19
if (!Array.prototype.map) {

    Array.prototype.map = function (callback, thisArg) {

        var T, A, k;

        if (this == null) {
            throw new TypeError(' this is null or not defined');
        }

        // 1. Положим O равным результату вызова ToObject с передачей ему
        //    значения |this| в качестве аргумента.
        var O = Object(this);

        // 2. Положим lenValue равным результату вызова внутреннего метода Get
        //    объекта O с аргументом "length".
        // 3. Положим len равным ToUint32(lenValue).
        var len = O.length >>> 0;

        // 4. Если вызов IsCallable(callback) равен false, выкидываем исключение TypeError.
        // Смотрите (en): http://es5.github.com/#x9.11
        // Смотрите (ru): http://es5.javascript.ru/x9.html#x9.11
        if (typeof callback !== 'function') {
            throw new TypeError(callback + ' is not a function');
        }

        // 5. Если thisArg присутствует, положим T равным thisArg; иначе положим T равным undefined.
        if (arguments.length > 1) {
            T = thisArg;
        }

        // 6. Положим A равным новому масиву, как если бы он был создан выражением new Array(len),
        //    где Array является стандартным встроенным конструктором с этим именем,
        //    а len является значением len.
        A = new Array(len);

        // 7. Положим k равным 0
        k = 0;

        // 8. Пока k < len, будем повторять
        while (k < len) {

            var kValue, mappedValue;

            // a. Положим Pk равным ToString(k).
            //   Это неявное преобразование для левостороннего операнда в операторе in
            // b. Положим kPresent равным результату вызова внутреннего метода HasProperty
            //    объекта O с аргументом Pk.
            //   Этот шаг может быть объединён с шагом c
            // c. Если kPresent равен true, то
            if (k in O) {

                // i. Положим kValue равным результату вызова внутреннего метода Get
                //    объекта O с аргументом Pk.
                kValue = O[k];

                // ii. Положим mappedValue равным результату вызова внутреннего метода Call
                //     функции callback со значением T в качестве значения this и списком
                //     аргументов, содержащим kValue, k и O.
                mappedValue = callback.call(T, kValue, k, O);

                // iii. Вызовем внутренний метод DefineOwnProperty объекта A с аргументами
                // Pk, Описатель Свойства
                // { Value: mappedValue,
                //   Writable: true,
                //   Enumerable: true,
                //   Configurable: true }
                // и false.

                // В браузерах, поддерживающих Object.defineProperty, используем следующий код:
                // Object.defineProperty(A, k, {
                //   value: mappedValue,
                //   writable: true,
                //   enumerable: true,
                //   configurable: true
                // });

                // Для лучшей поддержки браузерами, используем следующий код:
                A[k] = mappedValue;
            }
            // d. Увеличим k на 1.
            k++;
        }

        // 9. Вернём A.
        return A;
    };
}


// Шаги алгоритма ECMA-262, 5-е издание, 15.4.4.18
// Ссылка (en): http://es5.github.io/#x15.4.4.18
// Ссылка (ru): http://es5.javascript.ru/x15.4.html#x15.4.4.18
if (!Array.prototype.forEach) {

    Array.prototype.forEach = function (callback, thisArg) {

        var T, k;

        if (this == null) {
            throw new TypeError(' this is null or not defined');
        }

        // 1. Положим O равным результату вызова ToObject passing the |this| value as the argument.
        var O = Object(this);

        // 2. Положим lenValue равным результату вызова внутреннего метода Get объекта O с аргументом "length".
        // 3. Положим len равным ToUint32(lenValue).
        var len = O.length >>> 0;

        // 4. Если IsCallable(callback) равен false, выкинем исключение TypeError.
        // Смотрите: http://es5.github.com/#x9.11
        if (typeof callback !== 'function') {
            throw new TypeError(callback + ' is not a function');
        }

        // 5. Если thisArg присутствует, положим T равным thisArg; иначе положим T равным undefined.
        if (arguments.length > 1) {
            T = thisArg;
        }

        // 6. Положим k равным 0
        k = 0;

        // 7. Пока k < len, будем повторять
        while (k < len) {

            var kValue;

            // a. Положим Pk равным ToString(k).
            //   Это неявное преобразование для левостороннего операнда в операторе in
            // b. Положим kPresent равным результату вызова внутреннего метода HasProperty объекта O с аргументом Pk.
            //   Этот шаг может быть объединён с шагом c
            // c. Если kPresent равен true, то
            if (k in O) {

                // i. Положим kValue равным результату вызова внутреннего метода Get объекта O с аргументом Pk.
                kValue = O[k];

                // ii. Вызовем внутренний метод Call функции callback с объектом T в качестве значения this и
                // списком аргументов, содержащим kValue, k и O.
                callback.call(T, kValue, k, O);
            }
            // d. Увеличим k на 1.
            k++;
        }
        // 8. Вернём undefined.
    };
}


// Шаги алгоритма ECMA-262, 5-е издание, 15.4.4.14
// Ссылка (en): http://es5.github.io/#x15.4.4.14
// Ссылка (ru): http://es5.javascript.ru/x15.4.html#x15.4.4.14
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (searchElement, fromIndex) {
        var k;

        // 1. Положим O равным результату вызова ToObject с передачей ему
        //    значения this в качестве аргумента.
        if (this == null) {
            throw new TypeError('"this" is null or not defined');
        }

        var O = Object(this);

        // 2. Положим lenValue равным результату вызова внутреннего метода Get
        //    объекта O с аргументом "length".
        // 3. Положим len равным ToUint32(lenValue).
        var len = O.length >>> 0;

        // 4. Если len равен 0, вернём -1.
        if (len === 0) {
            return -1;
        }

        // 5. Если был передан аргумент fromIndex, положим n равным
        //    ToInteger(fromIndex); иначе положим n равным 0.
        var n = +fromIndex || 0;

        if (Math.abs(n) === Infinity) {
            n = 0;
        }

        // 6. Если n >= len, вернём -1.
        if (n >= len) {
            return -1;
        }

        // 7. Если n >= 0, положим k равным n.
        // 8. Иначе, n<0, положим k равным len - abs(n).
        //    Если k меньше нуля 0, положим k равным 0.
        k = Math.max(n >= 0 ? n : len - Math.abs(n), 0);

        // 9. Пока k < len, будем повторять
        while (k < len) {
            // a. Положим Pk равным ToString(k).
            //   Это неявное преобразование для левостороннего операнда в операторе in
            // b. Положим kPresent равным результату вызова внутреннего метода
            //    HasProperty объекта O с аргументом Pk.
            //   Этот шаг может быть объединён с шагом c
            // c. Если kPresent равен true, выполним
            //    i.  Положим elementK равным результату вызова внутреннего метода Get
            //        объекта O с аргументом ToString(k).
            //   ii.  Положим same равным результату применения
            //        Алгоритма строгого сравнения на равенство между
            //        searchElement и elementK.
            //  iii.  Если same равен true, вернём k.
            if (k in O && O[k] === searchElement) {
                return k;
            }
            k++;
        }
        return -1;
    };
}

if (!Array.prototype.every) {
    Array.prototype.every = function (callbackfn, thisArg) {
        'use strict';
        var T, k;

        if (this == null) {
            throw new TypeError('this is null or not defined');
        }

        // 1. Положим O равным результату вызова ToObject над значением
        //    this, переданным в качестве аргумента.
        var O = Object(this);

        // 2. Положим lenValue равным результату вызова внутреннего метода Get
        //    объекта O с аргументом "length".
        // 3. Положим len равным ToUint32(lenValue).
        var len = O.length >>> 0;

        // 4. Если IsCallable(callbackfn) равен false, выкинем исключение TypeError.
        if (typeof callbackfn !== 'function') {
            throw new TypeError();
        }

        // 5. Если thisArg присутствует, положим T равным thisArg; иначе положим T равным undefined.
        if (arguments.length > 1) {
            T = thisArg;
        }

        // 6. Положим k равным 0.
        k = 0;

        // 7. Пока k < len, будем повторять
        while (k < len) {

            var kValue;

            // a. Положим Pk равным ToString(k).
            //   Это неявное преобразование для левостороннего операнда в операторе in
            // b. Положим kPresent равным результату вызова внутреннего метода
            //    HasProperty объекта O с аргументом Pk.
            //   Этот шаг может быть объединён с шагом c
            // c. Если kPresent равен true, то
            if (k in O) {

                // i. Положим kValue равным результату вызова внутреннего метода Get
                //    объекта O с аргументом Pk.
                kValue = O[k];

                // ii. Положим testResult равным результату вызова внутреннего метода Call
                //     функции callbackfn со значением T в качестве this и списком аргументов,
                //     содержащим kValue, k и O.
                var testResult = callbackfn.call(T, kValue, k, O);

                // iii. Если ToBoolean(testResult) равен false, вернём false.
                if (!testResult) {
                    return false;
                }
            }
            k++;
        }
        return true;
    };
}