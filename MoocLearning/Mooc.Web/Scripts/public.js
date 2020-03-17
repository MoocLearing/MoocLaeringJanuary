
function isExistValue(_url,_data) {
    var bo = false;
    $.ajax({
        url: _url,
        data: _data,
        dataType: 'json', type: "POST", async: false,
        success: function (res) {
            if (res && res == 1)
                bo = true;
        }
    });

    return bo;
}

function getStorageItem(key) {
    var itemtemp = window.localStorage.getItem(key);
    var rettemp = "";
    if (itemtemp != null && itemtemp != "" && itemtemp != "undefined" && itemtemp != "null") {
        rettemp = itemtemp;
    }
    return rettemp;
}


function setStorageItem(key, value) {
    window.localStorage.setItem(key, value);
}

function removeStorageItem(key) {
    window.localStorage.removeItem(key);
}

function convertStrToDate(str) {
    if (str == undefined || str.length <= 0) {
        return null;
    }
    else {
        var arr = str.match(/\d+/g);
        if (arr != null && arr.length >= 3) {
            var date = new Date(arr[0], arr[1], arr[2]);
            switch (arr.length) {
                case 4:
                    date.setHours(arr[3]);
                    break;
                case 5:
                    date.setHours(arr[3]);
                    date.setMinutes(arr[4]);
                    break;
                case 6:
                    date.setHours(arr[3]);
                    date.setMinutes(arr[4]);
                    date.setSeconds(arr[5]);
                    break;
            }
            return date;
        }
        else
            return null;
    }
}