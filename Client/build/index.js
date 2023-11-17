"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const electron_1 = require("electron");
let window;
electron_1.app.on('ready', createWindow);
function createWindow() {
    window = new electron_1.BrowserWindow({
        width: 800,
        height: 600,
        webPreferences: { preload: __dirname + '/preload.js' },
        show: false
    });
    window.loadFile('./login.html');
    window.on('ready-to-show', window.show);
}
