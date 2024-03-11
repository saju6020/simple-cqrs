"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.close = exports.connect = void 0;
var es = null;
function connect() {
    var receivedArea = document.getElementById("received_text_area");
    es = new EventSource("http://localhost:5160/sse/connect");
    es.onopen = function (ev) {
        console.log(ev);
    };
    es.onmessage = function (ev) {
        var newText = document.createElement("div");
        newText.textContent = ev.data;
        receivedArea.appendChild(newText);
    };
    es.onerror = function (ev) {
        console.error(ev);
    };
}
exports.connect = connect;
function close() {
    es === null || es === void 0 ? void 0 : es.close();
}
exports.close = close;
//# sourceMappingURL=index.js.map