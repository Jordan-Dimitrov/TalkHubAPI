"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const electron_1 = require("electron");
const configData = require('fs').readFileSync('./config.json', 'utf8');
console.log(configData);
electron_1.contextBridge.exposeInMainWorld("api", {
    test: configData
});
