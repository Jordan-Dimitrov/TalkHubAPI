import {app, ipcMain, BrowserWindow} from 'electron';
let window : BrowserWindow;

app.on('ready', createWindow);

function createWindow() : void 
{
    window = new BrowserWindow(
    { 
        width: 800, 
        height: 600,
        webPreferences: { preload: __dirname + '/preload.js' },
        show: false
    });
    window.loadFile('./login.html');
    window.on('ready-to-show', window.show);
}
