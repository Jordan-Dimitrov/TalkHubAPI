import { ipcRenderer, contextBridge } from "electron";
const configData = require('fs').readFileSync('./config.json', 'utf8');
console.log(configData);

contextBridge.exposeInMainWorld("api", {
    test: configData
});
