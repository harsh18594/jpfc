var Jpfc = Jpfc || {};

Jpfc.Helper = function () {
    var base64ToArrayBuffer = function (base64) {
        var binaryString = window.atob(base64);
        var binaryLen = binaryString.length;
        var bytes = new Uint8Array(binaryLen);
        for (var i = 0; i < binaryLen; i++) {
            var ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }
        return bytes;
    };

    var downloadFile = function (reportName, byte) {
        var blob = new Blob([byte]);
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        var fileName = reportName;
        link.download = fileName;
        link.click();
    };

    var isNullOrEmpty = function (val) {
        var retVal = false;
        if (val === undefined || val === null || val === '') {
            retVal = true;
        }
        return retVal;
    };

    return {
        base64ToArrayBuffer: base64ToArrayBuffer,
        downloadFile: downloadFile,
        isNullOrEmpty: isNullOrEmpty
    };
}();